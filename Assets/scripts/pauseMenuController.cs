using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class pauseMenuController : MonoBehaviour {

	public Button returnToMenuButton;   //these will eventually have the ActionButton.cs script attached 
	public Button returnToDesktopButton; 
	public Button resumeButton; 
	public AudioSource pauseMusic;

	bool CanPause; 
	CountDown countdown;
	SabotageGameManager gm;
	AudioSource[] audios;
	PlayerActions actions;
	PlayerInput[] otherActions;


	// Use this for initialization
	void Start () {
	}


	/**  The player who paused game has access. 
	* In offline, everyone's game is paused.
	* In online, only that players game is paused.
	 **/
	public void initialize(PlayerActions actions) {
		resumeButton.Select ();
		this.actions = actions;
		audios = FindObjectsOfType <AudioSource> ();
		otherActions = FindObjectsOfType <PlayerInput> ();
		pauseGame ();
	}


	//TODO: need to configure the player's actions to control the the pause menu with controller and keyboard
	public void OnClickButton(string choice) {

		if( choice == "resume") {
			this.resumeGame ();
		}

		if( choice == "exitToMenu") {
			toggleAllButtons ();

			UnityEngine.Object modalPrefab = Resources.Load("Prefabs/ModalCanvas"); // note: not .prefab!
			GameObject modalObject = (GameObject) GameObject.Instantiate(modalPrefab, Vector3.zero, Quaternion.identity);

			modalObject.GetComponent<ModalController>().initialize ("Are you sure?", Color.yellow, "Yes", "No", 
				() => {returnToMainMenu(); },
				() => {toggleAllButtons(); });
			
		}

		if (choice == "exitToDesktop") {
			toggleAllButtons ();

			UnityEngine.Object modalPrefab = Resources.Load("Prefabs/ModalCanvas"); // note: not .prefab!
			GameObject modalObject = (GameObject) GameObject.Instantiate(modalPrefab, Vector3.zero, Quaternion.identity);

			modalObject.GetComponent<ModalController>().initialize ("Are you sure?", Color.yellow, "Yes", "No", 
				() => {returnToDesktop(); }, 
				() => {toggleAllButtons(); });
		}
	}


	private void pauseGame() {

		Time.timeScale = 0;
		pauseAudio ();
		togglePlayerActions ();
	}


	private void resumeGame() {

		Time.timeScale = 1;
		resumeAudio ();
		togglePlayerActions ();
		DestroyObject (this.transform.gameObject);
	}


	private void returnToMainMenu() {

		Time.timeScale = 1;
		gm.exitToCharacterSelect ();
		DestroyObject (this.transform.gameObject);
	}


	private void returnToDesktop() {
		Application.Quit();
	}


	private void pauseAudio() {

		foreach (AudioSource audio in audios) {
			audio.Pause();
		}

		pauseMusic.Play();
	}


	private void resumeAudio() {

		foreach (AudioSource audio in audios) {
			audio.Play();
		}
		pauseMusic.Stop();
	}


	private void toggleAllButtons() {

		returnToMenuButton.enabled = !returnToMenuButton.enabled;
		returnToDesktopButton.enabled = !returnToDesktopButton.enabled;
		resumeButton.enabled = !resumeButton.enabled;
	}


	private void togglePlayerActions() {
		foreach (PlayerInput input in otherActions) {
			print (input.Actions.Enabled);
			input.Actions.Enabled = !input.Actions.Enabled;
		}
	}


}
