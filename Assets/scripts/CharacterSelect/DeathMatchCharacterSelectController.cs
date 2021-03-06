﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;

public class DeathMatchCharacterSelectController : AbstractCharacterSelectController {


	// Use this for initialization
	void Start () {
		
		//GameObject.DontDestroyOnLoad (this.gameObject);
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;
		mode = GameTypeEnum.DeathMatch;

		for(int i = 0; i < numPlayers; i++) {
			initializePanel ();
		}	
			
		print ("characters: " + characters.Count);
		foreach (ShipEnum character in characters) {
			characterStatuses.Add (character.ToString(), false);
		}
			
		playersInPlay = players.Count;
	}


	public override bool lockCharacter(int index) {

		playersInPlay--;

		return true;

	}


	public override bool unlockCharacter(int index) {

		playersInPlay++;

		return true;	
	}



}
