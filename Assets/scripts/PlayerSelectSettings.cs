using UnityEngine;
using System.Collections.Generic;

public class PlayerSelectSettings : MonoBehaviour {

	public List<CharacterSelection> players = new List<CharacterSelection>();
	// Use this for initialization
	void Start () {
		GameObject.DontDestroyOnLoad (this);
	}

	public void setPlayerCharacters(List<CharacterSelect> playerList){
		foreach (CharacterSelect player in playerList) {
			players.Add (new CharacterSelection(player.selectedCharacter,player.Actions));
		}
	}

}
