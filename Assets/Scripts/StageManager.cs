using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class StageManager : MonoBehaviour
{
    private enum StageIndex
    {
        OMOTE = 0,
        URA
    }

    [SerializeField] private StageRuntimeData stageRuntimeData;
    [SerializeField] private SaveRuntimeData saveRuntimeData;
    [SerializeField] private TextMeshProUGUI playTimeView;
    [SerializeField] private bool isInitializePlayerPos;
    private float playTime;
    private PlayerController playerController;
    private bool isUramen = false;
    private SEPlayer sePlayer;

    public bool IsEnding { private get; set; }

    private void Awake() 
    {
        playerController = FindAnyObjectByType<PlayerController>();
        if (playerController != null && isInitializePlayerPos)
        {
            playerController.moveInitialPosOmote();
        }
        sePlayer = FindAnyObjectByType<SEPlayer>();
        playTime = saveRuntimeData.GetPlayTime();
    }

    public void InitStageManager() 
    {
        stageRuntimeData.ResetData();
        playTime = 0f;
    }

    private void Update()
    {
        if (!IsEnding)
        {
            playTime += Time.deltaTime;
        }
        int minute = (int)(playTime / 60);
        int second = (int)(playTime % 60);
        // MM:SS形式に変換してTextMeshProに表示
        playTimeView.text = "TIME " + minute + ":" + second.ToString("00");
    }
    public float GetPlayTime() 
    {
        return playTime;
    }

    public void CheckItemComplete() 
    {
        // Itemを全て集めたかどうか判定
        if (stageRuntimeData.isCompletedItems())
        {
            completedAllItems();
        }
    }

    // Itemを全て集めたときに呼び出す処理
    private void completedAllItems() 
    {
        if (!isUramen)
        {
            StartCoroutine(UraTransitionSequence());
            isUramen = true;
        }
    }
    private IEnumerator UraTransitionSequence()
    {
        // 物理停止
        Time.timeScale = 0f;
        sePlayer.PlayGOGOGOGO();
        // カメラ揺らし
        CameraShake shake = Camera.main.GetComponent<CameraShake>();
        if (shake != null)
        {
            yield return StartCoroutine(shake.Shake(2.0f, 0.3f));
        }
        else
        {
            // カメラ揺れがない場合でも1秒待つ
            yield return new WaitForSecondsRealtime(1.0f);
        }

        // プレイヤーを裏面へ移動
        if (playerController != null)
        {
            playerController.moveInitialPosUra();
        }

        // 少し余韻
        yield return new WaitForSecondsRealtime(0.1f);

        // 物理再開
        Time.timeScale = 1f;
        sePlayer.Stop();
    }
    public void RestartStage()
    {
        saveRuntimeData.DeleteSaveFile();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
