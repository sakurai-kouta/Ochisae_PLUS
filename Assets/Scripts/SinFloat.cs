using UnityEngine;

public class SinFloat : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float amplitude = 0.5f;   // 揺れ幅
    [SerializeField] private float frequency = 1f;     // 揺れる速さ（周期）
    [SerializeField] private bool useLocalPosition = true;

    private Vector3 startPos;
    private float timeOffset;

    private void Start()
    {
        startPos = useLocalPosition ? transform.localPosition : transform.position;
        // 個体差を出すためのランダム位相
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency + timeOffset) * amplitude;

        if (useLocalPosition)
        {
            transform.localPosition = startPos + Vector3.up * yOffset;
        }
        else
        {
            transform.position = startPos + Vector3.up * yOffset;
        }
    }
}
