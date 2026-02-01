using UnityEngine;
using System.Collections.Generic;

public class MoonStoryManager : MonoBehaviour
{
    [SerializeField] private StageRuntimeData stageRuntimeData;
    private ADVPartManager advPartManager;

    private void Start()
    {
        advPartManager = FindAnyObjectByType<ADVPartManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int isStory2 = SaveDataManager.Load("isStory2", 0);
            if (isStory2 == 0)
            {
                if (stageRuntimeData.isCompletedItems()) 
                {
                    // アイテムを全部集めた場合
                    SaveDataManager.Save("isStory2", 1);
                    Act3();
                }
                else 
                {
                    // アイテムが足りない場合
                    Act2();
                }
            }
        }
    }

    private void Act2() // アイテムが足りない場合
    {
        List<int> idList = new List<int>() { 11, 12 };
        advPartManager.StartADV(idList);
    }
    private void Act3() // アイテムを全部集めた場合
    {
        List<int> idList = new List<int>() { 13, 14, 15, 16, 17 };
        advPartManager.StartADV(idList);
    }
}
