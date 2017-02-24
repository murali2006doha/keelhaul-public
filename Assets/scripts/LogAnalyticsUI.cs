using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Analytics;
using UnityEngine.UI;
using InControl;


public class LogAnalyticsUI : MonoBehaviour {


    public static void SplashScreenEnter () {

        Analytics.CustomEvent("Splash Screen Enter", new Dictionary<string, object> {
            { "Event_category", "ui" },
            { "Event_name", "splash_screen_enter" }
        });
    }


    public static void SplashScreenFinished () {

        Analytics.CustomEvent("Spash Screen Finished", new Dictionary<string, object> {
            { "Event_category", "ui" },
            { "Event_name", "splash_screen_finished" }
        });
    }


    public static void SplashScreenSkipped () {

        Analytics.CustomEvent("Spash Screen Skipped", new Dictionary<string, object> {
            { "Event_category", "ui" },
            { "Event_name", "splash_screen_skipped" }
        });
    }

    // after main menu has the offline/online option
    public static void MainMenuOnlineSelected () {

        Analytics.CustomEvent("Main Menu Online Select", new Dictionary<string, object> {
            { "Event_category", "ui" },
            { "Event_name", "main_menu_online_selected" }
        });
    }

    // after main menu has the offline/online option
    public static void MainMenuLocalSelected () {

        Analytics.CustomEvent("Main Menu Local Select", new Dictionary<string, object> {
            { "Event_category", "ui" },
            { "Event_name", "main_menu_local_selected" }
        });
    }

    // put this in levelmanager after menu has been polished
    public static void MainMenuBackSelected (string page) {

        Analytics.CustomEvent("Main Menu Back Select", new Dictionary<string, object> {
            { "Event_category", "ui" },
            { "Event_name", "main_menu_back_selected" },
            { "Current_page", page }
        });
    }

    // put this in level manager 
	public static void MainMenuGameModeSelected (GameTypeEnum mode) {

        Analytics.CustomEvent("Main Menu Gamemode Select", new Dictionary<string, object> {
            { "Event_category", "ui" },
            { "Event_name", "main_menu_gamemode_selected" },
            { "Local", PhotonNetwork.offlineMode.ToString() },
            { "Game_mode", mode }
        });
    }

    //put this in character select or lobby manager or network manager
	public static void MainMenuGameStartedWithCharacters (GameTypeEnum mode, string map, List<CharacterSelectPanel> shipSelections) {
        Dictionary<String, String> players = new Dictionary<String, String> ();

        int i = 1;
        foreach(CharacterSelectPanel player in shipSelections) {
            if (player.Actions.Device == null) {
                players.Add (player.getSelectedCharacter() + i.ToString (), "keyboard");
            } else {
                players.Add (player.getSelectedCharacter() + i.ToString (), "controller");
            }
        }

        Analytics.CustomEvent("Main Menu Game Started with Characters", new Dictionary<string, object> {
            { "Event_category", "ui" },
            { "Event_name", "main_menu_game_started_with_characters" },
            { "Local", PhotonNetwork.offlineMode.ToString() },
            { "Game_mode", mode },
            { "Game_map", map },
            { "Characters", players.ToStringFull() }
        });
    }
}

