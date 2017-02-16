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
    public Text ok;
    public Text cancel;
    Action onButtonPress;

    void Start() {
        //initialize ("are you sure?!", Color.yellow, "sure", "nah", () => {actionMethod(); }   );

    }


    /**
     * the buttons are always white
     **/
    public void initialize(string messageText, Color color, string okText, String cancelText, Action action) {

        this.gameObject.SetActive (true);
        image.color = color;
        ok.text = okText;
        cancel.text = cancelText;
        text.text = messageText;
        modalAnimator.Play ("ModalEnter");
        this.onButtonPress = action;

    }


    void actionMethod() {

        print("ok");
    }

    
    // Update is called once per frame
    void Update () {
    }


    public void OnClickButton(string choice) {
        if( choice == "continue") {
            modalAnimator.Play ("ModalExit");
            this.onButtonPress ();
        }

		if( choice == "cancel") {
			modalAnimator.Play ("ModalExit");
			this.onButtonPress ();
		}
    }

}
