using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Dropdown roomDropdown;
    [SerializeField] private InputField nameField;
    [SerializeField] private Text notifBarCanvas;

    private List<RoomInfo> roomList = new List<RoomInfo>();

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            NotifMsg("Connecting to Photon...");
        }
        else if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            NotifMsg("Joining lobby...");
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        UpdateRoomDropdown();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        this.roomList = roomList;
        UpdateRoomDropdown();
    }

    private void UpdateRoomDropdown()
    {
        roomDropdown.ClearOptions();
        List<string> options = new List<string>();
        options.Add("Create New Room");
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList) continue;
            options.Add(room.Name);
        }
        roomDropdown.AddOptions(options);
    }

    public void CreateRoom()
    {
        string nameText = nameField.text;
        if (string.IsNullOrEmpty(nameText))
            nameText = "Player";

        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InLobby)
            return;

        PhotonNetwork.NickName = nameText;

        string selectedRoom = roomDropdown.options[roomDropdown.value].text;
        if (selectedRoom == "Create New Room")
        {
            string roomName = "Room_" + Random.Range(1, 1000).ToString();
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(roomName, roomOptions);
            NotifMsg("Creating room: " + roomName);
        }
        else
        {
            PhotonNetwork.JoinRoom(selectedRoom);
            NotifMsg("Joining room: " + selectedRoom);
        }
    }

    public void JoinSelectedRoom()
    {
        string nameText = nameField.text;
        if (string.IsNullOrEmpty(nameText))
            nameText = "Player";

        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InLobby)
        {
            NotifMsg("Not connected to lobby. Please wait...");
            return;
        }

        PhotonNetwork.NickName = nameText;

        string selectedRoom = roomDropdown.options[roomDropdown.value].text;
        if (selectedRoom != "Create New Room")
        {
            PhotonNetwork.JoinRoom(selectedRoom);
            NotifMsg("Joining room: " + selectedRoom);
        }
        else
        {
            NotifMsg("Please select a room or create a new one!");
        }
    }

    public override void OnJoinedRoom()
    {
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
