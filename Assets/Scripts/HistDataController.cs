using UnityEngine;
using TMPro;
public class HistDataController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI dataText1;
    [SerializeField] private TextMeshProUGUI dataText2;
    [SerializeField] private TextMeshProUGUI dataText3;

    private void OnEnable() 
    {
        UpdateView();
    }
    private void UpdateView()
    {
        int tmp;
        // ‘“oR‰ñ”
        int totalClimbCountBeginning = SaveDataManager.Load("totalClimbCountBeginning", 0);
        int totalClimbCountRuin = SaveDataManager.Load("totalClimbCountRuin", 0);
        int totalClimbCountHighAndHigh = SaveDataManager.Load("totalClimbCountHighAndHigh", 0);
        int totalClimbCountSoratoh = SaveDataManager.Load("totalClimbCountSoratoh", 0);
        int totalClimbCountAll = totalClimbCountBeginning + totalClimbCountRuin + totalClimbCountHighAndHigh + totalClimbCountSoratoh;
        // ‘“oRŠÔ
        int totalClimbSecondsBeginning = SaveDataManager.Load("totalClimbSecondsBeginning", 0);
        int totalClimbSecondsRuin = SaveDataManager.Load("totalClimbSecondsRuin", 0);
        int totalClimbSecondshighAndHigh = SaveDataManager.Load("totalClimbSecondshighAndHigh", 0);
        int totalClimbSecondsSoratoh = SaveDataManager.Load("totalClimbSecondsSoratoh", 0);
        tmp = totalClimbSecondsBeginning;
        string strTCS1 = $"{tmp / 60}:{(tmp % 60):00}";
        tmp = totalClimbSecondsRuin;
        string strTCS2 = $"{tmp / 60}:{(tmp % 60):00}";
        tmp = totalClimbSecondshighAndHigh;
        string strTCS3 = $"{tmp / 60}:{(tmp % 60):00}";
        tmp = totalClimbSecondsSoratoh;
        string strTCS4 = $"{tmp / 60}:{(tmp % 60):00}";
        // Å‘¬“oRŠÔ
        int fastestClimbSecondsBeginning = SaveDataManager.Load("fastestClimbSecondsBeginning", 59999);
        int fastestClimbSecondsRuin = SaveDataManager.Load("fastestClimbSecondsRuin", 59999);
        int fastestClimbSecondsHighAndHigh = SaveDataManager.Load("fastestClimbSecondsHighAndHigh", 59999);
        int fastestClimbSecondsSoratoh = SaveDataManager.Load("fastestClimbSecondsSoratoh", 59999);
        tmp = fastestClimbSecondsBeginning;
        string strFCS1 = $"{tmp / 60}:{(tmp % 60):00}";
        tmp = fastestClimbSecondsRuin;
        string strFCS2 = $"{tmp / 60}:{(tmp % 60):00}";
        tmp = fastestClimbSecondsHighAndHigh;
        string strFCS3 = $"{tmp / 60}:{(tmp % 60):00}";
        tmp = fastestClimbSecondsSoratoh;
        string strFCS4 = $"{tmp / 60}:{(tmp % 60):00}";

        if (ConfigMenuController.IsEnglish) // English
        {
            titleText.text = "Statistics <color=#FF6062>(No updates in NERD Mode!)</color>";
            dataText1.text = "<color=#FFC345>[Total Climbs]</color>\r\n" +
                $"Overall\r\n<color=#62FF78>{totalClimbCountAll} times</color>\r\n" +
                "Beginning Tower\r\n" +
                $"<color=#62FF78>{totalClimbCountBeginning} times</color>\r\n" +
                "Beginning Tower (Ruins)\r\n" +
                $"<color=#62FF78>{totalClimbCountRuin} times</color>\r\n" +
                "High and High Tower\r\n" +
                $"<color=#62FF78>{totalClimbCountHighAndHigh} times</color>\r\n" +
                "Soratoh\r\n" +
                $"<color=#62FF78>{totalClimbCountSoratoh} times</color>\r\n";

            dataText2.text = "<color=#FFC345>[Total Climbing Time]</color>\r\n" +
                "Beginning Tower\r\n" +
                $"<color=#62FF78>{strTCS1}</color>\r\n" +
                "Beginning Tower (Ruins)\r\n" +
                $"<color=#62FF78>{strTCS2}</color>\r\n" +
                "High and High Tower\r\n" +
                $"<color=#62FF78>{strTCS3}</color>\r\n" +
                "Soratoh\r\n" +
                $"<color=#62FF78>{strTCS4}</color>";

            dataText3.text = "<color=#FFC345>[Fastest Climb Time]</color>\r\n" +
                "Beginning Tower\r\n" +
                $"<color=#62FF78>{strFCS1}</color>\r\n" +
                "Beginning Tower (Ruins)\r\n" +
                $"<color=#62FF78>{strFCS2}</color>\r\n" +
                "High and High Tower\r\n" +
                $"<color=#62FF78>{strFCS3}</color>\r\n" +
                "Soratoh\r\n" +
                $"<color=#62FF78>{strFCS4}</color>\r\n";

        }
        else  // “ú–{Œê
        {
            titleText.text = "“Œvî•ñ <color=#FF6062>(‚´‚±ƒ‚[ƒh’†‚ÍXV‚³‚ê‚Ü‚¹‚ñI)</color>";
            dataText1.text = "<color=#FFC345>y‘“oR‰ñ”z</color>\r\n" +
                $"‘S‘Ì\r\n<color=#62FF78>{totalClimbCountAll}‰ñ</color>\r\n" +
                "‚Í‚¶‚Ü‚è‚Ì“ƒ\r\n" +
                $"<color=#62FF78>{totalClimbCountBeginning}‰ñ</color>\r\n" +
                "‚Í‚¶‚Ü‚è‚Ì“ƒ(•ö‰ó)\r\n" +
                $"<color=#62FF78>{totalClimbCountRuin}‰ñ</color>\r\n" +
                "‚‚¢‚‚¢“ƒ\r\n" +
                $"<color=#62FF78>{totalClimbCountHighAndHigh}‰ñ</color>\r\n" +
                "‹ó“ƒ\r\n" +
                $"<color=#62FF78>{totalClimbCountSoratoh}‰ñ</color>\r\n";
            dataText2.text = "<color=#FFC345>y‘“oRŠÔz</color>\r\n" +
                "‚Í‚¶‚Ü‚è‚Ì“ƒ\r\n" +
                $"<color=#62FF78>{strTCS1}</color>\r\n" +
                "‚Í‚¶‚Ü‚è‚Ì“ƒ(•ö‰ó)\r\n" +
                $"<color=#62FF78>{strTCS2}</color>\r\n" +
                "‚‚¢‚‚¢“ƒ\r\n" +
                $"<color=#62FF78>{strTCS3}</color>\r\n" +
                "‹ó“ƒ\r\n" +
                $"<color=#62FF78>{strTCS4}</color>";
            dataText3.text = "<color=#FFC345>yÅ‘¬“oRŠÔz</color>\r\n" +
                "‚Í‚¶‚Ü‚è‚Ì“ƒ\r\n" +
                $"<color=#62FF78>{strFCS1}</color>\r\n" +
                "‚Í‚¶‚Ü‚è‚Ì“ƒ(•ö‰ó)\r\n" +
                $"<color=#62FF78>{strFCS2}</color>\r\n" +
                "‚‚¢‚‚¢“ƒ\r\n" +
                $"<color=#62FF78>{strFCS3}</color>\r\n" +
                "‹ó“ƒ\r\n" +
                $"<color=#62FF78>{strFCS4}</color>\r\n";
        }
    }
}
