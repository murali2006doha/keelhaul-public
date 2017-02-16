using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using InControl;
using System;

public class KrakenHuntGameManager : AbstractGameManager
{
    //Use this script for general things like managing the state of the game, tracking players and so on.

    GameObject[] barrels;
    Vector3[] barrels_start_pos;
    Vector3 barrel_start_pos;

    public int playerWinPoints = 3;
    public int krakenWinPoints = 5;


    /* Later refactor into abstract common things */
    [HideInInspector] public cameraFollow[] cams;
    [HideInInspector] public PlayerSelectSettings ps;
    [HideInInspector] public bool includeKraken;
    [HideInInspector] public List<PlayerInput> players = new List<PlayerInput>();
    [HideInInspector] public KrakenInput kraken;
    [HideInInspector] public GameObject countDown;
    [HideInInspector] public GlobalCanvas globalCanvas;
    [HideInInspector] public Animator fadeInAnimator;
    [HideInInspector] public List<int> shipPoints = new List<int>();

    bool gameOver = false;
    bool done = false;
    MonoBehaviour winnerScript;
    GameObject winner;
    int krakenPoints;

    string lastPoint = "The Replace Needs <color=\"orange\">ONE</color> Point To Win!";

    void Start()
    {
        MapObjects mapObjects = GameObject.FindObjectOfType<MapObjects>();
        //Disable unused islands
        for (int z = shipPoints.Count; z < mapObjects.islands.Length; z++)
        {
            mapObjects.islands[z].gameObject.SetActive(false);
        }

        this.RunStartUpActions();
        this.kraken = GameObject.FindObjectOfType<KrakenInput>();


    }



    void gameStart()
    {

        foreach (PlayerInput player in players)
        {
            player.gameStarted = true;
            player.setStatus(ShipStatus.Alive);
        }
        if (kraken)
        {
            kraken.gameStarted = true;
        }
    }

    void destroyCountDown()
    {
        Destroy(countDown);
    }


    void Update()
    {

        //puts the camera in the starting positions as soon as the game starts
        if (!done)
        {
            if (!countDown.gameObject.activeSelf)
            {

                countDown.SetActive(true);
                this.screenSplitter.SetActive(true);

                foreach (cameraFollow k in cams)
                {
                    k.camera.gameObject.SetActive(true);
                }
            }

            if (countDown.GetComponent<CountDown>().done)
            {
                gameStart();
                done = true;

                Invoke("destroyCountDown", 2f);
            }
        }

        foreach (cameraFollow k in cams)
        {
            k.camera.gameObject.SetActive(true);
        }

        this.demoScript(cams);
    }



    override public void respawnPlayer(PlayerInput player, Vector3 startingPoint, Quaternion startingRotation)
    {
        player.gameObject.transform.position = startingPoint;
        player.gameObject.transform.rotation = startingRotation;
    }

    override public void respawnKraken(KrakenInput player, Vector3 startingPoint)
    {

        player.gameObject.transform.position = startingPoint;


    }

    public override bool isGameOver()
    {
        return gameOver;
    }

    override public void acknowledgeKill(StatsInterface attacker, StatsInterface victim)
    {
        if (attacker is KrakenInput)
        {
            krakenPoints++;
            kraken.uiManager.updatePoint(krakenPoints);
            kraken.incrementPoint();
            kraken.uiManager.setScoreBar(krakenPoints / krakenWinPoints);
            ((KrakenInput)attacker).gameStats.numOfKills++;
            if (krakenPoints == (krakenWinPoints - 1))
            {

                var textScripts = GameObject.FindObjectsOfType<ProgressScript>();
                foreach (ProgressScript script in textScripts)
                {
                    var newText = lastPoint.Replace("Replace", "<color=purple>Kraken</color>");
                    script.activatePopup(newText, "Kraken", "Kraken");
                }

            }
            if (krakenPoints == krakenWinPoints && winner == null)
            {
                activateVictoryText();
                Invoke("triggerVictory", 1.2f);
                winner = kraken.gameObject;
            }
        }
        else if (attacker is PlayerInput && victim is KrakenInput)
        {
            PlayerInput ship = ((PlayerInput) attacker);
            ship.gameStats.numOfKills++;
            int index = players.IndexOf(ship);
            int points = shipPoints[index];
            points++;
            shipPoints[index] = points;
            ship.uiManager.decrementEnemyHealth();
            ship.uiManager.setScoreBar(points / playerWinPoints);

            if (points == (playerWinPoints - 1))
            { //enemyIslandHealth == (0f);
                if (ship.shipName.Equals("Chinese Junk Ship"))
                {
                    var textScripts = GameObject.FindObjectsOfType<ProgressScript>();
                    foreach (ProgressScript script in textScripts)
                    {
                        var newText = lastPoint.Replace("Replace", "<color=red>Chinese Junk Ship</color>");
                        script.activatePopup(newText, "Chinese Junk Ship", "Ship");
                    }
                }
                else if (ship.shipName.Equals("Blackbeard Ship"))
                {
                    var textScripts = GameObject.FindObjectsOfType<ProgressScript>();
                    foreach (ProgressScript script in textScripts)
                    {
                        var newText = lastPoint.Replace("Replace", "<color=black>Blackbeard</color>");
                        script.activatePopup(newText, "Blackbeard", "Ship");
                    }
                }
                else
                {
                    var textScripts = GameObject.FindObjectsOfType<ProgressScript>();
                    foreach (ProgressScript script in textScripts)
                    {
                        var newText = lastPoint.Replace("Replace", "<color=blue>Atlantean Ship</color>");
                        script.activatePopup(newText, "Atlantean Ship", "Ship");
                    }
                }

            }
            if (points == playerWinPoints && winner == null)
            {
                activateVictoryText();
                Invoke("triggerVictory", 1.2f);
                winner = ship.gameObject;
            }
        }
    }
    public void decrementPoint(KrakenInput kraken)
    {
        kraken.uiManager.decrementPoint();

    }


    public void activateVictoryText()
    {
        Time.timeScale = 0.3f;
        foreach (PlayerInput p in players)
        {
            p.uiManager.activateFinishAndColorTint();
        }
        if (kraken)
        {
            kraken.uiManager.activateFinishAndColorTint();
        }
    }

    public void triggerVictory()
    {


        if (winner.GetComponent<PlayerInput>())
        {
            winnerScript = winner.GetComponent<PlayerInput>();
            ((PlayerInput)winnerScript).hasWon = true;

        }
        else
        {
            winnerScript = winner.GetComponent<KrakenInput>();
            ((KrakenInput)winnerScript).hasWon = true;

        }

        //player.victoryScreen.SetActive (true);
        foreach (PlayerInput p in players)
        {
            p.gameStarted = false;
        }
        if (kraken)
        {
            kraken.gameStarted = false;
        }


        //globalCanvas.finishText.SetActive(true);
        Texture2D texture = new Texture2D(Screen.width, Screen.height / 2, TextureFormat.RGB24, true);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height / 2), 0, 0);
        texture.Apply();
        Texture2D texture2 = new Texture2D(Screen.width, Screen.height / 2, TextureFormat.RGB24, true);
        texture2.ReadPixels(new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2), 0, 0);
        texture2.Apply();
        globalCanvas.panel1.texture = texture2;
        globalCanvas.panel2.texture = texture;
        globalCanvas.panel1.gameObject.SetActive(true);
        globalCanvas.panel2.gameObject.SetActive(true);
        Time.timeScale = 1f;
        gameOver = true;
        triggerVictoryScreen();
    }

    public void triggerVictoryScreen()
    {

        kraken.reset();
        kraken.followCamera.enabled = false;
        foreach (PlayerInput z in players)
        {
            z.reset();
            z.setStatus(ShipStatus.Waiting);
            z.followCamera.enabled = false;
        }

        screenSplitter.SetActive(false);
        MapObjects map = GameObject.FindObjectOfType<MapObjects>();
        //Refactor out of map

        map.gameOverCamera.gameObject.SetActive(true);

        GameOverStatsUI gameOverUI = globalCanvas.gameOverUI;
        gameOverUI.gameObject.SetActive(true);

        List<FreeForAllStatistics> shipStats = new List<FreeForAllStatistics>();
        List<FreeForAllStatistics> krakenStats = new List<FreeForAllStatistics>();

        FreeForAllStatistics winStat = null;
        List<GameObject> losers = new List<GameObject>();

        if (winnerScript is PlayerInput)
        {
            PlayerInput player = ((PlayerInput)winnerScript);
            winStat = player.gameStats;
            shipStats.Add(player.gameStats);
            gameOverUI.winnerText.text = gameOverUI.winnerText.text.Replace("Replace", player.shipName);
            gameOverUI.winners[0].name.text = player.shipName;
            foreach (PlayerInput input in players)
            {
                if (input != player)
                {
                    losers.Add(input.gameObject);
                    shipStats.Add(input.gameStats);
                }
            }
            losers.Add(kraken.gameObject);
            krakenStats.Add(kraken.gameStats);
        }
        else if (winnerScript is KrakenInput)
        {
            KrakenInput player = ((KrakenInput)winnerScript);
            winStat = player.gameStats;
            krakenStats.Add(player.gameStats);
            gameOverUI.winnerText.text = gameOverUI.winnerText.text.Replace("Replace", "Kraken");
            gameOverUI.winners[0].name.text = "Kraken";
            foreach (PlayerInput input in players)
            {
                losers.Add(input.gameObject);
                shipStats.Add(input.gameStats);
            }
        }

        //Winner
        winnerScript.gameObject.transform.position = new Vector3(map.winnerLoc.transform.position.x, winnerScript.gameObject.transform.position.y, map.winnerLoc.transform.position.z);
        winnerScript.gameObject.transform.localScale *= 2f;
        winnerScript.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

        //Losers
        losers[0].transform.position = new Vector3(map.loser1loc.transform.position.x, losers[0].transform.position.y, map.loser1loc.transform.position.z);
        losers[0].transform.position = map.loser1loc.transform.position;
        losers[0].transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

        losers[1].transform.position = new Vector3(map.loser2loc.transform.position.x, losers[1].transform.position.y, map.loser2loc.transform.position.z);
        losers[1].transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));


        GameObject titlesPrefab = Resources.Load(PathVariables.titlesPath, typeof(GameObject)) as GameObject;
        Titles titles = titlesPrefab.GetComponent<Titles>();
        titles.calculateTitles(shipStats, krakenStats);

        int num = 0;
        foreach (Title title in winStat.titles)
        {
            if (num >= gameOverUI.winners[0].titles.Length)
            {
                break;
            }
            gameOverUI.winners[0].titles[num].text = title.name;
            gameOverUI.winners[0].titleStats[num].text = title.statsString;
            num++;
        }

        for (int x = 0; x < 2; x++)
        {
            num = 0;
            PlayerInput loserInput = losers[x].GetComponent<PlayerInput>();
            FreeForAllStatistics loserStat = null;
            if (loserInput != null)
            {
                loserStat = loserInput.gameStats;
                gameOverUI.losers[x].name.text = loserInput.shipName;
            }
            else
            {
                KrakenInput krakn = losers[x].GetComponent<KrakenInput>();
                loserStat = krakn.gameStats;
                gameOverUI.losers[x].name.text = "Kraken";
            }

            foreach (Title title in loserStat.titles)
            {
                if (num >= gameOverUI.winners[0].titles.Length)
                {
                    break;
                }
                gameOverUI.losers[x].titles[num].text = title.name;
                gameOverUI.losers[x].titleStats[num].text = title.statsString;
                num++;
            }

        }
        Invoke("enableStats", 4f);


    }

    public void enableStats()
    {
        GameOverStatsUI gameOverUI = globalCanvas.gameOverUI;
        gameOverUI.startFading = true;
    }



    override public void exitToCharacterSelect()
    {
        if (!ps)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Destroy(ps.gameObject);
            SceneManager.LoadScene("start2");
        }
    }
    public void restartCurrentGame()
    {
        DontDestroyOnLoad(ps);

        SceneManager.LoadScene("free for all_vig");
    }

    internal override int getNumberOfTeams()
    {
        return shipPoints.Count;
    }
    public override List<PlayerInput> getPlayers()
    {
        return players;
    }
}