using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;


/*
 * Handles the player sign in and character selection for all the players
 */ 
public class OneVsOnePlayerSignIn : PlayerSignIn {  

	// Use this for initialization
	GameObject TCTF;

	void Start () {
		//GameObject.DontDestroyOnLoad (this.gameObject);
		TCTF = GameObject.Find("characterSelect1v1");
		cc = GameObject.FindObjectOfType<ControllerSelect> ();

		cc.withKeyboard = withKeyboard;

		cc.listening = false;

		player1.setIndex(0);
		player2.setIndex (0);

	}


	void Update(){ //need to make controllers work in menu
		var inputDevice = InputManager.ActiveDevice; //last device to

		if (TCTF != null && TCTF.activeSelf) {
			signIn ();
			cc.listening = true;
		}


	}


	void signIn() {
		var inputDevice = InputManager.ActiveDevice;

		if (cc.players.Count == 0) {


		} else if (cc.players.Count == 1) {
			player1.Actions = (PlayerActions)cc.players [0];
			if (player1.Actions.Green.WasReleased) {
				player1.isSignedIn = true;
			}

		} else if (cc.players.Count == 2) {
			player2.Actions = (PlayerActions)cc.players [1];
			if (player2.Actions.Green.WasReleased) {
				player2.isSignedIn = true;
			}
		}
	}
		


}
