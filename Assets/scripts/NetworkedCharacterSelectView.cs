using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
//using System.Object;

public class NetworkedCharacterSelectView : MonoBehaviour {

	public PlayerActions Actions { get; set; }
	public ControllerSelect cc;
	public NetworkManager nm;
	GameObject csPanel;
	public bool withKeyboard;
    [SerializeField] NetworkedOpponentCharacterListView listView;

    void Start() {
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;
	}


    [PunRPC]
    public void AddNetworkedPlayer(string playerId) {
        listView.AddCharacterView(playerId);
    }

    [PunRPC]
    public void SetCharacterForNetworkedPlayer(string type, string playerId)
    {
        listView.RefreshCharacterView(type, playerId);
    }

}
