using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class GraphicsMenu : AbstractMenu
{


	public ActionToggle shadowsToggle;
	public ActionToggle waterRefractToggle;
	public ActionToggle waterReflectToggle;


	protected override void SetActions() {
		shadowsToggle.SetAction(this.setShadowsToggle);
		waterRefractToggle.SetAction(this.setWaterRefractionToggle);
		waterReflectToggle.SetAction(this.setWaterReflectionToggle);
	}

	protected override void SetActionSelectables() {
		this.shadowsToggle.ToggleComponent.isOn = GlobalSettings.shadows;
		this.waterReflectToggle.ToggleComponent.isOn = GlobalSettings.waterReflection;
		this.waterRefractToggle.ToggleComponent.isOn = GlobalSettings.waterRefraction;

		actionSelectables.Add(shadowsToggle.gameObject);
		actionSelectables.Add(waterRefractToggle.gameObject);
		actionSelectables.Add(waterReflectToggle.gameObject);
	}

	void setShadowsToggle(bool isOn) {
		GlobalSettings.setShadows(isOn);
	}

	void setWaterRefractionToggle(bool isOn) {
		GlobalSettings.setRefraction(isOn);
	}

	void setWaterReflectionToggle(bool isOn) {
		GlobalSettings.setReflection(isOn);
	}

    public void OnClick()
    {
        GoBack();
    }
}

