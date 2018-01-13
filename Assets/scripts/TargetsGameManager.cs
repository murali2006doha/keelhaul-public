using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using InControl;
using System;
using UnityEngine.UI;

public class TargetsGameManager : AbstractGameManager
{
    //Use this script for general things like managing the state of the game, tracking players and so on.


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

    Dictionary<string, int> gamePoints;

    bool gameOver = false;
    bool done = true;

    float gameTime;

    public Action onInitialize;
    TargetRounds rounds;
    float roundStartTime;
    UIManager ui;
    PlayerInput player;
    public List<float> times = new List<float>();
    int maxTime = 30;
    public GameObject postGamePrefab;
    void Start()
    {
        base.Start();
        gamePoints = new Dictionary<string, int>();

     
        Physics.gravity = new Vector3(0f, -0.1f, 0f);
        Application.targetFrameRate = -1; //Unlocks the framerate at start
        Resources.UnloadUnusedAssets();
        //Temp fix
        ui = GameObject.FindObjectOfType<UIManager>();
        
        ui.gameObject.transform.Find("P1panel/Compass").gameObject.SetActive(false);
        ui.gameObject.transform.Find("P1panel/doubloonImage").gameObject.SetActive(true);
        ui.gameObject.transform.Find("P1panel/doubloonText").gameObject.SetActive(true);
        ui.gameObject.transform.Find("P1panel/enemyIslandSlider").gameObject.SetActive(false);
        ui.enableTutorials = false;
        ui.targetBreak.gameObject.SetActive(true);

        
        SetDone();
        onInitialize();
        player = FindObjectOfType<PlayerInput>();
        player.InitializeForDeathMatch();
        rounds = FindObjectOfType<TargetRounds>();
        foreach (cameraFollow k in cams)
        {
            k.camera.gameObject.SetActive(true);
        }
        rounds.EnableNextRound();
        countDown.GetComponent<CountDown>().AddCallBack(() => StartRound());
        Invoke("StartGameIn", 4f);
    }
    public void StartGameIn()
    {
       
        if (!countDown.gameObject.GetActive())
        {
            countDown.GetComponent<CountDown>().resetTime();
            countDown.SetActive(true);
            ui.round.gameObject.SetActive(true);
            ui.roundScore.gameObject.SetActive(true);
            ui.time.gameObject.SetActive(true);
            ui.timeCount.gameObject.SetActive(true);

        }
    }

        void StartRound()
    {
        player.setStatus(ShipStatus.Alive);
       
        this.gameStarted = true;
        player.gameStarted = true;
        player.setStatus(ShipStatus.Alive);
        gameTime += Time.deltaTime;
        countDown.SetActive(false);
        roundStartTime = Time.timeSinceLevelLoad;
    }



    [PunRPC]
    public void AddPlayer(int id)
    {
        
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
    public void SetDone()
    {
        globalCanvas.waitingForPlayers.SetActive(false);
        done = false;

    }




    void Update()
    {
        if (gameStarted)
        {
            if (rounds.GetBrokenCount() == rounds.GetTotalCount())
            {
                times.Add((Time.timeSinceLevelLoad - roundStartTime));
                if (rounds.round == 4)
                {
                    activateVictoryText();
                    gameStarted = false;
                }
                else
                {
                    //ui.activateColorTint();
                    Time.timeScale = 0.3f;
                    gameStarted = false;
                    ui.targetBreak.text = "Round Completed!!";
                    ui.targetBreak.CrossFadeAlpha(1, 0.01f, true);
                    Invoke("GoToNextRound", 1f);
                }
            }
            else if ((Time.timeSinceLevelLoad - roundStartTime) >= maxTime)
            {
                activateGameOver();
                gameStarted = false;
            }
            
            UpdateUI();
        }

        if (gameOver)
        {
            if (Input.GetKeyUp(KeyCode.E) || player.Actions != null && player.Actions.Blue.WasPressed)
            {
                RestartGame();
                gameOver = false;
            }
        }

    }

    public void GoToNextRound()
    {
        ui.targetBreak.GetComponent<FadeOutZoom>().FadeOut();
        //this.ui.deactivateColorTint();
        Time.timeScale = 1f;
        player.reset();
        player.setStatus(ShipStatus.Waiting);
        rounds.EnableNextRound();
        ui.round.text = "Round " + (rounds.round + 1);
        StartGameIn();
    }

    private void UpdateUI()
    {
        if(rounds.round == rounds.targetRounds.Count-1)
        {
            ui.round.text = "Final Round ";
        }
        else
        {
            ui.round.text = "Round " + (rounds.round + 1);
        }
        ui.roundScore.text = rounds.GetBrokenCount() + " / " + rounds.GetTotalCount();
        float timeLeft = Mathf.Max(0, maxTime - (Time.timeSinceLevelLoad - roundStartTime));
        
        string minutes = Mathf.Floor(timeLeft / 60).ToString("00");
        string seconds = Mathf.Floor(timeLeft % 60).ToString("00");
        string ms = (Mathf.Floor(timeLeft *1000) %1000).ToString("000");
        if(timeLeft % 60 < 10)
        {
            ui.timeCount.color = Color.red;
        }
        else
        {
            ui.timeCount.color = Color.white;
        }
        ui.timeCount.text = string.Format("{0}:{1}:{2}", minutes, seconds, ms);
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

    }
    public void decrementPoint(KrakenInput kraken)
    {
        kraken.uiManager.decrementPoint();

    }

    [PunRPC]
    public void IncrementPoint(int id)
    {

    }

    public override string getShipById(int id)
    {
        foreach (PlayerInput player in getPlayers())
        {
            if (player.GetId() == id)
            {
                return player.type.ToString();
            }
        }
        return "";
    }

    public void activateGameOver()
    {
        Time.timeScale = 0.3f;
        foreach (PlayerInput p in players)
        {
            p.uiManager.activateFinishAndColorTint("Time's Up!");
        }
        Invoke("TriggerStatsAnimation", 1.4f);
    }


    [PunRPC]
    private void ActivateLastPointPrompt(int id)
    {

        var textScript = FindObjectOfType<ProgressScript>();

        var newText = "One More Target To Go";
        textScript.activatePopup(newText, "You", "Ship");
 
        
    }

    public void activateVictoryText()
    {
        Time.timeScale = 0.3f;
        foreach (PlayerInput p in players)
        {
            p.uiManager.activateFinishAndColorTint("Game!!!");
        }
        Invoke("TriggerStatsAnimation", 1.4f);

    }

    public void TriggerStatsAnimation()
    {
        triggerScreenAnimation();
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
        
        enableStats();

    }


    public void enableStats()
    {
        player.setStatus(ShipStatus.Waiting);
        var gameOverScreen = Instantiate(this.postGamePrefab as GameObject);
        MapObjects map = GameObject.FindObjectOfType<MapObjects>();
        map.enabled = false;
        map.transform.root.gameObject.SetActive(false);
        foreach (cameraFollow k in cams)
        {
            k.camera.gameObject.SetActive(false);
        }

        TargetsPostGameStats controller = gameOverScreen.GetComponent<TargetsPostGameStats>();
        player.transform.position = controller.shipLocation.transform.position;
        player.transform.rotation = controller.shipLocation.transform.rotation;

        for(int x = 0; x < rounds.targetRounds.Count; x++)
        {
            float currentBest = 100;
            if(PlayerPrefs.HasKey(player.type.ToString() + "-" + x))
            {
                currentBest = PlayerPrefs.GetFloat(player.type.ToString() + "-" + x);
            }
            
            if(x >= times.Count)
            {
                controller.runScores[x].text = "None\n";
                controller.runScores[x].color = Color.grey;
                if(currentBest == 0)
                {
                    controller.bestScores[x].text = "None\n";
                    controller.bestScores[x].color = Color.grey;
                }
            }
            else
            {
                if (times[x] < currentBest)
                {
                    controller.runScores[x].color = Color.green;
                    PlayerPrefs.SetFloat(player.type.ToString() + "-" + x, times[x]);
                    currentBest = times[x];
                }
                var currTime = times[x];
                var bestTime = currentBest;
                SetTime(controller.runScores[x], currTime);
                SetTime(controller.bestScores[x], bestTime);

            }
        }


    }

    private void SetTime(Text text, float time)
    {
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = Mathf.Floor(time % 60).ToString("00");
        string ms = (Mathf.Floor(time * 1000) % 1000).ToString("000");
        text.text = string.Format("{0}:{1}:{2}", minutes, seconds, ms) + "\n";
    }

  

    public void RestartGame()
    {
        Time.timeScale = 1;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Game");
    }

    internal override int getNumberOfTeams()
    {
        return isTeam ? gamePoints.Count : 0;
    }

    public override List<PlayerInput> getPlayers()
    {
        return players.Filter(player => player != null);
    }


    public override Dictionary<string, int> getGamepoints()
    {

        return gamePoints;
    }

    public override int getPlayerPointsToWIn()
    {
        return 0;
    }

}
