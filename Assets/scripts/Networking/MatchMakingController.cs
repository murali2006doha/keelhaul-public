using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;
using System;
using UnityEngine.SceneManagement;


public class MatchMakingController : MonoBehaviour
{

    [SerializeField]
    MatchMakingView view;
    // Use this for initialization

    bool canMoveUp = true;
    bool canMoveDown = true;
    Action<Dictionary<int, bool>> onMatchButtonClick;
    List<GameObject> actionSelectables = new List<GameObject>();
    int index = 0;
    public float analogStickDelay = 0.15f;

    public void Initiailze(Action<Dictionary<int, bool>> matchOptions) {

        view.Initialize(matchOptions);

        foreach (ActionSelectable selectable in view.GetMatchOptions()) {
            actionSelectables.Add(selectable.gameObject);
        }

        actionSelectables.Add(view.GetFindMatch().gameObject);

    }


    //// Update is called once per frame
    void Update() {
        Navigate();
        if (AnyInputEnterWasReleased()) {
            this.DoAction();
        } else if (AnyInputBackWasReleased()) {
            if (view.GetMatchOptions().TrueForAll(b => !b.ToggleComponent.isOn)) {
                GoToStartMenu();
            } else {
                ClearOptions();
            }
        }
    }

    public void Navigate() {
        NavigateModal(actionSelectables.ToArray());
        NavigateModalWithMouse();
    }



    public void NavigateModalWithMouse() {

        for (int i = 0; i < actionSelectables.Count; i++) {
            if (actionSelectables[i].GetComponent<ActionSelectable>().isMouseHovering()) {
                index = i;
            }
        }
    }


    public void NavigateModal(GameObject[] passedInButtons) { //navigating main menu  
        if (passedInButtons.Length > 0) {
            passedInButtons[index].gameObject.GetComponent<Selectable>().Select();
        }

        if (AnyInputDownWasReleased()) {
            index = ListIterator.GetPositionIndex(passedInButtons.Length, index, "down");
        }

        if (AnyInputUpWasReleased()) {
            index = ListIterator.GetPositionIndex(passedInButtons.Length, index, "up");
        }

    }


    public void DoAction() {
        this.actionSelectables[index].GetComponent<ActionSelectable>().doAction();
    }


    bool AnyInputUpWasReleased() {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
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
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            return true;
        }

        foreach (InputDevice device in InputManager.Devices) {
            if (canMoveDown && device.LeftStickDown.IsPressed && device.LeftStickDown.RawValue > 0.9f) {
                canMoveDown = false;
                Invoke("ResetDownDelay", analogStickDelay);
                return true;
            } else if (device.DPadDown.WasPressed) {
                return true;
            }
        }

        return false;
    }

    bool AnyInputEnterWasReleased() {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space)) {
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
        if (Input.GetKeyDown(KeyCode.Escape)) {
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

    void GoToStartMenu() {
        SceneManager.LoadScene("Start");
    }


    void ClearOptions() {
        foreach (ActionToggle actionToggle in view.GetMatchOptions()) {
            actionToggle.ToggleComponent.isOn = false;
        }
    }
	
}
