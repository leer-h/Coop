using UnityEngine;
using DiscordRPC;

public class DiscordController : MonoBehaviour
{
    private DiscordRpcClient client;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        client = new DiscordRpcClient("1406739818733637783");
        client.Initialize();

        client.SetPresence(new RichPresence()
        {
            Details = "У грі",
            State = "Шото пілю"
        });
    }

    void Update()
    {
        if (client != null) 
            client.Invoke();
    }

    void OnApplicationQuit()
    {
        if (client != null)
            client.Dispose();
    }
}
