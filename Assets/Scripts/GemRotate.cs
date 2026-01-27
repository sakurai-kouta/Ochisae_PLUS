using UnityEngine;

public class GemRotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 90f;

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}
