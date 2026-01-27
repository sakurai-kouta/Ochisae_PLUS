using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private StageRuntimeData stageRuntimeData;
    [SerializeField] private SaveRuntimeData saveRuntimeData;
    [SerializeField] private GameObject[] Items;
    private List<int> collectedItemId = new();

    private void Start() 
    {
        stageRuntimeData.ResetData();
        Debug.Log("ItemManager初期化開始");
        // saveRuntimeDataの中身を参照して、アイテムを有効化したり無効化したりしろ。
        if (!saveRuntimeData.IsSaved()) // SaveDataなし
        {
            saveRuntimeData.ResetData();
            Debug.Log("SaveDataなし");
            return;
        }
        stageRuntimeData.SetOnlyData(
            saveRuntimeData.GetCollectedItemCount(),
            saveRuntimeData.GetRemainingCheckpointCount(), 
            saveRuntimeData.GetCheckpointPos());
        // いったん全部有効化
        foreach (var item in Items) 
        {
            item.gameObject.SetActive(true);
        }
        // 以前獲得済みの奴だけ無効化
        foreach (var itemId in saveRuntimeData.GetCollectedItems()) 
        {
            collectedItemId.Add(itemId);
            Items[itemId].gameObject.SetActive(false);
        }
        // プレイヤーをチェックポイントに移動する。
        PlayerController playerController = FindAnyObjectByType<PlayerController>();
        stageRuntimeData.returnCheckpointPos(playerController.gameObject.transform);
        stageRuntimeData.UpdateView();
        Debug.Log("ItemManager初期化完了");
    }

    public void AddCollectedItem(GameObject obj)
    {
        int i;
        for (i = 0; i < Items.Length; i++) 
        {
            if (Items[i] == obj) 
            {
                collectedItemId.Add(i);
                return;
            }
        }
    }

    public void SaveCollectedItem()
    {
        foreach (int id in collectedItemId) 
        {
            saveRuntimeData.AddCollectedItem(id);
        }
    }
}
