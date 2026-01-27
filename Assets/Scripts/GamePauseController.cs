using UnityEngine;
using UnityEngine.InputSystem;

public class GamePauseController : MonoBehaviour
{
    [SerializeField] private GameObject configUI;

    private InputAction escapeAction;
    private bool isPaused;

    private void Awake()
    {
        escapeAction = InputSystem.actions.FindAction("Escape");
        ConfigMenuController configMenuController = configUI.GetComponent<ConfigMenuController>();
        configMenuController.LoadConfigData();
        ResumeGame();
    }

    private void OnEnable()
    {
        escapeAction.started += OnEscapePressed;
        escapeAction.Enable();
    }

    private void OnDisable()
    {
        escapeAction.started -= OnEscapePressed;
        escapeAction.Disable();
    }

    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        Debug.Log("PauseGame");
        isPaused = true;
        configUI.SetActive(true);

        Time.timeScale = 0f; // ゲーム停止
        // カーソルは別途表示するので、ここでは表示しない。
        // Cursor.visible = true;
        // Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        Debug.Log("ResumeGame");
        isPaused = false;
        configUI.SetActive(false);

        Time.timeScale = 1f; // 再開
        // カーソルは別途表示するので、ここでは表示しない。
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
    }
}
