using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class SettingsModalComponent : AbstractModalComponent {

	public Button graphics;
	public Button controls;
	public Button audios;

	void Update() {
		if (isActive) {
			Control ();  
		}
	}


	// Use this for initialization
	public override void InitializeModal (PlayerActions actions) {
		this.actions = actions;
		this.buttons = new List<Button> (buttonToAction.Keys).ToArray ();

		buttonToAction.Add (graphics, () =>  {print("graphics");});
		buttonToAction.Add (audios, () => {	print("audios");});
		buttonToAction.Add (controls, () => {	print("controls");});

		this.buttons = new List<Button> (buttonToAction.Keys).ToArray ();
		this.index = 0;
	}

}
