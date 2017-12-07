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

        this.audioComponent.volume = intendedVolume;

    }


	public void setVolume(float volume) {
        this.audioComponent.volume = volume;
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
