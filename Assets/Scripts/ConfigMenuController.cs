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
    [SerializeField] private Button returnButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button saveButton;

    [Header("UI Informations")]
    [SerializeField] private TextMeshProUGUI returnButtonDetail;
    [SerializeField] private TextMeshProUGUI quitButtonDetail;

    public static bool IsEnglish { get; private set; }
    private bool isEnglish;
    private float masterVolume;
    private float bgmVolume;
    private float seVolume;

    private BGMManager bgmManager;
    private SEPlayer sePlayer;

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
        }
        else
        {
            // 初期値
            isEnglish = false;
            IsEnglish = isEnglish;
            masterVolume = 1.0f;
            bgmVolume = 1.0f;
            seVolume = 1.0f;
        }
        ApplySoundVolume();
    }

    public void SaveConfigData()
    {
        ConfigData data = new ConfigData
        {
            isEnglish = isEnglish,
            masterVolume = masterVolume,
            bgmVolume = bgmVolume,
            seVolume = seVolume
        };
        IsEnglish = isEnglish;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(ConfigPath, json);
        ApplySoundVolume();
        Debug.Log("Config saved");
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
    }

    private void ApplyConfigToUI()
    {
        masterVolumeSlider.SetValueWithoutNotify(masterVolume);
        bgmVolumeSlider.SetValueWithoutNotify(bgmVolume);
        seVolumeSlider.SetValueWithoutNotify(seVolume);

        englishModeToggle.SetIsOnWithoutNotify(isEnglish);

        if (isEnglish)
            ChangeLanguageEnglish();
        else
            ChangeLanguageJapanese();
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

        if (isEnglish)
            ChangeLanguageEnglish();
        else
            ChangeLanguageJapanese();
    }

    // =====================
    // Language
    // =====================

    private void ChangeLanguageJapanese()
    {
        returnButton.GetComponentInChildren<TextMeshProUGUI>().text = "拠点に戻る";
        quitButton.GetComponentInChildren<TextMeshProUGUI>().text = "ゲーム中断";
        saveButton.GetComponentInChildren<TextMeshProUGUI>().text = "設定を保存";

        returnButtonDetail.text =
            "拠点に戻ると、プレイ状況はリセットされます。";
        quitButtonDetail.text =
            "中断すると、チェックポイント位置から再開されます。";
    }

    private void ChangeLanguageEnglish()
    {
        returnButton.GetComponentInChildren<TextMeshProUGUI>().text = "Go to Base";
        quitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Quit Game";
        saveButton.GetComponentInChildren<TextMeshProUGUI>().text = "Save Config";

        returnButtonDetail.text =
            "Returning to the base will reset current progress.";
        quitButtonDetail.text =
            "If you quit, you will resume from the last checkpoint.";
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
    }
}
