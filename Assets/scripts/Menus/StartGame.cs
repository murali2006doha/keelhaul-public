using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class StartGame : MonoBehaviour {

    public bool withKeyboard; 

    ControllerSelect cc;
    PlayerActions actions;
    bool notStarted = true;
    public Transform startScreen;

    // Use this for initialization
    void Start () {
        Cursor.visible = true;
        FindObjectOfType<MenuModel>().mainMenu.gameObject.SetActive (true);
    }


    void OpenMenu() {
        if (AnyInputEnterWasReleased() && this.actions != null) {
            startScreen.gameObject.SetActive(false);
            LoadMenu();
            notStarted = false;
        }
    }


    void LoadMenu() {
        FindObjectOfType<MenuModel>().mainMenu.Initialize(actions, () => {
            FindObjectOfType<MainMenu>().ResetMenu();
			LoadMenu();
        });
    }

    void SignIn() {

    	if (cc.players.Count >= 1) {
    		this.actions = (PlayerActions)cc.players[0];
            cc.listening = false;
    	}
    }

    bool AnyInputEnterWasReleased() {
    	if (Input.GetKeyUp(KeyCode.Return)) {
    		return true;
    	}
    	foreach (InputDevice device in InputManager.Devices) {
    		if (device.Action1.WasReleased) {
    			return true;
    		}
    	}

    	return false;
    }
}
