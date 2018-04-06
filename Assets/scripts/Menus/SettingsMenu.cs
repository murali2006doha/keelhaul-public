using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class SettingsMenu : AbstractMenu
{


    public ActionButton graphics;
    public ActionButton sound;
    public ActionButton controls;


    protected override void SetActions()
    {
        graphics.SetAction(() => {
            this.enabled = false;
            ToggleSelectables();
            FindObjectOfType<MenuModel>().grapicsMenu.Initialize(this.actions, () => {
                ToggleSelectables();
                this.enabled = true;
            });
        });
        sound.SetAction(() => {
            this.enabled = false;
            ToggleSelectables();
            FindObjectOfType<MenuModel>().soundMenu.Initialize(this.actions, () => {
                ToggleSelectables();
                this.enabled = true;
            });
        });
        controls.SetAction(() => {
            this.enabled = false;
            ToggleSelectables();
            FindObjectOfType<MenuModel>().inputBindingsMenu.Initialize(this.actions, () => {
                ToggleSelectables();
                this.enabled = true;
            });
        });

    }

    protected override void SetActionSelectables()
    {
        actionSelectables.Add(graphics.gameObject);
        actionSelectables.Add(sound.gameObject);
        actionSelectables.Add(controls.gameObject);
    }

    public void OnClick()
    {
        GoBack();
    }

}

