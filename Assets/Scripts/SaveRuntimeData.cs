using UnityEngine;
using System.Collections.Generic;
using System.IO;

[CreateAssetMenu(fileName = "SaveRuntimeData", menuName = "RuntimeData/SaveRuntimeData")]
public class SaveRuntimeData : ScriptableObject
{
#if UNITY_EDITOR
[ContextMenu("DEBUG/Delete Save File")]
private void DeleteSaveFileFromEditor()
{
    DeleteSaveFile();
}
#endif
    [SerializeField] private StageRuntimeData stageRuntimeData;

    [SerializeField] private bool isSaved;   // 有効なセーブデータが存在するか
    [SerializeField] private Vector3 checkpointPos;  // チェックポイント位置
    [SerializeField] private int collectedItemCount; // 集めたアイテム数
    [SerializeField] private int remainingCheckpointCount; // 残りチェックポイント数
    [SerializeField] private float playTime;    // 現ステージのプレイ時間

    // 獲得済みアイテムID
    [SerializeField] private List<int> collectedItems = new();


    // ================================
    // Save / Load
    // ================================

    private string SavePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    private void OnEnable()
    {
        LoadFromJson();
    }

    private void SaveToJson()
    {
        SaveData data = new SaveData
        {
            isSaved = isSaved,
            checkpointPos = checkpointPos,
            collectedItemCount = collectedItemCount,
            remainingCheckpointCount = remainingCheckpointCount,
            collectedItems = new List<int>(collectedItems),
            playTime = playTime
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    private void LoadFromJson()
    {
        if (!File.Exists(SavePath))
        {
            ResetData();
            return;
        }

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        isSaved = data.isSaved;
        checkpointPos = data.checkpointPos;
        collectedItemCount = data.collectedItemCount;
        remainingCheckpointCount = data.remainingCheckpointCount;
        collectedItems = new List<int>(data.collectedItems);
        playTime = data.playTime;
    }

    // ================================
    // Public API
    // ================================

    public void ResetData()
    {
        isSaved = false;
        checkpointPos = Vector3.zero;
        collectedItemCount = 0;
        remainingCheckpointCount = 0;
        collectedItems.Clear();
        playTime = 0f;
    }

    public void AddCollectedItem(int id)
    {
        if (!collectedItems.Contains(id))
        {
            collectedItems.Add(id);
        }
    }

    public void SetCheckpointPos(Vector3 pos)
    {
        checkpointPos = pos;
    }
    public void DeleteSaveFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.json");

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted: " + path);
        }
        else
        {
            Debug.Log("Save file does not exist.");
        }

        ResetData();
    }


    /// <summary>
    /// 現在のステージ状態を保存する（＝セーブ）
    /// </summary>
    public void SetCounts(float _playTime)
    {
        collectedItemCount = stageRuntimeData.getCollectedItemCount();
        remainingCheckpointCount = stageRuntimeData.getRemainingCheckpointCount();
        playTime = _playTime;
        isSaved = true;

        SaveToJson();
    }

    // ================================
    // Getter
    // ================================

    public bool IsSaved() => isSaved;
    public int GetCollectedItemCount() => collectedItemCount;
    public int GetRemainingCheckpointCount() => remainingCheckpointCount;
    public float GetPlayTime() => playTime;
    public Vector3 GetCheckpointPos() => checkpointPos;
    public List<int> GetCollectedItems() => collectedItems;
}

[System.Serializable]
public class SaveData
{
    public bool isSaved;
    public Vector3 checkpointPos;
    public int collectedItemCount;
    public int remainingCheckpointCount;
    public List<int> collectedItems;
    public float playTime;
}


/* 旧ソース
using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "SaveRuntimeData", menuName = "RuntimeData/SaveRuntimeData")]
public class SaveRuntimeData : ScriptableObject
{
    [SerializeField] private StageRuntimeData stageRuntimeData;
    [SerializeField] private bool isSaved;   // 有効なセーブデータが存在するか。
    private Vector3 checkpointPos;  // チェックポイントの位置
    private int collectedItemCount; // 集めたアイテム数
    private int remainingCheckpointCount;  // 残りチェックポイント数
    // 獲得済みのアイテムたちはここに記録しておく
    [SerializeField] List<int> collectedItems = new();

    public void ResetData() 
    {
        isSaved = false;
        checkpointPos = Vector3.zero;
        collectedItemCount = 0;
        remainingCheckpointCount = 0;
        collectedItems.Clear();
    }

    public void AddCollectedItem(int id)
    {
        collectedItems.Add(id);
    }
    public void SetCheckpointPos(Vector3 _pos) 
    {
        checkpointPos = _pos;
    }
    public void SetCounts()
    {
        collectedItemCount = stageRuntimeData.getCollectedItemCount();
        remainingCheckpointCount = stageRuntimeData.getRemainingCheckpointCount();
        isSaved = true;
    }
    public bool IsSaved()
    {
        return isSaved;
    }

    public int GetCollectedItemCount()
    {
        return collectedItemCount;
    }
    public int GetRemainingCheckpointCount()
    {
        return remainingCheckpointCount;
    }
    public Vector3 GetCheckpointPos()
    {
        return checkpointPos;
    }
    public List<int> GetCollectedItems()
    {
        return collectedItems;
    }
}
*/