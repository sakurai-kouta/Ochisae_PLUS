using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip seClip_jump;
    [SerializeField] private AudioClip seClip_collision;
    [SerializeField] private AudioClip seClip_charge;
    [SerializeField] private AudioClip seClip_getGem;
    [SerializeField] private AudioClip seClip_gogogogo;
    [SerializeField] private AudioClip seClip_warp;
    [SerializeField] private AudioClip seClip_warpEnd;

    public void PlayJump()
    {
        audioSource.PlayOneShot(seClip_jump);
    }
    public void PlayCollision()
    {
        audioSource.PlayOneShot(seClip_collision);
    }
    public void PlayCharge()
    {
        audioSource.PlayOneShot(seClip_charge);
    }
    public void PlayGetItem()
    {
        audioSource.PlayOneShot(seClip_getGem);
    }
    public void PlayGOGOGOGO()
    {
        audioSource.PlayOneShot(seClip_gogogogo);
    }
    public void PlayWarp()
    {
        audioSource.PlayOneShot(seClip_warp);
    }
    public void PlayWarpEnd()
    {
        audioSource.PlayOneShot(seClip_warpEnd);
    }
    public void Stop()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume) {  audioSource.volume = volume; }
}
