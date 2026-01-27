using UnityEngine;
using UnityEngine.UI;

public class UICursorController : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image cursorImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite clickSprite;

    [Header("Offset")]
    [SerializeField] private Vector2 offset = Vector2.zero;

    void Awake()
    {
        Cursor.visible = false; // OSカーソル非表示
    }

    void Update()
    {
        bool overUI = UIMouseOverUtility.IsPointerOverUIWithTag("UI");

        cursorImage.enabled = overUI;
        if (!overUI) return;

        // マウス追従
        cursorImage.rectTransform.position =
            Input.mousePosition + (Vector3)offset;

        // クリック判定
        cursorImage.sprite =
            Input.GetMouseButton(0) ? clickSprite : normalSprite;
    }
}
