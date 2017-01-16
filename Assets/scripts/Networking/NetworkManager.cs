using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    [SerializeField]
    Text connectionText;
    public GameInitializer initializer;
    public bool offlineMode = false;
    void Start()
    {

        PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
        PhotonNetwork.ConnectUsingSettings("0.2");
        PhotonNetwork.offlineMode = offlineMode;

        if (offlineMode) {
            this.OnJoinedLobby();
        }
    }

    void Update()
    {
        if (connectionText) {
            connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();
        }
        
    }

    void OnJoinedLobby()
    {
        RoomOptions ro = new RoomOptions() { isVisible = true, maxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom("default", ro, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        if (PhotonNetwork.offlineMode)
        {
            initializer.isMaster = true;
            initializer.playerId = 1;
            initializer.enabled = true;
            initializer.gameObject.SetActive(true);
            initializer.onGameManagerCreated = this.onGameManagerCreated;
        }
        else {
            StartSpawnProcess();
        }
    
    }

    void StartSpawnProcess()
    {

        initializer.isMaster = true;
        initializer.playerId = PhotonNetwork.player.ID;
        initializer.onGameManagerCreated = onGameManagerCreated;
        initializer.enabled = true;
    }

    void onGameManagerCreated() {
        AbstractGameManager manager = FindObjectOfType<AbstractGameManager>();
        manager.enabled = true;
     //   manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.All, PhotonNetwork.player.ID);

    }
}