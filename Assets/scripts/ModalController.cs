using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class ModalController : MonoBehaviour {


    public Animator modalAnimator;
    public Text text;
    public Image image;
	public Button ok; //these will eventually have the ActionButton.cs script attached 
	public Button cancel; 
    Action onYesButtonPress;
	Action onNoButtonPress;

    void Start() {
		//modalAnimator.Play ("ModalEnter");
	}

    /**
     * can choose the message text, the color of the text box, and the texts for the two buttons
     **/
	public void initialize(string messageText, Color color, string okText, String cancelText, Action yesAction, Action noAction) {

        image.color = color;
		ok.GetComponentInChildren<Text>().text = okText;
		cancel.GetComponentInChildren<Text>().text = cancelText;
        text.text = messageText;
       // modalAnimator.Play ("ModalEnter");
		cancel.Select ();
		this.onYesButtonPress = yesAction;
		this.onNoButtonPress = noAction;

    }

	public void OnClickButton(string choice) {
		if( choice == "continue") {
		//	modalAnimator.Play ("ModalExit");
			this.onYesButtonPress ();
			DestroyObject (this.transform.gameObject);
		}

		if( choice == "cancel") {
		//	modalAnimator.Play ("ModalExit");
			this.onNoButtonPress ();
			DestroyObject (this.transform.gameObject);

		}
	}

   

}
