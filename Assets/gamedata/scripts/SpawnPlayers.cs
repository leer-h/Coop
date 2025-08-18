using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            Spawn();
        }
    }

    private Vector3 SelectSpawnPoint()
    {
        int rand = Random.Range(0, spawnPoints.Length);
        return spawnPoints[rand].position;
    }

    private void Spawn()
    {
        var point = SelectSpawnPoint();
        PhotonNetwork.Instantiate("Characters/Player", point, Quaternion.identity);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel("MainMenu");
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }
}
