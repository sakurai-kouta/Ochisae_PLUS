using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    [Header("生成するPrefab")]
    [SerializeField] private GameObject prefab;

    [Header("生成間隔（秒）")]
    [SerializeField] private float spawnInterval = 1.0f;

    [Header("生成位置のランダム範囲")]
    [SerializeField] private Vector2 positionOffsetMin = new Vector2(-1f, -1f);
    [SerializeField] private Vector2 positionOffsetMax = new Vector2(1f, 1f);

    [Header("初速のランダム範囲")]
    [SerializeField] private Vector2 velocityMin = new Vector2(-1f, 2f);
    [SerializeField] private Vector2 velocityMax = new Vector2(1f, 5f);

    [Header("生存時間（秒）")]
    [SerializeField] private float lifeTime = 5.0f;

    private float timer;

    private void Update()
    {
        if (prefab == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer -= spawnInterval;
            Spawn();
        }
    }

    private void Spawn()
    {
        // 位置のランダム化
        Vector3 offset = new Vector3(
            Random.Range(positionOffsetMin.x, positionOffsetMax.x),
            Random.Range(positionOffsetMin.y, positionOffsetMax.y),
            0f
        );

        GameObject obj = Instantiate(prefab, transform.position + offset, Quaternion.identity);

        // 速度のランダム化（Rigidbody2Dがある場合）
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 velocity = new Vector2(
                Random.Range(velocityMin.x, velocityMax.x),
                Random.Range(velocityMin.y, velocityMax.y)
            );
            rb.linearVelocity = velocity;
        }

        // 一定時間後に破壊
        Destroy(obj, lifeTime);
    }
}
