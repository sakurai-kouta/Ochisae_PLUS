using UnityEngine;
using TMPro;

public class TeleportZone : MonoBehaviour
{
    private enum TeleportPosIndex
    {
        HIDEOUT,
        OMOTE,
        URA,
    }

    [Header("Teleport Settings")]
//    [SerializeField] private Vector3 teleportPosition;
    [SerializeField] private float requiredTime = 2.0f;
    [SerializeField] private TeleportPosIndex teleportPosition = TeleportPosIndex.HIDEOUT;

    private float stayTimer = 0f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        stayTimer += Time.deltaTime;

        // プレイヤー頭上の表示を更新
        PlayerTeleportUI ui = other.GetComponent<PlayerTeleportUI>();
        if (ui != null)
        {
            ui.ShowRemainingTime(requiredTime - stayTimer);
        }

        // 転移
        if (stayTimer >= requiredTime)
        {
            Teleport(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        stayTimer = 0f;

        PlayerTeleportUI ui = other.GetComponent<PlayerTeleportUI>();
        if (ui != null)
        {
            ui.Hide();
        }
    }

    private void Teleport(Collider2D player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        switch (teleportPosition) 
        {
            case TeleportPosIndex.HIDEOUT:
                pc.moveInitialPosHideout();
                break;
            case TeleportPosIndex.OMOTE:
                pc.moveInitialPosOmote();
                break;
            case TeleportPosIndex.URA:
                pc.moveInitialPosUra();
                break;
            default:
                break;
        }

        // UIリセット
        PlayerTeleportUI ui = player.GetComponent<PlayerTeleportUI>();
        if (ui != null)
        {
            ui.Hide();
        }

        stayTimer = 0f;
    }
}
