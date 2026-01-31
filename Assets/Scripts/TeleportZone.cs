using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TeleportZone : MonoBehaviour
{
    private enum TeleportPosIndex
    {
        HIDEOUT,
        OMOTE,
        URA,
        EX1,
    }

    [Header("Teleport Settings")]
    [SerializeField] private float requiredTime = 2.0f;
    [SerializeField] private TeleportPosIndex teleportPosition = TeleportPosIndex.HIDEOUT;

    private float stayTimer = 0f;
    private GuiController guiController;

    private void Start()
    {
        guiController = FindAnyObjectByType<GuiController>();
    }

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
        // プレイヤーの移動
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
            case TeleportPosIndex.EX1:
                if (VersionManager.IsTrial)
                {
                    pc.moveInitialPosOmote();
                    ADVPartManager advPartManager = FindAnyObjectByType<ADVPartManager>();
                    List<int> idList = new List<int>() { 9, 10 };
                    advPartManager.StartADV(idList);
                }
                else 
                {
                    // EXステージに飛ばす。
                    pc.moveInitialPosEx1();
                }
                break;
            default:
                break;
        }
        // ざこモードが有効なら、ざこマークをGUI上に表示する。
        guiController.UpdateZakoMark();

        // UIリセット
        PlayerTeleportUI ui = player.GetComponent<PlayerTeleportUI>();
        if (ui != null)
        {
            ui.Hide();
        }

        stayTimer = 0f;
    }
}
