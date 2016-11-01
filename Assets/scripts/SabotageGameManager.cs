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
    [HideInInspector] public bool team;
    [HideInInspector] public List<playerInput> players = new List<playerInput>();
    [HideInInspector] public KrakenInput kraken;
    [HideInInspector] public GameObject countDown;
    [HideInInspector] public GlobalCanvas globalCanvas;
    [HideInInspector] public GameObject screenSplitter;
    [HideInInspector] public Animator fadeInAnimator;

    bool gameOver = false;
    bool done = false;
    MonoBehaviour winnerScript;
    GameObject winner;
    string lastPoint = "The Replace Needs <color=\"orange\">ONE</color> Point To Win!";

    void Start()
    {
       
        Physics.gravity = new Vector3(0f, -0.1f, 0f);
        Application.targetFrameRate = -1; //Unlocks the framerate at start
        Resources.UnloadUnusedAssets();
        barrels = GameObject.FindGameObjectsWithTag("barrel");
        barrels_start_pos = new Vector3[barrels.Length];

        int x = 0;

        foreach (GameObject barrel in barrels)
        {
            barrels_start_pos[x] = barrel.transform.position;
            x++;
        }

    }



    void gameStart()
    {

        foreach (playerInput player in players)
        {
            player.gameStarted = true;
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


    override public void respawnPlayer(playerInput player, Vector3 startingPoint)
    {

        player.gameObject.transform.position = startingPoint;


    }
    override public void respawnKraken(KrakenInput player, Vector3 startingPoint)
    {

        player.gameObject.transform.position = startingPoint;


    }


    IEnumerator teleportBarrel(playerInput player, GameObject barrel)
    {

        yield return new WaitForSeconds(1);

        Vector3 anchor = new Vector3(0, 0.06f, 0.06f);
        if (barrel.GetComponent<CharacterJoint>() != null)
        {
            anchor = barrel.GetComponent<CharacterJoint>().anchor;
            Destroy(barrel.GetComponent<CharacterJoint>());
        }

        barrel b = barrel.GetComponent<barrel>();
        b.explodeBarrel();

        player.uiManager.decrementEnemyHealth(); //set in UIManager itself

        int x = 0;
        foreach (GameObject barr in barrels)
        {
            if (barr == barrel)
            {
                barrel.transform.position = barrels_start_pos[x];
                barrel.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
                break;
            }
            x++;
        }
        barrel.AddComponent<CharacterJoint>();
        barrel.GetComponent<CharacterJoint>().anchor = anchor;
        barrel.GetComponent<barrel>().activatePillar();
    }

    public override bool isGameOver()
    {
        return gameOver;
    }

    override public void incrementPoint(KrakenInput kraken)
    {
        int points = kraken.uiManager.incrementPoint();
        kraken.uiManager.setScoreBar(points / krakenWinPoints);

        if (points == (krakenWinPoints - 1))
        {

            var textScripts = GameObject.FindObjectsOfType<ProgressScript>();
            foreach (ProgressScript script in textScripts)
            {
                var newText = lastPoint.Replace("Replace", "<color=purple>Kraken</color>");
                script.activatePopup(newText, "Kraken", "Kraken");
            }

        }
        if (points == krakenWinPoints && winner == null)
        {
            activateVictoryText();
            Invoke("triggerVictory", 1.2f);
            winner = kraken.gameObject;
        }

    }
    public void decrementPoint(KrakenInput kraken)
    {
        kraken.uiManager.decrementPoint();

    }

    override public void incrementPoint(playerInput player, GameObject barrl)
    {
        if (!team)
        {
            player.GetComponent<Hookshot>().UnHook();
            player.GetComponent<Hookshot>().enabled = false;
            StartCoroutine(teleportBarrel(player, barrl));
            player.GetComponent<Hookshot>().enabled = true;
            float points = (float)(player.uiManager.incrementPoint());
            player.uiManager.setScoreBar(points / playerWinPoints);

            //refactor so color in text script and just pass ship name into script.
            if (points == (playerWinPoints - 1))
            { //enemyIslandHealth == (0f);
                if (player.shipName.Equals("Chinese Junk Ship"))
                {
                    var textScripts = GameObject.FindObjectsOfType<ProgressScript>();
                    foreach (ProgressScript script in textScripts)
                    {
                        var newText = lastPoint.Replace("Replace", "<color=red>Chinese Junk Ship</color>");
                        script.activatePopup(newText, "Chinese Junk Ship", "Ship");
                    }
                }
                else if (player.shipName.Equals("Blackbeard Ship"))
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
                winner = player.gameObject;
            }
        }
        else
        {
            player.GetComponent<Hookshot>().UnHook();
            StartCoroutine(teleportBarrel(player, barrl));
            int points = 0;
            foreach (playerInput p in players)
            {
                points = p.uiManager.incrementPoint();
                p.uiManager.setScoreBar(points / playerWinPoints);
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
        foreach (playerInput p in players)
        {
            p.uiManager.activateFinishAndColorTint();
        }
        kraken.uiManager.activateFinishAndColorTint();
    }

    public void triggerVictory()
    {


        if (winner.GetComponent<playerInput>())
        {
            winnerScript = winner.GetComponent<playerInput>();
            ((playerInput)winnerScript).hasWon = true;

        }
        else
        {
            winnerScript = winner.GetComponent<KrakenInput>();
            ((KrakenInput)winnerScript).hasWon = true;

        }

        //player.victoryScreen.SetActive (true);
        foreach (playerInput p in players)
        {
            p.gameStarted = false;
        }
        kraken.gameStarted = false;


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
        triggerVictoryScreen();


    }



    public void triggerVictoryScreen()
    {
        Time.timeScale = 1f;
        //fadeInAnimator.SetBool("fade", true);
        gameOver = true;
        kraken.reset();
        foreach (playerInput z in players)
        {
            z.reset();
            z.followCamera.enabled = false;
        }
        kraken.followCamera.enabled = false;
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

        if (winnerScript is playerInput)
        {
            playerInput player = ((playerInput)winnerScript);
            winStat = player.gameStats;
            shipStats.Add(player.gameStats);
            gameOverUI.winnerText.text = gameOverUI.winnerText.text.Replace("Replace", player.shipName);
            gameOverUI.winners[0].name.text = player.shipName;
            foreach (playerInput input in players)
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
            foreach (playerInput input in players)
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
            playerInput loserInput = losers[x].GetComponent<playerInput>();
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


}