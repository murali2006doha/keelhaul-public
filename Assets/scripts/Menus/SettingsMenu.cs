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
            this.gameObject.SetActive(false);
            this.enabled = false;
            ToggleSelectables();
            FindObjectOfType<MenuModel>().grapicsMenu.Initialize(this.actions, () => {
                ToggleSelectables();
                this.enabled = true;
                this.gameObject.SetActive(true);

            });
        });
        sound.SetAction(() => {
            this.gameObject.SetActive(false);
            this.enabled = false;
            ToggleSelectables();
            FindObjectOfType<MenuModel>().soundMenu.Initialize(this.actions, () => {
                ToggleSelectables();
                this.enabled = true;
                this.gameObject.SetActive(true);

            });
        });

        controls.SetAction(() => {
            this.gameObject.SetActive(false);
            this.enabled = false;
            ToggleSelectables();
            FindObjectOfType<MenuModel>().inputBindingsMenu.Initialize(this.actions, () => {
                ToggleSelectables();
                this.enabled = true;
                this.gameObject.SetActive(true);

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

