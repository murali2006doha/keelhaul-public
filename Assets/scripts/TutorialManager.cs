using UnityEngine;
using InControl;

using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour {
		// Update is called once per frame
	void Update () {

		if (InputManager.ActiveDevice.Action1.WasReleased || Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.A)) {
			loadMenu ();
		}

	}


	void loadMenu(){
		Application.LoadLevel ("start2");
	}
}
	