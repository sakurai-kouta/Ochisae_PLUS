using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("References")]
    [SerializeField] private AltitudeBGMTable bgmTable;
    [SerializeField] private AudioSource audioSource;

    [Header("Settings")]
    [SerializeField] private float fadeTime = 1.0f;

    private AudioClip currentClip;
    private float currentTargetVolume = 1.0f;
    private Coroutine bgmCoroutine;
    private float masterVolume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource.loop = true;
        audioSource.volume = 0f;
    }

    public void UpdateHeight(float height)
    {
        var nextData = GetBGMDataByHeight(height);
        ChangeBGM_Manual(nextData);
    }

    public void ChangeBGM_Manual(AltitudeBGMTable.BGMData nextData)
    {
        // === BGMなしゾーン ===
        if (nextData == null)
        {
            if (currentClip == null)
                return; // すでに無音

            if (bgmCoroutine != null)
                StopCoroutine(bgmCoroutine);

            bgmCoroutine = StartCoroutine(FadeOutOnly());
            return;
        }

        // 同じBGMなら何もしない
        if (nextData.bgm == currentClip)
            return;

        if (bgmCoroutine != null)
            StopCoroutine(bgmCoroutine);

        bgmCoroutine = StartCoroutine(ChangeBGM(nextData));
    }

    private AltitudeBGMTable.BGMData GetBGMDataByHeight(float height)
    {
        AltitudeBGMTable.BGMData result = null;
        float bestHeight = float.MinValue;

        foreach (var data in bgmTable.bgmList)
        {
            if (height >= data.minHeight && data.minHeight > bestHeight)
            {
                bestHeight = data.minHeight;
                result = data;
            }
        }

        return result;
    }

    private IEnumerator ChangeBGM(AltitudeBGMTable.BGMData nextData)
    {
        // フェードアウト
        yield return FadeOutInternal();

        audioSource.clip = nextData.bgm;
        audioSource.Play();

        // フェードイン
        float t = 0f;
        currentTargetVolume = nextData.volume;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, currentTargetVolume * masterVolume, t / fadeTime);
            yield return null;
        }

        audioSource.volume = currentTargetVolume * masterVolume;
        currentClip = nextData.bgm;
        bgmCoroutine = null;
    }

    private IEnumerator FadeOutOnly()
    {
        yield return FadeOutInternal();

        audioSource.Stop();
        audioSource.clip = null;
        currentClip = null;
        bgmCoroutine = null;
    }

    private IEnumerator FadeOutInternal()
    {
        float t = 0f;
        float startVol = audioSource.volume;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVol, 0f, t / fadeTime);
            yield return null;
        }

        audioSource.volume = 0f;
    }
    public void SetVolume(float volume) 
    {
        masterVolume = volume;
        audioSource.volume = volume * currentTargetVolume;
    }
}
