using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public abstract class AbstractNotificationModal : AbstractModalComponent {

    public Image image;


    public override void SetupModal(PlayerActions actions) {
        this.actions = actions;
        this.isActive = true;
        this.popAction += GoBack;
    }


    protected override bool CanControl () {
        if (isActive) {
            return true;
        } 

        return false;
    }
}
