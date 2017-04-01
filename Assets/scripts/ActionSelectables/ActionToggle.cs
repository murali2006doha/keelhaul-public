using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[ExecuteInEditMode]
public class ActionToggle : ActionSelectable
{

    UnityAction<bool> actionToExecute;

    [SerializeField]
    Toggle toggle;
    public void SetAction(UnityAction<bool> action)
    {
        this.ToggleComponent.onValueChanged.AddListener(action);
        this.actionToExecute = action;
    }

    public override void doAction() {
        this.actionToExecute (true);
    }

    public Toggle ToggleComponent
    {
        get
        {
            if (this.toggle == null)
            {
                this.toggle = this.GetComponent<Toggle>();
            }

            return this.toggle;
        }
    }
}
