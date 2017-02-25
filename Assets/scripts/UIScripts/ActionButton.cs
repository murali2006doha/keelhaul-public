using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[ExecuteInEditMode]
public class ActionButton : MonoBehaviour {

    [SerializeField] Button button;
    public void SetAction(UnityAction action)
    {
        this.ButtonComponent.onClick.AddListener(action);
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