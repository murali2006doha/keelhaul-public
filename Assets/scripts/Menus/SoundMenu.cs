using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class SoundMenu : AbstractMenu
{


	public ActionSlider musicSlider;
	public ActionSlider soundSlider;
    public AudioMixer audioMixer;


	protected override void SetActions() {
		soundSlider.SetAction(this.setSoundVolume, this.actions);
		musicSlider.SetAction(this.setMusicVolume, this.actions);
	}

	protected override void SetActionSelectables() {
		this.soundSlider.SliderComponent.value = GlobalSettings.soundMultiplier;
		this.musicSlider.SliderComponent.value = GlobalSettings.musicMultiplier;

		actionSelectables.Add(musicSlider.gameObject);
		actionSelectables.Add(soundSlider.gameObject);
	}

	public void setSoundVolume(float vol) {
    audioMixer.SetFloat("SFX", LinearToDBConverter.LinearToDecibal(vol));
		GlobalSettings.setSoundMultiplier(vol);
	}

	public void setMusicVolume(float vol) {
    audioMixer.SetFloat("Music", LinearToDBConverter.LinearToDecibal(vol));
		GlobalSettings.setMusicMuliplier(vol);
	}

    public void OnClick()
    {
        GoBack();
    }

}

