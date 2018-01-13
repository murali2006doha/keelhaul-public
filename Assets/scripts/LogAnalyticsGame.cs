using UnityEngine;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Analytics;
using UnityEngine.UI;
using InControl;
using System.Collections;


public class LogAnalyticsGame : MonoBehaviour {




    public static void StartGame (List<PlayerInput> shipSelections, CountDown countdown) {

        GameInitializer gi = FindObjectOfType<GameInitializer> ();
        Dictionary<String, String> players = new Dictionary<String, String> ();

        int j = 1; //to avoid same dictionary keys
        foreach (PlayerInput player in shipSelections) {
            if(player.Actions == null)
            {
                players.Add(player.shipName.ToString() + j.ToString(), "bot");
            }
            else
            {
                if (player.Actions != null && player.Actions.Device == null)
                {
                    players.Add(player.shipName.ToString() + j.ToString(), "keyboard");
                }
                else
                {
                    players.Add(player.shipName.ToString() + j.ToString(), "controller");
                }
            }
           
            j++;
        }
            

        Scene scene = SceneManager.GetActiveScene();

        Analytics.CustomEvent("Match Started", new Dictionary<string, object> {
            { "Event_category", "game" },
            { "Event_name", "match started" },
            { "Mode", gi.gameType.ToString() },
            { "Map", scene.name.ToString() },
            { "Local", PhotonNetwork.offlineMode.ToString() },
            { "Characters", players.ToStringFull() },
            { "time_to_match", countdown.timer.text}
        });
            
    }
        

    


    public static void EndGame (List<PlayerInput> shipSelections, float gameTime) {
        GameInitializer gi = FindObjectOfType<GameInitializer> ();
        AbstractGameManager gameManager = GameObject.FindObjectOfType<AbstractGameManager> ();

        Dictionary<String, String> players = new Dictionary<String, String> ();
        Dictionary<String, String> scores = new Dictionary<String, String> ();
        Dictionary<String, String> stats = new Dictionary<String, String> ();

        int i = 1;
        foreach(PlayerInput player in shipSelections) {
            scores.Add(player.shipName.ToString() + i.ToString(), gameManager.getGamepoints()[player.GetId()+""].ToString());
            stats.Add(player.shipName.ToString() + i.ToString(), player.gameStats.numOfShotHits.ToString());
            i++;
        }

        Scene scene = SceneManager.GetActiveScene();

        Analytics.CustomEvent("Match Ended", new Dictionary<string, object> {
            { "Event_category", "game" },
            { "Event_name", "match ended" },
            { "Mode", gi.gameType.ToString() },
            { "Map", scene.name.ToString() },
            { "Scores", scores.ToStringFull() },
            { "Stats", stats.ToStringFull() },
            { "Local", PhotonNetwork.offlineMode.ToString() },
            { "Length", "min: " + (gameTime / 60.0).ToString()} 
        });

    }

    //can be called at anytime
    public static void FpsSnapshot() {

        Scene scene = SceneManager.GetActiveScene();
        GameInitializer gi = FindObjectOfType<GameInitializer> ();

        Analytics.CustomEvent("FPS snapshot", new Dictionary<string, object> {
            { "Event_category", "game" },
            { "Event_name", "fps snapshot" },
            { "Mode", gi.gameType.ToString() },
            { "Map", scene.name.ToString() },
            { "Local", PhotonNetwork.offlineMode.ToString() },
            { "Timestamp", Time.timeSinceLevelLoad.ToString() },
            { "fps", (1.0f / Time.deltaTime).ToString() }
        });
    }


    //Integrate this after exit game modal is complete
    public static void MatchIntentionalQuit (List<PlayerInput> shipSelections, float gameTime) {

        Scene scene = SceneManager.GetActiveScene();
        GameInitializer gi = FindObjectOfType<GameInitializer> ();

        Dictionary<String, String> scores = new Dictionary<String, String> ();
        Dictionary<String, FreeForAllStatistics> stats = new Dictionary<String, FreeForAllStatistics> ();

        int i = 1; 
        foreach(PlayerInput player in shipSelections) {
            print (player);
            scores.Add(player.shipName.ToString() + i.ToString(), player.uiManager.points.text.ToString());
            stats.Add(player.shipName.ToString() + i.ToString(), player.gameStats);
            i++;
        }
            
        Analytics.CustomEvent("Match Ended", new Dictionary<string, object> {
            { "Event_category", "game" },
            { "Event_name", "match ended" },
            { "Mode", gi.gameType.ToString() },
            { "Map", scene.name.ToString() },
            { "Scores", scores.ToStringFull() },
            { "Stats", stats.ToStringFull() },
            { "Local", PhotonNetwork.offlineMode.ToString() },
            { "Length", "min: " + (gameTime / 60.0).ToString()} 
        });
    }


    //one for each player (put this in playerInput or networkManager);
    
    public static void MatchPlayerDisconnect(PlayerInput player) {

        Scene scene = SceneManager.GetActiveScene();
        GameInitializer gi = FindObjectOfType<GameInitializer> ();

        Analytics.CustomEvent("Match Player Disconnect", new Dictionary<string, object> {
            { "Event_category", "game" },
            { "Event_name", "Match Player Disconnect" },
            { "Mode", gi.gameType.ToString() },
            { "Map", scene.name.ToString() },
            { "Player_id", player.playerId },
            { "Timestamp", Time.timeSinceLevelLoad.ToString() }
        });
    }


    //one for each player (put this in playerInput or networkManager);
    
    public static void MatchPlayerConnect(PlayerInput player) {

        Scene scene = SceneManager.GetActiveScene();
        GameInitializer gi = FindObjectOfType<GameInitializer> ();

        Analytics.CustomEvent("Match Player Disconnect", new Dictionary<string, object> {
            { "Event_category", "game" },
            { "Event_name", "Match Player Disconnect" },
            { "Mode", gi.gameType.ToString() },
            { "Map", scene.name.ToString() },
            { "Player_id", player.playerId },
            { "Timestamp", Time.timeSinceLevelLoad.ToString() }
        });
    }


    
    public static void Ping() {

        Scene scene = SceneManager.GetActiveScene();
        GameInitializer gi = FindObjectOfType<GameInitializer> ();

        Analytics.CustomEvent("Ping", new Dictionary<string, object> {
            { "Event_category", "game" },
            { "Event_name", "Ping" },
            { "Mode", gi.gameType.ToString() },
            { "Map", scene.name.ToString() },
            { "Local", PhotonNetwork.offlineMode.ToString() },
            { "Timestamp", Time.timeSinceLevelLoad.ToString() },
            { "Ping", PhotonNetwork.GetPing () }
        });

    }
        

}


