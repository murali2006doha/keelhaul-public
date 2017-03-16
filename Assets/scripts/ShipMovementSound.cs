using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementSound : MonoBehaviour {


    public AudioClip boostSound;
    public AudioClip movementSound;

    PlayerInput player;
    AudioSource audioSource;
    GameObject boostObject;

    float maxVelocity;
    float speed;
    float volume;
    bool boosted = false;

    // Use this for initialization
    void Start () {
        player = GetComponentInParent<PlayerInput> ();
        audioSource = GetComponent<AudioSource> ();
        audioSource.clip = movementSound;
        audioSource.Play ();
        audioSource.volume = 0f;
    }

    // Update is called once per frame
    void Update () {

        maxVelocity = player.stats.maxVelocity;
        speed = player.motor.getVelocity ();
	volume = (speed / maxVelocity) / 3.0f;

        if (player.motor.isBoosting ()) {
	    audioSource.volume = volume / 3.0f;
        } else {
            audioSource.volume = volume;
        }

        if (player.motor.isBoosting () && !boosted) {
            PlayBoostSound ();
        } 

    }


    void PlayBoostSound ()
    {
        boosted = true;
        audioSource.Pause ();
        boostObject = new GameObject ();
        boostObject.transform.SetParent (this.transform);
        boostObject.AddComponent<AudioSource> ();
        AudioSource audio = boostObject.GetComponent<AudioSource> ();
        audio.clip = boostSound;
        audio.Play ();
	audio.volume = 0.3f;
        Invoke ("ResumeMovementSound", boostSound.length);
        Invoke ("resetBoosted", player.stats.boostResetTime);
    }


    void ResumeMovementSound() {

        audioSource.Play ();
        Destroy (boostObject);

    }


    void resetBoosted() {
        boosted = false;
    }
}
