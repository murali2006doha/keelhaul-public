using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

using UnityEngine.UI;
using InControl;


/*
 * Attached to each player gameobject
 */ 
public class OneVOneCharacterSelect : CharacterSelect {


	// Use this for initialization
	void Start () {

		if (Actions != null) {
		}


		selectedCharacter = "";
	}


	// Update is called once per frame
	void Update () {
		if (Actions != null) {
			characterSelect ();

		}
		ps = GameObject.FindObjectOfType<OneVsOnePlayerSignIn> ();
	}
		
}
