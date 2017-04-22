using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[ExecuteInEditMode]
public class ActionButton : ActionSelectable 
{

    UnityAction actionToExecute;

    [SerializeField] 
    Button button;
    public void SetAction(UnityAction action)
    {
        this.ButtonComponent.onClick.AddListener(action);
        this.actionToExecute = action;
    }

    public override void doAction() {
		print ("do actions");
        this.actionToExecute ();
    }

    public Button ButtonComponent
    {
        get
        {
            if (this.button == null)
            {
                this.button = this.GetComponent<Button>();
            }

            return this.button;
        }
    }

}