using UnityEngine;
using TMPro;

public class PlayerTeleportUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.0f, 0);

    void Awake()
    {
        Hide();
    }

    void LateUpdate()
    {
        // èÌÇ…ì™è„Ç…í«è]
        text.transform.position = transform.position + offset;
    }

    public void ShowRemainingTime(float time)
    {
        time = Mathf.Max(0, time);
        text.gameObject.SetActive(true);
        //        text.text = "<color=#000000>" + time.ToString("0.0") + "</color>";
        if (ConfigMenuController.IsEnglish) 
        {
            text.text = "Transfer in " + time.ToString("0.0");
        }
        else 
        {
            text.text = "ì]ëóÇ‹Ç≈ " + time.ToString("0.0");
        }
    }

    public void Hide()
    {
        text.gameObject.SetActive(false);
    }
}
