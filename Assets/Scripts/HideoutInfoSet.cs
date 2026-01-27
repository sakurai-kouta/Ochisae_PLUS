using UnityEngine;

public class HideoutInfoSet : MonoBehaviour
{
    private StageTitleView stageTitleView;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        stageTitleView = FindAnyObjectByType<StageTitleView>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ConfigMenuController.IsEnglish) 
            {
                stageTitleView.SetText("Hideout");
            }
            else
            {
                stageTitleView.SetText("‰B‚ê‰Æ");
            }
        }
    }
}
