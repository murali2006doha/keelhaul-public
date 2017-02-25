using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class NotificationModal : AbstractModalComponent {

    public Text text;
    public Image image;
    public Button ok; //these will eventually have the ActionButton.cs script attached 
    public Button cancel; 

    Action onYesButtonPress;
    Action onNoButtonPress;

    void Update() {
        if (isActive) {
            Control ();  
        }
    }

    public override void InitializeModal(PlayerActions actions) {
        this.actions = actions;
        this.isActive = true;
        this.popAction += goBack;
    }


    public void Initialize(string messageText, Color color, string okText, String cancelText, Action yesAction, Action noAction) {

        image.color = color;
        ok.GetComponentInChildren<Text>().text = okText;
        cancel.GetComponentInChildren<Text>().text = cancelText;
        text.text = messageText;
        modalAnimator.Play ("ModalEnter");
        this.onNoButtonPress = noAction;
        this.onYesButtonPress = yesAction;

        buttonToAction.Add (ok, () =>  {
            this.onYesButtonPress();
            this.popAction ();
        });

        buttonToAction.Add (cancel, () =>  {
            this.onNoButtonPress();
            this.popAction ();
        });

        this.buttons = new List<Button> (buttonToAction.Keys).ToArray ();
        this.index = buttons.Length - 1;

    }
}
