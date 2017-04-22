﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeMenu : AbstractMenu {

    public ActionButton deathmatch;
    public ActionButton sabotage;
    public bool isOnline;

    // Use this for initialization
    void Start () {

        SetActions ();

        if (isOnline) {
            actionSelectables.Add (deathmatch.gameObject);
		} else {
			actionSelectables.Add (deathmatch.gameObject);
			actionSelectables.Add (sabotage.gameObject);
        }
    }

    public override void SetActions (){

        deathmatch.SetAction (() => {
            ToggleSelectables();
            FindObjectOfType<GameModeSelectSettings>().SetGameModeSettings(GameTypeEnum.DeathMatch, isOnline);
            SceneManager.LoadScene("Game");
        });

        sabotage.SetAction (() => {
            ToggleSelectables();
            FindObjectOfType<GameModeSelectSettings>().SetGameModeSettings(GameTypeEnum.Sabotage, isOnline);
            SceneManager.LoadScene("Game");
        });
    }




}
