using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;

public class MainMenuSetPieceAnimator : MonoBehaviour {

    ControllerSelect cc;
    PlayerActions actions;
    bool notStarted = true;
  bool holding = false;

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
    private bool loaded = false;

    public void Start() {
        actions = PlayerActions.CreateAllControllerBinding();


        System.Random rand = new System.Random();
        foreach (SeagullAnimator gull in seagulls) {
            gull.Initialize(rand);
        }
    }

    public void Update() {
        if (notStarted && AnyInputWasPressed() && this.actions != null) {
            this.SkipAnimation();
            notStarted = false;
        }
        else if (!notStarted && !this.loaded) {
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
	krakenMesh.enabled = true;
	this.krakenAnimator.SetBool("underShip", false);
    this.krakenAnimator.SetBool("submerge", false);
    this.mainMenuAnimator.SetTrigger ("skip");
    this.mainMenu.gameObject.SetActive(true);

    }

    void OpenMenu() {
        if (AnyInputWasPressed() && this.actions != null) {
            this.SkipAnimation();
            notStarted = false;
        }
    }


    void LoadMenu() {
        this.loaded = true;
        this.mainMenu.gameObject.SetActive(true);
        if (mainMenu.gameObject.GetActive()) {
            FindObjectOfType<MenuModel>().mainMenu.Initialize(this.actions,true, () => {
                FindObjectOfType<MainMenu>().ResetMenu();
                LoadMenu();
            });
        }
    }


  bool AnyInputWasPressed()
      {
    
      if (Input.anyKey)
      {
        holding = true;
      }

    if (!Input.anyKey && holding)
      {
      holding = false;
      return true;

      }





        foreach (InputDevice device in InputManager.Devices)
        {
        if (device.AnyButtonWasReleased)
          {
            return true;
          }
        }

        return false;
      }



}
