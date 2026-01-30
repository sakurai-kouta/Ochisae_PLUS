using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using static ADVDatabase;

public class ADVPartManager : MonoBehaviour
{
    [SerializeField] private ADVDatabase advDatabase;

    [Header("ADV UI References")]
    [SerializeField] private GameObject advRoot;
    [SerializeField] private Image speakerImage;
    [SerializeField] private Image listenerImage;
    [SerializeField] private TextMeshProUGUI serifText;

    private GamePauseController gamePauseController;
    private SEPlayer sePlayer;
    private InputAction interactInput;
    private bool isClicked;

    private void Start()
    {
        gamePauseController = FindAnyObjectByType<GamePauseController>();
        sePlayer = FindAnyObjectByType<SEPlayer>();
        interactInput = InputSystem.actions.FindAction("Click");
        advRoot.SetActive(false);
    }

    public void StartADV(List<int> sceneIdList)
    {
        if (advDatabase == null) return;
        if (gamePauseController == null) return;
        if (sceneIdList == null || sceneIdList.Count == 0) return;

        StartCoroutine(ADVSequence(sceneIdList));
    }

    private IEnumerator ADVSequence(List<int> sceneIdList)
    {
        gamePauseController.InvalidPause();

        foreach (int id in sceneIdList)
        {
            ADVData data = advDatabase.GetData(id);
            if (data != null)
            {
                yield return ShowADVScene(data);
            }
        }

        gamePauseController.ValidPause();
    }

    private IEnumerator ShowADVScene(ADVData advData)
    {
        advRoot.SetActive(true);

        // 話者
        SetCharacterImage(speakerImage, advData.speaker);

        // 聞き手
        SetCharacterImage(listenerImage, advData.listener);

        // セリフ
        serifText.text = ConfigMenuController.IsEnglish
            ? advData.serif_EN
            : advData.serif_JP;

        isClicked = false;
        interactInput.started += OnClick;

        yield return new WaitUntil(() => isClicked);
        sePlayer.PlaySerifOkuri();
        interactInput.started -= OnClick;
        advRoot.SetActive(false);
    }

    private void SetCharacterImage(Image image, Sprite sprite)
    {
        if (image == null) return;

        image.sprite = sprite;

        if (sprite != null)
        {
            image.SetNativeSize();   // ★ 元画像サイズで表示
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        isClicked = true;
    }
}
