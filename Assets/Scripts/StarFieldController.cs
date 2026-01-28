using System.Collections.Generic;
using UnityEngine;

public class StarFieldController : MonoBehaviour
{
    [System.Serializable]
    public class StarPrefabSetting
    {
        public GameObject prefab;
        public int count = 10;

        [Range(0f, 1f)]
        public float minScale = 0.5f;

        [Range(0f, 1f)]
        public float maxScale = 1.0f;
    }

    [Header("Star Prefab Settings")]
    [SerializeField]
    private List<StarPrefabSetting> starSettings = new();

    [Header("Settings")]
    [SerializeField] private float depthZ = 0f;

    [Header("Reference")]
    [SerializeField] private Camera targetCamera;

    private readonly List<Transform> stars = new();
    private Vector3 lastCameraPos;

    // =====================
    // Unity Lifecycle
    // =====================

    private void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (starSettings.Count == 0)
        {
            Debug.LogError("StarFieldController: No star settings assigned.");
            enabled = false;
            return;
        }

        CreateStars();
        lastCameraPos = targetCamera.transform.position;
    }

    private void LateUpdate()
    {
        FollowCamera();
        KeepStarsInsideView();
    }

    // =====================
    // Star Creation
    // =====================

    private void CreateStars()
    {
        foreach (var setting in starSettings)
        {
            if (setting.prefab == null || setting.count <= 0)
                continue;

            for (int i = 0; i < setting.count; i++)
            {
                Vector3 pos = GetRandomPositionInView();
                GameObject star =
                    Instantiate(setting.prefab, pos, Quaternion.identity, transform);

                // ★ スケールをランダム化
                float scale = Random.Range(setting.minScale, setting.maxScale);
                star.transform.localScale = Vector3.one * scale;

                stars.Add(star.transform);
            }
        }
    }

    // =====================
    // Camera Follow
    // =====================

    private void FollowCamera()
    {
        Vector3 delta = targetCamera.transform.position - lastCameraPos;

        foreach (var star in stars)
        {
            if (star != null)
                star.position += delta;
        }

        lastCameraPos = targetCamera.transform.position;
    }

    // =====================
    // View Control
    // =====================

    private void KeepStarsInsideView()
    {
        foreach (var star in stars)
        {
            if (star == null) continue;

            Vector3 viewportPos =
                targetCamera.WorldToViewportPoint(star.position);

            bool outOfView =
                viewportPos.x < 0f || viewportPos.x > 1f ||
                viewportPos.y < 0f || viewportPos.y > 1f;

            if (outOfView)
            {
                star.position = GetRandomPositionInView();
            }
        }
    }

    // =====================
    // Utility
    // =====================

    private Vector3 GetRandomPositionInView()
    {
        Vector3 viewportPos = new Vector3(
            Random.value,
            Random.value,
            Mathf.Abs(targetCamera.transform.position.z) + depthZ
        );

        Vector3 worldPos =
            targetCamera.ViewportToWorldPoint(viewportPos);
        worldPos.z = depthZ;

        return worldPos;
    }
}
