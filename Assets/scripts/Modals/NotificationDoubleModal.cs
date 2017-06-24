using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class NotificationDoubleModal : AbstractNotificationModal {

    public ActionButton ok; //these will eventually have the ActionButton.cs script attached 
    public ActionButton cancel; 

    Action onYesButtonPress;
    Action onNoButtonPress;


    public void Spawn(String messageText, String okText, String cancelText, Action yesAction, Action noAction) {

        image.sprite = Resources.Load<Sprite>(messageText);
        ok.GetComponent<Image>().sprite = Resources.Load<Sprite>(okText);
        cancel.GetComponent<Image>().sprite = Resources.Load<Sprite>(cancelText);
        modalAnimator.Play ("ModalEnter");
        this.onNoButtonPress = noAction;
        this.onYesButtonPress = yesAction;

        SetUpButtonToActionDictionary ();
            
    }

    void SetUpButtonToActionDictionary () {

        actionSelectables.Add (ok.gameObject);
        actionSelectables.Add (cancel.gameObject);

        ok.SetAction (() =>  {
            this.onYesButtonPress ();
            this.popAction ();
        });
        cancel.SetAction (() =>  {
            this.onNoButtonPress ();
            this.popAction ();
        });
    }
}
