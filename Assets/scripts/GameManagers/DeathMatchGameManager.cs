using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using InControl;
using System;
using UnityEngine.UI;

public class DeathMatchGameManager : AbstractGameManager
{
    //Use this script for general things like managing the state of the game, tracking players and so on.


    public int playerWinPoints = 5;
    public int krakenWinPoints = 5;


    /* Later refactor into abstract common things */
    [HideInInspector]
    public cameraFollow[] cams;
    [HideInInspector]
    public PlayerSelectSettings ps;
    [HideInInspector]
    public bool isTeam;
    [HideInInspector]
    public bool includeKraken;
    [HideInInspector]
    public List<PlayerInput> players = new List<PlayerInput>();
    [HideInInspector]
    public KrakenInput kraken;
    [HideInInspector]
    public GameObject countDown;
    [HideInInspector]
    public GlobalCanvas globalCanvas;
    [HideInInspector]
    public Animator fadeInAnimator;
    [HideInInspector]
    public List<int> shipPoints = new List<int>();
    [HideInInspector]
    public GameObject map = null;

    Dictionary<string, int> gamePoints;

    bool gameOver = false;
    bool done = true;
    MonoBehaviour winnerScript;
    GameObject winner;
    int krakenPoints;

    int totalNumberOfReadyOnlinePlayers;
    int numOfStatsSynced = 1;
    int winnerId = -1;
    float gameTime;

    List<string> teamNames = new List<string> { "Red Team", "Blue Team", "Green Team", "Yellow Team" };
    Dictionary<string, string> teamToColor = new Dictionary<string, string> { { "Red Team", "red" }, { "Blue Team", "blue" }, { "Green Team", "green" }, { "Yellow Team", "yellow" } };
    string lastPoint = "The Replace Needs <color=\"orange\">ONE</color> Point To Win!";
    public Action onInitialize;
    public  GameObject gameOverStatPrefab;

    void Start()
    {
        MapObjects mapObjects = GameObject.FindObjectOfType<MapObjects>();
        gamePoints = new Dictionary<string, int>();

        GameObject.FindObjectOfType<Barrel>().gameObject.SetActive(false);
        Physics.gravity = new Vector3(0f, -0.1f, 0f);
        Application.targetFrameRate = -1; //Unlocks the framerate at start
        Resources.UnloadUnusedAssets();
        if (includeKraken)
        {
            kraken = GameObject.FindObjectOfType<KrakenInput>();
        }
        //Temp fix
        var uis = GameObject.FindObjectsOfType<UIManager>();
        foreach(UIManager ui in uis)
        {
            ui.gameObject.transform.Find("P1panel/Compass").gameObject.SetActive(false);
            ui.gameObject.transform.Find("P1panel/doubloonImage").gameObject.SetActive(true);
            ui.gameObject.transform.Find("P1panel/doubloonText").gameObject.SetActive(true);
            ui.gameObject.transform.Find("P1panel/enemyIslandSlider").gameObject.SetActive(false);
        }

        onInitialize();
        foreach(PlayerInput player in FindObjectsOfType<PlayerInput>())
        {
            if (player.GetId() == PhotonNetwork.player.ID || PhotonNetwork.offlineMode)
            {
                player.InitializeForDeathMatch();
            }
            gamePoints[player.GetId().ToString()] = 0;
        }
        LogAnalyticsGame.StartGame (players, this.countDown.GetComponent<CountDown>());
        LogAnalyticsGame.FpsSnapshot ();
        LogAnalyticsGame.Ping ();
    }



    [PunRPC]
    public void AddPlayer(int id) {
        //        Debug.Log(gamePoints.Keys.Count.ToString());
        if (GetComponent<PhotonView>().isMine)
        {
            gamePoints.Add(id.ToString(), 0);
            this.totalNumberOfReadyOnlinePlayers++;
            if (this.gamePoints.Keys.Count >= minPlayersRequiredToStartGame || (PhotonNetwork.offlineMode && id >= 1))
            {
                this.GetComponent<PhotonView>().RPC("SetDone", PhotonTargets.All);
            }
        }
    }

    [PunRPC]
    public void RemovePlayer(int id)
    {
        if (gamePoints.ContainsKey(id.ToString()))
        {
            gamePoints.Remove(id.ToString());
        }
    }

    [PunRPC]
    public void SetDone() {
        globalCanvas.waitingForPlayers.SetActive(false);
        done = false;
        
    }
    void gameStart()
    {
        this.gameStarted = true;
        PlayerInput [] playerInputs = FindObjectsOfType<PlayerInput>();
        players.Clear();
        players.AddRange(playerInputs);
        foreach (PlayerInput player in playerInputs)
        {
            player.gameStarted = true;
            player.setStatus(ShipStatus.Alive);
            gamePoints[player.GetId().ToString()] = 0;
        }
        if (kraken)
        {
            kraken.gameStarted = true;
        }

        gameTime += Time.deltaTime;

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
    }


    override public void respawnPlayer(PlayerInput player, Vector3 startingPoint, Quaternion startingRotation) {
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
        else if (attacker is PlayerInput)
        {
            var attackingPlayer = ((PlayerInput)attacker);
            attackingPlayer.gameStats.numOfKills++;
            if (!isTeam)
            {
                int index = players.IndexOf(attackingPlayer);
                int points = shipPoints[index];
                points++;
                shipPoints[index] = points;
                attackingPlayer.uiManager.updatePoint(points);

                //refactor so color in text script and just pass ship name into script.
                if (points == (playerWinPoints - 1))
                { //enemyIslandHealth == (0f);
                    if (attackingPlayer.shipName.Equals("Chinese Junk Ship"))
                    {
                        var textScripts = GameObject.FindObjectsOfType<ProgressScript>();
                        foreach (ProgressScript script in textScripts)
                        {
                            var newText = lastPoint.Replace("Replace", "<color=red>Chinese Junk Ship</color>");
                            script.activatePopup(newText, "Chinese Junk Ship", "Ship");
                        }
                    }
                    else if (attackingPlayer.shipName.Equals("Blackbeard Ship"))
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
                    winner = attackingPlayer.gameObject;
                }
            }
            else
            {
                shipPoints[attackingPlayer.teamNo] = shipPoints[attackingPlayer.teamNo] + 1;
                int points = shipPoints[attackingPlayer.teamNo];
                foreach (PlayerInput playr in this.getPlayers())
                {
                    if (playr.teamNo == attackingPlayer.teamNo)
                    {
                     
                        playr.uiManager.updatePoint(points);

                    }
                }
                if (points == playerWinPoints - 1)
                {

                    string teamName = teamNames[attackingPlayer.teamNo];
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
                    winner = attackingPlayer.gameObject;
                }
            }
        }


    }
    public void decrementPoint(KrakenInput kraken)
    {
        kraken.uiManager.decrementPoint();

    }

    [PunRPC]
    public void IncrementPoint(int id) {

        if (PhotonNetwork.player.ID == id && !PhotonNetwork.offlineMode) {

            var playerToIncrement = this.getPlayers().Find(playerToFilter => playerToFilter.GetId() == id) ;
            playerToIncrement.uiManager.updatePoint(int.Parse((playerToIncrement.uiManager.points.text)) + 1);
            
        }
        var players2 = getPlayers();
        foreach (PlayerInput player in players2)
        {
            if(player.GetId() == id)
            {
                player.AddKillStats(id);
                if (PhotonNetwork.offlineMode)
                {
                    player.uiManager.updatePoint(gamePoints[id.ToString()]+1);
                }
                break;
            }
        }

        if (gamePoints.ContainsKey(id.ToString()))
        {
            
            gamePoints[id.ToString()]++;
            if (gamePoints[id.ToString()] >= playerWinPoints && PhotonNetwork.isMasterClient)
            {
                this.GetComponent<PhotonView>().RPC("TriggerNetworkedVictory", PhotonTargets.All, id);
            }
            else if (gamePoints[id.ToString()] == playerWinPoints - 1)
            {
                this.GetComponent<PhotonView>().RPC("ActivateLastPointPrompt", PhotonTargets.All, id);

            }
        }
        
        
    }

    public override string getShipById(int id)
    {
        foreach(PlayerInput player in getPlayers())
        {
            if (player.GetId() == id)
            {
                return player.type.ToString();
            }
        }
        return "";
    }


    [PunRPC]
    private void ActivateLastPointPrompt(int id)
    {
        //TODO: Refactor into score keeper/kill feed controller
        if (PhotonNetwork.offlineMode)
        {
            foreach (PlayerInput player in this.getPlayers())
            {
                if (player.GetId() == id)
                {
                    var newText = lastPoint.Replace("The Replace", "You").Replace("Needs", "Need");
                    player.uiManager.GetComponentInChildren<ProgressScript>().activatePopup(newText, "You", "Ship");
                }
                else
                {
                    var newText = lastPoint.Replace("The Replace", "Player " + id.ToString());
                    player.uiManager.GetComponentInChildren<ProgressScript>().activatePopup(newText, "They", "Ship");
                }

            }

        }
        else
        {
            var textScript = GameObject.FindObjectOfType<ProgressScript>();
            if (PhotonNetwork.player.ID == id)
            {
                var newText = lastPoint.Replace("The Replace", "You").Replace("Needs", "Need");
                textScript.activatePopup(newText, "You", "Ship");
            }
            else {
                var newText = lastPoint.Replace("The Replace", "Player " + id.ToString());
                textScript.activatePopup(newText, "They", "Ship");
            }
        }
    }

    public void activateVictoryText()
    {
        Time.timeScale = 0.3f;
        foreach (PlayerInput p in this.getPlayers())
        {
            if(PhotonNetwork.offlineMode || p.GetId() == PhotonNetwork.player.ID)
            {
                p.uiManager.activateFinishAndColorTint();
            }
        }
        if (kraken)
        {
            kraken.uiManager.activateFinishAndColorTint();
        }
    }

    [PunRPC]
    public void TriggerNetworkedVictory(int id) {

        //Calculate Stats
        
        winnerId = id;
        SyncStats();

        players[0].gameStarted = false;
        players[0].hasWon = true;

    }

    private void SyncStats()
    {
        if (PhotonNetwork.offlineMode)
        {
            activateVictoryText();
            Invoke("TriggerStatsAnimation", 1.4f);
            return;
        }


        foreach (PlayerInput player in this.getPlayers())
        {
            var photonView = player.GetComponent<PhotonView>();
          
            if (photonView.ownerId == PhotonNetwork.player.ID)
            {
                this.GetComponent<PhotonView>().RPC("SyncStat", PhotonTargets.Others, player.GetId(), ArrayHelper.ObjectToByteArray(player.gameStats));
            }
        }
        Invoke("TimeOutStats", 4f);
    }

    [PunRPC]
    public void SyncStat(int id, byte[] statsBinary)
    {

        foreach (PlayerInput player in this.getPlayers())
        {
            if (player.GetComponent<PhotonView>().ownerId == id)
            { 
                FreeForAllStatistics stats = (FreeForAllStatistics)ArrayHelper.ByteArrayToObject(statsBinary);
                player.gameStats = stats;
                numOfStatsSynced++;
                break;
            }
        }
        if (numOfStatsSynced == this.getPlayers().Count)
        {
            activateVictoryText();
            Invoke("TriggerStatsAnimation", 1.4f);
            
        }
    }

    public void TimeOutStats()
    {
        CancelInvoke("TriggerStatsAnimation");
        TriggerStatsAnimation();
    }

    public void TriggerStatsAnimation()
    {
        CancelInvoke("TimeOutStats");
        triggerScreenAnimation();
        triggerStatScreen();
    }

    private void triggerStatScreen()
    {
        screenSplitter.SetActive(false);
        MapObjects map = GameObject.FindObjectOfType<MapObjects>();
        GameObject gameOverScreen = null;
        map.enabled = false;
        if(map.gameOverStatPrefab != null)
        {
            gameOverScreen = Instantiate(map.gameOverStatPrefab as GameObject);
        }
        else
        {
            gameOverScreen = Instantiate(this.gameOverStatPrefab as GameObject);
        }

        GameObject winnerModel = null;
        foreach(PlayerInput players in getPlayers())
        {
            if (players.GetId() == winnerId)
            {
                winnerModel = Instantiate(Resources.Load(PathVariables.GetAssociatedModelForShip(players.type), typeof(GameObject)) as GameObject);
                break;
            }
        }
        if (winnerModel == null)
        {
            return;
        }

        var animationController = gameOverStatPrefab.GetComponent<StatsAnimationController>();
        animationController.HookupCallback(CalculateAndShowStats);
       
    }

    private void triggerScreenAnimation()
    {
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
        LogAnalyticsGame.EndGame (this.getPlayers(), gameTime);

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
        foreach (PlayerInput p in this.getPlayers())
        {
            p.gameStarted = false;
        }
        if (kraken)
        {
            kraken.gameStarted = false;
        }


        //globalCanvas.finishText.SetActive(true);
        if (!isTeam)
        {
            
        }
        else
        {
            triggerVictoryScreenForTeamGame();
        }

    }

    private void triggerVictoryScreenForTeamGame()
    {
        Dictionary<int, List<PlayerInput>> teamToPlayers = new Dictionary<int, List<PlayerInput>>();
        foreach (PlayerInput z in this.getPlayers())
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
        for (int x = 0; x < shipPoints.Count; x++)
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


    public void CalculateAndShowStats()
    {

        GameOverStatsUI gameOverUI = globalCanvas.gameOverUI;

        List<FreeForAllStatistics> shipStats = new List<FreeForAllStatistics>();
        List<FreeForAllStatistics> krakenStats = new List<FreeForAllStatistics>();

        PlayerInput winner = null;
        List<GameObject> losers = new List<GameObject>();
        foreach (PlayerInput ship in this.getPlayers())
        {
            ship.reset();
            ship.gameStats.titles = new List<Title>();
            ship.setStatus(ShipStatus.Inactive);
            if (ship.followCamera)
            {
                ship.followCamera.enabled = false;
            }
            if (ship.GetId() == winnerId)
            {
                winner = ship;
            }
            else
            {
                losers.Add(ship.gameObject);
            }
            ship.DisableUIForStats();
            shipStats.Add(ship.gameStats);
        }


        GameObject titlesPrefab = Resources.Load(PathVariables.titlesPath, typeof(GameObject)) as GameObject;
        Titles titles = titlesPrefab.GetComponent<Titles>();
        titles.calculateTitles(shipStats, krakenStats);

        gameOverUI.startFading = true;
    }



    override public void ExitToCharacterSelect()
    {
        Time.timeScale = 1;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("start");
    }
    public void restartCurrentGame()
    {
        DontDestroyOnLoad(ps);

        SceneManager.LoadScene("free for all_vig");
    }

    internal override int getNumberOfTeams()
    {
        return gamePoints.Count;
    }

    public override List<PlayerInput> getPlayers()
    {
        return players.Filter(player => player != null);
    }

    public string getTeamName(PlayerInput player)
    {
        return teamNames[player.teamNo];
    }


    public override Dictionary<string, int> getGamepoints() {

        return gamePoints;
    }

    public override int getPlayerPointsToWIn () {
        return playerWinPoints;
    }

}
