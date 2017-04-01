using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class SettingsMenu : AbstractMenu {


	public ActionToggle shadowsToggle;
	public ActionToggle waterToggle;
	public ActionSlider soundSlider;
	public ActionSlider musicSlider;

	// Use this for initialization
	void Start () {

		actionSelectables.Add (shadowsToggle.gameObject);
		actionSelectables.Add (waterToggle.gameObject);
		actionSelectables.Add (soundSlider.gameObject);
		actionSelectables.Add (musicSlider.gameObject);

		//setButtonsToActions ();
	}


	public override void Navigate() {
		NavigateModal (actionSelectables.ToArray ());
		NavigateModalWithMouse ();
	}


	public override void SetActions () {
		soundSlider.SetAction (this.setSoundVolume);
	}

	void setSoundVolume(float multiplier) {
		//GlobalVariables.soundMultiplier = multiplier;
	}
			
}

