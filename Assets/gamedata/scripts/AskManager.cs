using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class AskManager : MonoBehaviourPun
{
    [SerializeField] private GameObject askCanvas;
    [SerializeField] private GameObject subjectCanvas;

    [SerializeField] private Text askText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private VoteVisualizer voteVisualizer;
    [SerializeField] private VoteHandler voteHandler;

    private string textAsk = "У нас було два пакети трави, сімдесят п'ять пігулок мескаліну,\n" +
                             "п'ять листків з марками сильнодіючої кислоти й безліч усякого штибу\n" +
                             "різнокольорових стимуляторів, транків, метамфетамінів та екстазі,\n" +
                             "а ще кварта текіли, кварта рому, ящик Budweiser'а, пінта чистого ефіру\n" +
                             "й два десятки амілонітритів.";

    private string[] answerTexts = {
        "1",
        "2",
        "3",
        "4"
    };

    private void Awake()
    {
        photonView.RPC(nameof(RpcSetTextTo), RpcTarget.AllBuffered);

        voteHandler.Init(answerButtons.Length);
        voteHandler.OnVotesUpdated += voteVisualizer.UpdateVoteCounts;
        voteHandler.OnAllVoted += SwitchToSubject;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int choice = i + 1;
            answerButtons[i].onClick.AddListener(() => voteHandler.SendVote(choice));
        }
    }

    [PunRPC]
    private void RpcSetTextTo()
    {
        askText.text = textAsk;

        for (int i = 0; i < answerButtons.Length && i < answerTexts.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = answerTexts[i];
        }
    }

    private void SwitchToSubject()
    {
        Debug.Log("ask");

        subjectCanvas.SetActive(true);
        askCanvas.SetActive(false);

        voteHandler.ResetVotes();
    }
}
