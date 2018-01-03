using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class SetUpAudioMixer : MonoBehaviour {

    public AudioMixer audioMixer;


    // Use this for initialization
	void Start () {
        float soundMultiplier = PlayerPrefs.GetFloat("Sound multiplier", 1f);
        float musicMultiplier = PlayerPrefs.GetFloat("Music multiplier", 1f);
		audioMixer.SetFloat("Music", (100 * musicMultiplier) - 80);
        audioMixer.SetFloat("SFX", (100 * soundMultiplier) - 80);

	}

	
}
