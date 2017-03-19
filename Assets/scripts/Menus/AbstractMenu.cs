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

	protected PlayerActions actions;
	protected List<ActionButton> actionButtons = new List<ActionButton>();
	protected Action onReturnAction;
	protected bool canReturn = true;
	protected int index = 0;

	/// <summary>
	/// Initialize the specified actions and goBackAction.
	/// <"actions">Actions
	/// <"goBackAction"> The action for the previous menu when back is pressed
	public void initialize (PlayerActions actions, Action goBackAction) {
		this.gameObject.SetActive (true);
		this.actions = actions;
		this.onReturnAction = goBackAction;
		this.canReturn = true;
	}


	// Update is called once per frame
	void Update () {
		if (actions.Green.WasReleased) { 
			this.DoAction ();  
		}
		if (actions.Red.WasReleased && canReturn) {
			goBack ();
		} 
		NavigateModal (actionButtons.ToArray ());
	}


	public abstract void setButtonsToActions();

		
	public void Exit() {
		Application.Quit();
	}

	//do action might contain closeAction or openAction depending on what type of button is pressed
	public void DoAction() {
		this.actionButtons [index].doAction ();
	}

	public void goBack() {
		onReturnAction ();		
		this.gameObject.SetActive (false);
	}


	public void ToggleButtons() {
		foreach (ActionButton b in actionButtons) {
			b.ButtonComponent.interactable = !b.ButtonComponent.interactable;
		}
	}








	public void NavigateModal (ActionButton[] passedInButtons) { //navigating main menu  
		print(passedInButtons [index].gameObject);
		passedInButtons [index].gameObject.GetComponent<Button>().Select ();

		if (actions.Down.WasReleased || actions.R_Down.RawValue > 0.5f) {
			index = GetPositionIndex (passedInButtons, index, "down");
		}

		if (actions.Up.WasReleased || actions.R_Up.RawValue > 0.5f) {
			index = GetPositionIndex (passedInButtons, index, "up");
		}

		if (actions.Right.WasReleased || actions.R_Right.RawValue > 0.5f) {
			index = GetPositionIndex (passedInButtons, index, "right");
		}

		if (actions.Left.WasReleased || actions.R_Left.RawValue > 0.5f) {
			index = GetPositionIndex (passedInButtons, index, "left");
		}

	}


	private int GetPositionIndex (ActionButton[] items, int item, string direction) {
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

}

