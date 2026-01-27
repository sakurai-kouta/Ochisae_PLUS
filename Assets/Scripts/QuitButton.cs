using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        // エディタ上で実行した場合
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルド後（exeなど）
        Application.Quit();
#endif
    }
}
