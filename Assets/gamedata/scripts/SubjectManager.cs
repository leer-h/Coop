using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class SubjectManager : MonoBehaviourPun
{
    [SerializeField] private AskManager askManager;

    [SerializeField] private GameObject askCanvas;
    [SerializeField] private GameObject subjectCanvas;

    [SerializeField] private VoteVisualizer voteVisualizer;

    [SerializeField] private Button button_1;
    [SerializeField] private Button button_2;
    [SerializeField] private Button button_3;
    [SerializeField] private Button button_4;
    [SerializeField] private Button button_5;

    private string textButton1 = "1";
    private string textButton2 = "2";
    private string textButton3 = "3";
    private string textButton4 = "4";
    private string textButton5 = "5";

    private Dictionary<int, int> playerVotes = new Dictionary<int, int>(); // key = ActorNumber, value = choice (1-5)

    void Awake()
    {
        photonView.RPC(nameof(RpcSetTextToSubjects), RpcTarget.AllBuffered);

        button_1.onClick.AddListener(() => SendVote(1));
        button_2.onClick.AddListener(() => SendVote(2));
        button_3.onClick.AddListener(() => SendVote(3));
        button_4.onClick.AddListener(() => SendVote(4));
        button_5.onClick.AddListener(() => SendVote(5));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchToAsk();
        }
    }

    [PunRPC]
    private void RpcSetTextToSubjects()
    {
        button_1.GetComponentInChildren<Text>().text = textButton1;
        button_2.GetComponentInChildren<Text>().text = textButton2;
        button_3.GetComponentInChildren<Text>().text = textButton3;
        button_4.GetComponentInChildren<Text>().text = textButton4;
        button_5.GetComponentInChildren<Text>().text = textButton5;
    }

    private void SendVote(int choice)
    {
        photonView.RPC(nameof(RpcReceiveVote), RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, choice);
    }

    [PunRPC]
    private void RpcReceiveVote(int playerId, int choice)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerVotes[playerId] = choice;

            Dictionary<int, int> voteCounts = new Dictionary<int, int>();
            for (int i = 1; i <= 5; i++)
                voteCounts[i] = 0;
            foreach (var kvp in playerVotes)
                voteCounts[kvp.Value]++;

            photonView.RPC(nameof(RpcUpdateVoteVisualizer), RpcTarget.All, voteCounts);
        }
    }

    [PunRPC]
    private void RpcUpdateVoteVisualizer(Dictionary<int, int> voteCounts)
    {
        if (voteVisualizer != null)
            voteVisualizer.UpdateVoteCounts(voteCounts);
    }

    private void UpdateVoteUI()
    {
        Dictionary<int, int> voteCounts = new Dictionary<int, int>();
        for (int i = 1; i <= 5; i++)
            voteCounts[i] = 0;

        foreach (var kvp in playerVotes)
        {
            int choice = kvp.Value;
            voteCounts[choice]++;
        }

        string voteSummary = "Votes: ";
        for (int i = 1; i <= 5; i++)
        {
            voteSummary += $"{i}={voteCounts[i]} ";
        }
        Debug.Log(voteSummary);
    }

    private void SwitchToAsk()
    {
        askCanvas.SetActive(true);
        subjectCanvas.SetActive(false);
        this.enabled = false;
        askManager.enabled = true;
    }
}
