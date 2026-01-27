using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private string serifEnglish;
    [SerializeField] private string serifJapanese;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) 
        {
            return;
        }
        if (ConfigMenuController.IsEnglish) 
        {
            text.SetText(serifEnglish);
        }
        else
        {
            text.SetText(serifJapanese);
        }
        text.gameObject.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        text.gameObject.SetActive(false);
    }
}
