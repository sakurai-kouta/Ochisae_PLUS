using UnityEngine;
using UnityEngine.UI;
public class GemView : MonoBehaviour
{
    [SerializeField] private Sprite[] sprite = new Sprite[6];
    [SerializeField] private Image targetImage;

    public void SetSprite(int gemNum)
    {
        Debug.Log("gemNum = " + gemNum);
        if (sprite == null || sprite.Length == 0 || targetImage == null)
            return;

        Debug.Log("‚±‚±‚Ü‚Å‚«‚½");
        if (gemNum < 0 || gemNum >= sprite.Length)
        {
            Debug.LogWarning($"SetSprite: index out of range ({gemNum})");
            return;
        }
        Debug.Log("‚±‚±‚Ü‚Å‚«‚½");
        targetImage.sprite = sprite[gemNum];
    }
}
