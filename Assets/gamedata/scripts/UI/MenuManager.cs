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
    private Callback<LobbyCreated_t> lobbyCreated;
    private Callback<LobbyEnter_t> lobbyEnter;
    private CSteamID currentLobbyID;

    void Start()
    {
        if (SteamAPI.Init())
        {
            Debug.Log("SteamAPI initialized successfully.");
            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
        }
        else
        {
            NotifMsg("SteamAPI initialization failed. Make sure Steam is running.");
        }

        if (SteamAPI.IsSteamRunning())
        {
            UpdateSteamFriends();
        }
    }

    public override void OnConnectedToMaster()
    {
        SetPlayerNickname();
        NotifMsg("Connected to Photon as " + PhotonNetwork.NickName);
        PhotonNetwork.JoinLobby();
    }

    private void SetPlayerNickname()
    {
        string steamName = "Player";
        if (SteamAPI.IsSteamRunning() && SteamAPI.Init())
        {
            steamName = SteamFriends.GetPersonaName();
            if (string.IsNullOrEmpty(steamName))
            {
                steamName = "Player_" + Random.Range(1000, 9999); 
            }
        }
        else
        {
            steamName = "Player_" + Random.Range(1000, 9999);
        }
        PhotonNetwork.NickName = steamName;
        Debug.Log($"Nickname set for local player: {PhotonNetwork.NickName}");
    }

    public void CreateRoom()
    {
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

        if (SteamAPI.IsSteamRunning() && currentLobbyID.IsValid())
        {
            SteamMatchmaking.SetLobbyData(currentLobbyID, "photonRoomName", PhotonNetwork.CurrentRoom.Name);
            Debug.Log("Set photonRoomName in Steam lobby: " + PhotonNetwork.CurrentRoom.Name);
        }
    }

    public override void OnJoinedRoom()
    {
        if (string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            SetPlayerNickname();
        }
        NotifMsg("Joined room. Loading lobby...");
        SceneTransitionManager.instance.LoadPhotonSceneWithFade("Lobby");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        NotifMsg($"Failed to join room: {message}");
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult == EResult.k_EResultOK)
        {
            currentLobbyID = new CSteamID(callback.m_ulSteamIDLobby);
            Debug.Log("Steam lobby created with ID: " + currentLobbyID);
        }
        else
        {
            Debug.LogError("Failed to create Steam lobby: " + callback.m_eResult);
        }
    }

    private void OnLobbyEnter(LobbyEnter_t callback)
    {
        CSteamID lobbyID = new CSteamID(callback.m_ulSteamIDLobby);
        Debug.Log("Entered Steam lobby: " + lobbyID);

        string photonRoom = SteamMatchmaking.GetLobbyData(lobbyID, "photonRoomName");
        if (!string.IsNullOrEmpty(photonRoom))
        {
            NotifMsg("Joining Photon room: " + photonRoom);
            PhotonNetwork.JoinRoom(photonRoom);
        }
        else
        {
            NotifMsg("Lobby has no Photon room data.");
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

        CSteamID friendID = steamFriends[index];
        FriendGameInfo_t gameInfo;

        if (SteamFriends.GetFriendGamePlayed(friendID, out gameInfo))
        {
            if (gameInfo.m_gameID.AppID() == SteamUtils.GetAppID())
            {
                if (!gameInfo.m_steamIDLobby.IsValid())
                {
                    NotifMsg("Selected friend is not in a lobby.");
                    return;
                }

                SteamMatchmaking.JoinLobby(gameInfo.m_steamIDLobby);
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

    public void ExitGame()
    {
        Application.Quit();
    }
}