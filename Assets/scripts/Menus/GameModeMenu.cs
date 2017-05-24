using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeMenu : AbstractMenu {

    public ActionButton deathmatch;
    public ActionButton sabotage;
    public bool isOnline;


    protected override void SetActions (){

        deathmatch.SetAction (() => {
            print("not available for beta");
            ToggleSelectables();
            FindObjectOfType<GameModeSelectSettings>().SetGameModeSettings(GameTypeEnum.DeathMatch, isOnline);
            SceneManager.LoadScene("Game");
        });

        sabotage.SetAction (() => {
            print("not available for beta");
            ToggleSelectables();
            FindObjectOfType<GameModeSelectSettings>().SetGameModeSettings(GameTypeEnum.Sabotage, isOnline);
            SceneManager.LoadScene("Game");
        });
    }


    protected override void SetActionSelectables ()
    {
        if (isOnline) {
            actionSelectables.Add (deathmatch.gameObject);
        }
        else {
            actionSelectables.Add (deathmatch.gameObject);
            actionSelectables.Add (sabotage.gameObject);
        }
    }






}
