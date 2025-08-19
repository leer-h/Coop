using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField nameField;
    [SerializeField] private Text notifBarCanvas;
    [SerializeField] private Dropdown friendsDropdown;

    private List<CSteamID> steamFriends = new List<CSteamID>();

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            NotifMsg("Connecting to Photon...");
        }

        if (SteamAPI.Init())
        {
            Debug.Log("SteamAPI initialized successfully.");
            UpdateSteamFriends();
        }
        else
        {
            NotifMsg("SteamAPI initialization failed. Make sure Steam is running.");
        }
    }

    public override void OnConnectedToMaster()
    {
        NotifMsg("Connected to Photon.");
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        string playerName = string.IsNullOrEmpty(nameField.text) ? "Player" : nameField.text;
        PhotonNetwork.NickName = playerName;

        string roomName = "Room_" + Random.Range(1, 1000).ToString();
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        NotifMsg("Creating room: " + roomName);

            if (SteamAPI.IsSteamRunning())
            {
                SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
            }
    }

    public override void OnCreatedRoom()
    {
        NotifMsg("Room created: " + PhotonNetwork.CurrentRoom.Name);

        if (SteamAPI.IsSteamRunning() && PhotonNetwork.CurrentRoom != null)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(0);
            if (lobbyID.IsValid())
            {
                SteamMatchmaking.SetLobbyData(lobbyID, "photonRoomName", PhotonNetwork.CurrentRoom.Name);
            }
        }
    }

    private void UpdateSteamFriends()
    {
        steamFriends.Clear();
        friendsDropdown.ClearOptions();

        int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
        List<string> options = new List<string>();

        for (int i = 0; i < friendCount; i++)
        {
            CSteamID friendID = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);

            EPersonaState friendState = SteamFriends.GetFriendPersonaState(friendID);
            if (friendState == EPersonaState.k_EPersonaStateOnline)
            {
                string friendName = SteamFriends.GetFriendPersonaName(friendID);
                steamFriends.Add(friendID);
                options.Add(friendName);
            }
        }

        friendsDropdown.AddOptions(options);
    }

    public void InviteSelectedFriend()
    {
        int index = friendsDropdown.value;
        if (index < 0 || index >= steamFriends.Count)
            return;

        CSteamID friendID = steamFriends[index];
        SteamFriends.ActivateGameOverlayInviteDialog(friendID);
        NotifMsg("Invite sent to " + SteamFriends.GetFriendPersonaName(friendID));
    }

    public void JoinFriendRoom()
    {
        int index = friendsDropdown.value;
        if (index < 0 || index >= steamFriends.Count)
        {
            NotifMsg("No friend selected.");
            return;
        }

        string playerName = string.IsNullOrEmpty(nameField.text) ? "Player" : nameField.text;
        PhotonNetwork.NickName = playerName;

        CSteamID friendID = steamFriends[index];
        FriendGameInfo_t gameInfo;

        if (SteamFriends.GetFriendGamePlayed(friendID, out gameInfo))
        {
            if (gameInfo.m_gameID.AppID() == new AppId_t(480))
            {
                if (!gameInfo.m_steamIDLobby.IsValid())
                {
                    NotifMsg("Selected friend is not in a lobby.");
                    return;
                }

                string lobbyName = SteamMatchmaking.GetLobbyData(gameInfo.m_steamIDLobby, "photonRoomName");
                if (string.IsNullOrEmpty(lobbyName))
                {
                    NotifMsg("Could not find friend's room.");
                    return;
                }

                NotifMsg($"Joining {lobbyName}...");
                PhotonNetwork.JoinRoom(lobbyName);
            }
            else
            {
                NotifMsg("Selected friend is not playing this game.");
            }
        }
        else
        {
            NotifMsg("Selected friend is not in a game.");
        }
    }

    public override void OnJoinedRoom()
    {
        NotifMsg("Joined room. Loading lobby...");
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        NotifMsg($"Failed to join room: {message}");
    }

    private void NotifMsg(string msg)
    {
        if (notifBarCanvas != null)
        {
            notifBarCanvas.text = msg;
            StartCoroutine(ClearNotifBar());
        }
    }

    private IEnumerator ClearNotifBar()
    {
        yield return new WaitForSeconds(3);
        if (notifBarCanvas != null)
            notifBarCanvas.text = "";
    }
}