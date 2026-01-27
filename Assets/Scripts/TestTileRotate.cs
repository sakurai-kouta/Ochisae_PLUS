using UnityEngine;

public class TestTileRotate : MonoBehaviour
{
    [Header("‰ñ“]İ’è")]
    [SerializeField] private float rotateSpeed = 90f; // 1•b‚ ‚½‚è‚Ì‰ñ“]Šp“xi“xj
    [SerializeField] private bool clockwise = true;

    void Update()
    {
        float dir = clockwise ? -1f : 1f;
        transform.Rotate(0f, 0f, rotateSpeed * dir * Time.deltaTime);
    }
}
