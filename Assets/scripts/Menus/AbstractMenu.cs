using UnityEngine.EventSystems;
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

		if (actions.Green.WasReleased) { 
            this.DoAction ();  
        }
        if (actions.Red.WasReleased && canReturn) {
            GoBack ();
        } 
    }


    public abstract void Navigate ();
    public abstract void SetActions();

        
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

        if (actions.Down.WasReleased) {
            index = GetPositionIndex (passedInButtons.Length, index, "down");
        }

        if (actions.Up.WasReleased) {
            index = GetPositionIndex (passedInButtons.Length, index, "up");
        }

		if (passedInButtons [index].gameObject.GetComponent<ActionSlider> ()) {
			NavigateSlider ();
		}

    }


	void NavigateSlider () {
		if (actions.Left.WasReleased) {
			this.actionSelectables [index].GetComponent<ActionSlider> ().SliderComponent.value -= volumeChange * Time.deltaTime;
			this.actionSelectables [index].GetComponent<ActionSlider> ().doAction ();
		}
		if (actions.Right.WasReleased) {
			this.actionSelectables [index].GetComponent<ActionSlider> ().SliderComponent.value += volumeChange * Time.deltaTime;
			this.actionSelectables [index].GetComponent<ActionSlider> ().doAction ();
		}
	}


    private int GetPositionIndex (int length, int item, string direction) {
        if (direction == "up") {
            if (item == 0) {
                item = length - 1;
            } else {
                item -= 1;
            }
        }

        if (direction == "down") {
            if (item == length - 1) {
                item = 0;
            } else {
                item += 1;
            }
        }

        return item;
    }


}

