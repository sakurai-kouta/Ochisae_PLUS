using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(
    fileName = "StageRuntimeData",
    menuName = "RuntimeData/Stage Runtime Data"
)]
public class StageRuntimeData : ScriptableObject
{
    [Header("Items")]
    [SerializeField] private int collectedItemCount;
    [SerializeField] private int totalItemNum;

    [Header("Checkpoints")]
    [SerializeField] private int remainingCheckpointCount;
    [SerializeField] private int checkpointPerItem;

    private Vector3 checkpointPos;
    private bool isValidCheckpoint;
    private GemView gemView;
    private CheckpointView checkpointView;

    public void SetOnlyData(int _collectedItemCount, int _remainingCheckpointCount, Vector3 _checkpointPos)
    {
        collectedItemCount = _collectedItemCount;
        remainingCheckpointCount = _remainingCheckpointCount;
        checkpointPos = _checkpointPos;
        isValidCheckpoint = true;
    }
    /// <summary>
    /// ステージ開始時に呼ぶ
    /// </summary>
    public void ResetData()
    {
        collectedItemCount = 0;
        remainingCheckpointCount = 0;
        gemView = FindAnyObjectByType<GemView>();
        checkpointView = FindAnyObjectByType<CheckpointView>();
        Debug.Log("checkpointView = " + checkpointView);
        isValidCheckpoint = false;
    }

    public int getCollectedItemCount()
    {
        return collectedItemCount;
    }
    public int getRemainingCheckpointCount()
    {
        return remainingCheckpointCount;
    }
    public bool isCompletedItems() 
    {
        return collectedItemCount >= totalItemNum;
    }

    public void AddItem(int amount = 1)
    {
        collectedItemCount += amount;
        AddCheckpoint(checkpointPerItem * amount);
        gemView.SetSprite(collectedItemCount);
    }
    public void AddCheckpoint(int amount = 1) 
    {
        remainingCheckpointCount += amount;
        checkpointView.SetCount(remainingCheckpointCount);
    }

    public bool UseCheckpoint(Vector3 _currentPlayerPos)
    {
        if (remainingCheckpointCount <= 0)
        {
            return false;
        }
        if (checkpointPos == null) 
        {
            return false;
        }
        remainingCheckpointCount--;
        checkpointView.SetCount(remainingCheckpointCount);
        checkpointPos = _currentPlayerPos;
        isValidCheckpoint = true;
        return true;
    }
    public void returnCheckpointPos(Transform _transform)
    {
        if (isValidCheckpoint)
        {
            _transform.position = checkpointPos;
        }
    }
    public void InvalidCheckpoint() 
    {
        isValidCheckpoint = false;
    }

    public bool IsValidCheckpoint() 
    {
        return isValidCheckpoint;
    }
    public void UpdateView() 
    {
        checkpointView.SetCount(remainingCheckpointCount);
        gemView.SetSprite(collectedItemCount);
    }
}
