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


	void Start() {
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;
	}




}
