using UnityEngine;
using System.Collections.Generic;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private GameObject teleportZone_Ex1;
    [SerializeField] private GameObject teleportZone_Ex2;

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
        int isStory3 = SaveDataManager.Load("isStory3", 0);
        if (isStory3 == 1)
        {
            teleportZone_Ex2.SetActive(true);
        }
        else
        {
            teleportZone_Ex2.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Act1();
            Act4();
        }
    }

    private void Act1() 
    {
        int isUramenClear = SaveDataManager.Load("isUramenClear", 0);
        int isStory1 = SaveDataManager.Load("isStory1", 0);
        if (isUramenClear == 1 && isStory1 != 1) // 裏面クリア済みかつ、Story1は見視聴
        {
            SaveDataManager.Save("isStory1", 1);
            List<int> idList = new List<int>() {0, 1, 2, 3, 4, 5, 6, 7, 8};
            advPartManager.StartADV(idList);
            teleportZone_Ex1.SetActive(true);
        }
    }
    private void Act4()
    {
        int isStory2 = SaveDataManager.Load("isStory2", 0);
        int isStory3 = SaveDataManager.Load("isStory3", 0);
        if (isStory2 == 1 && isStory3 != 1) // Ex1クリア済みかつ、Story3は見視聴
        {
            SaveDataManager.Save("isStory3", 1);
            teleportZone_Ex2.SetActive(true);
            List<int> idList = new List<int>() { 18, 19, 20, 21, 22 };
            advPartManager.StartADV(idList);
        }
    }
}
