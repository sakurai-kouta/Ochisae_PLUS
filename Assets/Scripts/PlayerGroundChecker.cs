using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerGroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float extraHeight = 0.05f; // è≠Çµâ∫Ç…êLÇŒÇ∑

    private Collider2D col;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // BoxCastÇ≈ê⁄ínîªíË
        Bounds bounds = col.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y); // â∫í[íÜêS
        Vector2 size = new Vector2(bounds.size.x * 1f, extraHeight);
        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            size,
            0f,
            Vector2.down,
            0.01f,
            groundLayer
        );

        IsGrounded = hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (col == null) return;
        Bounds bounds = col.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y);
        Vector2 size = new Vector2(bounds.size.x * 0.9f, extraHeight);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(origin - Vector2.up * 0.005f, size);
    }
}
