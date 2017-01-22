using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
//using System.Object;

public class NetworkedCharacterSelectView : MonoBehaviour {

	public PlayerActions Actions { get; set; }
    public NetworkedCharacterSelectButton [] buttons;
	private List<ShipEnum> characters = new List<ShipEnum> ();
	public ControllerSelect cc;
	public NetworkManager nm;
	GameObject csPanel;
	public bool withKeyboard;

	//new CharacterSelection(player.selectedCharacter,player.Actions);

    // Use this for initialization
//   	public void Initialize(Action<ShipEnum> onClick) {
//        foreach (NetworkedCharacterSelectButton button in buttons) {
//            button.onClickAction = onClick;
//      }
//
//    }


	void Start() {
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;
	}




}
