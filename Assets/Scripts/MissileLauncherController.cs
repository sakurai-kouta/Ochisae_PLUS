using UnityEngine;
using System.Collections;

public class MissileLauncherController : MonoBehaviour
{
    [SerializeField] private MissileController missilePrefab;
    [SerializeField] private float fireRate = 2f;        // 発射間隔（秒）
    [SerializeField] private float launchForce = 100f;

    [Header("Spawn Offset")]
    [SerializeField] private float spawnOffset = 0.5f;  // 前方オフセット距離

    private Coroutine fireCoroutine;

    private void OnEnable()
    {
        fireCoroutine = StartCoroutine(FireLoop());
    }

    private void OnDisable()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    private IEnumerator FireLoop()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Fire()
    {
        if (missilePrefab == null) return;

        // 発射方向（基本は右）
        Vector2 direction = transform.right.normalized;

        // 生成位置を前方にオフセット
        Vector3 spawnPos =
            transform.position + (Vector3)(direction * spawnOffset);

        // ★ 親を this.transform に指定
        MissileController missile =
            Instantiate(missilePrefab, spawnPos, Quaternion.identity, transform);

        // 発射
        missile.Launch(direction, launchForce);
    }
}
