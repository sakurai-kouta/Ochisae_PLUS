using UnityEngine;

public class PlayerVanishEffect : MonoBehaviour
{
    [Header("Effect Settings")]
    [SerializeField] private float rotateSpeed = 720f;

    [Header("Physics Control")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D[] colliders;

    private Vector3 baseScale;
    private Quaternion baseRotation;

    private Vector2 savedVelocity;
    private float savedAngularVelocity;
    private bool isPlaying;

    private void Awake()
    {
        baseScale = transform.localScale;
        baseRotation = transform.localRotation;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (colliders == null || colliders.Length == 0)
            colliders = GetComponentsInChildren<Collider2D>();
    }

    /// <summary>
    /// 演出開始（最初に1回だけ呼ぶ）
    /// </summary>
    public void Begin()
    {
        if (isPlaying) return;
        isPlaying = true;

        // 物理状態保存
        if (rb != null)
        {
            savedVelocity = rb.linearVelocity;
            savedAngularVelocity = rb.angularVelocity;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false; // ★ 完全停止
        }

        // コライダー無効化
        foreach (var col in colliders)
            col.enabled = false;
    }

    /// <summary>
    /// 長押し中（0〜1）
    /// </summary>
    public void Play(float progress01)
    {
        if (!isPlaying)
            Begin();

        progress01 = Mathf.Clamp01(progress01);

        transform.localScale =
            Vector3.Lerp(baseScale, Vector3.zero, progress01);

        transform.localRotation =
            baseRotation * Quaternion.Euler(0f, 0f, rotateSpeed * progress01);
    }

    /// <summary>
    /// キャンセル時
    /// </summary>
    public void ResetEffect()
    {
        transform.localScale = baseScale;
        transform.localRotation = baseRotation;

        RestorePhysics();
    }

    /// <summary>
    /// 完了時（帰還直前）
    /// </summary>
    public void Finish()
    {
        transform.localScale = Vector3.zero;
        RestorePhysics();
    }

    // =========================
    // Physics Restore
    // =========================

    private void RestorePhysics()
    {
        if (!isPlaying) return;
        isPlaying = false;

        // コライダー復帰
        foreach (var col in colliders)
            col.enabled = true;

        if (rb != null)
        {
            rb.simulated = true;
            rb.linearVelocity = savedVelocity;
            rb.angularVelocity = savedAngularVelocity;
        }
    }
}
