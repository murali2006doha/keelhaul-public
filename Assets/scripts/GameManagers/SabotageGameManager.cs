using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using InControl;
using System;

public class SabotageGameManager : AbstractGameManager
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
    [HideInInspector] public bool isTeam;
    [HideInInspector] public bool includeKraken;
    [HideInInspector] public List<PlayerInput> players = new List<PlayerInput>();
    [HideInInspector] public KrakenInput kraken;
    [HideInInspector] public GameObject countDown;
    [HideInInspector] public GlobalCanvas globalCanvas;
    [HideInInspector] public Animator fadeInAnimator;
    public List<int> shipPoints = new List<int>();

    bool gameOver = false;
    bool done = false;
    MonoBehaviour winnerScript;
    GameObject winner;
    int krakenPoints;
    float gameTime;

    List<string> teamNames = new List<string> { "Red Team", "Blue Team", "Green Team", "Yellow Team" };
    Dictionary<string, string> teamToColor = new Dictionary<string, string> { { "Red Team", "red" }, { "Blue Team", "blue" }, { "Green Team", "green" }, { "Yellow Team", "yellow" } };



    string lastPoint = "The Replace Needs <color=\"orange\">ONE</color> Point To Win!";

    void Start()
    {
        MapObjects mapObjects = GameObject.FindObjectOfType<MapObjects>();
        //Disable unused islands
        for(int z = shipPoints.Count; z < mapObjects.islands.Length; z++)
        {
            mapObjects.islands[z].gameObject.SetActive(false);
        }

        this.RunStartUpActions();
        barrels = GameObject.FindGameObjectsWithTag("barrel");
        barrels_start_pos = new Vector3[barrels.Length];

        int x = 0;

        foreach (GameObject barrel in barrels)
        {
            barrels_start_pos[x] = barrel.transform.position;
            x++;
        }
        if (includeKraken)
        {
            kraken = GameObject.FindObjectOfType<KrakenInput>();
        }
        if(includeKraken && shipPoints.Count == 1)
        {
            //Temp fix
            var uis = GameObject.FindObjectsOfType<UIManager>();
            foreach (UIManager ui in uis)
            {
                if (ui.gameObject.transform.FindChild("P1panel/Compass") != null)
                {
                   
                    ui.gameObject.transform.FindChild("P1panel/doubloonImage").gameObject.SetActive(true);
                    ui.gameObject.transform.FindChild("P1panel/doubloonText").gameObject.SetActive(true);
                    ui.gameObject.transform.FindChild("P1panel/enemyIslandSlider").gameObject.SetActive(false);
                }
               
            }
            foreach (GameObject barrel in barrels)
            {
                barrel.GetComponent<Barrel>().disableDetonate();
               
            }

        }
        LogAnalyticsGame.StartGame (players, this.countDown.GetComponent<CountDown>());


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
                screenSplitter.SetActive(true);

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

        demoScript();
    }





    void demoScript()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            screenSplitter.SetActive(false);
            cams[0].camera.rect = new Rect(0, 0, 1, 1);
            cams[1].camera.rect = new Rect(0, 0, 0, 0);
            cams[2].camera.rect = new Rect(0, 0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            screenSplitter.SetActive(false);
            cams[1].camera.rect = new Rect(0, 0, 1, 1);
            cams[0].camera.rect = new Rect(0, 0, 0, 0);
            cams[2].camera.rect = new Rect(0, 0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            screenSplitter.SetActive(false);
            cams[2].camera.rect = new Rect(0, 0, 1, 1);
            cams[0].camera.rect = new Rect(0, 0, 0, 0);
            cams[1].camera.rect = new Rect(0, 0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SceneManager.LoadScene(1);
        }
    }


    override public void respawnPlayer(PlayerInput player, Vector3 startingPoint)
    {

        player.gameObject.transform.position = startingPoint;


    }
    override public void respawnKraken(KrakenInput player, Vector3 startingPoint)
    {

        player.gameObject.transform.position = startingPoint;


    }


    IEnumerator teleportBarrel(PlayerInput player, GameObject barrel)
    {

        yield return new WaitForSeconds(1);

        Vector3 anchor = new Vector3(0, 0.06f, 0.06f);
        if (barrel.GetComponent<CharacterJoint>() != null)
        {
            anchor = barrel.GetComponent<CharacterJoint>().anchor;
            Destroy(barrel.GetComponent<CharacterJoint>());
        }

        Barrel b = barrel.GetComponent<Barrel>();
        b.explodeBarrel();


        int x = 0;
        foreach (GameObject barrelObj in barrels)
        {
            if (barrelObj == barrel)
            {
                barrel.transform.position = barrels_start_pos[x];
                barrel.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
                break;
            }
            x++;
        }
        barrel.AddComponent<CharacterJoint>();
        barrel.GetComponent<CharacterJoint>().anchor = anchor;
        barrel.GetComponent<Barrel>().activatePillar();
    }

    public override bool isGameOver()
    {
        return gameOver;
    }

    override public void acknowledgeKill(StatsInterface attacker, StatsInterface victim)
    {
        if(attacker is KrakenInput)
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
        } else if(attacker is PlayerInput)
        {
            ((PlayerInput)attacker).gameStats.numOfKills++;
        }
       

    }
    public void decrementPoint(KrakenInput kraken)
    {
        kraken.uiManager.decrementPoint();

    }

    override public void acknowledgeBarrelScore(PlayerInput player, GameObject barrel)
    {
        if (!isTeam)
        {
            player.GetComponent<HookshotComponent>().UnHook();
            player.GetComponent<HookshotComponent>().enabled = false;
            StartCoroutine(teleportBarrel(player, barrel));
            player.GetComponent<HookshotComponent>().enabled = true;
            int index = players.IndexOf(player);
            int points = shipPoints[index];
            points++;
            shipPoints[index] = points;
            player.uiManager.decrementEnemyHealth();
            player.uiManager.setScoreBar(points / playerWinPoints);

            if (points == playerWinPoints - 1)
            {
                var textScripts = GameObject.FindObjectsOfType<ProgressScript>();
                foreach (ProgressScript script in textScripts)
                {
                    var newText = "<color=" + ">" + GlobalVariables.ShipToColor[player.type] + "</color>" + " needs one point to win!!!";
                    script.activatePopup(newText, "Ship", "Ship");
                }

            }

            if (points == playerWinPoints)
            {
                activateVictoryText();
                Invoke("triggerVictory", 1.2f);
                winner = player.gameObject;
            }

            //refactor so color in text script and just pass ship name into script.
        }
        else
        {
            
            player.GetComponent<HookshotComponent>().UnHook();
            StartCoroutine(teleportBarrel(player, barrel));
            shipPoints[player.teamNo] = shipPoints[player.teamNo] + 1;
            int points = shipPoints[player.teamNo];
            foreach(PlayerInput playr in players)
            {
                if (playr.teamNo == player.teamNo)
                {
                    playr.uiManager.updatePoint(points);
                    playr.uiManager.setScoreBar(points / playerWinPoints);
                    playr.uiManager.decrementEnemyHealth();

                }
            }
            if(points == playerWinPoints - 1)
            {
                
                string teamName = teamNames[player.teamNo];
                var textScripts = GameObject.FindObjectsOfType<ProgressScript>();
                foreach (ProgressScript script in textScripts)
                {
                    var newText = "<color=" + teamToColor[teamName] + ">" + teamName + "</color>" + " needs one point to win!!!";
                    script.activatePopup(newText, "Ship", "Ship");
                }

            }

            if (points == playerWinPoints)
            {
                activateVictoryText();
                Invoke("triggerVictory", 1.2f);
                winner = player.gameObject;
            }
        }


    }

    public void activateVictoryText()
    {
        Time.timeScale = 0.3f;
        foreach (PlayerInput p in players)
        {
            p.uiManager.activateFinishAndColorTint();
        }
        if (kraken) {
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
        if (!isTeam)
        {
            triggerVictoryScreen();
        }
        else
        {
            if (includeKraken && shipPoints.Count == 1)
            {
                triggerVictorForKrakenVsPirates();
            }
            else
            {
                triggerVictoryScreenForTeamGame();
            }
        }

    }

    private void triggerVictorForKrakenVsPirates()
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
        map.gameOverCamera.gameObject.SetActive(true);

        GameOverStatsUI gameOverUI = globalCanvas.gameOverUI;
        gameOverUI.gameObject.SetActive(true);
        if (winnerScript.GetType() == typeof(PlayerInput))
        {
            gameOverUI.winnerText.text = gameOverUI.winnerText.text.Replace("Replace", "The Pirates");
            Transform winnerTransform = map.winnerLoc.transform;
            foreach (PlayerInput player in players)
            {
                Transform trans = winnerTransform.GetChild(player.placeInTeam);
                player.gameObject.transform.position = new Vector3(trans.position.x, player.gameObject.transform.position.y, trans.position.z);
                player.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                player.gameObject.transform.localScale *= 2f;
            }

            Transform loserTransform = map.loser1loc.transform;

            Transform t = loserTransform;
            kraken.gameObject.transform.position = new Vector3(t.position.x, kraken.gameObject.transform.position.y, t.position.z);
            kraken.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

       }
        else
        {
            gameOverUI.winnerText.text = gameOverUI.winnerText.text.Replace("Replace", "Kraken");
            Transform winnerTransform = map.winnerLoc.transform;
            Transform trans = winnerTransform;
            kraken.gameObject.transform.position = new Vector3(trans.position.x, kraken.gameObject.transform.position.y, trans.position.z);
            kraken.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            kraken.gameObject.transform.localScale *= 2f;

            Transform loserTransform = map.loser1loc.transform;
            foreach (PlayerInput player in players)
            {
                Transform t = loserTransform.GetChild(player.placeInTeam);
                player.gameObject.transform.position = new Vector3(t.position.x, player.gameObject.transform.position.y, t.position.z);
                player.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            }
        }
    }

    private void triggerVictoryScreenForTeamGame()
    {
        Dictionary<int, List<PlayerInput>> teamToPlayers = new Dictionary<int, List<PlayerInput>>();
        foreach (PlayerInput z in players)
        {
            z.reset();
            z.setStatus(ShipStatus.Waiting);
            z.followCamera.enabled = false;
            if (!teamToPlayers.ContainsKey(z.teamNo))
            {
                teamToPlayers.Add(z.teamNo, new List<PlayerInput>());
            }
            teamToPlayers[z.teamNo].Add(z);
        }
        int winningTeam = ((PlayerInput)winnerScript).teamNo;
        screenSplitter.SetActive(false);
        MapObjects map = GameObject.FindObjectOfType<MapObjects>();
        map.gameOverCamera.gameObject.SetActive(true);

        GameOverStatsUI gameOverUI = globalCanvas.gameOverUI;
        gameOverUI.gameObject.SetActive(true);

        gameOverUI.winnerText.text = gameOverUI.winnerText.text.Replace("Replace", teamNames[winningTeam]);


        int losingTeamNum = 0;
        for(int x = 0; x < shipPoints.Count; x++)
        {
            List<PlayerInput> teamPlayers = teamToPlayers[x];
            if (x == winningTeam)
            {
                Transform winnerTransform = map.winnerLoc.transform;
                foreach (PlayerInput player in teamPlayers)
                {
                    Transform t = winnerTransform.GetChild(player.placeInTeam);
                    player.gameObject.transform.position = new Vector3(t.position.x, player.gameObject.transform.position.y, t.position.z);
                    player.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                    player.gameObject.transform.localScale *= 2f;
                }
            }
            else
            {

                Transform loserTransform = losingTeamNum == 0 ? map.loser1loc.transform : map.loser2loc.transform;
                foreach (PlayerInput player in teamPlayers)
                {
                    Transform t = loserTransform.GetChild(player.placeInTeam);
                    player.gameObject.transform.position = new Vector3(t.position.x, player.gameObject.transform.position.y, t.position.z);
                    player.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                }

            }
            

        }



        
        



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
        else {
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