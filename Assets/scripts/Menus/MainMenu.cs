using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;
using System;

public class MainMenu : AbstractMenu
{

    public ActionButton online;
    public Transform onlineSubmenu;
    public ActionButton deathMatchOnline;
    public ActionButton sabotageOnline;
    public Transform offlineSubmenu;
    public ActionButton offline;
    public ActionButton deathMatchOffline;
    public ActionButton sabotageOffline;
	public ActionButton settings;
	public ActionButton exit;
    public Transform offlineNotAvailableText;
    public Transform sabotageNotAvailableText;

    [SerializeField]
    private CharacterSelectController csController;

    bool dontReset= false;

	protected override void SetActions() {

		online.SetAction(() => {
            CloseOfflineSubmenu();
            canReturn = true;
            onlineSubmenu.gameObject.SetActive(true);
            actionSelectables.Insert(actionSelectables.IndexOf(online.gameObject) + 1, deathMatchOnline.gameObject);
            actionSelectables.Insert(actionSelectables.IndexOf(online.gameObject) + 2, sabotageOnline.gameObject);
            index = index + 1;
		});

        deathMatchOnline.SetAction (() => {
            FindObjectOfType<GameModeSelectSettings>().SetGameModeSettings(GameTypeEnum.DeathMatch, true);
            SceneManager.LoadScene("Game");
        });

        sabotageOnline.SetAction (() => {
            sabotageNotAvailableText.gameObject.SetActive(true);
            Invoke("DestroySabotageNotAvailableText", 1f);
            //FindObjectOfType<GameModeSelectSettings>().SetGameModeSettings(GameTypeEnum.Sabotage, true);
            //SceneManager.LoadScene("Game");
        });

		offline.SetAction(() => {
            CloseOnlineSubmenu();
            canReturn = true;
            offlineSubmenu.gameObject.SetActive(true);            
            actionSelectables.Insert(actionSelectables.IndexOf(offline.gameObject) + 1, deathMatchOffline.gameObject);
            actionSelectables.Insert(actionSelectables.IndexOf(offline.gameObject) + 2, sabotageOffline.gameObject);
			index = index + 1;
		});

        deathMatchOffline.SetAction (() => {
            FindObjectOfType<PlayerSelectSettings>().gameType = GameTypeEnum.DeathMatch;
            this.TransitionToCharacterSelect();

        });

        sabotageOffline.SetAction(() => {
            GameObject.FindObjectOfType<PlayerSelectSettings>().gameType = GameTypeEnum.Sabotage;
            this.TransitionToCharacterSelect();
        });


		settings.SetAction(() => {
            CloseOnlineSubmenu();
            CloseOfflineSubmenu();
            canReturn = true;
            this.enabled = false;
			ToggleSelectables();
			FindObjectOfType<MenuModel>().settingsMenu.Initialize(this.actions, () => {
                index = actionSelectables.IndexOf(settings.gameObject);
				this.enabled = true;
                ToggleSelectables();
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
            index = actionSelectables.IndexOf(exit.gameObject);

		});

		exit.SetAction(() => {
            CloseOnlineSubmenu();
	        CloseOfflineSubmenu();
			ModalStack.InitializeModal(this.actions, ModalsEnum.notificationDoubleModal, modalActions);
            FindObjectOfType<NotificationDoubleModal>().Spawn(NotificationImages.quitConfirm, 
                                                        NotificationImages.yes, 
                                                        NotificationImages.no, () => {
				Exit();
			}, () => {
				exit.ButtonComponent.Select();
			});
		});
	}



    protected override void SetActionSelectables() {
		actionSelectables.Add(online.gameObject);
		actionSelectables.Add(offline.gameObject);        //commented out because this is not currently in use
		actionSelectables.Add(settings.gameObject);
		actionSelectables.Add(exit.gameObject);
	}


    void CloseOnlineSubmenu() {
        canReturn = false;

        if (onlineSubmenu.gameObject.GetActive()) {
    		onlineSubmenu.gameObject.SetActive(false);
            actionSelectables.RemoveAt(actionSelectables.IndexOf(deathMatchOnline.gameObject));
            actionSelectables.RemoveAt(actionSelectables.IndexOf(sabotageOnline.gameObject));
            index = 0;
            dontReset = true;

        }
    }

    void CloseOfflineSubmenu() {
        canReturn = false;
        if (offlineSubmenu.gameObject.GetActive()) {
            offlineSubmenu.gameObject.SetActive(false);
            actionSelectables.RemoveAt(actionSelectables.IndexOf(deathMatchOffline.gameObject));
            actionSelectables.RemoveAt(actionSelectables.IndexOf(sabotageOffline.gameObject));
            index = 1;
            dontReset = true;
        }
    }


    public void ResetMenu() {
       
        DestroySabotageNotAvailableText();
        //DestroyOfflineNotAvailableText();
        CloseOnlineSubmenu();
        CloseOfflineSubmenu();
        if(!dontReset)
            index = 0;
        canReturn = false;
        dontReset = false;
    }

    private void TransitionToCharacterSelect() {
        this.csController.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void DestroySabotageNotAvailableText() {
        sabotageNotAvailableText.gameObject.SetActive(false);
    }

    void DestroyOfflineNotAvailableText() {
        offlineNotAvailableText.gameObject.SetActive(false);
    }

}
