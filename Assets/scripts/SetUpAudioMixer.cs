using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class SetUpAudioMixer : MonoBehaviour {

    public AudioMixer audioMixer;


    // Use this for initialization
	void Start () {
        float soundMultiplier = GlobalSettings.soundMultiplier;
        float musicMultiplier = GlobalSettings.musicMultiplier;
    audioMixer.SetFloat("SFX", LinearToDBConverter.LinearToDecibal(soundMultiplier));
    audioMixer.SetFloat("Music", LinearToDBConverter.LinearToDecibal(musicMultiplier));
	} 
	
}
