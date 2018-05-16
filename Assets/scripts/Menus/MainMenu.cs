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
    public ActionButton settings;
	public ActionButton exit;

  public Transform background;

    bool dontReset= false;

	protected override void SetActions() {

		offline.SetAction(() => {
            Cursor.visible = false;
            canReturn = true;
            this.enabled = false;
            ToggleSelectables();
            FindObjectOfType<MenuModel>().offlineSubmenu.Initialize(this.actions, () => {
                index = actionSelectables.IndexOf(offline.gameObject);
                this.enabled = true;
                ToggleSelectables();
                Cursor.visible = true;
            });
		});


        settings.SetAction(() => {
            canReturn = true;
            this.enabled = false;
			ToggleSelectables();
      background.gameObject.SetActive(true);
			FindObjectOfType<MenuModel>().settingsMenu.Initialize(this.actions, () => {
                index = actionSelectables.IndexOf(settings.gameObject);
				this.enabled = true;
                ToggleSelectables();
        background.gameObject.SetActive(false);
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


    public void ResetMenu() {

        if(!dontReset)
            index = 0;
        canReturn = false;
        dontReset = false;
    }


}
