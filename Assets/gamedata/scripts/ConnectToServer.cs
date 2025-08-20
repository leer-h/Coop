using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using Steamworks;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

        if (SteamAPI.RestartAppIfNecessary((AppId_t)480))
        {
            Application.Quit();
            return;
        }

        if (!SteamAPI.Init())
        {
            Debug.LogError("SteamAPI init failed");
            Application.Quit();
            return;
        }
    }

    public override void OnConnectedToMaster()
    {
        StartCoroutine(LoadMenu());
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(3f);

        SceneTransitionManager.instance.LoadSceneWithFade("MainMenu");
    }
}
