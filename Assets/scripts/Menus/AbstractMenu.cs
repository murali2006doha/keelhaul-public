using UnityEngine;
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


    public virtual void Navigate() {
        if (actionSelectables.Count > 0) {
            NavigateModal(actionSelectables.ToArray());
        }
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

        if (passedInButtons[index].gameObject.GetComponent<ActionSlider>()) {
            NavigateVolumeSlider();
        } else {
            if (AnyInputRightWasReleased()) {
                index = ListIterator.GetPositionIndex (passedInButtons.Length, index, "down");
            }

            if (AnyInputLeftWasReleased()) {
                index = ListIterator.GetPositionIndex (passedInButtons.Length, index, "up");
            }
        }

    }

    void NavigateVolumeSlider() {

    	if (null == actions || null == actions.Device) {
    		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
    			this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
    		}
    	} else {
    		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
    			this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
    		} else if (canMoveLeft && actions.Device.LeftStickLeft.RawValue > 0.9f) {
    			this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
    			canMoveLeft = false;
    			Invoke("ResetLeftDelay", analogStickDelay);
    		} else if (canMoveLeft && actions.Device.DPadLeft.IsPressed) {
    			this.actionSelectables[index].GetComponent<ActionSlider>().SliderComponent.value -= volumeChange;
    			this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
    			canMoveLeft = false;
    			Invoke("ResetLeftDelay", analogStickDelay);
    		}
    	}


    	if (null == actions || null == actions.Device) {
    		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
    			this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
    		}
    	} else {
    		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
    			this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
    		} else if (canMoveRight && actions.Device.LeftStickRight.RawValue > 0.9f) {
    			this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
    			canMoveRight = false;
    			Invoke("ResetRightDelay", analogStickDelay);
    		} else if (canMoveRight && actions.Device.DPadRight.IsPressed) {
    			this.actionSelectables[index].GetComponent<ActionSlider>().SliderComponent.value += volumeChange;
    			this.actionSelectables[index].GetComponent<ActionSlider>().doAction();
    			canMoveRight = false;
    			Invoke("ResetRightDelay", analogStickDelay);
    		}
    	}
    }


    bool AnyInputUpWasReleased() {

    	if (null == actions || null == actions.Device) {
    		if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) {
    			return true;
    		}
    	} else {
    		if (canMoveUp && actions.Device.LeftStickUp.IsPressed && actions.Device.LeftStickUp.RawValue > 0.9f) {
    			canMoveUp = false;
    			Invoke("ResetUpDelay", analogStickDelay);
    			return true;
    		} else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) || actions.Device.DPadUp.WasPressed) {
    			return true;
    		}
    	}

    	return false;
    }


    bool AnyInputDownWasReleased() {

    	if (null == actions || null == actions.Device) {
    		if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)) {
    			return true;
    		}
    	} else {
    		if (canMoveDown && actions.Device.LeftStickDown.IsPressed && actions.Device.LeftStickDown.RawValue > 0.9f) {
    			canMoveDown = false;
    			Invoke("ResetDownDelay", analogStickDelay);
    			return true;
    		} else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S) || actions.Device.DPadDown.WasPressed) {
    			return true;
    		}

    	}

    	return false;
    }


    bool AnyInputRightWasReleased() {
    	if (null == actions || null == actions.Device) {
    		if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)) {
    			return true;
    		}
    	} else {
    		if (canMoveRight && actions.Device.LeftStickRight.IsPressed && actions.Device.LeftStickRight.RawValue > 0.9f) {
    			canMoveRight = false;
    			Invoke("ResetRightDelay", analogStickDelay);
    			return true;
    		} else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) || actions.Device.DPadRight.WasPressed) {
    			return true;
    		}
    	}

    	return false;
    }


    bool AnyInputLeftWasReleased() {

    	if (null == actions || null == actions.Device) {
    		if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A)) {
    			return true;
    		}
    	} else {
    		if (canMoveLeft && actions.Device.LeftStickLeft.IsPressed && actions.Device.LeftStickLeft.RawValue > 0.9f) {
    			canMoveLeft = false;
    			Invoke("ResetLeftDelay", analogStickDelay);
    			return true;
    		} else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) || actions.Device.DPadLeft.WasPressed) {
    			return true;
    		}
    	}

    	return false;
    }


    bool AnyInputEnterWasReleased() {
        if (null != actions.Device) {
            if (actions.Device.Action1.WasReleased) {
                return true;
            }
        }

    	return false;
    }


    public bool AnyInputBackWasReleased() {

    	if (null == actions || null == actions.Device) {
    		if (Input.GetKeyUp(KeyCode.Escape)) {
    			return true;
    		}
    	} else if (Input.GetKeyUp(KeyCode.Escape) || actions.Red.WasReleased || actions.Start.WasReleased) {
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

