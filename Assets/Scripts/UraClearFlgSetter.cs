using UnityEngine;

public class UraClearFlgSetter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SaveDataManager.Save("isUramenClear", 1);
        }
    }
}
