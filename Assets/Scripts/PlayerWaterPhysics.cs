using UnityEngine;

public class PlayerWaterPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float normalGravityScale = 1.0f;
    [SerializeField] private float waterGravityScale = 0.4f;

    private int waterCount = 0;

    void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            waterCount++;
            UpdateGravity();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            waterCount--;
            UpdateGravity();
        }
    }

    void UpdateGravity()
    {
        rb.gravityScale = (waterCount > 0)
            ? waterGravityScale
            : normalGravityScale;
    }
}
