using UnityEngine;
using System.Collections.Generic;

public class PlayerSelectSettings : MonoBehaviour {

	public List<CharacterSelection> players = new List<CharacterSelection>();
    // Use this for initialization
    public bool isTeam;
    public GameTypeEnum gameType;
	public MapEnum map;
    public bool includeKraken;
	public bool isOnline;
	public string mode;

	void Start () {
		GameObject.DontDestroyOnLoad (this);
	}


	public void setPlayerCharacters(List<CharacterSelectPanel> playerList){
		foreach (CharacterSelectPanel player in playerList) {
			players.Add (new CharacterSelection(player.selectedCharacter,player.Actions));
		}
	}

}
