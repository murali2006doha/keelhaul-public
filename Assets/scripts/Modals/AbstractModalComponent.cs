using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public abstract class AbstractModalComponent : MonoBehaviour {


    protected PlayerActions actions;
    protected AbstractGameManager gm;
    protected Dictionary<Button, Action> buttonToAction = new Dictionary<Button, Action>();
    protected int index;
    protected Button[] buttons;

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

    public abstract void initializeModal(PlayerActions actions);


    public void control() {
        navigateModal (buttons);  

        if (this.getActions ().Green.WasReleased) { 
            this.doAction ();   
        } else if (this.getActions ().Red.WasReleased) { 
            this.popAction ();
        }

    }


    public PlayerActions getActions() {
        return actions;
    }


    public Button getButtons() {
        return new List<Button> (buttonToAction.Keys).ToArray () [index];
    }

    //do action might contain closeAction or openAction depending on what type of button is pressed
    public void doAction() {
        this.buttonToAction [new List<Button> (buttonToAction.Keys).ToArray ()[index]]();
    }

    //for mouse
    public void doAction(int i) {
        this.buttonToAction [new List<Button> (buttonToAction.Keys).ToArray ()[i]]();
    }



    public void toggleButtons() {
        foreach (Button b in buttons) {
            b.interactable = !b.interactable;
        }
    }
        
    public int getPositionIndex (Button[] items, int item, string direction) {
        if (direction == "up") {
            if (item == 0) {
                item = items.Length - 1;
            } else {
                item -= 1;
            }
        }

        if (direction == "down") {
            if (item == items.Length - 1) {
                item = 0;
            } else {
                item += 1;
            }
        }

        if (direction == "right") {
            if (item == 0) {
                item = items.Length - 1;
            } else {
                item -= 1;
            }
        }

        if (direction == "left") {
            if (item == items.Length - 1) {
                item = 0;
            } else {
                item += 1;
            }
        }

        return item;
    }


    public void navigateModal (Button[] buttonsX) { //navigating main menu  

        buttonsX [index].Select ();

        if (actions.Down.WasReleased || actions.R_Down.RawValue > 0.5f) {
            index = getPositionIndex (buttonsX, index, "down");
        }

        if (actions.Up.WasReleased || actions.R_Up.RawValue > 0.5f) {
            index = getPositionIndex (buttonsX, index, "up");
        }

        if (actions.Right.WasReleased || actions.R_Right.RawValue > 0.5f) {
            index = getPositionIndex (buttonsX, index, "right");
        }

        if (actions.Left.WasReleased || actions.R_Left.RawValue > 0.5f) {
            index = getPositionIndex (buttonsX, index, "left");
        }

    }

    public void goBack() {
        StartCoroutine (exit ());
    }

    private IEnumerator exit() {
        //this.gameObject.SetActive (false);
        modalAnimator.Play ("ModalExit");
        yield return new WaitForSeconds(1.0f); 
    }


    public void setUpMouseControls() {

        int i = 0;
        foreach (Button b in buttons) {
            buttons[i].onClick.AddListener(delegate {doAction(i);});//adds a listener for when you click the button
            i++;
        }
    }

}

