using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using TMPro.Examples;

public class ConfigMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider seVolumeSlider;
    [SerializeField] private Toggle englishModeToggle;
    [SerializeField] private Toggle windowModeToggle;
    [SerializeField] private Toggle easyModeToggle;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private TextMeshProUGUI VersionText;

    [Header("UI Informations")]
    [SerializeField] private TextMeshProUGUI returnButtonDetail;
    [SerializeField] private TextMeshProUGUI quitButtonDetail;
    [SerializeField] private TextMeshProUGUI easyModeDetail;


    public static bool IsEnglish { get; private set; }
    public static bool IsEasy { get; private set; }
    public static bool IsPreEasy { get; set; }

    private bool isEnglish;
    private float masterVolume;
    private float bgmVolume;
    private float seVolume;
    private bool isWindow;
    private bool isEasy;

    private BGMManager bgmManager;
    private SEPlayer sePlayer;
    private GuiController guiController;

    private string ConfigPath =>
        Path.Combine(Application.persistentDataPath, "config.json");

    // =====================
    // Unity Lifecycle
    // =====================

    private void Start()
    {
        Debug.Log("ConfigMenuController Start");
        InitVolumeSlider();
        InitToggle();
        bgmManager = FindAnyObjectByType<BGMManager>();
        sePlayer = FindAnyObjectByType<SEPlayer>();
        guiController = FindAnyObjectByType<GuiController>();
    }

    private void OnEnable()
    {
        LoadConfigData();
        ApplyConfigToUI();
    }

    // =====================
    // Config Load / Save
    // =====================

    public void LoadConfigData()
    {
        if (File.Exists(ConfigPath))
        {
            string json = File.ReadAllText(ConfigPath);
            ConfigData data = JsonUtility.FromJson<ConfigData>(json);

            isEnglish = data.isEnglish;
            IsEnglish = isEnglish;
            masterVolume = data.masterVolume;
            bgmVolume = data.bgmVolume;
            seVolume = data.seVolume;
            isWindow = data.isWindow;
            isEasy = data.isEasy;
            IsEasy  = isEasy;
            IsPreEasy = data.isPreEasy;
        }
        else
        {
            // 初期値
            isEnglish = false;
            IsEnglish = isEnglish;
            masterVolume = 1.0f;
            bgmVolume = 1.0f;
            seVolume = 1.0f;
            isWindow = false;
            isEasy = false;
            IsEasy = isEasy;
            IsPreEasy = isEasy;
        }
        ApplySoundVolume();
        ApplyScreenMode();
        Debug.Log("load flg = " + IsPreEasy);
    }

    public void SaveConfigData()
    {
        IsEnglish = isEnglish;
        IsEasy = isEasy;
        guiController.UpdateZakoMarkRuntime();
        ConfigData data = new ConfigData
        {
            isEnglish = isEnglish,
            masterVolume = masterVolume,
            bgmVolume = bgmVolume,
            seVolume = seVolume,
            isWindow = isWindow,
            isEasy = isEasy,
            isPreEasy = IsPreEasy,
        };
        Debug.Log("Saved flg = " + IsPreEasy);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(ConfigPath, json);
        ApplySoundVolume();
        ApplyScreenMode();
    }

    public void OnClickSaveButton() 
    {
        sePlayer.PlaySaveConfig();
        SaveConfigData();
    }

    // =====================
    // UI Initialize
    // =====================

    private void InitVolumeSlider()
    {
        masterVolumeSlider.onValueChanged.AddListener(v => masterVolume = v);
        bgmVolumeSlider.onValueChanged.AddListener(v => bgmVolume = v);
        seVolumeSlider.onValueChanged.AddListener(v => seVolume = v);
    }

    private void InitToggle()
    {
        englishModeToggle.onValueChanged.AddListener(OnLanguageToggleChanged);
        windowModeToggle.onValueChanged.AddListener(OnWindowModeToggleChanged);
        easyModeToggle.onValueChanged.AddListener(OnEasyModeToggleChanged);
    }

    private void ApplyConfigToUI()
    {
        masterVolumeSlider.SetValueWithoutNotify(masterVolume);
        bgmVolumeSlider.SetValueWithoutNotify(bgmVolume);
        seVolumeSlider.SetValueWithoutNotify(seVolume);

        englishModeToggle.SetIsOnWithoutNotify(isEnglish);
        windowModeToggle.SetIsOnWithoutNotify(isWindow);
        easyModeToggle.SetIsOnWithoutNotify(isEasy);

        if (isEnglish)
            ChangeLanguageEnglish();
        else
            ChangeLanguageJapanese();

        VersionText.text = Application.version;
    }

    private void ApplyScreenMode() 
    {
        if (isWindow) 
        {
            //Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(
            1280,      // 横解像度
            720,     // 縦解像度
            FullScreenMode.Windowed  // フルスクリーン指定
            );
        }
        else 
        {
            //Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(
            1920,      // 横解像度
            1080,     // 縦解像度
            FullScreenMode.FullScreenWindow  // フルスクリーン指定
            );
        }
    }
    
    private void ApplySoundVolume() 
    {
        float seVol = masterVolume * seVolume;
        float bgmVol = masterVolume * bgmVolume;
        if(bgmManager == null) bgmManager = FindAnyObjectByType<BGMManager>();
        if (sePlayer == null) sePlayer = FindAnyObjectByType<SEPlayer>();
        sePlayer.SetVolume(seVol);
        bgmManager.SetVolume(bgmVol);
    }

    private void OnLanguageToggleChanged(bool value)
    {
        isEnglish = value;
        sePlayer.PlayToggle();

        if (isEnglish)
            ChangeLanguageEnglish();
        else
            ChangeLanguageJapanese();
    }
    private void OnWindowModeToggleChanged(bool value)
    {
        isWindow = value;
        sePlayer.PlayToggle();
    }
    private void OnEasyModeToggleChanged(bool value)
    {
        isEasy = value;
        sePlayer.PlayToggle();
    }

    // =====================
    // Language
    // =====================

    private void ChangeLanguageJapanese()
    {
        returnButton.GetComponentInChildren<TextMeshProUGUI>().text = "拠点に戻る";
        quitButton.GetComponentInChildren<TextMeshProUGUI>().text = "ゲーム中断";
        saveButton.GetComponentInChildren<TextMeshProUGUI>().text = "設定を保存";
        easyModeToggle.GetComponentInChildren<Text>().text = "ざこ専用モード";

        returnButtonDetail.text =
            "拠点に戻ると、プレイ状況はリセットされます。";
        quitButtonDetail.text =
            "中断すると、チェックポイント位置から再開されます。";
        easyModeDetail.text = 
            "1階層ごとに追加のチェックポイントが貰えます。一度でも有効にすると、拠点に戻るまで「ざこマーク」が付きます。";
    }

    private void ChangeLanguageEnglish()
    {
        returnButton.GetComponentInChildren<TextMeshProUGUI>().text = "Go to Base";
        quitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Quit Game";
        saveButton.GetComponentInChildren<TextMeshProUGUI>().text = "Save Config";
        easyModeToggle.GetComponentInChildren<Text>().text = "NERD Mode";

        returnButtonDetail.text =
            "Returning to the base will reset current progress.";
        quitButtonDetail.text =
            "If you quit, you will resume from the last checkpoint.";
        easyModeDetail.text =
            "You earn an extra checkpoint on every floor. Once it’s activated, a \"NERD\" mark remains until you return to the base.";
    }

    // =====================
    // Save Data Class
    // =====================

    [System.Serializable]
    private class ConfigData
    {
        public bool isEnglish;
        public float masterVolume;
        public float bgmVolume;
        public float seVolume;
        public bool isWindow;
        public bool isEasy;
        public bool isPreEasy;
    }
}
