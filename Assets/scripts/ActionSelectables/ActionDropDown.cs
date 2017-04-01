using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
[ExecuteInEditMode]
public class ActionDropDown : ActionSelectable 
{

    UnityAction<int> actionToExecute;

    [SerializeField]
    Dropdown dropdown;
    public void SetAction(UnityAction<int> action)
    {
        this.DropDownComponent.onValueChanged.AddListener(action);
        this.actionToExecute = action;
    }

    public override void doAction() {
        this.actionToExecute (2);
    }

    public Dropdown DropDownComponent
    {
        get
        {
            if (this.dropdown == null)
            {
                this.dropdown = this.GetComponent<Dropdown>();
            }

            return this.dropdown;
        }
    }
}
