using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class SettingsMenu : AbstractMenu {


    public ActionToggle shadowsToggle;
    public ActionToggle waterRefractToggle;
    public ActionToggle waterReflectToggle;
    public ActionSlider soundSlider;
    public ActionSlider musicSlider;


	protected override void SetActions () {
        shadowsToggle.SetAction (this.setShadowsToggle);
        waterRefractToggle.SetAction (this.setWaterRefractionToggle);
        waterReflectToggle.SetAction (this.setWaterReflectionToggle);
        soundSlider.SetAction (this.setSoundVolume, this.actions);
        musicSlider.SetAction (this.setMusicVolume, this.actions);
    }

	protected override void SetActionSelectables ()
	{
		this.shadowsToggle.ToggleComponent.isOn = GlobalSettings.shadows;
		this.waterReflectToggle.ToggleComponent.isOn = GlobalSettings.waterReflection;
		this.waterRefractToggle.ToggleComponent.isOn = GlobalSettings.waterRefraction;
		this.soundSlider.SliderComponent.value = GlobalSettings.soundMultiplier;
		this.musicSlider.SliderComponent.value = GlobalSettings.musicMultiplier;
		actionSelectables.Add (shadowsToggle.gameObject);
		actionSelectables.Add (waterRefractToggle.gameObject);
		actionSelectables.Add (waterReflectToggle.gameObject);
		actionSelectables.Add (soundSlider.gameObject);
		actionSelectables.Add (musicSlider.gameObject);
	}



    void setSoundVolume(float multiplier) {
        GlobalSettings.setSoundMultiplier (multiplier);
    }

    void setMusicVolume(float multiplier) {
        GlobalSettings.setMusicMuliplier (multiplier);
    }

    void setShadowsToggle(bool isOn) {
        GlobalSettings.setShadows (isOn);
    }

    void setWaterRefractionToggle(bool isOn) {
        GlobalSettings.setRefraction (isOn);
    }

    void setWaterReflectionToggle(bool isOn) {
        GlobalSettings.setReflection (isOn);
    }
}

