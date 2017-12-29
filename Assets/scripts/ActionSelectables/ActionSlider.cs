using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[ExecuteInEditMode]
public class ActionSlider : ActionSelectable
{

    UnityAction<float> actionToExecute;
	PlayerActions input;

    [SerializeField]
    Slider slider;
	public void SetAction(UnityAction<float> action, PlayerActions input) {
        this.SliderComponent.onValueChanged.AddListener(action);
        this.actionToExecute = action;
		this.input = input;
    }

	public override void doAction() {
        var value = input.Left.RawValue > 0?-input.Left.RawValue:input.Right.RawValue;
        this.SliderComponent.value += value * 0.1f;
		this.actionToExecute (this.SliderComponent.value);
	}


    public Slider SliderComponent
    {
        get
        {
            if (this.slider == null)
            {
                this.slider = this.GetComponent<Slider>();
            }

            return this.slider;
        }
    }

}