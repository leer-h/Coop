using System;
using System.Collections.Generic;
using Photon.Pun;

public class VoteHandler : MonoBehaviourPun
{
    public int OptionCount { get; private set; }
    public Dictionary<int, int> PlayerVotes { get; private set; } = new Dictionary<int, int>();

    public event Action<Dictionary<int, int>> OnVotesUpdated;
    public event Action OnAllVoted;

    public void Init(int optionCount)
    {
        OptionCount = optionCount;
        PlayerVotes.Clear();
    }

    public void SendVote(int choice)
    {
        photonView.RPC(nameof(RpcReceiveVote), RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber, choice);
    }

    [PunRPC]
    private void RpcReceiveVote(int playerId, int choice)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerVotes[playerId] = choice;

            var voteCounts = new Dictionary<int, int>();
            for (int i = 1; i <= OptionCount; i++)
                voteCounts[i] = 0;

            foreach (var kvp in PlayerVotes)
                voteCounts[kvp.Value]++;

            photonView.RPC(nameof(RpcUpdateVotes), RpcTarget.All, SerializeVoteCounts(voteCounts));

            if (PlayerVotes.Count >= PhotonNetwork.CurrentRoom.PlayerCount)
            {
                photonView.RPC(nameof(RpcAllVoted), RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void RpcUpdateVotes(int[] serializedVoteCounts)
    {
        var voteCounts = DeserializeVoteCounts(serializedVoteCounts);
        OnVotesUpdated?.Invoke(voteCounts);
    }

    private int[] SerializeVoteCounts(Dictionary<int, int> votes)
    {
        int[] arr = new int[OptionCount];
        for (int i = 0; i < OptionCount; i++)
            arr[i] = votes[i + 1];
        return arr;
    }

    private Dictionary<int, int> DeserializeVoteCounts(int[] arr)
    {
        var dict = new Dictionary<int, int>();
        for (int i = 0; i < arr.Length; i++)
            dict[i + 1] = arr[i];
        return dict;
    }


    [PunRPC]
    private void RpcAllVoted()
    {
        OnAllVoted?.Invoke();
    }

    public void ResetVotes()
    {
        PlayerVotes.Clear();

        var emptyVoteCounts = new Dictionary<int, int>();
        for (int i = 1; i <= OptionCount; i++)
            emptyVoteCounts[i] = 0;

        OnVotesUpdated?.Invoke(emptyVoteCounts);
    }
}
