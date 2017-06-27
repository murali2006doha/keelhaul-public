﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class MainMenuSetPieceAnimator : MonoBehaviour {

    ControllerSelect cc;
    PlayerActions actions;
    bool notStarted = true;

    [SerializeField]
    bool withKeyboard;

    [SerializeField]
    Animator mainMenuAnimator;

    [SerializeField]
    Animator krakenAnimator;



    [SerializeField]
    SkinnedMeshRenderer krakenMesh;
    [SerializeField]
    SeagullAnimator[] seagulls;


    [SerializeField]
    GameObject bubbles;


    [SerializeField]
    GameObject mainMenu;

    private bool skipped = false;

    public void Start() {
        cc = GameObject.FindObjectOfType<ControllerSelect>();
        cc.withKeyboard = withKeyboard;
        cc.listening = true;

        System.Random rand = new System.Random();
        foreach (SeagullAnimator gull in seagulls) {
            gull.Initialize(rand);
        }
    }

    public void Update() {
        SignIn();
        if (notStarted) {
            OpenMenu();
        }
        else if (!notStarted) {
			LoadMenu();
        }   
    }

    public void SubmergeKraken()
    {
        krakenMesh.enabled = false;
        this.krakenAnimator.SetBool("submerge", true);
    }

    public void EmergeKraken() {

        krakenAnimator.SetFloat("speed", 0.5f);
        krakenMesh.enabled = true;
        this.krakenAnimator.SetBool("underShip", true);
        this.krakenAnimator.SetBool("submerge", false);
    }

    public void BubblesAndSeagulls() {
        bubbles.SetActive(true);
        foreach (SeagullAnimator seagull in seagulls)
        {
            seagull.RandomTakeOff();
        }
    }

  public void SkipAnimation() {
    if (skipped) {
      return;
    }
    foreach (SeagullAnimator seagull in seagulls) {
      seagull.gameObject.SetActive (false);
    }
    this.skipped = true;
    this.krakenAnimator.SetBool("underShip", false);
    this.krakenAnimator.SetBool("submerge", false);
    this.mainMenuAnimator.SetTrigger ("skip");
  
  }

    void OpenMenu() {
        if (AnyInputEnterWasReleased() && this.actions != null) {
            this.SkipAnimation();
            notStarted = false;
        }
    }


    void LoadMenu() {
        if (mainMenu.gameObject.GetActive()) {
            FindObjectOfType<MenuModel>().mainMenu.Initialize(this.actions, () => {
                FindObjectOfType<MainMenu>().ResetMenu();
                LoadMenu();
            });
        }
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
