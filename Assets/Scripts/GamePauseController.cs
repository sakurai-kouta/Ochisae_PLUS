using UnityEngine;
using UnityEngine.InputSystem;

public class GamePauseController : MonoBehaviour
{
    [SerializeField] private GameObject configUI;

    private InputAction escapeAction;
    private bool isPaused;
    private bool isInvalid;   // 会話イベント中やクレジット中はポーズできないようにする。

    private SEPlayer sePlayer;

    private void Awake()
    {
        isInvalid = false;
        escapeAction = InputSystem.actions.FindAction("Escape");
        ConfigMenuController configMenuController = configUI.GetComponent<ConfigMenuController>();
        configMenuController.LoadConfigData();
        ResumeGame();
        sePlayer = FindAnyObjectByType<SEPlayer>();
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
        if (isInvalid) 
        {
            Debug.Log("PauseGame(Invalid)");
            return;
        }
        Debug.Log("PauseGame");
        isPaused = true;
        configUI.SetActive(true);

        Time.timeScale = 0f; // ゲーム停止
        // カーソルは別途表示するので、ここでは表示しない。
        // Cursor.visible = true;
        // Cursor.lockState = CursorLockMode.None;
        if(sePlayer != null) 
        {
            sePlayer.PlayOpenConfig();
        }
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
        if(sePlayer != null) 
        {
            sePlayer.PlayCloseConfig();
        }
    }

    public void InvalidPause()
    {
        isInvalid = true;
    }
    public void ValidPause()
    {
        isInvalid = false;
    }
}
