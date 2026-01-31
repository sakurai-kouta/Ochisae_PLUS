using UnityEngine;

public class MissileController : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public void Launch(Vector2 direction, float force)
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null) return;

        // ”O‚Ì‚½‚ß³‹K‰»
        direction = direction.normalized;

        // direction ‚Æ“¯‚¶•ûŒü‚ğŒü‚©‚¹‚éiZ‰ñ“]‚Ì‚İj
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        rb2d.linearVelocity = direction * force;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0.25f);
    }

}
