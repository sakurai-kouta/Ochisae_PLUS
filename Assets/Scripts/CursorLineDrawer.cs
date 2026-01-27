using UnityEngine;

public class CursorLineDrawer : MonoBehaviour
{
    private Transform player;

    [Header("References")]
    [SerializeField] private GameObject imagePrefab;

    [Header("Sprites")]
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite redSprite;

    [Header("Settings")]
    [SerializeField] private int count = 10;
    [SerializeField] private float spacing = 0.5f;

    [Range(0f, 1f)]
    [SerializeField] private float param = 0f;

    private SpriteRenderer[] renderers;
    private bool visible = true;

    [Header("Reticle")]
    [SerializeField] private GameObject reticlePrefab;
    private Transform reticle;
    [SerializeField] private float reticleMaxScale = 2f;
    [SerializeField] private float reticleMinScale = 1f;

    private bool initialized = false;

    // =========================
    // 外部初期化
    // =========================
    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }

    private void Awake()
    {
        InitializeObjects();
    }

    private void InitializeObjects()
    {
        if (initialized) return;

        Cursor.visible = false;

        renderers = new SpriteRenderer[count];

        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(imagePrefab, transform);
            renderers[i] = obj.GetComponent<SpriteRenderer>();
        }

        if (reticlePrefab != null)
        {
            var obj = Instantiate(reticlePrefab, transform);
            reticle = obj.transform;
        }

        initialized = true;
    }

    private void Update()
    {
        if (!initialized) return;
        if (player == null) return;
        if (!visible) return;
        if (Camera.main == null) return;

        Vector3 mouseWorldPos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 dir = (mouseWorldPos - player.position).normalized;

        int redCount = Mathf.RoundToInt(param * count);

        for (int i = 0; i < count; i++)
        {
            var r = renderers[i];
            if (!r) continue; // ★ MissingReference 対策

            r.transform.position =
                player.position + dir * spacing * (i + 1);

            r.sprite = (i < redCount) ? redSprite : blueSprite;
        }

        UpdateReticle(mouseWorldPos);
    }

    public void SetParam(float value)
    {
        param = Mathf.Clamp01(value);
    }

    // =========================
    // 表示制御 API
    // =========================
    public void SetVisible(bool isVisible)
    {
        visible = isVisible;

        if (renderers != null)
        {
            foreach (var r in renderers)
            {
                if (!r) continue;
                r.enabled = isVisible;
            }
        }

        if (reticle != null)
        {
            reticle.gameObject.SetActive(isVisible);
        }
    }

    public void Show()
    {
        SetVisible(true);
    }

    public void Hide()
    {
        SetVisible(false);
        SetParam(0f);
    }

    // =========================
    // Reticle
    // =========================
    private void UpdateReticle(Vector3 mouseWorldPos)
    {
        if (reticle == null) return;

        reticle.position = new Vector3(
            mouseWorldPos.x,
            mouseWorldPos.y,
            reticle.position.z
        );

        float scale =
            Mathf.Lerp(reticleMaxScale, reticleMinScale, param);
        reticle.localScale = new Vector3(scale, scale, 1f);
    }
}


/* 20260126対策前
using UnityEngine;

public class CursorLineDrawer : MonoBehaviour
{
    private Transform player;

    [Header("References")]
    public GameObject imagePrefab;

    [Header("Sprites")]
    public Sprite blueSprite;
    public Sprite redSprite;

    [Header("Settings")]
    public int count = 10;
    public float spacing = 0.5f;

    [Range(0f, 1f)]
    public float param = 0f;

    private SpriteRenderer[] renderers;
    private bool visible = true;

    [Header("Reticle")]
    public GameObject reticlePrefab;
    private Transform reticle;
    public float reticleMaxScale = 2f; // param=0 → 200%
    public float reticleMinScale = 1f; // param=1 → 100%

    // ★ 追加：外部から呼ぶ初期化
    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }

    void Start()
    {
        Cursor.visible = false;
        renderers = new SpriteRenderer[count];

        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(imagePrefab, transform);
            renderers[i] = obj.GetComponent<SpriteRenderer>();
        }
        if (reticlePrefab != null)
        {
            var obj = Instantiate(reticlePrefab, transform);
            reticle = obj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 dir = (mouseWorldPos - player.position).normalized;

        int redCount = Mathf.RoundToInt(param * count);
        int blueCount = count - redCount;

        for (int i = 0; i < count; i++)
        {
            renderers[i].transform.position =
                player.position + dir * spacing * (i + 1);
            if (i >= redCount)
                renderers[i].sprite = blueSprite;
            else
                renderers[i].sprite = redSprite;
        }
        UpdateReticle(mouseWorldPos);
    }

    public void SetParam(float _param)
    {
        param = _param;
    }

    // =========================
    // ★ 表示／非表示 切り替えAPI
    // =========================
    public void SetVisible(bool isVisible)
    {
        visible = isVisible;

        if (renderers != null)
        {
            foreach (var r in renderers)
            {
                r.enabled = isVisible;
            }
        }

        if (reticle != null)
        {
            reticle.gameObject.SetActive(isVisible);
        }
    }

    public void Show()
    {
        SetVisible(true);
    }

    public void Hide()
    {
        SetVisible(false);
        SetParam(0f);
    }
    private void UpdateReticle(Vector3 mouseWorldPos)
    {
        if (reticle == null) return;

        // 位置：マウスカーソル位置
        reticle.position = new Vector3(
            mouseWorldPos.x,
            mouseWorldPos.y,
            reticle.position.z
        );

        // サイズ：param に応じて変更
        float scale = Mathf.Lerp(reticleMaxScale, reticleMinScale, param);
        reticle.localScale = new Vector3(scale, scale, 1f);

        // 表示状態同期
        reticle.gameObject.SetActive(visible);
    }
}
*/