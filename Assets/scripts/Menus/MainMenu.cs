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
	public ActionButton offline;
	public ActionButton settings;
	public ActionButton exit;



	protected override void SetActions() {

		online.SetAction(() => {
			this.enabled = false;
			ToggleSelectables();
			FindObjectOfType<MenuModel>().onlineModeMenu.Initialize(this.actions, () => {
				ToggleSelectables();
				this.enabled = true;
			});
			FindObjectOfType<MenuModel>().onlineModeMenu.isOnline = true;
		});


		offline.SetAction(() => {
			this.enabled = false;
			ToggleSelectables();
			FindObjectOfType<MenuModel>().offlineModeMenu.Initialize(this.actions, () => {
				ToggleSelectables();
				this.enabled = true;
			});
			FindObjectOfType<MenuModel>().offlineModeMenu.isOnline = false;
		});


		settings.SetAction(() => {

			this.enabled = false;
			ToggleSelectables();
			//this.gameObject.SetActive(false);
			FindObjectOfType<MenuModel>().settingsMenu.Initialize(actions, () => {

				this.enabled = true;
				ToggleSelectables();
				//this.gameObject.SetActive(true);
			});
		});


		Dictionary<ModalActionEnum, Action> modalActions = new Dictionary<ModalActionEnum, Action>();
		modalActions.Add(ModalActionEnum.onOpenAction, () => {
			this.enabled = false;
			ToggleSelectables();
			//this.gameObject.SetActive(false);
			canReturn = false;
		});
		modalActions.Add(ModalActionEnum.onCloseAction, () => {
			this.enabled = true;
			ToggleSelectables();
			//this.gameObject.SetActive(true);
			canReturn = true;
		});

		exit.SetAction(() => {

			ModalStack.InitializeModal(this.actions, ModalsEnum.notificationModal, modalActions);
			FindObjectOfType<NotificationModal>().Spawn("Exit to Desktop?", Color.yellow, "Yes", "No", () => {
				Exit();
			}, () => {
				exit.ButtonComponent.Select();
			});
		});
	}


	void resetMenu() {

		this.Initialize(actions, () => {
		});
	}


	protected override void SetActionSelectables() {
		actionSelectables.Add(online.gameObject);
		actionSelectables.Add(offline.gameObject);        //commented out because this is not currently in use
		actionSelectables.Add(settings.gameObject);
		actionSelectables.Add(exit.gameObject);
	}
}
