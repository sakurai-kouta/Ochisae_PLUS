using UnityEngine;

public class PlayerWaterPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float normalGravityScale = 1.0f;
    [SerializeField] private float waterGravityScale = 0.4f;
    [SerializeField] private float heavyGravityScale = 1.5f;

    private int waterCount = 0;
    private int gravityCount = 0;

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
        if (other.CompareTag("Gravity"))
        {
            gravityCount++;
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
        if (other.CompareTag("Gravity"))
        {
            gravityCount--;
            UpdateGravity();
        }
    }

    void UpdateGravity()
    {
        if (waterCount > 0)
        {
            rb.gravityScale = waterGravityScale;
        }
        else if (gravityCount > 0) 
        {
            rb.gravityScale = heavyGravityScale;
        }
        else 
        {
            rb.gravityScale = normalGravityScale;
        }
    }
}
