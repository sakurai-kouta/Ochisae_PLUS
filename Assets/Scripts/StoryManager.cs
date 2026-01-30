using UnityEngine;
using System.Collections.Generic;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private GameObject teleportZone_Ex1;

    private ADVPartManager advPartManager;

    private void Start() 
    {
        advPartManager = FindAnyObjectByType<ADVPartManager>();

        int isStory1 = SaveDataManager.Load("isStory1", 0);
        if (isStory1 == 1) 
        {
            teleportZone_Ex1.SetActive(true);
        }
        else
        {
            teleportZone_Ex1.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Act1();
        }
    }

    private void Act1() 
    {
        int isUramenClear = SaveDataManager.Load("isUramenClear", 0);
        int isStory1 = SaveDataManager.Load("isStory1", 0);
        if (isUramenClear == 1 && isStory1 != 1) // ó†ñ ÉNÉäÉAçœÇ›Ç©Ç¬ÅAStory1ÇÕå©éãíÆ
        {
            SaveDataManager.Save("isStory1", 1);

            List<int> idList = new List<int>() {0, 1, 2, 3, 4, 5, 6, 7, 8};
            advPartManager.StartADV(idList);
            teleportZone_Ex1.SetActive(true);
        }
    }
}
