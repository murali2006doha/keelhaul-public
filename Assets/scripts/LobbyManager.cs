using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class LobbyManager : MonoBehaviour {

	public AbstractCharacterSelectController csc;

	// Update is called once per frame
	void Update () {

		csc = GameObject.FindObjectOfType<AbstractCharacterSelectController> ();
		if (csc != null) {
			csc.OnSelectCharacterAction (
				() => {
					csc.setPlayerSelectSettings ();

					if (!csc.loadingScene) {
						csc.loadingScreen.SetActive (true);  
						StartCoroutine (LoadNewScene ());
						csc.loadingScene = true;
					}
					;
				});
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
