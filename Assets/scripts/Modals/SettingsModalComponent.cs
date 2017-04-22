﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class SettingsModalComponent : AbstractModalComponent {

    public ActionToggle shadowsToggle;
    public ActionToggle waterRefractToggle;
    public ActionToggle waterReflectToggle;
    public ActionSlider soundSlider;
    public ActionSlider musicSlider;

    void Update() {
        if (isActive) {
            Control (); 
        }
    }

    // Use this for initialization
    public override void InitializeModal (PlayerActions actions) {
        this.actions = actions;

        this.soundSlider.SliderComponent.value = GlobalSettings.soundMultiplier;
        this.musicSlider.SliderComponent.value = GlobalSettings.musicMultiplier;
        SetUpButtonToActionDictionary ();

    }

    void SetUpButtonToActionDictionary ()
    {
        actionSelectables.Add (shadowsToggle.gameObject);
        actionSelectables.Add (waterRefractToggle.gameObject);
        actionSelectables.Add (waterReflectToggle.gameObject);
        actionSelectables.Add (soundSlider.gameObject);
        actionSelectables.Add (musicSlider.gameObject);

        shadowsToggle.SetAction (this.setShadowsToggle);
        waterRefractToggle.SetAction (this.setWaterRefractionToggle);
        waterReflectToggle.SetAction (this.setWaterReflectionToggle);
        soundSlider.SetAction (this.setSoundVolume, this.actions);
        musicSlider.SetAction (this.setMusicVolume, this.actions);
    }

    void setSoundVolume(float multiplier) {
        GlobalSettings.setSoundMultiplier (multiplier);
    }

    void setMusicVolume(float multiplier) {
        GlobalSettings.setMusicMuliplier (multiplier);
    }

    void setShadowsToggle(bool isOn) {
        if (isOn) {
            QualitySettings.shadows = ShadowQuality.All;
        } else {
            QualitySettings.shadows = ShadowQuality.Disable;
        }
    }

    void setWaterRefractionToggle(bool isOn) {
        GlobalSettings.waterRefraction = isOn;
    }

    void setWaterReflectionToggle(bool isOn) {
        GlobalSettings.waterReflection = isOn;
    }

}
