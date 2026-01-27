using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using TMPro;
public class CheckpointView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    public void SetCount(int _cnt)
    {
        if (_cnt < 0)
        {
            return;
        }
        textMesh.text = "~" + _cnt;
    }
}
