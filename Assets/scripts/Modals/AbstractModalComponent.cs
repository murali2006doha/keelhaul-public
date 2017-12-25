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
        if (actionSelectables.Count > 0)
        {
            index = NavigationUtils.NavigateModal(this.actions, actionSelectables.ToArray(), index);
        }
        index = NavigationUtils.NavigateModalWithMouse(actionSelectables, index);
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


    bool AnyInputEnterWasReleased() {
        return actions.Start.WasReleased || actions.Green.WasReleased;
    }


    bool AnyInputBackWasReleased() {
        return actions.Red.WasReleased;
    }


}

