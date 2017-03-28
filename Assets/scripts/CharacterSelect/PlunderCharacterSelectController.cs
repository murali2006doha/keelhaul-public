using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;

public class PlunderCharacterSelectController : AbstractCharacterSelectController {


	// Use this for initialization
	void Start () {
		
		//GameObject.DontDestroyOnLoad (this.gameObject);
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;
		mode = GameTypeEnum.Sabotage;

		for(int i = 0; i < numPlayers; i++) {
			initializePanel ();
		}	
			
		foreach (ShipEnum character in characters) {
			characterStatuses.Add (character.ToString(), false);
		}
			
		playersInPlay = players.Count;
	}



	public override bool lockCharacter(int index) {
		if (!characterStatuses [getCharacterKeys() [index]]) {

			//character is locked
			
			playersInPlay--;

			//lock character for all players
			if (getCharacterKeys()[index] == ShipEnum.Kraken.ToString()) {
                characterStatuses[getCharacterKeys()[index]] = true;
                isolateKraken (index);
			}

			return true;
		}

		return false;
	}

	public override bool unlockCharacter(int index) {

		if (characterStatuses [getCharacterKeys() [index]]) {

			//character is unlocked
			characterStatuses [getCharacterKeys() [index]] = false;
			playersInPlay++;

			//unlock character for all players
			if (characterStatuses.ContainsKey (ShipEnum.Kraken.ToString ())) {
				deIsolateKraken (index);
				isolateKraken (index);
			}

			return true;
		}

		return false;	
	}


	/*
	 * When a player has chosen a character, remove that character from all the other player lists
	 */
	private void isolateKraken(int index) { 
		if (characterStatuses.ContainsKey (ShipEnum.Kraken.ToString ())) {
			if (playersInPlay == 1 && !characterStatuses [ShipEnum.Kraken.ToString ()]) {
				foreach (string character in getCharacterKeys()) {
					if (!characterStatuses [character] && character != ShipEnum.Kraken.ToString ()) {
						characterStatuses [character] = true;
					}
				}
			} 
		}
	}


	private void deIsolateKraken(int index) {
		List<string> charactersSelected = new List<string>();
		if (playersInPlay > 1 && !characterStatuses [ShipEnum.Kraken.ToString ()]) {
			foreach (CharacterSelectPanel player in players) {
				if (player.selectedCharacter != "") {
					charactersSelected.Add (player.selectedCharacter);
				}
			}

			foreach (string character in getCharacterKeys()) {
				if (characterStatuses [character]) {
					if (!charactersSelected.Contains (character)) {
						characterStatuses [character] = false;
					}
				}
			}
		}
	}

    internal override GameTypeEnum getGameType()
    {
        return GameTypeEnum.Sabotage;
    }


}
