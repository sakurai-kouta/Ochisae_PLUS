using UnityEngine;

public class HistoryBook : MonoBehaviour
{
    [SerializeField] private GameObject HistoryView;
    private GamePauseController gamePauseController;

    private void Start() 
    {
        gamePauseController = FindAnyObjectByType<GamePauseController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gamePauseController.InvalidPause();
            HistoryView.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HistoryView.SetActive(false);
            gamePauseController.ValidPause();
        }
    }
}
