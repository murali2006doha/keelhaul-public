using UnityEngine;
using System.Collections;

public class fadeIn : MonoBehaviour {

	// Use this for initialization

	public float startingVolume = 0f;
	public float endingVolume = .5f;
	public float volumeRaiseSpeed = 10f;
	public AudioSource clip;
	void Start () {
		clip.volume = startingVolume;
	}
	
	// Update is called once per frame
	void Update () {
		FadeIn ();
	}


	void FadeIn(){

		if (clip.volume < endingVolume) {
			clip.volume += volumeRaiseSpeed * (Time.deltaTime * GlobalVariables.gameSpeed);
		}
	}
}
