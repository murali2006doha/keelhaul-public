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
    public MainMenu mainMenu;

    ControllerSelect cc;
    PlayerActions actions;


    // Use this for initialization
    void Start () {
        cc = GameObject.FindObjectOfType<ControllerSelect> ();
        cc.withKeyboard = withKeyboard;
        mainMenu.gameObject.SetActive (false);
    }

    // Update is called once per frame
    void Update () {

        if (this.gameObject.activeSelf) {
            cc.listening = true;
            signIn ();
        }

        if (actions != null && actions.Green.WasReleased) {
            initialText.gameObject.SetActive (false);
            mainMenu.initialize (actions, () => {
                initialText.gameObject.SetActive (true);
            });
        }
    }   


    public void signIn() {
        if (cc.players.Count == 1) {
            this.actions = (PlayerActions)cc.players [0];
        }
    }
}
