using System.Collections.Generic;
using UnityEngine;

public class SunStoryTrigger : MonoBehaviour
{
    [SerializeField] private StageRuntimeData stageRuntimeData;
    [SerializeField] private GameObject simpleTeleport;
    private ADVPartManager advPartManager;

    private void Start()
    {
        advPartManager = FindAnyObjectByType<ADVPartManager>();
        simpleTeleport.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int collectedItemCount = stageRuntimeData.getCollectedItemCount();
            if (collectedItemCount < 3 )
            {   // GemÇ™ë´ÇËÇ»Ç¢èÍçá
                List<int> idList = new List<int>() { 23, 24, 25 };
                advPartManager.StartADV(idList);
                simpleTeleport.SetActive(false);
            }
            else
            {   // GemÇ™ë´ÇËÇƒÇ¢ÇÈèÍçá
                List<int> idList = new List<int>() { 26, 27, 28 };
                advPartManager.StartADV(idList);
                simpleTeleport.SetActive(true);
            }
        }
    }
}
