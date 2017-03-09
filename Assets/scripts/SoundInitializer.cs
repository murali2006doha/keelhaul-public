using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInitializer : MonoBehaviour {

    public AudioClip soundClip;
    GameObject soundObject;


    // Use this for initialization
    void Start () {
        soundObject = new GameObject();
        soundObject.AddComponent<AudioSource> ();
		soundObject.AddComponent<PrefabSound> ();
		soundObject.GetComponent<AudioSource> ().clip = soundClip;
		soundObject.GetComponent<PrefabSound> ().StartSound();
    }
		
}
