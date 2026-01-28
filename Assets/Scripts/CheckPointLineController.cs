using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CheckPointLineController : MonoBehaviour
{
    [SerializeField] private StageRuntimeData stageRuntimeData;
    [SerializeField] private FloorData floorData;
    private ItemManager itemManager;
    private StageTitleView stageTitleView;
    private SEPlayer sePlayer;

    private void Awake()
    {
        sePlayer = FindAnyObjectByType<SEPlayer>();
        if (sePlayer == null)
        {
            Debug.LogError("SEPlayer Ç™ÉVÅ[ÉìÇ…ë∂ç›ÇµÇ‹ÇπÇÒ");
        }
        stageTitleView = FindAnyObjectByType<StageTitleView>();
        itemManager = FindAnyObjectByType<ItemManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null)
            {
                if (rb.linearVelocity.y > 0)
                {
                    stageTitleView.UpdateText(transform.position.y);
                    if (ConfigMenuController.IsEasy)
                    {
                        stageRuntimeData.AddCheckpoint(1);
                    }
                    sePlayer.PlayGetItem();
                    BGMManager.Instance?.UpdateHeight(transform.position.y);
                    gameObject.SetActive(false);
                    itemManager.AddCollectedItem(gameObject);
                }
            }
        }
    }
}
