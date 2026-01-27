using UnityEngine;
using DiscordRPC;

public class DiscordManager : MonoBehaviour
{
    private DiscordRpcClient client;

    [SerializeField]
    private string applicationId = "1465484575936610449";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        client = new DiscordRpcClient(applicationId);
        client.Initialize();

        SetIdlePresence();
    }

    private void OnDestroy()
    {
        client?.Dispose();
    }

    // =====================
    // Presence ê›íË
    // =====================

    public void SetIdlePresence()
    {
        client.SetPresence(new RichPresence
        {
            Details = "Exploring the tower",
            State = "Preparing adventure",
            Assets = new Assets
            {
                LargeImageKey = "main_icon",
                LargeImageText = "Stand by..."
            }
        });
    }

    public void SetPlayingPresence(string stageName)
    {
        client.SetPresence(new RichPresence
        {
            Details = "Exploring the tower",
            State = stageName,
            Timestamps = Timestamps.Now,
            Assets = new Assets
            {
                LargeImageKey = "tower_icon",
                LargeImageText = stageName
            }
        });
    }
}
