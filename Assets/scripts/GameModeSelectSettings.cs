using UnityEngine;
using System.Collections;

public class GameModeSelectSettings : MonoBehaviour {
	
	protected GameTypeEnum gameType;
	public bool isOnline;

	void Start () {
	}

	public GameTypeEnum getGameType() {
		return gameType;
	}

	public void setGameType(GameTypeEnum gameType) {
		this.gameType = gameType;
	}
}

