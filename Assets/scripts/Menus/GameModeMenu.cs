using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeMenu : AbstractMenu {

    public ActionButton deathmatch;
    public ActionButton sabotage;

    protected override void SetActions (){

        deathmatch.SetAction (() => {
            print("not available for beta");
            ToggleSelectables();
            FindObjectOfType<GameModeSelectSettings>().SetGameModeSettings(GameTypeEnum.DeathMatch);
            SceneManager.LoadScene("Game");
        });

        sabotage.SetAction (() => {
            print("not available for beta");
            ToggleSelectables();
            FindObjectOfType<GameModeSelectSettings>().SetGameModeSettings(GameTypeEnum.Sabotage);
            SceneManager.LoadScene("Game");
        });
    }


    protected override void SetActionSelectables ()
    {
        actionSelectables.Add (deathmatch.gameObject);
        actionSelectables.Add (sabotage.gameObject);
    }






}
