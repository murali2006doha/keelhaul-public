using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CustomAudioSource : MonoBehaviour {

	float ogvol;

	[SerializeField]
	bool isSound;

	[SerializeField]
	AudioSource audioSource;


	void Start() {
		ogvol = this.audioComponent.volume;
		GlobalSettings.OnSoundChange += setSoundVolume;
		GlobalSettings.OnMusicChange += setMusicVolume;
	}
		
	void setSoundVolume () {
		if (isSound) {
			this.audioComponent.volume = ogvol * GlobalSettings.soundMultiplier;
		}
	}


	void setMusicVolume () {
		if (!isSound) {
			this.audioComponent.volume = ogvol * GlobalSettings.musicMultiplier;
		}
	}


	public AudioSource audioComponent {
		get {
			if (this.audioSource == null) {
				this.audioSource = this.GetComponent<AudioSource>();
			}

			return this.audioSource;
		}
	}

}
