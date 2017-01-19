using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;

public class PlunderCharacterSelectController : AbstractCharacterSelectController {


	// Use this for initialization
	void Start () {
		
		//GameObject.DontDestroyOnLoad (this.gameObject);
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;

		for(int i = 0; i < numPlayers; i++) {
			initializePanel ();
		}	
			
		foreach (ShipEnum character in characters) {
			characterStatuses.Add (character.ToString(), false);
		}
			
		playersInPlay = players.Count;
	}


	// Update is called once per frame
	public void Update () {

		var inputDevice = InputManager.ActiveDevice; //last device to

		if (this.gameObject.activeSelf) {
			cc.listening = true;
			signIn ();
		} else {
			cc.listening = false;
		}

		if (this.gameObject != null) {

			if (playersInPlay == 0) {

				start.gameObject.SetActive (true); //change to next when there is map selection
				if (!started && players.Exists(p => p.Actions.Green.IsPressed)) {
					started = true;
					GameObject.FindObjectOfType<PlayerSelectSettings> ().setPlayerCharacters (players);

					if (!loadingScene) {
						loadingScreen.SetActive(true);  
						StartCoroutine(LoadNewScene());
						loadingScene = true;
					}
				}
			} else {
				start.gameObject.SetActive (false);
			}
		}
	}





	public override bool lockCharacter(int index) {
		if (!characterStatuses [getCharacterKeys() [index]]) {

			//character is locked
			characterStatuses [getCharacterKeys() [index]] = true;
			playersInPlay--;

			//lock character for all players
			if (characterStatuses.ContainsKey (ShipEnum.Kraken.ToString ())) {
				isolateKraken (index);
			}

			return true;
		}

		return false;
	}

	public override bool unlockCharacter(int index) {

		if (characterStatuses [getCharacterKeys() [index]]) {

			//character is unlocked
			characterStatuses [getCharacterKeys() [index]] = false;
			playersInPlay++;

			//unlock character for all players
			if (characterStatuses.ContainsKey (ShipEnum.Kraken.ToString ())) {
				deIsolateKraken (index);
				isolateKraken (index);
			}

			return true;
		}

		return false;	
	}


	/*
	 * When a player has chosen a character, remove that character from all the other player lists
	 */
	private void isolateKraken(int index) { 
		if (characterStatuses.ContainsKey (ShipEnum.Kraken.ToString ())) {
			if (playersInPlay == 1 && !characterStatuses [ShipEnum.Kraken.ToString ()]) {
				foreach (string character in getCharacterKeys()) {
					if (!characterStatuses [character] && character != ShipEnum.Kraken.ToString ()) {
						characterStatuses [character] = true;
					}
				}
			} 
		}
	}


	private void deIsolateKraken(int index) {
		List<string> charactersSelected = new List<string>();
		if (playersInPlay > 1 && !characterStatuses [ShipEnum.Kraken.ToString ()]) {
			foreach (CharacterSelectPanel player in players) {
				if (player.selectedCharacter != "") {
					charactersSelected.Add (player.selectedCharacter);
				}
			}

			foreach (string character in getCharacterKeys()) {
				if (characterStatuses [character]) {
					if (!charactersSelected.Contains (character)) {
						characterStatuses [character] = false;
					}
				}
			}
		}
	}





	IEnumerator LoadNewScene() {
		//To do: move this logic out of playerSignIn, make it more generic
		AsyncOperation async = SceneManager.LoadSceneAsync(GlobalVariables.getMapToLoad());
		while (!async.isDone) {
			yield return null;
		}

	}


}
