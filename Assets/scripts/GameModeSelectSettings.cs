using UnityEngine;
using System.Collections;

public class GameModeSelectSettings : MonoBehaviour {
    
    protected GameTypeEnum gameType;
    protected bool online;


    public void SetGameModeSettings(GameTypeEnum gameType, bool isOnline) {
        this.gameType = gameType;
        this.online = isOnline;
    }


    public GameTypeEnum getGameType() {
        return gameType;
    }


    public bool isOnline() {
        return this.online;
    }


    void Awake () {
        if (this != null) {
            DontDestroyOnLoad (this.transform.gameObject);
        }
    }
}

