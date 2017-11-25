using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class NavigationUtils : MonoBehaviour {

    const float volumeChange = 0.1f;
    bool canMoveDown = true; 
    bool canMoveUp = true;
    bool canMoveLeft = true;
    bool canMoveRight = true;
    float analogStickDelay = 0.15f;


    public int NavigateModalWithMouse(List<GameObject> actionSelectables, int index) {

    	for (int i = 0; i < actionSelectables.Count; i++) {
    		if (actionSelectables[i].GetComponent<ActionSelectable>().isMouseHovering()) {
    			index = i;
    		}
        }

        return index;
    }

    public int NavigateModal(PlayerActions actions, GameObject[] passedInButtons, int index) { //navigating main menu  
        if (actions == null)
        {
            return index;
        }
        if (passedInButtons.Length > 0) {
    		passedInButtons[index].gameObject.GetComponent<Selectable>().Select();
    	}
    	if (AnyInputDownWasReleased(actions)) {
    		index = ListIterator.GetPositionIndex(passedInButtons.Length, index, "down");
    	}

    	if (AnyInputUpWasReleased(actions)) {
            index = ListIterator.GetPositionIndex(passedInButtons.Length, index, "up");
    	}

       


        if (passedInButtons[index].gameObject.GetComponent<ActionSlider>()) {
    		NavigateVolumeSlider(actions, passedInButtons, index);
    	} else if (!(AnyInputDownWasReleased(actions) && AnyInputUpWasReleased(actions)))
        {
            if (AnyInputLeftWasReleased(actions))
            {
                index = ListIterator.GetPositionIndex(passedInButtons.Length, index, "left");
            }

            if (AnyInputRightWasReleased(actions))
            {
                index = ListIterator.GetPositionIndex(passedInButtons.Length, index, "right");
            }
        }

        return index;
    }


    void NavigateVolumeSlider(PlayerActions actions, GameObject[] actionSelectables, int index) {
    	
    	if (AnyInputLeftWasReleased(actions) || AnyInputRightWasReleased(actions)) {
    		actionSelectables[index].GetComponent<ActionSlider>().doAction();
    	}
    	
    }

    bool AnyInputUpWasReleased(PlayerActions actions) {
        return (actions.Up.WasPressed || (actions.Up.RawValue > 0.9 && actions.Up.WasRepeated));
 
    }

    bool AnyInputDownWasReleased(PlayerActions actions) {
        return (actions.Down.WasPressed || actions.Down.RawValue > 0.9 && actions.Down.WasRepeated);
    }


    bool AnyInputRightWasReleased(PlayerActions actions) {
        return actions.Right.RawValue > 0.9 && (actions.Right.WasPressed || actions.Right.WasRepeated);
    }


    bool AnyInputLeftWasReleased(PlayerActions actions) {
        return actions.Left.RawValue > 0.9 && (actions.Left.WasPressed ||  actions.Left.WasRepeated);
    }

    public void ToggleSelectables(List<GameObject> actionSelectables, bool interactable) {
    	foreach (GameObject b in actionSelectables) {
    		if (b.GetComponent<ActionButton>()) {
    			b.GetComponent<ActionButton>().ButtonComponent.interactable = !b.GetComponent<ActionButton>().ButtonComponent.interactable;

    		} else if (b.GetComponent<ActionSlider>()) {
    			b.GetComponent<ActionSlider>().SliderComponent.interactable = !b.GetComponent<ActionSlider>().SliderComponent.interactable;

    		} else if (b.GetComponent<ActionToggle>()) {
    			b.GetComponent<ActionToggle>().ToggleComponent.interactable = !b.GetComponent<ActionToggle>().ToggleComponent.interactable;

    		} else if (b.GetComponent<ActionDropDown>()) {
    			b.GetComponent<ActionDropDown>().DropDownComponent.interactable = !b.GetComponent<ActionDropDown>().DropDownComponent.interactable;

    		}
    	}
    	interactable = !interactable;

    }

    IEnumerator ResetUpDelay() {
    	yield return StartCoroutine(CoroutineUtils.WaitForRealTime(analogStickDelay));
    	canMoveUp = true;
    }

    IEnumerator ResetDownDelay() {
    	yield return StartCoroutine(CoroutineUtils.WaitForRealTime(analogStickDelay));
    	canMoveDown = true;
    }

    IEnumerator ResetRightDelay() {
    	yield return StartCoroutine(CoroutineUtils.WaitForRealTime(analogStickDelay));
    	canMoveRight = true;
    }

    IEnumerator ResetLeftDelay() {
    	yield return StartCoroutine(CoroutineUtils.WaitForRealTime(analogStickDelay));
    	canMoveLeft = true;
    }


}
