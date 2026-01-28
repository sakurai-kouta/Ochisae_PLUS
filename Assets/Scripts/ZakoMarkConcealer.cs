using UnityEngine;

public class ZakoMarkConcealer : MonoBehaviour
{
    [SerializeField] private GameObject zakoMark;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (zakoMark != null) 
        {
            if(collision.CompareTag("Player"))
            zakoMark.SetActive(false);
        }
    }
}
