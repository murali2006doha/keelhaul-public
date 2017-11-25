using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
public class SettingsMenu : AbstractMenu
{


	public ActionToggle shadowsToggle;
	public ActionToggle waterRefractToggle;
	public ActionToggle waterReflectToggle;
	public ActionSlider soundSlider;
	public ActionSlider musicSlider;
	public ActionButton keyboardControls;
    public ActionButton controllerControls;


	protected override void SetActions() {
		shadowsToggle.SetAction(this.setShadowsToggle);
		waterRefractToggle.SetAction(this.setWaterRefractionToggle);
		waterReflectToggle.SetAction(this.setWaterReflectionToggle);
		soundSlider.SetAction(this.setSoundVolume, this.actions);
		musicSlider.SetAction(this.setMusicVolume, this.actions);
		
        keyboardControls.SetAction(() => {
			this.enabled = false;
			ToggleSelectables();
            this.gameObject.SetActive(false);
            FindObjectOfType<MenuModel>().inputBindingsMenu.Initialize(this.actions, () => {
                FindObjectOfType<InputBindingsMenu>().ResetPage();
				ToggleSelectables();
				this.enabled = true;
                this.gameObject.SetActive(true);
			});
		});

        controllerControls.SetAction(() => {
            this.enabled = false;
			ToggleSelectables();
            this.gameObject.SetActive(false);
            FindObjectOfType<MenuModel>().controllerLayoutMenu.Initialize(this.actions, () => {
	            ToggleSelectables();
	            this.enabled = true; 
                this.gameObject.SetActive(true);
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
		actionSelectables.Add(soundSlider.gameObject);
		actionSelectables.Add(musicSlider.gameObject);
		/*actionSelectables.Add(keyboardControls.gameObject);
        actionSelectables.Add(controllerControls.gameObject);*/
	}



	void setSoundVolume(float multiplier) {
		GlobalSettings.setSoundMultiplier(multiplier);
	}

	void setMusicVolume(float multiplier) {
		GlobalSettings.setMusicMuliplier(multiplier);
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

