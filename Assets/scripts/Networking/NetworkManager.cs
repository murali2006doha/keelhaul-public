
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public GameObject NetworkedCharacterSelect;
    public GameObject OfflineCharacterSelect;
    public MatchMakingController matchMaker;
    public GameInitializer initializer;
    public bool offlineMode = false;

    private MatchMakingController instantiatedController;
    private Dictionary<int, bool> selectedMatchOptions;
    private AbstractGameManager instantiatedManager;
    private GameModeSelectSettings gs;
    
    public bool connected = false;

    [SerializeField]
    private Camera camera;
    private Dictionary<int, bool> matchOptions;
    private float timeoutTime;
    private int randomTimeOutSeconds;
    private bool hasFoundRoom;

   

    void Start() {

        if (FindObjectOfType<GameModeSelectSettings> ()) {
            offlineMode = !FindObjectOfType<GameModeSelectSettings> ().isOnline();
        }

        PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
        PhotonNetwork.offlineMode = offlineMode;

        if (offlineMode)
        {
            RoomOptions ro = new RoomOptions() { isVisible = true, maxPlayers = 4 };
            PhotonNetwork.JoinOrCreateRoom("localRoom", ro, TypedLobby.Default);
        }
        else {
            PhotonNetwork.ConnectUsingSettings("0.2");
        }

        

            
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        
        if (!instantiatedManager.isGameOver())
        {
            
            if (PhotonNetwork.room.playerCount == 1)
            {
                PlayerInput currentPlayer = null;
                List<PlayerInput> players = instantiatedManager.getPlayers();
                foreach(PlayerInput player in players)
                {
                    if (player.GetId() == PhotonNetwork.player.ID)
                    {
                        currentPlayer = player;
                    }
                }
                Dictionary<ModalActionEnum, Action> modalActions = new Dictionary<ModalActionEnum, Action>();
                modalActions.Add(ModalActionEnum.onOpenAction, () => { currentPlayer.clearShipInput(); });
                modalActions.Add(ModalActionEnum.onCloseAction, () => { PhotonNetwork.LeaveRoom(); SceneManager.LoadScene(0, LoadSceneMode.Single); });
                ModalStack.initialize(currentPlayer.Actions, ModalsEnum.disconnectModal, modalActions);
                FindObjectOfType<NotificationModal>().Spawn("No players left. Disconnecting.", Color.yellow, "Ok", "Ok2",
                () => {
                    PhotonNetwork.LeaveRoom(); SceneManager.LoadScene(0, LoadSceneMode.Single);
                },
                () => {
                    PhotonNetwork.LeaveRoom(); SceneManager.LoadScene(0, LoadSceneMode.Single);
                });
       
        }
        }
        
    }

    void OnJoinedLobby() {

        if (!offlineMode) {
            instantiatedController = Instantiate(matchMaker);
            instantiatedController.GetComponent<Canvas>().worldCamera = this.camera;
          
            instantiatedController.Initiailze(this.GetMatchOptions);
            
            
        }
     
   
    }

    void GetMatchOptions(Dictionary<int, bool> matchOptions)
    {
        this.matchOptions = matchOptions;
        Destroy(instantiatedController.gameObject);
        this.timeoutTime = Time.realtimeSinceStartup;
        this.randomTimeOutSeconds = UnityEngine.Random.Range(2, 10 + PhotonNetwork.countOfPlayersInRooms / 2);
        this.hasFoundRoom = false;
        StartCoroutine("FindOrCreateRoom");
    }

    IEnumerator FindOrCreateRoom() {
        
        string roomName = "";
       
        this.selectedMatchOptions = matchOptions;
        while (!hasFoundRoom && (Time.realtimeSinceStartup - timeoutTime) < randomTimeOutSeconds) { 
            foreach (RoomInfo info in PhotonNetwork.GetRoomList()) {
               
                Dictionary<int, bool> roomOptions = (Dictionary<int, bool>)info.customProperties["matchOptions"];
                for (int i = 0; i < 3; i++) {

                    if (matchOptions.ContainsKey(i) && roomOptions.ContainsKey(i) && !hasFoundRoom) {

                        if (matchOptions[i] && roomOptions[i]) {
                            hasFoundRoom = true;
                            roomName = info.name;
                        }
                    }
                }
            }
            yield return null;
        }

        if (hasFoundRoom)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else {
            RoomOptions ro = new RoomOptions() { isVisible = true, maxPlayers = 4};
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("matchOptions", matchOptions);
            h.Add("map", MapTypeHelper.GetRandomMap());
            string [] options = new string[] {"matchOptions","map"};
            ro.customRoomPropertiesForLobby = options;
            ro.customRoomProperties = h;
            PhotonNetwork.CreateRoom(null, ro, TypedLobby.Default);
            ro.IsOpen = false;
        }
       
        yield return true;
    }
    

    void OnJoinedRoom() {
        if (PhotonNetwork.offlineMode) {

            GameObject instantiated = Instantiate (OfflineCharacterSelect);
            AbstractCharacterSelectController csc = null;

            if (FindObjectOfType<GameModeSelectSettings> () != null) {
                if (FindObjectOfType<GameModeSelectSettings> ().getGameType () == GameTypeEnum.DeathMatch) {
                    csc = instantiated.GetComponentInChildren<DeathMatchCharacterSelectController> ();
                } else if (FindObjectOfType<GameModeSelectSettings> ().getGameType () == GameTypeEnum.Sabotage) {
                    csc = instantiated.GetComponentInChildren<PlunderCharacterSelectController> ();
                }
            } else {
                if (this.gameObject.GetComponent<GameInitializer> ().gameType == GameTypeEnum.DeathMatch) {
                    csc = instantiated.GetComponentInChildren<DeathMatchCharacterSelectController> ();
                } else if (this.gameObject.GetComponent<GameInitializer> ().gameType == GameTypeEnum.Sabotage) {
                    csc = instantiated.GetComponentInChildren<PlunderCharacterSelectController> ();
                }

                csc.numPlayers = this.gameObject.GetComponent<GameInitializer> ().shipSelections.Count;
            }

            csc.enabled = true;
            csc.gameObject.GetComponent<Canvas> ().worldCamera = this.camera;
        
            if (csc != null) {
                csc.OnSelectCharacterAction (
                    () => {
                        csc.setPlayerSelectSettings ();

                        if (!csc.loadingScene) {
                            csc.loadingScreen.SetActive (true);  
                            csc.loadingScene = true;
                        };

                        StartSpawnProcessOffline ();
                        Destroy (instantiated);
                        FindObjectOfType<PlayerSelectSettings>().transform.parent = null;
                        Destroy (this.camera);
                    });
            }
                
        } else {

            Dictionary<int, bool> roomOptions = (Dictionary<int, bool>)PhotonNetwork.room.customProperties["matchOptions"];
            this.RefitRoomOptions(this.selectedMatchOptions, roomOptions);
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("matchOptions", roomOptions);
            PhotonNetwork.room.SetCustomProperties(h);
            MapEnum mapType = (MapEnum)PhotonNetwork.room.customProperties["map"];
            GameObject instantiated = Instantiate(NetworkedCharacterSelect);
            this.GetComponent<PhotonView>().RPC("ResetLowestRequiredPlayers",PhotonTargets.MasterClient);
            AbstractCharacterSelectController csc = instantiated.GetComponent<AbstractCharacterSelectController> ();
            csc.gameObject.GetComponent<Canvas> ().worldCamera = this.camera;
              
            csc.OnSelectCharacterAction(
                () => {

                    csc.setPlayerSelectSettings ();
                    StartSpawnProcessOnline(csc.getPlayerSelectSettings().players[0].selectedCharacter, mapType);
                    if (PhotonNetwork.isMasterClient)
                    {
                        PhotonNetwork.room.open = true;
                    }
                   
                    FindObjectOfType<PlayerSelectSettings>().transform.parent = null;
                    Destroy(this.camera);
                    csc.gameObject.SetActive(false);
            });
        }
    }

    void StartSpawnProcessOnline(ShipEnum type, MapEnum map) {
        initializer.shipSelections[0].selectedCharacter = type;
        initializer.isMaster = true;
        initializer.map = map;
        initializer.playerId = PhotonNetwork.player.ID;
        initializer.onGameManagerCreated = onGameManagerCreated;
        initializer.enabled = true;
    }

    void StartSpawnProcessOffline ()
    {
        initializer.isMaster = true;
        initializer.playerId = 1;
        initializer.enabled = true;
        initializer.gameObject.SetActive (true);
        initializer.onGameManagerCreated = this.onGameManagerCreated;
    }

    void onGameManagerCreated() {
        instantiatedManager = FindObjectOfType<AbstractGameManager>();
        instantiatedManager.enabled = true;
        instantiatedManager.minPlayersRequiredToStartGame = this.GetLowestRequiredPlayersToStartGame();
    }


    [PunRPC]
    void ResetLowestRequiredPlayers() {
        if (instantiatedManager) {
            instantiatedManager.minPlayersRequiredToStartGame = this.GetLowestRequiredPlayersToStartGame();
        }
    }


    int GetLowestRequiredPlayersToStartGame() {
        int lowestRequiredPlayers = 0;
        if (PhotonNetwork.offlineMode)
        {
            return 2;
        }
        Dictionary<int, bool> roomOptions = (Dictionary<int, bool>)PhotonNetwork.room.customProperties["matchOptions"];
        for (int i = 0; i < 3; i++) {
            if (roomOptions.ContainsKey(i)) {
                if (roomOptions[i]) {
                    
                    lowestRequiredPlayers = i;
                    break;
                }
            }
        }
  
        return lowestRequiredPlayers + 2;
    }


    void RefitRoomOptions(Dictionary<int, bool> playerOptions, Dictionary<int, bool> roomOptions) {
        for (int i = 0; i < 3; i++)
        {
            if (playerOptions.ContainsKey(i))
            {
                roomOptions[i] = playerOptions[i];
            }
            else if (roomOptions.ContainsKey(i)) {
                roomOptions[i] = false;
            }
        }
    }
}