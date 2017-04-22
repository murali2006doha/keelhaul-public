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
        UnityEngine.Object prefab = Resources.Load(PathVariables.soundPrefab); 
        soundObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        soundObject.transform.Translate (this.gameObject.transform.position);
        soundObject.GetComponent<CustomAudioSource>().audioComponent.clip = soundClip;
        soundObject.GetComponent<CustomAudioSource>().audioComponent.volume = volume;
        soundObject.GetComponent<PrefabSound> ().StartSound();
    }


    void Update() {
        if (this.gameObject != null && soundObject != null) {
            soundObject.transform.position = this.gameObject.transform.position;
        }
    }
        
}
