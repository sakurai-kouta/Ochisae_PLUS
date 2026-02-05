using System.Collections.Generic;
using UnityEngine;

public class EndingStoryTrigger : MonoBehaviour
{
    private ADVPartManager advPartManager;
    private void Start()
    {
        advPartManager = FindAnyObjectByType<ADVPartManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        Act();
    }
    private void Act()
    {
        List<int> idList = new List<int>() { 29, 30, 31, 32, 33, 34, 35, 36 };
        advPartManager.StartADV(idList);
    }
}
