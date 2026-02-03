using UnityEngine;

public class SimpleTeleport : MonoBehaviour
{
    [SerializeField] private Transform movePoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            collision.transform.position = movePoint.position;
        }
    }
}
