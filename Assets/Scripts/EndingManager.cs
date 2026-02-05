using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using static AltitudeBGMTable;

public class EndingManager : MonoBehaviour
{
    [Header("Credit Settings")]
    [SerializeField] private string creditText;
    [SerializeField] private string creditText_EN;
    [SerializeField] private float showTime = 5f;

    [Header("Text Style")]
    [SerializeField] private TMP_FontAsset font;
    [SerializeField] private int fontSize = 36;
    [SerializeField] private Color fontColor = Color.white;

    [Header("Ending BGM")]
    [SerializeField] private BGMData endingBGMData;

    [Header("History")]
    [SerializeField] private string totalSecondsStr;
    [SerializeField] private string totalCountStr;
    [SerializeField] private string fastestSecondsStr;

    private StageManager stageManager;

    private GameObject creditTextObject;
    private BGMManager bgmManager;

    private void Start()
    {
        bgmManager = FindAnyObjectByType<BGMManager>();
        stageManager = FindAnyObjectByType<StageManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        StartEnding();
    }

    private void updateHistoryData()
    {
        if (ConfigMenuController.IsPreEasy) 
        {
            return;
        }
        int curPlayTime = (int) stageManager.GetPlayTime();
        // 総登山時間の更新
        if (totalSecondsStr != "" || totalSecondsStr != null) 
        {
            int playTime = SaveDataManager.Load(totalSecondsStr, 0);
            SaveDataManager.Save(totalSecondsStr, playTime + curPlayTime);
        }
        // 総登山回数の更新
        if (totalCountStr != "" || totalCountStr != null)
        {
            int playCount = SaveDataManager.Load(totalCountStr, 0);
            SaveDataManager.Save(totalCountStr, playCount + 1);
        }
        // 最速登山時間の更新
        if (fastestSecondsStr != "" || fastestSecondsStr != null)
        {
            int fastestTime = SaveDataManager.Load(fastestSecondsStr, 59999);
            if(fastestTime > curPlayTime) 
            {
                SaveDataManager.Save(fastestSecondsStr, curPlayTime);
            }
        }
    }

    private void StartEnding()
    {
        updateHistoryData();
        // Canvas を探す（なければ作成）
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("EndingCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // TextMeshProUGUI 作成
        creditTextObject = new GameObject("CreditText");
        creditTextObject.transform.SetParent(canvas.transform, false);

        TextMeshProUGUI text = creditTextObject.AddComponent<TextMeshProUGUI>();
        if (ConfigMenuController.IsEnglish) 
        {
            text.text = creditText_EN;
        }
        else
        {
            text.text = creditText;
        }
        text.font = font;
        text.fontSize = fontSize;
        text.color = fontColor;
        text.alignment = TextAlignmentOptions.Center;

        // 画面中央に配置
        RectTransform rect = text.rectTransform;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(1920, 1080);

        bgmManager.ChangeBGM_Manual(endingBGMData);

        // 一定時間後に削除
        StartCoroutine(AutoDestroy());
        // タイマーストップ
        stageManager.IsEnding = true;
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(showTime);

        if (creditTextObject != null)
        {
            Destroy(creditTextObject);
        }
    }
}
