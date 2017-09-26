using UnityEngine;
using System.Collections.Generic;
using System;

public class PauseMenu : AbstractMenu
{

    public ActionButton resumeButton;
    public ActionButton settingsButton;
    public ActionButton exitToMenuButton;
    public ActionButton exitToDesktopButton;   
    public AudioSource pauseMusic;
    AbstractGameManager gm;

    CountDown countdown;
    AudioSource[] audios;
    PlayerInput[] otherActions;


    public void Initialize(PlayerActions actions, Action goBackAction) {
    	this.gameObject.SetActive(true);
    	this.actions = actions;
    	this.onReturnAction = goBackAction;
    	navUtils = new GameObject("navigation", typeof(NavigationUtils));
        PauseGame();
    }

    protected override void SetActions() {

        resumeButton.SetAction (() => {
            GoBack();
        });

        settingsButton.SetAction(() => {
            this.enabled = false;
            ToggleSelectables();
            FindObjectOfType<MenuModel>().settingsMenu.Initialize(this.actions, () => {
                index = actionSelectables.IndexOf(settingsButton.gameObject);
                this.enabled = true;
                ToggleSelectables();
                FindObjectOfType<MenuModel>().settingsMenu.gameObject.SetActive(false);
            });
        });


        Dictionary<ModalActionEnum, Action> modalActions = new Dictionary<ModalActionEnum, Action>();
        modalActions.Add(ModalActionEnum.onOpenAction, () => {
            this.enabled = false;
            ToggleSelectables();
            canReturn = false;
        });
        modalActions.Add(ModalActionEnum.onCloseAction, () => {
            this.enabled = true;
            ToggleSelectables();
            canReturn = true;
        });

        exitToMenuButton.SetAction(() => {
            ModalStack.InitializeModal(this.actions, ModalsEnum.notificationDoubleModal, modalActions);
			FindObjectOfType<NotificationDoubleModal>().Spawn(NotificationImages.quitConfirm, 
															NotificationImages.yes, 
															NotificationImages.no, () => {
                ExitToMainMenu();
            }, () => {
                index = actionSelectables.IndexOf(exitToMenuButton.gameObject);
                exitToMenuButton.ButtonComponent.Select();
            });
        });

        exitToDesktopButton.SetAction(() => {
            ModalStack.InitializeModal(this.actions, ModalsEnum.notificationDoubleModal, modalActions);
			FindObjectOfType<NotificationDoubleModal>().Spawn(NotificationImages.quitConfirm, 
																NotificationImages.yes, 
																NotificationImages.no, () => {
                ExitToDesktop();
            }, () => {
                index = actionSelectables.IndexOf(exitToDesktopButton.gameObject);
                exitToDesktopButton.ButtonComponent.Select();
            });
        });
    }

    protected override void SetActionSelectables() {
        actionSelectables.Add (resumeButton.gameObject);
        actionSelectables.Add (settingsButton.gameObject);
        actionSelectables.Add (exitToMenuButton.gameObject);
        actionSelectables.Add (exitToDesktopButton.gameObject);
    }


    public void PauseGame() {

    	audios = FindObjectsOfType<AudioSource>();

    	if (PhotonNetwork.offlineMode) {
            otherActions = FindObjectsOfType<PlayerInput>();
    		Time.timeScale = 0;
            TogglePlayerActions();
    	}

    	PauseAudio();
    }

    //put this in onReturnAction when calling from playerInput
    public void ResumeGame() {

    	if (PhotonNetwork.offlineMode) {
    		Time.timeScale = 1;
            TogglePlayerActions();
    	}
    	ResumeAudio();
    }


    private void ExitToMainMenu() {
        Time.timeScale = 1;
        gm.ExitToCharacterSelect();
        DestroyObject(this.transform.gameObject);
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
            if (null != audio)
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
