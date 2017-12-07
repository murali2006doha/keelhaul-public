using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public abstract class AbstractModalComponent : MonoBehaviour {


    protected const float volumeChange = 0.1f;
    protected PlayerActions actions;
    protected AbstractGameManager gm;
    protected List<GameObject> actionSelectables = new List<GameObject>();
    protected int index = 0;
    protected bool interactable = true;
    protected bool canMoveDown = true;
    protected bool canMoveUp = true;
    protected bool canMoveLeft  = true;
    protected bool canMoveRight = true;


    public Animator modalAnimator;
    public Action pushAction;           //pushed to stack
    public Action popAction;            //pops from stack
    public Boolean isActive = true; 


    /// <summary>
    /// DOACTION(); method performs the action that is the value of the selected buttons (this could include push or pop actions
    /// as part of it)
    /// 
    /// PUSHACTION; called when a new modal shows up. It is added as a value to the dictionary of buttons and their actions
    /// depending on what button it is. E.g. pushAction is not an action for the RESUME, but it is an action for 
    /// EXIT TO MENU (notification pop up). no need to call this if a new modal will not be initialized
    /// 
    /// POPACTION; is called when BACK is pressed. It is ALSO added as a value to the dictionary for some special buttons
    /// such as YES and NO on the notification modal or RESUME on the pause menu
    /// </summary>
    /// <param name="actions">Actions.</param>
    //this is CALLED from MODALSTACK only
    public abstract void SetupModal(PlayerActions actions);

    /// Determines whether this instance can control.
    protected abstract bool CanControl();

    void Start() {
    }

    void Update() {
        
        if (CanControl()) {
            Control (); 
        }
    }


    public void Control() {
        Navigate ();  

        if (AnyInputEnterWasReleased()) {
            this.DoAction (); 
        } else if (AnyInputBackWasReleased()) { 
            this.popAction ();
            this.GoBack (); //use this once all the modals have an animation
        }

    }


    public void Navigate() {
        NavigateModal (actionSelectables.ToArray ());
        NavigateModalWithMouse ();
    }


    public PlayerActions GetActions() {
        return actions;
    }


    //do action might contain closeAction or openAction depending on what type of button is pressed
    public void DoAction() {
        if (interactable) {
            this.actionSelectables [index].GetComponent<ActionSelectable>().doAction ();
        }
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
        } else {
            if (AnyInputLeftWasReleased()) {
                index = ListIterator.GetPositionIndex (passedInButtons.Length, index, "left");
            }

            if (AnyInputRightWasReleased()) {
                index = ListIterator.GetPositionIndex (passedInButtons.Length, index, "right");
            }
        }
    }


    void NavigateVolumeSlider () {

        if (null == actions || null == actions.Device) {
            if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.A)) {
                this.actionSelectables [index].GetComponent<ActionSlider> ().doAction ();
            }
        } else {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
            }
            else if (canMoveLeft && actions.Device.LeftStickLeft.RawValue > 0.9f) {
                this.actionSelectables [index].GetComponent<ActionSlider>().doAction();
                canMoveLeft = false;
                Invoke("ResetLeftDelay", 0.2f);
            } else if (canMoveLeft && actions.Device.DPadLeft.IsPressed) {
                this.actionSelectables[index].GetComponent<ActionSlider>().SliderComponent.value -= volumeChange;
                this.actionSelectables [index].GetComponent<ActionSlider>().doAction();
                canMoveLeft = false;
                Invoke("ResetLeftDelay", 0.2f);
            }
        }


        if (null == actions || null == actions.Device) {
            if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.D)) {
                this.actionSelectables[index].GetComponent<ActionSlider> ().doAction ();
            } 
        } else {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
            }
            else if (canMoveRight && actions.Device.LeftStickRight.RawValue > 0.9f) {
                this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
                canMoveRight = false;
                Invoke("ResetRightDelay", 0.2f);
            } else if (canMoveRight && actions.Device.DPadRight.IsPressed) {
                this.actionSelectables[index].GetComponent<ActionSlider>().SliderComponent.value += volumeChange;
                this.actionSelectables [index].GetComponent<ActionSlider>().doAction();
                canMoveRight = false;
                Invoke("ResetRightDelay", 0.2f);
            }
        }
    }

    public void GoBack() {
        //Exit();
        DestroyObject(this.gameObject);  
    }


    public void Exit() {
        modalAnimator.Play ("ModalExit");
        Invoke("Destroy", 1f);
    }


    void Destroy() {
        Destroy(this.gameObject);
    }


    bool AnyInputUpWasReleased() {

		if (null == actions || null == actions.Device) {
            if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
                return true;
            }
        } else if (canMoveUp && (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W) || 
                                 actions.Device.LeftStickUp.RawValue > 0.9f || actions.Device.DPadUp.IsPressed)) {
            canMoveUp = false;
            Invoke("ResetUpDelay", 0.1f);
            return true;
        }

        return false;
    }


    bool AnyInputDownWasReleased() {

		if (null == actions || null == actions.Device) {
            if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
                return true;
            }
        } else if (canMoveDown && (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S) ||
                                   actions.Device.LeftStickDown.RawValue > 0.9f || actions.Device.DPadDown.IsPressed)) {
            canMoveDown = false;
            Invoke("ResetDownDelay", 0.1f);
            return true;
        }

        return false;
    }


    bool AnyInputRightWasReleased() {
        if (null == actions || null == actions.Device) {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                return true;
            }
        }
        else if (canMoveRight && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || 
                                  actions.Device.LeftStickRight.RawValue > 0.9f || actions.Device.DPadRight.IsPressed)) {
            canMoveRight = false;
            Invoke("ResetRightDelay", 0.1f);
            return true;
        }

        return false;
    }


    bool AnyInputLeftWasReleased() {

    	if (null == actions || null == actions.Device) {
    		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
    			return true;
    		}
    	}
    	else if (canMoveLeft && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || 
                                 actions.Device.LeftStickLeft.RawValue > 0.9f || actions.Device.DPadLeft.IsPressed)) {
            canMoveLeft= false;
            Invoke("ResetLeftDelay", 0.1f);
			return true;
    	}

    	return false;
    }


    bool AnyInputEnterWasReleased() {

		if (null == actions || null == actions.Device) {
            if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.R) || Input.GetKeyDown (KeyCode.Space)) {
                return true;
            }
        } else if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.R) || Input.GetKeyDown (KeyCode.Space) || actions.Green.WasReleased) {
            return true;
        }

        return false;
    }


    bool AnyInputBackWasReleased() {

		if (null == actions || null == actions.Device) {
            if (Input.GetKeyUp (KeyCode.Escape)) {
                return true;
            }
        } else if (Input.GetKeyUp (KeyCode.Escape) || actions.Red.WasReleased || actions.Start.WasReleased) {
            return true;
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

