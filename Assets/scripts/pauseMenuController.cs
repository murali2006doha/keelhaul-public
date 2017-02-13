using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;


/**TODO: 
 * music and sound ON/OFF with sliders
 * RESTART match
 * exit to matchmaking lobby
 **/
public class pauseMenuController : MonoBehaviour {

    public Button exitToMenuButton;   //these will eventually have the ActionButton.cs script attached 
    public Button exitToDesktopButton; 
    public Button resumeButton; 
    public AudioSource pauseMusic;

    bool isPaused; 
    CountDown countdown;
    AbstractGameManager gm;
    AudioSource[] audios;
    PlayerActions actions;
    PlayerInput[] otherActions;
    Button[] allButtons = new Button[3];
    int index;



    void Update() {

        if (isPaused) {
            //navigateScreen (allButtons);  // TODO: After actionButton is merged. 
        }
    }



    /**  The player who paused game has access. 
    * In offline, everyone's game is paused.
    * In online, only that players game is paused.
     **/
    public void initialize(PlayerActions actions) {

        allButtons [0] = resumeButton;
        allButtons [1] = exitToMenuButton;
        allButtons [2] = exitToDesktopButton;

        gm = FindObjectOfType<AbstractGameManager> ();
        this.actions = actions;

        pauseGame ();
    }


    //TODO: need to configure the player's actions to control the the pause menu with controller and keyboard
    public void OnClickButton(string choice) {

        if( choice == "resume") {
            this.resumeGame ();
        }

        if( choice == "exitToMenu") {

            UnityEngine.Object modalPrefab = Resources.Load("Prefabs/ModalCanvas"); 
            GameObject modalObject = (GameObject) GameObject.Instantiate(modalPrefab, Vector3.zero, Quaternion.identity);

            modalObject.GetComponent<ModalController>().initialize ("Are you sure?", Color.yellow, "Yes", "No", 
                () => {exitToMainMenu();    },          //on yes action
                () => {exitToMenuButton.Select();   }   //on no action
            );
        }


        if (choice == "exitToDesktop") {

            UnityEngine.Object modalPrefab = Resources.Load("Prefabs/ModalCanvas");
            GameObject modalObject = (GameObject) GameObject.Instantiate(modalPrefab, Vector3.zero, Quaternion.identity);

            modalObject.GetComponent<ModalController>().initialize ("Are you sure?", Color.yellow, "Yes", "No", 
                () => {exitToDesktop();     }, 
                () => {exitToDesktopButton.Select();    }
            );
        }
    }


    public void pauseGame() {

        audios = FindObjectsOfType <AudioSource> ();
        otherActions = FindObjectsOfType <PlayerInput> ();

        resumeButton.Select ();
        Time.timeScale = 0;
        pauseAudio ();

        togglePlayerActions ();
        isPaused = true;
    }


    public void resumeGame() {

        Time.timeScale = 1;
        resumeAudio ();
        togglePlayerActions ();
        DestroyObject (this.transform.gameObject);
    }


    private void exitToMainMenu() {

        Time.timeScale = 1;
        gm.exitToCharacterSelect ();
        DestroyObject (this.transform.gameObject);
    }


    private void exitToDesktop() {
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


    /**
     * turns of all the players actions except the player who paused the game
     **/
    private void togglePlayerActions() {
        foreach (PlayerInput input in otherActions) {
            if (input.Actions != actions) {
                input.Actions.Enabled = !input.Actions.Enabled;
            }
        }
    }


    public int getPositionIndex (Button[] items, int item, string direction) {
        if (direction == "up") {
            if (item == 0) {
                item = items.Length - 1;
            } else {
                item -= 1;
            }
        }

        if (direction == "down") {
            if (item == items.Length - 1) {
                item = 0;
            } else {
                item += 1;
            }
        }

        return item;
    }


    public void navigateScreen (Button[] buttons) { //navigating main menu      
        if (actions.Down.WasReleased || actions.R_Down.RawValue > 0.5f) {
            index = getPositionIndex (buttons, index, "down");
            buttons [index].Select ();
        }

        if (actions.Up.WasReleased || actions.R_Up.RawValue > 0.5f) {
            index = getPositionIndex (buttons, index, "up");
            buttons [index].Select ();
        }

        if (actions.Green.WasReleased) {
            buttons [index].onClick.Invoke();
        }
    }


}
