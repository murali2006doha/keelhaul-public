using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class StatsModal : MonoBehaviour {


    DeathMatchGameManager gm;
    public GameObject gridLayoutContainer;

    //need player name, points, isteam or not, team name if isteam
    public void InitializeStats() {
        DeathMatchGameManager[] x = FindObjectsOfType<DeathMatchGameManager> ();
        foreach (DeathMatchGameManager manager in FindObjectsOfType<DeathMatchGameManager> ()) {
            //if (manager.gameObject.GetComponent<PhotonView>().ownerId == PhotonNetwork.masterClient.ID) {
            if (manager.gameObject.GetComponent<PhotonView>().isMine) {
                gm = manager;
            }
        }
        ShowStats ();
    }



    void ShowStats() {


        int i = 1;
        foreach (string id in gm.getGamepoints().Keys) {
            //string name = gm.getShipById (int.Parse (id));
            int score = gm.getGamepoints() [id];
            string scoreText =  score.ToString() + "/" + gm.getPlayerPointsToWIn().ToString();
            //string team = gm.getTeamName (player);
            float scorePercent = ((float) score / (float) gm.getPlayerPointsToWIn ());
            Sprite image = Resources.Load<Sprite>(PathVariables.GetAssociatedPortraitPath (ShipEnum.AtlanteanShip));

            PlayerStatsPanel statsBox = InstantiatePlayerStatsBox ();
            statsBox.Initialize (scorePercent, scoreText, i.ToString(), "player: " + id, image, 1);
            i++;
        }
    }



    PlayerStatsPanel InstantiatePlayerStatsBox () {
        UnityEngine.Object playerStatsPrefab = Resources.Load (PathVariables.playerStatsPath);
        GameObject statsObject = (GameObject)GameObject.Instantiate (playerStatsPrefab, Vector3.zero, Quaternion.identity);
        PlayerStatsPanel statsBox = (PlayerStatsPanel)statsObject.GetComponent<PlayerStatsPanel> ();
        statsObject.transform.SetParent (gridLayoutContainer.transform, false);
        return statsBox;
    }


    Dictionary<string, int> SortPlayers() {
        Dictionary<string, int> players = new Dictionary<string, int>();
        foreach (KeyValuePair<string,int> item in gm.getGamepoints ().OrderBy(key => key.Value)) {
            players.Add (item.Key, item.Value);
        }
        return players;
    }


    public void ClearStats() {
        PlayerStatsPanel[] panels = gridLayoutContainer.gameObject.GetComponentsInChildren<PlayerStatsPanel> ();
        foreach (PlayerStatsPanel panel in panels) {
            Destroy(panel.gameObject);
        }
    }


    void RefreshStats() {
        ClearStats ();
        ShowStats();
    }

    
    // Update is called once per frame
    void Update () {
        
    }
}
