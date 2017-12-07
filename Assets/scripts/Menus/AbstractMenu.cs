using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    bool canMoveDown = true;
    bool canMoveUp = true;
    bool canMoveLeft = true;
    bool canMoveRight = true;
    float analogStickDelay = 0.15f;

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
            index = NavigateModal(this.actions, actionSelectables.ToArray(), index);
        }
        index = NavigateModalWithMouse(actionSelectables, index);
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


    bool AnyInputEnterWasReleased() {
        if (null != actions.Device) {
            if (actions.Device.Action1.WasReleased) {
                return true;
            }
        }

    	return false;
    }


    public bool AnyInputBackWasReleased() {

    	if (Input.GetKeyUp(KeyCode.Escape) || actions.Red.WasReleased || actions.Start.WasReleased) {
    		return true;
    	}

    	return false;
    }



    public int NavigateModalWithMouse(List<GameObject> actionSelectables, int index)
    {

        for (int i = 0; i < actionSelectables.Count; i++)
        {
            if (actionSelectables[i].GetComponent<ActionSelectable>().isMouseHovering())
            {
                index = i;
            }
        }

        return index;
    }

    public int NavigateModal(PlayerActions actions, GameObject[] passedInButtons, int index)
    { //navigating main menu  
        if (passedInButtons.Length > 0)
        {
            passedInButtons[index].gameObject.GetComponent<Selectable>().Select();
        }

        if (AnyInputDownWasReleased(actions))
        {
            index = ListIterator.GetPositionIndex(passedInButtons.Length, index, "down");
        }

        if (AnyInputUpWasReleased(actions))
        {
            index = ListIterator.GetPositionIndex(passedInButtons.Length, index, "up");
        }

        if (passedInButtons[index].gameObject.GetComponent<ActionSlider>())
        {
            NavigateVolumeSlider(actions, passedInButtons, index);
        }
        else
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


    void NavigateVolumeSlider(PlayerActions actions, GameObject[] actionSelectables, int index)
    {
        if (null == actions || null == actions.Device)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                actionSelectables[index].GetComponent<ActionSlider>().doAction();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                actionSelectables[index].GetComponent<ActionSlider>().doAction();
            }
            else if (canMoveLeft && actions.Device.LeftStickLeft.RawValue > 0.9f)
            {
                actionSelectables[index].GetComponent<ActionSlider>().doAction();
                canMoveLeft = false;
                StartCoroutine(ResetLeftDelay());
            }
            else if (canMoveLeft && actions.Device.DPadLeft.IsPressed)
            {
                actionSelectables[index].GetComponent<ActionSlider>().SliderComponent.value -= volumeChange;
                actionSelectables[index].GetComponent<ActionSlider>().doAction();
                canMoveLeft = false;
                StartCoroutine(ResetLeftDelay());
            }
        }


        if (null == actions || null == actions.Device)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                actionSelectables[index].GetComponent<ActionSlider>().doAction();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                actionSelectables[index].GetComponent<ActionSlider>().doAction();
            }
            else if (canMoveRight && actions.Device.LeftStickRight.RawValue > 0.9f)
            {
                actionSelectables[index].GetComponent<ActionSlider>().doAction();
                canMoveRight = false;
                StartCoroutine(ResetRightDelay());
            }
            else if (canMoveRight && actions.Device.DPadRight.IsPressed)
            {
                actionSelectables[index].GetComponent<ActionSlider>().SliderComponent.value += volumeChange;
                actionSelectables[index].GetComponent<ActionSlider>().doAction();
                canMoveRight = false;
                StartCoroutine(ResetRightDelay());
            }
        }
    }

    bool AnyInputUpWasReleased(PlayerActions actions)
    {

        if (null == actions || null == actions.Device)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                return true;
            }
        }
        else
        {
            if (canMoveUp && actions.Device.LeftStickUp.IsPressed && actions.Device.LeftStickUp.RawValue > 0.9f)
            {
                canMoveUp = false;
                StartCoroutine(ResetUpDelay());
                return true;
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W) || actions.Device.DPadUp.WasPressed)
            {
                return true;
            }
        }

        return false;
    }

    bool AnyInputDownWasReleased(PlayerActions actions)
    {

        if (null == actions || null == actions.Device)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                return true;
            }
        }
        else
        {
            if (canMoveDown && actions.Device.LeftStickDown.IsPressed && actions.Device.LeftStickDown.RawValue > 0.9f)
            {
                canMoveDown = false;
                StartCoroutine(ResetDownDelay());
                return true;
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S) || actions.Device.DPadDown.WasPressed)
            {
                return true;
            }

        }

        return false;
    }


    bool AnyInputRightWasReleased(PlayerActions actions)
    {
        if (null == actions || null == actions.Device)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                return true;
            }
        }
        else
        {
            if (canMoveRight && actions.Device.LeftStickRight.IsPressed && actions.Device.LeftStickRight.RawValue > 0.9f)
            {
                canMoveRight = false;
                StartCoroutine(ResetRightDelay());
                return true;
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D) || actions.Device.DPadRight.WasPressed)
            {
                return true;
            }
        }

        return false;
    }


    bool AnyInputLeftWasReleased(PlayerActions actions)
    {

        if (null == actions || null == actions.Device)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                return true;
            }
        }
        else
        {
            if (canMoveLeft && actions.Device.LeftStickLeft.IsPressed && actions.Device.LeftStickLeft.RawValue > 0.9f)
            {
                canMoveLeft = false;
                StartCoroutine(ResetLeftDelay());
                return true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) || actions.Device.DPadLeft.WasPressed)
            {
                return true;
            }
        }

        return false;
    }

    public void ToggleSelectables(List<GameObject> actionSelectables, bool interactable)
    {
        foreach (GameObject b in actionSelectables)
        {
            if (b.GetComponent<ActionButton>())
            {
                b.GetComponent<ActionButton>().ButtonComponent.interactable = !b.GetComponent<ActionButton>().ButtonComponent.interactable;

            }
            else if (b.GetComponent<ActionSlider>())
            {
                b.GetComponent<ActionSlider>().SliderComponent.interactable = !b.GetComponent<ActionSlider>().SliderComponent.interactable;

            }
            else if (b.GetComponent<ActionToggle>())
            {
                b.GetComponent<ActionToggle>().ToggleComponent.interactable = !b.GetComponent<ActionToggle>().ToggleComponent.interactable;

            }
            else if (b.GetComponent<ActionDropDown>())
            {
                b.GetComponent<ActionDropDown>().DropDownComponent.interactable = !b.GetComponent<ActionDropDown>().DropDownComponent.interactable;

            }
        }
        interactable = !interactable;

    }

    IEnumerator ResetUpDelay()
    {
        yield return StartCoroutine(CoroutineUtils.WaitForRealTime(analogStickDelay));
        canMoveUp = true;
    }

    IEnumerator ResetDownDelay()
    {
        yield return StartCoroutine(CoroutineUtils.WaitForRealTime(analogStickDelay));
        canMoveDown = true;
    }

    IEnumerator ResetRightDelay()
    {
        yield return StartCoroutine(CoroutineUtils.WaitForRealTime(analogStickDelay));
        canMoveRight = true;
    }

    IEnumerator ResetLeftDelay()
    {
        yield return StartCoroutine(CoroutineUtils.WaitForRealTime(analogStickDelay));
        canMoveLeft = true;
    }
}

