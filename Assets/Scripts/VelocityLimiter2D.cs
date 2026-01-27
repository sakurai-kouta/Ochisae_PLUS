using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VelocityLimiter2D : MonoBehaviour
{
    [Header("Max Speed (per axis)")]
    public float maxSpeedX = 8f;
    public float maxSpeedY = 12f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 v = rb.linearVelocity;
        v.x = Mathf.Clamp(v.x, -maxSpeedX, maxSpeedX);
        v.y = Mathf.Clamp(v.y, -maxSpeedY, maxSpeedY);

        rb.linearVelocity = v;
    }
}
