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

    private GameObject creditTextObject;
    private BGMManager bgmManager;

    private void Start()
    {
        bgmManager = FindAnyObjectByType<BGMManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        StartEnding();
    }

    private void StartEnding()
    {
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
