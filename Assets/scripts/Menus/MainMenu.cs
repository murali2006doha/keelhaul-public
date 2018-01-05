using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;
using System;

public class MainMenu : AbstractMenu
{

    public Transform offlineSubmenu;
    public ActionButton offline;
    public ActionButton deathMatchOffline;
    public ActionButton sabotageOffline;
	public ActionButton settings;
	public ActionButton exit;

    [SerializeField]
    private CharacterSelectController csController;

    bool dontReset= false;

	protected override void SetActions() {

        this.csController.onTranstionToMainMenu = this.TransitionOutOfCharacterSelect;

		offline.SetAction(() => {
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
            FindObjectOfType<PlayerSelectSettings>().gameType = GameTypeEnum.Sabotage;
            this.TransitionToCharacterSelect();
        });


		settings.SetAction(() => {
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
		actionSelectables.Add(offline.gameObject);        //commented out because this is not currently in use
		actionSelectables.Add(settings.gameObject);
		actionSelectables.Add(exit.gameObject);
	}


    void CloseOfflineSubmenu() {
        canReturn = false;
        if (offlineSubmenu.gameObject.GetActive()) {
            offlineSubmenu.gameObject.SetActive(false);
            actionSelectables.RemoveAt(actionSelectables.IndexOf(deathMatchOffline.gameObject));
            actionSelectables.RemoveAt(actionSelectables.IndexOf(sabotageOffline.gameObject));
            index = 0;
            dontReset = true;
        }
    }


    public void ResetMenu() {

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

    private void TransitionOutOfCharacterSelect() {
        this.gameObject.SetActive(true);
    }

}
