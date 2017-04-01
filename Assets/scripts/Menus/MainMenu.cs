using UnityEngine.EventSystems;
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



	// Use this for initialization
	void Start () {

		SetActions ();
		actionSelectables.Add (online.gameObject);
		actionSelectables.Add (offline.gameObject);
		//actionSelectables.Add (settings.gameObject);
		actionSelectables.Add (exit.gameObject);
	
	}


	public override void Navigate() {
		NavigateModal (actionSelectables.ToArray ());
		NavigateModalWithMouse ();
	}


	public override void SetActions () {

		online.SetAction (() => {
			this.enabled = false;
			ToggleButtons();
			FindObjectOfType<MenuModel>().onlineModeMenu.Initialize(this.actions, () => {
				ToggleButtons(); 
				this.enabled = true;
			});
			FindObjectOfType<MenuModel>().onlineModeMenu.isOnline = true;
		});
		

		offline.SetAction (() =>  {
			this.enabled = false;
			ToggleButtons();
			FindObjectOfType<MenuModel>().offlineModeMenu.Initialize(this.actions, () => {
				ToggleButtons(); 
				this.enabled = true;
			});
			FindObjectOfType<MenuModel>().offlineModeMenu.isOnline = false;
		});


		settings.SetAction (() => {
			ToggleButtons(); 
			this.gameObject.SetActive (false);
			FindObjectOfType<MenuModel> ().settingsMenu.Initialize (actions, () => {
				ToggleButtons(); 
				this.gameObject.SetActive (true);
			});
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
