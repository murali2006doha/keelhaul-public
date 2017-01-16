using UnityEngine;
using System.Collections.Generic;

public class PlayerSelectSettings : MonoBehaviour {

	public List<CharacterSelection> players = new List<CharacterSelection>();
    // Use this for initialization
    public bool isTeam;
    public GameTypeEnum gameType;
    public bool includeKraken;

	void Start () {
		GameObject.DontDestroyOnLoad (this);
        isTeam = false;
        gameType = GameTypeEnum.Sabotage;
        includeKraken = true;
	}

	public void setPlayerCharacters(List<CharacterSelect> playerList){
		foreach (CharacterSelect player in playerList) {
			players.Add (new CharacterSelection(player.selectedCharacter,player.Actions));
		}
	}

	public void setPlayerCharacters(List<CharacterSelectPanel> playerList){
		foreach (CharacterSelectPanel player in playerList) {
			players.Add (new CharacterSelection(player.selectedCharacter,player.Actions));
		}
	}

}
