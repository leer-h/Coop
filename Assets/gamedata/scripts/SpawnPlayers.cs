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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.Disconnect();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
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
