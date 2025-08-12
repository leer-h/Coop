using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;


public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField joinField;
    [SerializeField] private InputField nameField;
    [SerializeField] private Text notifBarCanvas;
    private NameHandler nameHandler;

    private void Start()
    {
        NameHandler handler = FindFirstObjectByType<NameHandler>();
        nameHandler = handler;
    }

    public void CreateRoom()
    {
        string nameText = nameField.text;
        string roomText = joinField.text;

        if (string.IsNullOrEmpty(nameText) || string.IsNullOrEmpty(roomText))
        {
            NotifMsg("Fill in all the fields");
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(joinField.text, roomOptions);
    }

    public void JoinRoom()
    {
        string nameText = nameField.text;
        string roomText = joinField.text;

        if (string.IsNullOrEmpty(nameText) || string.IsNullOrEmpty(roomText))
        {
            NotifMsg("Fill in all the fields");
            return;
        }

        PhotonNetwork.JoinRoom(joinField.text);
    }

    public override void OnJoinedRoom()
    {
        nameHandler.name = nameField.text;
        PhotonNetwork.LoadLevel("Lobby");
    }


    public void Quit()
    {
        Application.Quit();
    }

    private void NotifMsg(string msg)
    {
        notifBarCanvas.text = msg;
        StartCoroutine(nameof(ClearNotifBar));
    }

    IEnumerator ClearNotifBar()
    {
        yield return new WaitForSeconds(3);

        notifBarCanvas.text = "";
    }
}
