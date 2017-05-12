using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovementSound : MonoBehaviour {


    public AudioClip boostSound;
    public AudioClip movementSound;

    PlayerInput player;
	CustomAudioSource audioSource;
    GameObject boostObject;

    float maxVelocity;
    float speed;
    float volume;
    bool boosted = false;

    [SerializeField]
    float maxVolume = .65f;
    [SerializeField]
    float boostVolume = 1f;
    // Use this for initialization
    void Start () {
        player = GetComponentInParent<PlayerInput> ();
        audioSource = GetComponent<CustomAudioSource> ();
		audioSource.audioComponent.clip = movementSound;
		audioSource.audioComponent.Play ();
		audioSource.setVolume(0f);
    }

    // Update is called once per frame
    void Update () {

        maxVelocity = player.stats.maxVelocity;
        speed = player.motor.getVelocity ();
        volume = (speed / maxVelocity);

        if (player.motor.isBoosting ()) {
            audioSource.setVolume((Mathf.Min(volume, maxVolume)) / 3.0f);
        } else {
			audioSource.setVolume(Mathf.Min(volume, maxVolume));
        }

        if (player.motor.isBoosting () && !boosted) {
            PlayBoostSound ();
        } 

    }


    void PlayBoostSound ()
    {
        boosted = true;
		audioSource.audioComponent.Pause ();

        UnityEngine.Object prefab = Resources.Load(PathVariables.soundPrefab); 
        boostObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        boostObject.transform.SetParent (this.transform);

        boostObject.GetComponent<CustomAudioSource>().audioComponent.clip = boostSound;
        boostObject.GetComponent<CustomAudioSource>().setVolume(boostVolume);
        boostObject.GetComponent<CustomAudioSource> ().audioComponent.Play ();
        
        Invoke ("ResumeMovementSound", boostSound.length);
        Invoke ("resetBoosted", player.stats.boostResetTime);
    }


    void ResumeMovementSound() {

		audioSource.audioComponent.Play ();
        Destroy (boostObject);

    }


    void resetBoosted() {
        boosted = false;
    }
}
