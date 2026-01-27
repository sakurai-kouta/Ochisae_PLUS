using UnityEngine;
using TMPro;
public class GuiController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI howToText;
    
    void Start()
    {
        Debug.Log("IsEnglish = " + ConfigMenuController.IsEnglish);
        if (ConfigMenuController.IsEnglish) 
        {
            howToText.text = "<color=#FFC345>【HowTo】</color>\r\n" +
                "MLeft ... Jump\r\n" +
                "<color=#FF4576>C</color>key ... <color=#FF4576>Put </color>CP\r\n" +
                "<color=#45FF7C>Z</color>key ... <color=#45FF7C>Return </color>CP\r\n" +
                "<color=#8BAAFF>Esc</color>/MRight ... menu";
        }
        else
        {
            howToText.text = "<color=#FFC345>【操作方法】</color>\r\n" +
                "左クリック ... じゃんぷ\r\n" +
                "<color=#FF4576>C</color>キー ... ﾁｪｯｸﾎﾟｲﾝﾄ<color=#FF4576>設置</color>\r\n" +
                "<color=#45FF7C>Z</color>キー ... ﾁｪｯｸﾎﾟｲﾝﾄ<color=#45FF7C>帰還</color>\r\n" +
                "<color=#8BAAFF>Esc</color>/右ｸﾘｯｸ ... メニュー";
        }
    }
}
