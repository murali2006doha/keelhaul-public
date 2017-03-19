using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;
using System;

public class MainMenu : AbstractMenu {

	public ActionButton online;
	public ActionButton offline;
	public ActionButton settings;
	public ActionButton exit;
	public OnlineModeMenu onlineModeMenu;


	// Use this for initialization
	void Start () {

		setButtonsToActions ();
		actionButtons.Add (online);
		//actionButtons.Add (offline);
		actionButtons.Add (settings);
		actionButtons.Add (exit);
	}


	public override void setButtonsToActions () {

		online.SetAction (() => {
			this.enabled = false;
			ToggleButtons();
			onlineModeMenu.initialize(this.actions, () => {
				ToggleButtons(); 
				this.enabled = true;
			});
		});
		

		offline.SetAction (() =>  {
			this.gameObject.SetActive(false);
			//FindObjectOfType<OfflineModeMenu>().initialize(actions, () => {this.gameObject.SetActive (true);
		});

		settings.SetAction (() =>  {
			this.gameObject.SetActive(false);
			//FindObjectOfType<SettingsMenu>().initialize(actions, () => {this.gameObject.SetActive (true);
		});


		Dictionary<ModalActionEnum, Action> modalActions = new Dictionary<ModalActionEnum, Action> ();
		modalActions.Add (ModalActionEnum.onOpenAction, () => {ToggleButtons();});
		modalActions.Add (ModalActionEnum.onCloseAction, () => {ToggleButtons();});

		exit.SetAction (() =>  {
			canReturn = false;
			ModalStack.initialize (this.actions, ModalsEnum.notificationModal, modalActions);
			FindObjectOfType<NotificationModal> ().Initialize ("Exit to Desktop?", Color.yellow, "Yes", "No", () =>  {
				Exit ();
			}, () =>  {
				exit.ButtonComponent.Select ();
				canReturn = true;
			});
		});
	}
}
