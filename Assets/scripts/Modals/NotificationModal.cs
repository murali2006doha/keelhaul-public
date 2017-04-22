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
	public ActionButton ok; //these will eventually have the ActionButton.cs script attached 
	public ActionButton cancel; 

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
        this.popAction += GoBack;	
    }


    public void Spawn(string messageText, Color color, string okText, String cancelText, Action yesAction, Action noAction) {


        image.color = color;
        ok.GetComponentInChildren<Text>().text = okText;
        cancel.GetComponentInChildren<Text>().text = cancelText;
        text.text = messageText;
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
