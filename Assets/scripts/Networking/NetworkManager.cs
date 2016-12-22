using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    [SerializeField]
    Text connectionText;
    public GameInitializer initializer;

    void Start()
    {

        PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
        PhotonNetwork.ConnectUsingSettings("0.2");

    }

    void Update()
    {
        connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    void OnJoinedLobby()
    {
        RoomOptions ro = new RoomOptions() { isVisible = true, maxPlayers = 10 };
        PhotonNetwork.JoinOrCreateRoom("Mike", ro, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        StartSpawnProcess(0f);
    }

    void StartSpawnProcess(float respawnTime)
    {

        initializer.isMaster = true;
        initializer.playerId = PhotonNetwork.player.ID;
        initializer.onGameManagerCreated = onGameManagerCreated;
        initializer.gameObject.SetActive(true);
    }

    void onGameManagerCreated() {
        AbstractGameManager manager = FindObjectOfType<AbstractGameManager>();
        manager.enabled = true;
     //   manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.All, PhotonNetwork.player.ID);

    }
}