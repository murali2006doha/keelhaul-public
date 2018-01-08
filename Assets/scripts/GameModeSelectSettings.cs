using UnityEngine;
using System.Collections;

public class GameModeSelectSettings : MonoBehaviour {
    
    protected GameTypeEnum gameType;


    public void SetGameModeSettings(GameTypeEnum gameType) {
        this.gameType = gameType;
    }


    public GameTypeEnum getGameType() {
        return gameType;
    }

    void Awake () {
        if (this != null) {
            DontDestroyOnLoad (this.transform.gameObject);
        }
    }
}

