using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class CustomAudioSource : MonoBehaviour {

    [SerializeField]
    [Range(0.0f, 1.0f)]
    public float intendedVolume = 1f;

    [SerializeField]
    bool isSound;

    [SerializeField]
    AudioSource audioSource;


    void Start() {

        if (isSound) {
        this.audioComponent.volume = intendedVolume * GlobalSettings.soundMultiplier;
        } else {
        this.audioComponent.volume = intendedVolume * GlobalSettings.musicMultiplier;
        }
            
        GlobalSettings.OnSoundChange += setSoundVolume;
        GlobalSettings.OnMusicChange += setMusicVolume;
    }


    void setSoundVolume () {
        if (isSound) {
            this.audioComponent.volume = intendedVolume * GlobalSettings.soundMultiplier;
        }
    }


    void setMusicVolume () {
        if (!isSound) {
            this.audioComponent.volume = intendedVolume * GlobalSettings.musicMultiplier;
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
