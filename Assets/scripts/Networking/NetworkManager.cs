using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

  [SerializeField]
  Text connectionText;
  public GameObject NetworkedCharacterSelect;
  public GameInitializer initializer;
  public bool offlineMode = false;
  
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
      connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

  }

   void OnJoinedLobby() {
    RoomOptions ro = new RoomOptions() { isVisible = true, maxPlayers = 4 };
    PhotonNetwork.JoinOrCreateRoom("default", ro, TypedLobby.Default);
  }

  void OnJoinedRoom() {
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
    Debug.Log(type);
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