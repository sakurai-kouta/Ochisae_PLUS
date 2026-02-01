using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.Tilemaps;
using System;
using TMPro.Examples;

public class PlayerController : MonoBehaviour
{
    public TileParamDB tileParamDB;
    public Tilemap tilemap;
    [SerializeField] private double maxJumpPower;
    [SerializeField] private double maxPressTime;
    [SerializeField] private PhysicsMaterial2D idleMaterial;
    [SerializeField] private GameObject cldPrefab;
    [SerializeField] private SEPlayer sePlayer;
    [SerializeField] private PlayerGroundChecker groundChecker;
    [SerializeField] private Vector3 initalPosHideout;
    [SerializeField] private Vector3 initalPosOmote;
    [SerializeField] private Vector3 initalPosUra;
    [SerializeField] private Vector3 initalPosEx1;
    [SerializeField] private StageManager stageManager;
    [SerializeField] private StageRuntimeData stageRuntimeData;
    [SerializeField] private StageTitleView stageTitleView;
    private double jumpPower;
    private double pressTime;
    private InputAction _interactInput;
    private Rigidbody2D rb2d;
    private BoxCollider2D collider;
    private Vector2 preVelocity;
    private CursorLineDrawer cld;
    private bool isChage = false;
    private Animator animator;
    private float sePlayTimer;
    private CameraCellFollow ccf;
    private GuiController guiController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        _interactInput = InputSystem.actions.FindAction("Click");
        _interactInput.started += OnClickStart;
        _interactInput.canceled += OnClickEnd;
        jumpPower = 0f;
        collider = GetComponent<BoxCollider2D>();
        collider.sharedMaterial = idleMaterial;
        cld = Instantiate(cldPrefab).GetComponent<CursorLineDrawer>();
        cld.Initialize(transform);
        animator = GetComponent<Animator>();
        ccf = FindAnyObjectByType<CameraCellFollow>();
        BGMManager.Instance?.UpdateHeight(transform.position.y);
        stageTitleView.UpdateText(transform.position.y);
        guiController = FindAnyObjectByType<GuiController>();
    }

    private void OnClickStart(InputAction.CallbackContext obj)
    {
        // UIにマウスカーソルが重なっている場合
        if (UIMouseOverUtility.IsPointerOverUI())
        {
            return;
        }
        // ★ 空中ならジャンプ不可
        if (groundChecker != null && !groundChecker.IsGrounded)
        {
            return;
        }
        pressTime = Time.timeAsDouble;
        cld.Show();
        isChage = true;
        animator.SetBool("isCharge", true);
        sePlayTimer = 0;
    }
    private void OnClickEnd(InputAction.CallbackContext obj)
    {
        // ★ 空中ならジャンプ不可
        if (!isChage)
        {
            return;
        }
        // do something.
        Vector3 dir = transform.position;
        dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - dir;
        // dir = new Vector3(dir.x, Math.Abs(dir.y), 0f);
        dir = new Vector3(dir.x, dir.y, 0f);
        dir = dir.normalized;
        pressTime = Time.timeAsDouble - pressTime;
        jumpPower = Mathf.Lerp(0f, (float)maxJumpPower, (float)(pressTime / maxPressTime));
        rb2d.linearVelocity = Vector2.zero;
        rb2d.AddForce(new Vector2(dir.x, dir.y) * (float)jumpPower);
        // 一瞬浮かせておかないと、側面で当たった判定になる。
        transform.position += new Vector3(0, 0.05f, 0);
        cld.Hide();
        isChage = false;
        animator.SetBool("isCharge", false);
        sePlayer.PlayJump();
    }

    private void LateUpdate() 
    {
        // 前Fの速度情報を保持
        preVelocity = rb2d.linearVelocity;
        // 現在のジャンプ力を計算する
        if (isChage)
        {
            double deltaTime = Time.timeAsDouble - pressTime;
            jumpPower = Mathf.Lerp(0f, (float)maxJumpPower, (float)(deltaTime / maxPressTime));
            float param = (float)(deltaTime / maxPressTime);
            cld.SetParam(param);
            SetSquash(param);
            if (param < 0.99f) 
            {
                sePlayTimer += Time.deltaTime;
                if (sePlayTimer > 1f / 20f) 
                {
                    sePlayer.PlayCharge();
                    sePlayTimer -= 1f / 20f;
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 contactNormal = collision.contacts[0].normal;
        contactNormal = Mathf.Abs(contactNormal.x) > Mathf.Abs(contactNormal.y) ? new Vector2(Mathf.Sign(contactNormal.x), 0f) : new Vector2(0f, Mathf.Sign(contactNormal.y));
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector3 hitPos = contact.point - contactNormal * 0.1f;
            Vector3Int cellPos = tilemap.WorldToCell(hitPos);

            TileBase tile = tilemap.GetTile<TileBase>(cellPos);
            if (tile != null)
            {
                TileParamData tileParamData = tileParamDB.searchData(tile);
                if (tileParamData != null)
                {
                    rb2d.linearVelocity = ColisionController.CulcVelocityStay(preVelocity, contactNormal, tileParamData);
                    cld.Show();
                }
            }
            else
            {
                // Debug.Log("tile is Null.");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactNormal = collision.contacts[0].normal;
        contactNormal = Mathf.Abs(contactNormal.x) > Mathf.Abs(contactNormal.y) ? new Vector2(Mathf.Sign(contactNormal.x), 0f) : new Vector2(0f, Mathf.Sign(contactNormal.y));
        if (contactNormal.y < 1f)
        {
            sePlayer.PlayCollision();
        }
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector3 hitPos = contact.point - contactNormal * 0.1f;
            Vector3Int cellPos = tilemap.WorldToCell(hitPos);

            TileBase tile = tilemap.GetTile<TileBase>(cellPos);
            if (tile != null)
            {
                TileParamData tileParamData = tileParamDB.searchData(tile);
                if (tileParamData != null)
                {
                    rb2d.linearVelocity = ColisionController.CulcVelocityEnter(preVelocity, contactNormal, tileParamData);
                    SetSquash(0);
                    cld.Show();
                }
            }
            else
            {
                // Debug.Log("tile is Null.");
            }
        }
    }
    private void SetSquash(float param)
    {
        param = Mathf.Clamp01(param);

        Vector3 scale = transform.localScale;
        scale.y = Mathf.Lerp(1.0f, 0.75f, param); // 縦だけ縮める
        transform.localScale = scale;
    }
    public void moveInitialPosOmote()
    {
        transform.position = initalPosOmote;
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;        // 速度をリセット
            rb2d.angularVelocity = 0f;           // 回転速度もリセット
        }
        isChage = false;
        ccf.SnapToPlayer();
        BGMManager.Instance?.UpdateHeight(transform.position.y);
        stageManager.InitStageManager();
        stageRuntimeData.InvalidCheckpoint();
        SaveDataManager.Save("isUramenValid", 1);
    }
    public void moveInitialPosUra()
    {
        transform.position = initalPosUra;
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;        // 速度をリセット
            rb2d.angularVelocity = 0f;           // 回転速度もリセット
        }
        isChage = false;
        ccf.SnapToPlayer();
        BGMManager.Instance?.UpdateHeight(transform.position.y);
        stageRuntimeData.InvalidCheckpoint();
    }
    public void moveInitialPosHideout()
    {
        sePlayer.PlayReturnHideout();
        stageManager.RestartStage();
        transform.position = initalPosHideout;
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;        // 速度をリセット
            rb2d.angularVelocity = 0f;           // 回転速度もリセット
        }
        isChage = false;
        ccf.SnapToPlayer();
        BGMManager.Instance?.UpdateHeight(transform.position.y);
        stageRuntimeData.InvalidCheckpoint();
        SaveDataManager.Save("isUramenValid", 0);
    }
    public void moveInitialPosEx1()
    {
        transform.position = initalPosEx1;
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;        // 速度をリセット
            rb2d.angularVelocity = 0f;           // 回転速度もリセット
        }
        isChage = false;
        ccf.SnapToPlayer();
        BGMManager.Instance?.UpdateHeight(transform.position.y);
        stageManager.InitStageManager();
        stageRuntimeData.InvalidCheckpoint();
        SaveDataManager.Save("isUramenValid", 0);
    }
    void OnDestroy()
    {
        if (_interactInput != null)
        {
            _interactInput.started -= OnClickStart;
            _interactInput.canceled -= OnClickEnd;
        }
    }

}
