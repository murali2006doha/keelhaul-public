
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviour
{

  [SerializeField]
  Text connectionText;
  public GameObject NetworkedCharacterSelect;
  public MatchMakingController matchMaker;
  public GameInitializer initializer;
  public bool offlineMode = false;

  private MatchMakingController instantiatedController;
  void Start() {
        PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
        PhotonNetwork.offlineMode = offlineMode;

        if (offlineMode)
        {
            this.OnJoinedLobby();
        }
        else {
            PhotonNetwork.ConnectUsingSettings("0.2");
        }
    }


  void Update() {
    if (connectionText) {
  //    connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

  }

   void OnJoinedLobby() {


     instantiatedController = Instantiate(matchMaker);
    instantiatedController.Initiailze(this.FindOrCreateRoom);
   
  }

    void FindOrCreateRoom(Dictionary<int, bool> matchOptions) {

        string roomName = "";
        bool hasFoundRoom = false;

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

        if (hasFoundRoom)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else {
            RoomOptions ro = new RoomOptions() { isVisible = true, maxPlayers = 4 };
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h.Add("matchOptions", matchOptions);
            string [] options = new string[] {"matchOptions"};
            ro.customRoomPropertiesForLobby = options;
            ro.customRoomProperties = h;
            PhotonNetwork.CreateRoom(null, ro, TypedLobby.Default);
        }
        Destroy(instantiatedController);

    }
    

  void OnJoinedRoom() {
        connectionText.text = PhotonNetwork.room.name;
    if (PhotonNetwork.offlineMode)
    {
      initializer.isMaster = true;
      initializer.playerId = 1;
      initializer.enabled = true;
      initializer.gameObject.SetActive(true);
      initializer.onGameManagerCreated = this.onGameManagerCreated;
    }
    else {

      GameObject instantiated = Instantiate(NetworkedCharacterSelect);
      AbstractCharacterSelectController csc = instantiated.GetComponent<AbstractCharacterSelectController> ();
      csc.OnSelectCharacterAction(
        () => {
          csc.setPlayerSelectSettings ();         
            
          foreach(CharacterSelection cs in csc.getPlayerSelectSettings().players) {
            print(cs.selectedCharacter);
          }

          StartSpawnProcess(csc.getPlayerSelectSettings().players[0].selectedCharacter);
          print(csc.getPlayerSelectSettings().players[0].selectedCharacter);
          Destroy(instantiated);

      });
    }
  }

  void StartSpawnProcess(ShipEnum type) {
    initializer.shipSelections[0].selectedCharacter = type;
    initializer.isMaster = true;
    initializer.playerId = PhotonNetwork.player.ID;
    initializer.onGameManagerCreated = onGameManagerCreated;
    initializer.enabled = true;
  }

  void onGameManagerCreated() {
    AbstractGameManager manager = FindObjectOfType<AbstractGameManager>();
    manager.enabled = true;
  }
}