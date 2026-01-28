using UnityEngine;
using UnityEngine.InputSystem;

public class Checkpoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StageRuntimeData stageRuntimeData;
    [SerializeField] private SaveRuntimeData saveRuntimeData;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject checkpointPrefab;
    [SerializeField] private CameraHoldEffect cameraHoldEffect;
    [SerializeField] private PlayerVanishEffect playerVanishEffect;
    [SerializeField] private PlayerGroundChecker groundChecker;


    [Header("Hold Settings")]
    [SerializeField] private float holdTime = 1.0f;


    private SEPlayer sePlayer;

    private InputAction interactInput_PutCheckpoint;
    private InputAction interactInput_ReturnCheckpoint;

    private float putTimer;
    private float returnTimer;

    private bool isHoldingPut;
    private bool isHoldingReturn;

    private GameObject currentCheckpoint;
    private Animator animator;

    private ItemManager itemManager;
    private StageManager stageManager;

    // ★ 長押しゲージ用（UI用）
    public float PutHoldParam { get; private set; }
    public float ReturnHoldParam { get; private set; }

    private void Start()
    {
        interactInput_PutCheckpoint =
            InputSystem.actions.FindAction("PutCheckpoint");
        interactInput_ReturnCheckpoint =
            InputSystem.actions.FindAction("ReturnCheckpoint");

        interactInput_PutCheckpoint.started += OnPushStart_PutCheckpoint;
        interactInput_PutCheckpoint.canceled += OnPushEnd_PutCheckpoint;

        interactInput_ReturnCheckpoint.started += OnPushStart_ReturnCheckpoint;
        interactInput_ReturnCheckpoint.canceled += OnPushEnd_ReturnCheckpoint;

        sePlayer = FindAnyObjectByType<SEPlayer>();
        if (sePlayer == null)
        {
            Debug.LogError("SEPlayer がシーンに存在しません");
        }
        animator = player.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator がシーンに存在しません");
        }
        itemManager = FindAnyObjectByType<ItemManager>();
        stageManager = FindAnyObjectByType<StageManager>();

        if (stageRuntimeData.IsValidCheckpoint()) 
        {
            currentCheckpoint = Instantiate(
            checkpointPrefab,
            player.position,
            Quaternion.identity
            );
        }
    }
    void OnDestroy()
    {
        if (interactInput_PutCheckpoint != null)
        {
            interactInput_PutCheckpoint.started -= OnPushStart_PutCheckpoint;
            interactInput_PutCheckpoint.canceled -= OnPushEnd_PutCheckpoint;
        }
        if (interactInput_ReturnCheckpoint != null)
        {
            interactInput_ReturnCheckpoint.started -= OnPushStart_ReturnCheckpoint;
            interactInput_ReturnCheckpoint.canceled -= OnPushEnd_ReturnCheckpoint;
        }
    }

    private void Update()
    {
        // =========================
        // チェックポイント設置
        // =========================
        if (isHoldingPut)
        {
            putTimer += Time.deltaTime;
            PutHoldParam = Mathf.Clamp01(putTimer / holdTime);

            if (putTimer >= holdTime)
            {
                PutHoldParam = 1f;
                isHoldingPut = false;

                // ★ カメラ演出終了
                if (cameraHoldEffect != null)
                    cameraHoldEffect.EndHold();

                // ★ SE 停止
                if (sePlayer != null)
                    sePlayer.Stop();

                PutCheckpoint();
                animator.SetBool("isPuttingFlag", false);
            }
        }

        // =========================
        // チェックポイント帰還
        // =========================
        if (isHoldingReturn)
        {
            returnTimer += Time.deltaTime;
            ReturnHoldParam = Mathf.Clamp01(returnTimer / holdTime);

            // ★ 消失演出更新
            if (playerVanishEffect != null)
                playerVanishEffect.Play(ReturnHoldParam);

            if (returnTimer >= holdTime)
            {
                ReturnHoldParam = 1f;
                isHoldingReturn = false;

                // ★ 完全消失
                if (playerVanishEffect != null)
                    playerVanishEffect.Finish();
                // ★ SE 停止
                if (sePlayer != null)
                {
                    sePlayer.Stop();
                    sePlayer.PlayWarpEnd();
                }

                ReturnCheckpoint();
            }
        }
    }

    // ===== 入力開始 =====

    private void OnPushStart_PutCheckpoint(InputAction.CallbackContext obj)
    {
        // ★ 空中なら設置不可
        if (groundChecker != null && !groundChecker.IsGrounded)
        {
            return;
        }

        putTimer = 0f;
        PutHoldParam = 0f;
        isHoldingPut = true;

        // ★ カメラ演出開始
        if (cameraHoldEffect != null)
            cameraHoldEffect.BeginHold();

        // ★ SE 再生
        if (sePlayer != null)
            sePlayer.PlayGOGOGOGO();
        animator.SetBool("isPuttingFlag", true);
    }

    private void OnPushEnd_PutCheckpoint(InputAction.CallbackContext obj)
    {
        isHoldingPut = false;
        putTimer = 0f;
        PutHoldParam = 0f;

        // ★ 即座に元に戻す
        if (cameraHoldEffect != null)
            cameraHoldEffect.EndHold();

        // ★ SE 停止
        if (sePlayer != null)
            sePlayer.Stop();
        animator.SetBool("isPuttingFlag", false);
    }

    private void OnPushStart_ReturnCheckpoint(InputAction.CallbackContext obj)
    {
        returnTimer = 0f;
        ReturnHoldParam = 0f;
        isHoldingReturn = true;
        // ★ SE 再生
        if (sePlayer != null)
            sePlayer.PlayWarp();
    }

    private void OnPushEnd_ReturnCheckpoint(InputAction.CallbackContext obj)
    {
        isHoldingReturn = false;
        returnTimer = 0f;
        ReturnHoldParam = 0f;

        // ★ 元に戻す
        if (playerVanishEffect != null)
            playerVanishEffect.ResetEffect();
        // ★ SE 停止
        if (sePlayer != null)
            sePlayer.Stop();
    }

    // ===== 実処理 =====

    private void PutCheckpoint()
    {

        if (!stageRuntimeData.UseCheckpoint(player.position))
            return;

        if (currentCheckpoint != null)
        {
            Destroy(currentCheckpoint);
        }

        currentCheckpoint = Instantiate(
            checkpointPrefab,
            player.position,
            Quaternion.identity
        );
        sePlayer.PlayGetItem();
        Save();
    }

    private void ReturnCheckpoint()
    {
        stageRuntimeData.returnCheckpointPos(player);
        // ★ 帰還後は即復帰（ワープ先で）
        if (playerVanishEffect != null)
            playerVanishEffect.ResetEffect();
    }

    private void Save() 
    {
        itemManager.SaveCollectedItem();
        saveRuntimeData.SetCheckpointPos(currentCheckpoint.transform.position);
        saveRuntimeData.SetCounts(stageManager.GetPlayTime());
    }
}
