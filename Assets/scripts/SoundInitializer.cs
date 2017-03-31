using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInitializer : MonoBehaviour {

    public AudioClip soundClip;
    GameObject soundObject;
    [SerializeField]
    float volume;

    // Use this for initialization
    void Start () {
        soundObject = new GameObject();
        var audioSource = soundObject.AddComponent<AudioSource> ();
		soundObject.AddComponent<PrefabSound> ();
        audioSource.clip = soundClip;
        audioSource.volume = volume;
		soundObject.GetComponent<PrefabSound> ().StartSound();
    }
		
}
