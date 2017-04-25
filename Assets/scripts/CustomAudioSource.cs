using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CustomAudioSource : MonoBehaviour {

	float ogVol;

    [SerializeField]
    bool isSound;

    [SerializeField]
    AudioSource audioSource;


    void Start() {

		if (isSound) {
			this.audioComponent.volume = GlobalSettings.soundMultiplier;
		} else {
			this.audioComponent.volume = GlobalSettings.musicMultiplier;
		}

        ogVol = this.audioComponent.volume;

        GlobalSettings.OnSoundChange += setSoundVolume;
		GlobalSettings.OnMusicChange += setMusicVolume;
    }


    void setSoundVolume () {
        if (isSound) {
            this.audioComponent.volume = ogVol * GlobalSettings.soundMultiplier;
        }
    }


    void setMusicVolume () {
        if (!isSound) {
            this.audioComponent.volume = ogVol * GlobalSettings.musicMultiplier;
        }
    }


	void OnDestroy() {
		GlobalSettings.OnSoundChange -= setSoundVolume;
		GlobalSettings.OnMusicChange -= setMusicVolume;
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
