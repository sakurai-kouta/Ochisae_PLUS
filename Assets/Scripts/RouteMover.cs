using UnityEngine;

public class RouteMover : MonoBehaviour
{
    [Header("Route Settings")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Gizmo Settings")]
    [SerializeField] private Color routeColor = Color.cyan;
    [SerializeField] private float pointRadius = 0.1f;

    private int currentIndex = 0;
    private int direction = 1; // 1 = forward, -1 = backward

    private void Update()
    {
        if (waypoints == null || waypoints.Length < 2)
            return;

        Transform target = waypoints[currentIndex];
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // 到達判定
        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            // 次のインデックスへ
            currentIndex += direction;

            // 端に到達したら折り返す
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = waypoints.Length - 2;
                direction = -1;
            }
            else if (currentIndex < 0)
            {
                currentIndex = 1;
                direction = 1;
            }
        }
    }

    // =========================
    // Sceneビュー用表示
    // =========================
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2)
            return;

        Gizmos.color = routeColor;

        // ルート線
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(
                    waypoints[i].position,
                    waypoints[i + 1].position
                );
            }
        }

        // 中継点
        foreach (var wp in waypoints)
        {
            if (wp != null)
            {
                Gizmos.DrawSphere(wp.position, pointRadius);
            }
        }
    }
}
