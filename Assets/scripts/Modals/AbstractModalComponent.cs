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

    public abstract void InitializeModal(PlayerActions actions);


    public void Control() {
        NavigateModal (buttons);  

		if (this.GetActions ().Green.WasReleased) {
            this.DoAction ();   
        } else if (this.GetActions ().Red.WasReleased) { 
            this.popAction ();
        }

    }


    public PlayerActions GetActions() {
        return actions;
    }


    public Button GetButtons() {
        return new List<Button> (buttonToAction.Keys).ToArray () [index];
    }

    //do action might contain closeAction or openAction depending on what type of button is pressed
    public void DoAction() {
        this.buttonToAction [new List<Button> (buttonToAction.Keys).ToArray ()[index]]();
    }

    //for mouse
    public void DoAction(int i) {
        this.buttonToAction [new List<Button> (buttonToAction.Keys).ToArray ()[i]]();
    }



    public void ToggleButtons() {
        foreach (Button b in buttons) {
            b.interactable = !b.interactable;
        }
    }


	public void NavigateModal (Button[] passedInButtons) { //navigating main menu  

		passedInButtons [index].Select ();

		if (actions.Down.WasReleased) {
			index = GetPositionIndex (passedInButtons, index, "down");
		}

		if (actions.Up.WasReleased) {
			index = GetPositionIndex (passedInButtons, index, "up");
		}
	}

        
	private int GetPositionIndex (Button[] items, int item, string direction) {
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

        return item;
    }



    public void GoBack() {
        StartCoroutine (Exit ());
    }

    private IEnumerator Exit() {
        //this.gameObject.SetActive (false);
        modalAnimator.Play ("ModalExit");
        yield return new WaitForSeconds(1.0f); 
		Destroy(this.gameObject);  

    }


    public void SetUpMouseControls() {

        int i = 0;
        foreach (Button b in buttons) {
            buttons[i].onClick.AddListener(delegate {DoAction(i);});//adds a listener for when you click the button
            i++;
        }
    }

}

