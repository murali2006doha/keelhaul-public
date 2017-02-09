using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[ExecuteInEditMode]
public class ActionDropDown : MonoBehaviour {

    [SerializeField]
    Dropdown dropdown;
    public void SetAction(UnityAction<int> action)
    {
        this.DropDownComponent.onValueChanged.AddListener(action);
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
