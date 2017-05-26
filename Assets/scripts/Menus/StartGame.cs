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

    // Use this for initialization
    void Start () {
        Cursor.visible = true;
        cc = GameObject.FindObjectOfType<ControllerSelect> ();
        cc.withKeyboard = withKeyboard;
        FindObjectOfType<MenuModel>().mainMenu.gameObject.SetActive (false);
    }

    // Update is called once per frame
    void Update () {
       
        if (this.gameObject.activeSelf) {
            cc.listening = true;
            SignIn ();
        }

		openMenu();
    }


	void openMenu()
	{
		//if (notStarted && AnyInputEnterWasReleased()) { 
            FindObjectOfType<MenuModel>().mainMenu.Initialize (actions, () => {
                FindObjectOfType<MainMenu>().ResetMenu();
				openMenu();
            });
            //notStarted = false;
        //}
	}


    public void SignIn() {
        if (cc.players.Count == 1) {
            this.actions = (PlayerActions)cc.players [0];
        }
    }


    bool AnyInputEnterWasReleased() {
        if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.R)) {
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
