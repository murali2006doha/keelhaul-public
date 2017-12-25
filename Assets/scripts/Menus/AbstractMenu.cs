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
    protected bool dontDestroy;

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
    }

    public void Initialize(PlayerActions actions, bool dontDestroy,Action goBackAction)
    {
        this.gameObject.SetActive(true);
        this.actions = actions;
        this.onReturnAction = goBackAction;
        this.dontDestroy = dontDestroy;
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
            index = NavigationUtils.NavigateModal(this.actions, actionSelectables.ToArray(), index);
        }
        index = NavigationUtils.NavigateModalWithMouse(actionSelectables, index);
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

    public virtual void GoBack() {
        if (!dontDestroy)
        {
            this.gameObject.SetActive (false);
        }
        onReturnAction ();      
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


    bool AnyInputEnterWasReleased() {
        if (actions !=null) {
            if (actions.Green.WasReleased) {
                return true;
            }
        }

    	return false;
    }


    public bool AnyInputBackWasReleased() {

    	if (Input.GetKeyUp(KeyCode.Escape) || (actions !=null && (actions.Red.WasReleased))) {
    		return true;
    	}

    	return false;
    }
}

