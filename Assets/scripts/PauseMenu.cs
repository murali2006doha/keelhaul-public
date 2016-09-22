using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;

public class PauseMenu : MonoBehaviour {

	public PlayerActions Actions { get; set; }
	public Transform pauseMenu;
	public List<PlayerActions> playerActions = new List<PlayerActions>(); //to pass on to 
	bool CanPause; 
	CountDown countdown;
	gameManager gm;
	AudioSource[] audios;
	AudioSource pauseMusic;

	// Use this for initialization
	void Start () {
		CanPause = true;
		//Controls.gameObject.SetActive (false);
		countdown = GetComponent<CountDown>();
		pauseMusic = pauseMenu.gameObject.GetComponent<AudioSource> ();
		audios = FindObjectsOfType <AudioSource> ();

	}


	// Update is called once per frame
	void Update () {
		gm = GameObject.FindObjectOfType<gameManager> ();

		if (gm.players.Count != 0) {
			foreach (playerInput player in gm.players) {
				playerActions.Add (player.Actions);
			}
			playerActions.Add (gm.kraken.Actions);
		}
		if (gm.countDown == null) {
			Pause ();
		}
	}

	public void Pause(){
		if (!pauseMenu.gameObject.activeSelf) {
			if (playerActions.Exists (p => p.Start.WasReleased)) {
				Time.timeScale = 0;
				pauseMenu.gameObject.SetActive (true);
				pauseAudio ();
			} 
		} else {
			onPauseScreen ();
		}
	}


	public void onPauseScreen() {
		if (pauseMenu.gameObject.activeSelf) {
			if (playerActions.Exists (p => p.Start.WasReleased)) {
				Time.timeScale = 1;
				pauseMenu.gameObject.SetActive (false);
				resumeAudio ();
			}
			if (playerActions.Exists (p => p.Green.WasReleased)) {
				Time.timeScale = 1;
				gm.exitToCharacterSelect ();
			}

		}
	}

	public void pauseAudio() {

		foreach (AudioSource audio in audios) {
			audio.Pause();
		}
		pauseMusic.Play();
	}


	public void resumeAudio() {
		foreach (AudioSource audio in audios) {
			audio.Play();
		}
		pauseMusic.Stop();
	}


	/*
	 * cycles through a list 
	 */ 
	int upAndDown (List<Transform> items, int i, string direction) {

		if (direction == "up") {
			if (i == 0) {
				i = items.Count - 1;
			} else {
				i -= 1;
			}
		}
		if (direction == "down") {
			if (i == items.Count - 1) {
				i = 0;
			} else {
				i += 1;
			}
		}

		return i;
	}
}
