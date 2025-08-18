using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SubjectManager : MonoBehaviourPun
{
    [SerializeField] private GameObject askCanvas;
    [SerializeField] private GameObject subjectCanvas;

    [SerializeField] private VoteVisualizer voteVisualizer;
    [SerializeField] private VoteHandler voteHandler;

    [SerializeField] private Button[] subjectButtons;

    private string[] subjectButtonTexts = {
        "1",
        "2",
        "3",
        "4",
        "5" 
    };

    void Awake()
    {
        photonView.RPC(nameof(RpcSetTextToSubjects), RpcTarget.AllBuffered);

        voteHandler.Init(subjectButtons.Length);
        voteHandler.OnVotesUpdated += voteVisualizer.UpdateVoteCounts;
        voteHandler.OnAllVoted += SwitchToAsk;

        for (int i = 0; i < subjectButtons.Length; i++)
        {
            int choice = i + 1;
            subjectButtons[i].onClick.AddListener(() => voteHandler.SendVote(choice));
        }
    }

    [PunRPC]
    private void RpcSetTextToSubjects()
    {
        for (int i = 0; i < subjectButtons.Length && i < subjectButtonTexts.Length; i++)
        {
            subjectButtons[i].GetComponentInChildren<Text>().text = subjectButtonTexts[i];
        }
    }

    private void SwitchToAsk()
    {
        Debug.Log("sub");
        askCanvas.SetActive(true);
        subjectCanvas.SetActive(false);

        voteHandler.ResetVotes();
    }
}
