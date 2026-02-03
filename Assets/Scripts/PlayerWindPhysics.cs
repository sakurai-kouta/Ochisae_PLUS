using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerWindPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float windForce = 1.0f;
    [SerializeField] private TileBase windTileRight;
    [SerializeField] private TileBase windTileLeft;
    [SerializeField] private PlayerGroundChecker groundChecker;

    void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
        groundChecker = GetComponent<PlayerGroundChecker>();

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Wind")) return;
        if (groundChecker.IsGrounded) return;

        // Tilemap を取得
        Tilemap tilemap = other.GetComponent<Tilemap>();
        if (tilemap == null) return;

        // プレイヤーの位置 → セル座標に変換
        Vector3Int cellPos = tilemap.WorldToCell(transform.position);

        // そのセルのタイルを取得
        TileBase tile = tilemap.GetTile(cellPos);
        if (tile == null) return;

        // 風向き判定
        if (tile == windTileRight)
        {
            rb.AddForce(Vector2.right * windForce, ForceMode2D.Force);
        }
        else if (tile == windTileLeft)
        {
            rb.AddForce(Vector2.left * windForce, ForceMode2D.Force);
        }
    }
}
