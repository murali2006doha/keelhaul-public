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
			characterList[0].SetActive (true);
		}

		characterList.Add (blue);
		characterList.Add (red);
		characterList.Add (green);
		selectedCharacter = "";
	}


	// Update is called once per frame
	void Update () {
		if (Actions != null) {
			characterSelect ();
			if (selected != true) {
				characterList[index].SetActive (true);
			}
		}
		ps = GameObject.FindObjectOfType<OneVsOnePlayerSignIn> ();
	}
		
}
