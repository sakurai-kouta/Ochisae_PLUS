using UnityEngine;

public class GemController : MonoBehaviour
{
    [SerializeField] private StageRuntimeData stageRuntimeData;
    private ItemManager itemManager;
    private SEPlayer sePlayer;
    private GemView gemView;
    private StageManager stageManager;

    private void Awake()
    {
        sePlayer = FindAnyObjectByType<SEPlayer>();
        if (sePlayer == null)
        {
            Debug.LogError("SEPlayer がシーンに存在しません");
        }
        gemView = FindAnyObjectByType<GemView>();
        if (gemView == null)
        {
            Debug.LogError("GemView がシーンに存在しません");
        }
        itemManager = FindAnyObjectByType<ItemManager>();
        stageManager = FindAnyObjectByType<StageManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: ここで所持数を増やす等
            stageRuntimeData.AddItem(1);
            sePlayer.PlayGetItem();
            gameObject.SetActive(false);
            itemManager.AddCollectedItem(gameObject);
            stageManager.CheckItemComplete();
        }
    }
}
