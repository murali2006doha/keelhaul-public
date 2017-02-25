using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[ExecuteInEditMode]
public class ActionToggle : MonoBehaviour
{
    [SerializeField]
    Toggle toggle;
    public void SetAction(UnityAction<bool> action)
    {
        this.ToggleComponent.onValueChanged.AddListener(action);
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
