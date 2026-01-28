using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    private string serifEnglish;
    private string serifJapanese;

    public void SetSerif(string jp, string en)
    {
        serifEnglish = "<mark=#000000DF padding=\"30, 30, 5, 5\">" + en + "</mark>";
        serifJapanese = "<mark=#000000DF padding=\"30, 30, 5, 5\">" + jp + "</mark>";
    }

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
