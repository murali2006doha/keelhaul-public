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
        audioMixer.SetFloat("SFX", LinearToDecibal(soundMultiplier));
        audioMixer.SetFloat("Music", LinearToDecibal(musicMultiplier));
	}
     float LinearToDecibal(float linear)     {         if (linear == 0.0f)         {             return -80f;         }         else         {             return (20.0f * Mathf.Log10(linear)) + 20;         }      }     
	
}
