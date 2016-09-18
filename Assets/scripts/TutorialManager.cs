using UnityEngine;
using InControl;

using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour {
	public RawImage krakenTut;
	public RawImage shipTut;
	public RawImage splashTut;
	MovieTexture krakenMov;
	MovieTexture shipMov;
	MovieTexture splashMov;
	bool splashDone = false;
	public AudioSource waterSounds;

	// Use this for initialization

	void Start () {
		krakenMov = (MovieTexture)krakenTut.GetComponent<RawImage> ().mainTexture;
		shipMov = (MovieTexture)shipTut.GetComponent<RawImage> ().mainTexture;
		splashMov = (MovieTexture)splashTut.GetComponent<RawImage> ().mainTexture;
	//	Invoke ("startShip", 32);
		splashMov.Play ();
		this.GetComponent<AudioSource> ().clip = splashMov.audioClip;
		this.GetComponent<AudioSource> ().Play ();
		waterSounds.loop = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (InputManager.ActiveDevice.Action1.WasReleased || Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.A)) {
			if (!splashDone) {
				krakenTut.enabled = true;
				shipTut.enabled = false;
				Invoke ("startShip", 32);
				krakenMov.Play ();
				splashTut.enabled = false;
				splashDone = true;
				this.GetComponent<AudioSource> ().Stop ();
			}

			else {
				
				loadMenu ();
			}

		}

	}

	void startShip(){
		shipTut.enabled = true;
		krakenTut.enabled = false;
		Invoke ("loadMenu", 50);
		shipMov.Play ();
	}

	void loadMenu(){
		Application.LoadLevel ("start2");
	}
}
	