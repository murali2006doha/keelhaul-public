using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class NotificationSingleModal : AbstractNotificationModal {

    public ActionButton ok; //these will eventually have the ActionButton.cs script attached 

    Action onYesButtonPress;
   


    public void Spawn(String messageText, String okText, Action yesAction) {

        image.sprite = Resources.Load<Sprite>(messageText);
        ok.GetComponent<Image>().sprite = Resources.Load<Sprite>(okText);
        modalAnimator.Play ("ModalEnter");
        this.onYesButtonPress = yesAction;

        SetUpButtonToActionDictionary ();
            
    }

    void SetUpButtonToActionDictionary () {

        actionSelectables.Add (ok.gameObject);

        ok.SetAction (() =>  {
            this.onYesButtonPress ();
            this.popAction ();
        });
    }
}
