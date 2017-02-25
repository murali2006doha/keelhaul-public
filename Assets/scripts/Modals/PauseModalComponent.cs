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
public class PauseModalComponent : AbstractModalComponent { //after networking, => offline pause and online pause modal

    public Button exitToMenuButton;   //these will eventually have the ActionButton.cs script attached 
    public Button exitToDesktopButton; 
    public Button settingsButton;
    public Button resumeButton; 
    public AudioSource pauseMusic;

    CountDown countdown;
    AudioSource[] audios;
    PlayerInput[] otherActions;
    bool isPaused = false;

    void Update() {
        if (isPaused && isActive) {
            Control (); 
        }
    }

    //The player who paused game has access. 
    public override void InitializeModal(PlayerActions actions) {

        this.SetUpButtonToActionDictionary (actions);

        this.gm = FindObjectOfType<AbstractGameManager> ();
        this.actions = actions;
        this.buttons = new List<Button> (buttonToAction.Keys).ToArray ();
        this.index = buttons.Length - 1;
        this.PauseGame ();
        this.popAction += ResumeGame; //because this will be no modal before this so game will resume
    }

    void SetUpButtonToActionDictionary (PlayerActions actions) {


        Dictionary<ModalActionEnum, Action> modalActions = new Dictionary<ModalActionEnum, Action> ();
        modalActions.Add (ModalActionEnum.onOpenAction, () => {ToggleButtons();});
        modalActions.Add (ModalActionEnum.onCloseAction, () => {ToggleButtons();});
    

        buttonToAction.Add (exitToMenuButton, () =>  {
            this.pushAction ();

            ModalStack.initialize (this.actions, ModalsEnum.notificationModal, modalActions);
            FindObjectOfType<NotificationModal>().Initialize ("Are you sure?", Color.yellow, "Yes", "No", 
                () =>  {
                ExitToMainMenu ();
                    isActive = true;
            }, 
                () =>  {
                    exitToMenuButton.Select ();
                    isActive = true;
                }
        );});

        buttonToAction.Add (exitToDesktopButton, () =>  {
            this.pushAction ();

            ModalStack.initialize (this.actions, ModalsEnum.notificationModal, modalActions);
            FindObjectOfType<NotificationModal>().Initialize ("Are you sure?", Color.yellow, "Yes", "No",
                () =>  {
                ExitToDesktop ();
                isActive = true;
                    }, 
                () =>  {
                exitToDesktopButton.Select ();
                isActive = true;

        });});

        buttonToAction.Add (settingsButton, () =>  {
            this.pushAction ();

            ModalStack.initialize (this.actions, ModalsEnum.settingsModal, modalActions);
        });

        buttonToAction.Add (resumeButton, () =>  {
            popAction();
        });
    }


    public void PauseGame() {

        audios = FindObjectsOfType <AudioSource> ();
        otherActions = FindObjectsOfType <PlayerInput> ();

        Time.timeScale = 0;
        PauseAudio ();

        TogglePlayerActions ();
        isPaused = true;
    }


    public void ResumeGame() {

        Time.timeScale = 1;
        ResumeAudio ();
        TogglePlayerActions ();
    }


    private void ExitToMainMenu() {

        Time.timeScale = 1;
        gm.exitToCharacterSelect ();
        DestroyObject (this.transform.gameObject);
    }


    private void ExitToDesktop() {
        Application.Quit();
    }


    private void PauseAudio() {

        foreach (AudioSource audio in audios) {
            audio.Pause();
        }

        pauseMusic.Play();
    }


    private void ResumeAudio() {
        foreach (AudioSource audio in audios) {
            audio.Play();
        }
        pauseMusic.Stop();
    }


    /**
     * turns of all the players actions except the player who paused the game
     **/
    private void TogglePlayerActions() {
        foreach (PlayerInput input in otherActions) {
            if (input.Actions != actions) {
                input.Actions.Enabled = !input.Actions.Enabled;
            }
        }
    }




}
    