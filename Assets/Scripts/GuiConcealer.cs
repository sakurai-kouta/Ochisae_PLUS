using UnityEngine;

public class GuiConcealer : MonoBehaviour
{
    // このオブジェクトに接触している間はguiを非表示にする
    [SerializeField] private GameObject gui;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gui == null) return;

        if (other.CompareTag("Player"))
        {
            gui.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (gui == null) return;

        if (other.CompareTag("Player"))
        {
            gui.SetActive(true);
        }
    }
}
