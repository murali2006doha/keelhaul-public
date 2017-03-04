using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInitializer : MonoBehaviour {

    public AudioClip soundClip;
    AudioSource audioSource;
    GameObject soundObject;


    // Use this for initialization
    void Start () {
        soundObject = new GameObject();
        soundObject.AddComponent<AudioSource> ();
        audioSource = soundObject.GetComponent<AudioSource> ();

        //sets the cannonball the child of the empty game object
        this.transform.SetParent (soundObject.gameObject.transform);

        audioSource.clip = soundClip;
        StartCoroutine (PlaySound ());

    }


    IEnumerator PlaySound() {
        audioSource.Play ();
        yield return new WaitForSeconds(audioSource.clip.length);

        Destroy (soundObject);
    }

}
