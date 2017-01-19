using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;

public class DeathMatchCharacterSelectController : AbstractCharacterSelectController {


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
	void Update () {

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

					if (!loadingScene)
					{
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

		playersInPlay--;

		return true;

	}


	public override bool unlockCharacter(int index) {

		playersInPlay++;

		return true;	
	}


	IEnumerator LoadNewScene() {
		//To do: move this logic out of playerSignIn, make it more generic
		AsyncOperation async = SceneManager.LoadSceneAsync(GlobalVariables.getMapToLoad());
		while (!async.isDone) {
			yield return null;
		}

	}


}
