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

		players_in_play.Add (player1, false);
		players_in_play.Add (player2, false);
		characters.Add ("atlantean", false);
		characters.Add ("blackbeard", false);
		characters.Add ("chinese", false);
		players.Add (player1); 
		players.Add (player2);

		cc.listening = false;

		player1.setIndex(0);
		player2.setIndex (0);

		pressAtoJoin1.SetActive (true);
		pressAtoJoin2.SetActive (true);
	}


	void Update(){ //need to make controllers work in menu
		var inputDevice = InputManager.ActiveDevice; //last device to

		if (TCTF != null && TCTF.activeSelf) {
			signIn ();
			selectCharacter ();
			unSelectCharacter ();
			addLocks ();
			cc.listening = true;
		}

		if (TCTF != null) {

			if (players.TrueForAll(p => players_in_play [p])) {

				start.gameObject.SetActive (true);

				if (players.Exists(p => p.Actions.Green.WasPressed)) {
					
					GameObject.FindObjectOfType<PlayerSelectSettings> ().setPlayerCharacters (players);
					this.gameObject.SetActive (false);
					mapSelect.gameObject.SetActive (true);
				}

			} else {
				start.gameObject.SetActive (false);
			}
		}
	}


	void signIn() {
		var inputDevice = InputManager.ActiveDevice;

		if (cc.players.Count == 0) {
			if (pressAtoJoin1 != null) {
				pressAtoJoin1.SetActive (true);
			}

		} else if (cc.players.Count == 1) {
			player1.Actions = (PlayerActions)cc.players [0];
			if (player1.Actions.Green.WasReleased) {
				pressAtoJoin1.SetActive (false);
				player1.isSignedIn = true;
			}

		} else if (cc.players.Count == 2) {
			player2.Actions = (PlayerActions)cc.players [1];
			if (player2.Actions.Green.WasReleased) {
				pressAtoJoin2.SetActive (false);
				player2.isSignedIn = true;
			}
		}
	}
		


}
