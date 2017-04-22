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

    [SerializeField]
    float maxVolume = .14f;
    [SerializeField]
    float boostVolume = .14f;
    // Use this for initialization
    void Start () {
        player = GetComponentInParent<PlayerInput> ();
        audioSource = GetComponent<CustomAudioSource> ().audioComponent;
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
            audioSource.volume = Mathf.Min(volume / 3.0f, maxVolume);
        } else {
            audioSource.volume = Mathf.Min(volume, maxVolume);
        }

        if (player.motor.isBoosting () && !boosted) {
            PlayBoostSound ();
        } 

    }


    void PlayBoostSound ()
    {
        boosted = true;
        audioSource.Pause ();

        UnityEngine.Object prefab = Resources.Load(PathVariables.soundPrefab); 
        boostObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        boostObject.transform.SetParent (this.transform);

        boostObject.GetComponent<CustomAudioSource>().audioComponent.clip = boostSound;
        boostObject.GetComponent<CustomAudioSource> ().audioComponent.volume = boostVolume;
        boostObject.GetComponent<CustomAudioSource> ().audioComponent.Play ();
        
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
