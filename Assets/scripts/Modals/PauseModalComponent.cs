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

    public ActionButton exitToMenuButton;   //these will eventually have the ActionButton.cs script attached 
    public ActionButton exitToDesktopButton; 
    public ActionButton settingsButton;
    public ActionButton resumeButton; 
    public AudioSource pauseMusic;

    CountDown countdown;
    AudioSource[] audios;
    PlayerInput[] otherActions;
    bool isPaused = false;
   
 


    //The player who paused game has access. 
    public override void SetupModal(PlayerActions actions) {
        this.SetUpButtonToActionDictionary (actions);

        this.gm = FindObjectOfType<AbstractGameManager> ();
        this.actions = actions;
        this.PauseGame ();
        this.popAction += ResumeGame; //because this will be no modal before this so game will resume
    
    }

    protected override bool CanControl () 
    {
        if (isPaused && isActive) {
            return true;
        } 

        return false;
    }


    void SetUpButtonToActionDictionary (PlayerActions actions) {

        Dictionary<ModalActionEnum, Action> modalActions = new Dictionary<ModalActionEnum, Action> ();
        modalActions.Add (ModalActionEnum.onOpenAction, () => {ToggleSelectables();});
        modalActions.Add (ModalActionEnum.onCloseAction, () => {ToggleSelectables();});

        actionSelectables.Add (exitToMenuButton.gameObject);
        actionSelectables.Add (exitToDesktopButton.gameObject);
        actionSelectables.Add (settingsButton.gameObject);
        actionSelectables.Add (resumeButton.gameObject);

        exitToMenuButton.SetAction (() =>  {
            this.pushAction ();

            ModalStack.InitializeModal (this.actions, ModalsEnum.notificationModal, modalActions);
            FindObjectOfType<NotificationModal>().Spawn ("Are you sure?", Color.yellow, "Yes", "No", 
                () =>  {
                    ExitToMainMenu ();
                    isActive = true;
                }, 
                () =>  {
                    exitToMenuButton.ButtonComponent.Select ();
                    isActive = true;
                }
            );});

        exitToDesktopButton.SetAction (() =>  {
            this.pushAction ();

            ModalStack.InitializeModal (this.actions, ModalsEnum.notificationModal, modalActions);
            FindObjectOfType<NotificationModal>().Spawn ("Are you sure?", Color.yellow, "Yes", "No",
                () =>  {
                    ExitToDesktop ();
                    isActive = true;
                }, 
                () =>  {
                    exitToDesktopButton.ButtonComponent.Select ();
                    isActive = true;

                });});

        settingsButton.SetAction (() => {
            this.pushAction ();
            ModalStack.InitializeModal (this.actions, ModalsEnum.settingsModal, modalActions);
        });

        resumeButton.SetAction (() => {
            popAction ();
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
            audio.mute = true;
        }

        pauseMusic.Play();
    }


    private void ResumeAudio() {
        foreach (AudioSource audio in audios) {
            if(null != audio) 
                audio.mute = false;
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
    