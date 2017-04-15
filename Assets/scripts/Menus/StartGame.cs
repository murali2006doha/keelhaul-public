using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class StartGame : MonoBehaviour {

    public Transform initialText;
    public bool withKeyboard;

    ControllerSelect cc;
    PlayerActions actions;
	bool notStarted = true;

    // Use this for initialization
    void Start () {
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

		if (notStarted && actions != null && actions.Green.WasReleased) {
            initialText.gameObject.SetActive (false);
            FindObjectOfType<MenuModel>().mainMenu.Initialize (actions, () => {
                initialText.gameObject.SetActive (true);
            });
			notStarted = false;
        }
    }   


    public void SignIn() {
        if (cc.players.Count == 1) {
            this.actions = (PlayerActions)cc.players [0];
        }
    }
}
