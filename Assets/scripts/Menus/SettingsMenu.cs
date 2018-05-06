using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class SettingsMenu : AbstractMenu
{


	public ActionToggle shadowsToggle;
	public ActionToggle waterRefractToggle;
	public ActionToggle waterReflectToggle;
	public ActionSlider musicSlider;
	public ActionSlider soundSlider;
	public ActionButton keyboardControls;
    public ActionButton controllerControls;
    public AudioMixer audioMixer;


	protected override void SetActions() {
		shadowsToggle.SetAction(this.setShadowsToggle);
		waterRefractToggle.SetAction(this.setWaterRefractionToggle);
		waterReflectToggle.SetAction(this.setWaterReflectionToggle);
		soundSlider.SetAction(this.setSoundVolume, this.actions);
		musicSlider.SetAction(this.setMusicVolume, this.actions);
		
        keyboardControls.SetAction(() => {
            this.enabled = false;
            ToggleSelectables();
            FindObjectOfType<MenuModel>().inputBindingsMenu.Initialize(this.actions, () => {
              ToggleSelectables();
              this.enabled = true;
            });
        });

        controllerControls.SetAction(() => {
            this.enabled = false;
            ToggleSelectables();
            FindObjectOfType<MenuModel>().controllerLayoutMenu.Initialize(this.actions, () => {
              ToggleSelectables();
              this.enabled = true; 
            });
        });
	}

	protected override void SetActionSelectables() {
		this.shadowsToggle.ToggleComponent.isOn = GlobalSettings.shadows;
		this.waterReflectToggle.ToggleComponent.isOn = GlobalSettings.waterReflection;
		this.waterRefractToggle.ToggleComponent.isOn = GlobalSettings.waterRefraction;
		this.soundSlider.SliderComponent.value = GlobalSettings.soundMultiplier;
		this.musicSlider.SliderComponent.value = GlobalSettings.musicMultiplier;

		actionSelectables.Add(shadowsToggle.gameObject);
		actionSelectables.Add(waterRefractToggle.gameObject);
		actionSelectables.Add(waterReflectToggle.gameObject);
		actionSelectables.Add(musicSlider.gameObject);
		actionSelectables.Add(soundSlider.gameObject);
		actionSelectables.Add(keyboardControls.gameObject);
        actionSelectables.Add(controllerControls.gameObject);
	}

	public void setSoundVolume(float vol) {
    audioMixer.SetFloat("SFX", LinearToDBConverter.LinearToDecibal(vol));
		GlobalSettings.setSoundMultiplier(vol);
	}

	public void setMusicVolume(float vol) {
    audioMixer.SetFloat("Music", LinearToDBConverter.LinearToDecibal(vol));
		GlobalSettings.setMusicMuliplier(vol);
	}

	void setShadowsToggle(bool isOn) {
		GlobalSettings.setShadows(isOn);
	}

	void setWaterRefractionToggle(bool isOn) {
		GlobalSettings.setRefraction(isOn);
	}

	void setWaterReflectionToggle(bool isOn) {
		GlobalSettings.setReflection(isOn);
	}

    public void OnClick()
    {
        GoBack();
    }
}

