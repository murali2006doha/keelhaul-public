﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
public class SettingsMenu : AbstractMenu {


    public ActionToggle shadowsToggle;
	public ActionToggle waterRefractToggle;
	public ActionToggle waterReflectToggle;
    public ActionSlider soundSlider;
    public ActionSlider musicSlider;

    // Use this for initialization
    void Start () {

        actionSelectables.Add (shadowsToggle.gameObject);
		actionSelectables.Add (waterRefractToggle.gameObject);
		actionSelectables.Add (waterReflectToggle.gameObject);
        actionSelectables.Add (soundSlider.gameObject);
        actionSelectables.Add (musicSlider.gameObject);

		SetActions ();
    }


    public override void Navigate() {
        NavigateModal (actionSelectables.ToArray ());
        NavigateModalWithMouse ();
    }


    public override void SetActions () {
		shadowsToggle.SetAction (this.setShadowsToggle);
		waterRefractToggle.SetAction (this.setWaterRefractionToggle);
		waterReflectToggle.SetAction (this.setWaterReflectionToggle);
		soundSlider.SetAction (this.setSoundVolume);
		musicSlider.SetAction (this.setMusicVolume);
    }

    void setSoundVolume(float multiplier) {
		GlobalSettings.soundMultiplier = multiplier;
    }

	void setMusicVolume(float multiplier) {
		GlobalSettings.musicMultiplier = multiplier;
	}

	void setShadowsToggle(bool isOn) {
		if (isOn) {
			QualitySettings.shadows = ShadowQuality.All;
		} else {
			QualitySettings.shadows = ShadowQuality.Disable;
		}
	}

	void setWaterRefractionToggle(bool isOn) {
		GlobalSettings.waterRefraction = isOn;
	}

	void setWaterReflectionToggle(bool isOn) {
		GlobalSettings.waterReflection = isOn;
	}
}

