﻿using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;
using System;



/// <summary>
/// InControl -> Default Unity
/// Green/R -> Mouse and Space
/// Red/Space -> 
/// </summary>

public abstract class AbstractMenu : MonoBehaviour
{

    protected const float volumeChange = 0.1f;
    protected PlayerActions actions;
    protected List<GameObject> actionSelectables = new List<GameObject>();
    protected Action onReturnAction;
    protected bool canReturn = true;
    protected bool interactable = true;
    protected int index = 0;
    protected bool canMoveUp = true;
    protected bool canMoveDown = true;
    protected bool canMoveLeft = true;
    protected bool canMoveRight = true;
    protected float analogStickDelay = 0.15f;

    void Start() {
        SetActions();
        SetActionSelectables();
    }

    protected abstract void SetActions();
    protected abstract void SetActionSelectables();



    /// <summary>
    /// Initialize the specified actions and goBackAction.
    /// <"actions">Actions
    /// <"goBackAction"> The action for the previous menu when back is pressed
    public void Initialize (PlayerActions actions, Action goBackAction) {
        this.gameObject.SetActive (true);
        this.actions = actions;
        this.onReturnAction = goBackAction;
        this.canReturn = true;
    }



    // Update is called once per frame
    void Update () {
        Navigate ();
        if (AnyInputEnterWasReleased()) {  
            this.DoAction ();  
        }
        if (AnyInputBackWasReleased() && canReturn) {
            GoBack ();
        } 
    }


    public void Navigate() {
        NavigateModal (actionSelectables.ToArray ());
        NavigateModalWithMouse ();
    }

        
    public void Exit() {
        Application.Quit();
    }

    //do action might contain closeAction or openAction depending on what type of button is pressed
    public void DoAction() {
        if (interactable) {
            this.actionSelectables [index].GetComponent<ActionSelectable>().doAction ();
        }
    }

    public void GoBack() {
        onReturnAction ();      
        this.gameObject.SetActive (false);
    }


    public void ToggleSelectables() {
        foreach (GameObject b in actionSelectables) {
            if (b.GetComponent<ActionButton> ()) {
                b.GetComponent<ActionButton> ().ButtonComponent.interactable = !b.GetComponent<ActionButton> ().ButtonComponent.interactable;
            
            } else if (b.GetComponent<ActionSlider> ()) {
                b.GetComponent<ActionSlider> ().SliderComponent.interactable = !b.GetComponent<ActionSlider> ().SliderComponent.interactable;

            } else if (b.GetComponent<ActionToggle> ()) {
                b.GetComponent<ActionToggle> ().ToggleComponent.interactable = !b.GetComponent<ActionToggle> ().ToggleComponent.interactable;

            } else if (b.GetComponent<ActionDropDown> ()) {
                b.GetComponent<ActionDropDown> ().DropDownComponent.interactable = !b.GetComponent<ActionDropDown> ().DropDownComponent.interactable;

            }
        }
        interactable = !interactable;

    }


    public void NavigateModalWithMouse() {

        for (int i = 0; i < actionSelectables.Count; i++) {
            if (actionSelectables[i].GetComponent<ActionSelectable>().isMouseHovering ()) {
                index = i;
            }
        }
    }


    public void NavigateModal (GameObject[] passedInButtons) { //navigating main menu  
        if (passedInButtons.Length > 0) {
            passedInButtons [index].gameObject.GetComponent<Selectable> ().Select ();       
        }

        if (AnyInputDownWasReleased()) {
            index = ListIterator.GetPositionIndex (passedInButtons.Length, index, "down");
        }

        if (AnyInputUpWasReleased()) {
            index = ListIterator.GetPositionIndex (passedInButtons.Length, index, "up");
        }

        if (passedInButtons [index].gameObject.GetComponent<ActionSlider> ()) {
            NavigateVolumeSlider ();
        }

    }


    void NavigateVolumeSlider () {
        
        if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
            this.actionSelectables[index].GetComponent<ActionSlider> ().doAction ();  
        }

        foreach (InputDevice device in InputManager.Devices) {
            if (canMoveLeft && device.DPadLeft.IsPressed) {
                this.actionSelectables[index].GetComponent<ActionSlider>().SliderComponent.value -= volumeChange;
                this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
                canMoveLeft = false;
                Invoke("ResetLeftDelay", analogStickDelay);
            }
            else if (canMoveLeft && device.LeftStickLeft.RawValue > 0.9f) {
                this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
                canMoveLeft = false;
                Invoke("ResetLeftDelay", analogStickDelay);
            }
        }

        if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
            this.actionSelectables[index].GetComponent<ActionSlider> ().doAction ();
        }

        foreach (InputDevice device in InputManager.Devices) {
            if (canMoveRight && device.DPadRight.IsPressed) {
                this.actionSelectables[index].GetComponent<ActionSlider>().SliderComponent.value += volumeChange;
                this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
                canMoveRight = false;
                Invoke("ResetRightDelay", analogStickDelay);
            } else if (canMoveRight && device.LeftStickRight.RawValue > 0.9f) {
                this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
                canMoveRight = false;
                Invoke("ResetRightDelay", analogStickDelay);
            }
        }
    }


    bool AnyInputUpWasReleased() {
		if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
            return true;
        }

        foreach (InputDevice device in InputManager.Devices) {
            if (canMoveUp && device.LeftStickUp.IsPressed && device.LeftStickUp.RawValue > 0.9f) {
                canMoveUp = false;
                Invoke("ResetUpDelay", analogStickDelay);
                return true;
            } else if (device.DPadUp.WasPressed) {
                return true;
            }
        }

        return false;
    }

    bool AnyInputDownWasReleased() {
        if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
            return true;
        }

        foreach (InputDevice device in InputManager.Devices) {
            if (canMoveDown && device.LeftStickDown.IsPressed && device.LeftStickDown.RawValue > 0.9f) {
                canMoveDown = false;
                Invoke("ResetDownDelay", analogStickDelay);
                return true;
            }
            else if (device.DPadDown.WasPressed) {
                return true;
            }
        }

        return false;
    }

    bool AnyInputEnterWasReleased() {
        if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.R) || Input.GetKeyDown (KeyCode.Space)) {
            return true;
        }

        foreach (InputDevice device in InputManager.Devices) {
            if (device.Action1.WasReleased) {
                return true;
            }
        }

        return false;
    }

    bool AnyInputBackWasReleased() {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            return true;
        }

        foreach (InputDevice device in InputManager.Devices) {
            if (device.Action2.WasReleased) {
                return true;
            }
        }

        return false;
    }


    void ResetUpDelay() {
	    canMoveUp = true;
    }

    void ResetDownDelay() {
	    canMoveDown = true;
    }

    void ResetRightDelay() {
        canMoveRight = true;
    }

    void ResetLeftDelay() {
        canMoveLeft = true;
    }
}

