using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;
public class StageTitleView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stageTitleText;
    [SerializeField] private FloorData floorData;

    public void SetText(string floorName) 
    {
        Debug.Log(floorName);
        stageTitleText.text = floorName;
        FindAnyObjectByType<DiscordManager>().SetPlayingPresence(floorName);
    }

    public void UpdateText(float yPos)
    {
        string floorName;
        if (ConfigMenuController.IsEnglish)
        {
            floorName = "Reached Floor\n" + floorData.GetFloorName_EN(yPos);
        }
        else
        {
            floorName = "ç≈çÇìûíBäK\n" + floorData.GetFloorName(yPos);
        }
        SetText(floorName);
    }


}
