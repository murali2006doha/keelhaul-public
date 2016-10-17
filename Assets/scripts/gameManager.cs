using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using InControl;
public class gameManager : AbstractGameManager {
	//Use this script for general things like managing the state of the game, tracking players and so on.

	public float respawnTimer;
	// Use this for initialization
	public cameraFollow  [] cams;
	GameObject[] barrels;
	Vector3[] barrels_start_pos;
	public Vector3 barrel_start_pos;
	
	public List<playerInput> players = new List<playerInput>();
	public KrakenInput kraken;
	public int playerWinPoints = 3;
	public int krakenWinPoints = 5;
	public GameObject pressAtoJoin;
	public bool signedIn = false;
	int assign_index =0;
	public bool freeForAll = true;
	
	public signInCamera cam;
	public PlayerManager manager;
	public ControllerSelect controller;
	public GameObject ship;
	public int maxNoOfShips = 2;
	bool done = false;
    FFAGlobalCanvas globalCanvas;
    GameObject screenSplitter;
    Animator fadeInAnimator;
    public GameObject countDown;

    MonoBehaviour script;
	PlayerSelectSettings ps;
	public bool gameOver = false;
    GameObject winner;

    string lastPoint = "The Replace Needs <color=\"orange\">ONE</color> Point To Win!";


	void Start () {
        globalCanvas = GameObject.FindObjectOfType<FFAGlobalCanvas>();
        screenSplitter = globalCanvas.splitscreenImages;
        fadeInAnimator = globalCanvas.fadePanelAnimator;
        countDown = globalCanvas.countDownTimer;

		gameOver = false;
        
        GameObject[] winds = GameObject.FindObjectOfType<MapObjects> ().winds;

		if (winds != null && winds.Length > 0) {
			foreach (GameObject obj in winds) {
				obj.SetActive (false);
			}
		}

		ps = GameObject.FindObjectOfType<PlayerSelectSettings> ();
		controller = GameObject.FindObjectOfType<ControllerSelect> ();
        Physics.gravity = new Vector3 (0f, -0.1f, 0f);
		Application.targetFrameRate = -1; //Unlocks the framerate at start
		Resources.UnloadUnusedAssets();
		barrels = GameObject.FindGameObjectsWithTag ("barrel");
		barrels_start_pos = new Vector3[barrels.Length];

		int x = 0;

		foreach(GameObject barrel in barrels){
			barrels_start_pos [x] = barrel.transform.position;
			x++;
		}
		int num = 0;

        //spawning players and attaching player input to objects.
        SoundManager.initLibrary();
        if(ps == null || ps.players.Count == 0) //Default behaviour if didn't come from character select screen. 
        {
            int numDevices = 0;
            this.GetComponent<InControlManager>().enabled = true;
            if (InputManager.Devices != null && InputManager.Devices.Count > 0) {
               
                print("devices found");
                List<InputDevice> devices = new List<InputDevice>();
                foreach (InputDevice device in InputManager.Devices)
                {
                    print(device.Name);
                    //add only controllers?
                    if (device.Name.ToLower().Contains("controller") || device.Name.ToLower().Contains("joy") || device.IsKnown)
                    {
                        devices.Add(device);
                    }
                    
                }
                
                foreach(InputDevice device in devices)
                {
                    PlayerActions action = PlayerActions.CreateWithJoystickBindings();
                    action.Device = device;
                    if (numDevices == 0)
                    {
                        kraken.Actions = action;
                        kraken.followCamera.ready = true;
                    } else if (numDevices == 1)
                    {
						num = createShipWithName(num, action, ShipEnum.AtlanteanShip.ToString());
                        break;
                    } else if (numDevices == 2)
                    {
                      
                        break;
                    }
                    numDevices++;
                }

                if(numDevices == 1)
                {
					num = createShipWithName(num, PlayerActions.CreateWithKeyboardBindings_2(), ShipEnum.ChineseJunkShip.ToString());
					num = createShipWithName(num, PlayerActions.CreateWithKeyboardBindings_2(), ShipEnum.AtlanteanShip.ToString());
                }  else if (numDevices == 2)
                {
					num = createShipWithName(num, PlayerActions.CreateWithKeyboardBindings_2(), ShipEnum.ChineseJunkShip.ToString());
            
                }
            }
            if(numDevices == 0)
            {
                print("no devices or characters selected - adding default");
                kraken.Actions = PlayerActions.CreateWithKeyboardBindings();
                kraken.followCamera.ready = true;
				num = createShipWithName(num, PlayerActions.CreateWithKeyboardBindings_2(), ShipEnum.ChineseJunkShip.ToString());
				num = createShipWithName(num, PlayerActions.CreateWithKeyboardBindings_2(), ShipEnum.AtlanteanShip.ToString());
            }
           
        }
        else
        {
            foreach (CharacterSelect player in ps.players)
            {

				if (player.selectedCharacter == ShipEnum.Kraken.ToString())
                {
                    kraken.Actions = player.Actions;
                    kraken.followCamera.ready = true;
                }
                else
                {
                    if (num <= maxNoOfShips)
                    {
                        num = createPlayerShip(num, player);
                    }
                }
            }

        }
	}

    private int createShipWithName(int num, PlayerActions action, string name)
    {
        CharacterSelect shipOne = new CharacterSelect();
        shipOne.Actions = action;
        shipOne.selectedCharacter = name;
        num = createPlayerShip(num, shipOne);
        return num;
    }

    private int createPlayerShip(int num, CharacterSelect player)
    {
        GameObject newShip = null;
        string path = GlobalVariables.shipToPrefabLocation[player.selectedCharacter];
        if (path != null)
        {
            newShip = Instantiate(Resources.Load(path, typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
        }
        if (newShip != null)
        {
            playerInput input = newShip.GetComponent<playerInput>();
            input.Actions = player.Actions;
            input.shipNum = num;
            players.Add(input);
            num++;
        }

        return num;
    }

    void gameStart(){
		signedIn = true;
		foreach (playerInput player in players) {
			player.gameStarted = true;
		}
		kraken.gameStarted = true;
	}

	void destroyCountDown(){
		Destroy (countDown);
	}


    void Update(){

		//puts the camera in the starting positions as soon as the game starts
		if (!done) {
			if (!countDown.gameObject.activeSelf) {
				cam.gameObject.SetActive (false);
				countDown.SetActive (true);
				screenSplitter.SetActive (true);

				foreach (cameraFollow k  in cams) {
					k.camera.gameObject.SetActive (true);
				}
			}

			if (countDown.GetComponent<CountDown> ().done) {
				gameStart ();
				done = true;
				print ("gamestart");
				Invoke ("destroyCountDown", 2f);
			}
		}

		foreach (cameraFollow k  in cams) {
			k.camera.gameObject.SetActive (true);
		}

		demoScript ();
	}


	public void assign(){
		assign_index++;

	}


	void demoScript(){

		if (Input.GetKeyDown(KeyCode.Alpha1)){
			screenSplitter.SetActive (false);
			cams [0].camera.rect = new Rect (0, 0, 1, 1);
			cams [1].camera.rect = new Rect (0, 0, 0, 0);
			cams [2].camera.rect = new Rect (0, 0, 0, 0);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)){
			screenSplitter.SetActive (false);
			cams [1].camera.rect = new Rect (0, 0, 1, 1);
			cams [0].camera.rect = new Rect (0, 0, 0, 0);
			cams [2].camera.rect = new Rect (0, 0, 0, 0);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)){
			screenSplitter.SetActive (false);
			cams [2].camera.rect = new Rect (0, 0, 1, 1);
			cams [0].camera.rect = new Rect (0, 0, 0, 0);
			cams [1].camera.rect = new Rect (0, 0, 0, 0);
		}

		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			SceneManager.LoadScene (1);
		}
	}


    override public void respawnPlayer(playerInput player, Vector3 startingPoint){

		player.gameObject.transform.position = startingPoint;


	}
	public void respawnKraken(KrakenInput player, Vector3 startingPoint){

		player.gameObject.transform.position = startingPoint;


	}
				

	IEnumerator teleportBarrel(playerInput player, GameObject barrel){

		yield return new WaitForSeconds(1);

		Vector3 anchor = new Vector3(0,0.06f,0.06f);
		if (barrel.GetComponent<CharacterJoint> () != null) {
			anchor = barrel.GetComponent<CharacterJoint> ().anchor;
			Destroy (barrel.GetComponent<CharacterJoint> ());
		}
			
		barrel b = barrel.GetComponent<barrel> ();
		b.explodeBarrel ();

		player.uiManager.setEnemyIslandBar (player.uiManager.enemyIslandHealthBar.value - 1);

		int x = 0;
		foreach (GameObject barr in barrels) {
			if (barr == barrel) {
				barrel.transform.position = barrels_start_pos[x];
				barrel.transform.rotation = Quaternion.Euler(new Vector3 (-90f, 0f, 0f));
				break;
			}
			x++;
		}
		barrel.AddComponent<CharacterJoint> ();
		barrel.GetComponent<CharacterJoint> ().anchor = anchor;
        barrel.GetComponent<barrel>().activatePillar();
	}

	public void incrementPoint(KrakenInput kraken){
		int points = kraken.uiManager.incrementPoint ();
		kraken.uiManager.setScoreBar (points / krakenWinPoints);

		if (points == (krakenWinPoints-1)) {

			var textScripts = GameObject.FindObjectsOfType<ProgressScript> ();
			foreach (ProgressScript script in textScripts) {
				var newText = lastPoint.Replace ("Replace", "<color=purple>Kraken</color>");
				script.activatePopup (newText, "Kraken", "Kraken");
			}

		}
		if (points == krakenWinPoints && winner == null) {
            activateVictoryText();
            Invoke("triggerVictory", 1.2f);
            winner = kraken.gameObject;
        }

	}
	public void decrementPoint(KrakenInput kraken){
		kraken.uiManager.decrementPoint();

	}

	override public void incrementPoint(playerInput player, GameObject barrl){
		if (freeForAll)
		{
			player.GetComponent<Hookshot>().UnHook();
			player.GetComponent<Hookshot> ().enabled = false;
			StartCoroutine(teleportBarrel(player, barrl));
			player.GetComponent<Hookshot> ().enabled = true;
			float points = (float)(player.uiManager.incrementPoint());
			player.uiManager.setScoreBar (points / playerWinPoints);

			//refactor so color in text script and just pass ship name into script.
			if (points == (playerWinPoints - 1)) { //enemyIslandHealth == (0f);
				if (player.shipName.Equals ("Chinese Junk Ship")) {
					var textScripts = GameObject.FindObjectsOfType<ProgressScript> ();
					foreach (ProgressScript script in textScripts) {
						var newText = lastPoint.Replace ("Replace", "<color=red>Chinese Junk Ship</color>");
						script.activatePopup (newText, "Chinese Junk Ship", "Ship");
					}
				} else if (player.shipName.Equals ("Blackbeard Ship")) {
					var textScripts = GameObject.FindObjectsOfType<ProgressScript> ();
					foreach (ProgressScript script in textScripts) {
						var newText = lastPoint.Replace ("Replace", "<color=black>Blackbeard</color>");
						script.activatePopup (newText, "Blackbeard", "Ship");
					}
				} else {
					var textScripts = GameObject.FindObjectsOfType<ProgressScript> ();
					foreach (ProgressScript script in textScripts) {
						var newText = lastPoint.Replace ("Replace", "<color=blue>Atlantean Ship</color>");
						script.activatePopup (newText, "Atlantean Ship", "Ship");
					}
				}

			}
            if (points == playerWinPoints && winner ==null)
            { 
                activateVictoryText();
                Invoke("triggerVictory",1.2f);
                winner = player.gameObject;
			} else {
				GameObject[] winds = GameObject.FindObjectOfType<MapObjects> ().winds;
				if (winds != null && winds.Length > 0) {
					foreach (GameObject obj in winds) {
						obj.SetActive (true);
					}
					Invoke ("disableWinds", 4f);
				}
			}
		}
		else
		{
			player.GetComponent<Hookshot>().UnHook();
			StartCoroutine(teleportBarrel(player, barrl));
			int points = 0;
			foreach (playerInput p in players) {
				points = p.uiManager.incrementPoint();
				p.uiManager.setScoreBar (points / playerWinPoints);
			}
			if (points == playerWinPoints) {
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

	public void triggerVictory(){

       
        if (winner.GetComponent<playerInput>())
        {
            script = winner.GetComponent<playerInput>();
            ((playerInput)script).hasWon = true;
           
        }
        else {
            script = winner.GetComponent<KrakenInput>();
            ((KrakenInput)script).hasWon = true;
           
        }
		
		//player.victoryScreen.SetActive (true);
		foreach (playerInput p in players) {
			p.gameStarted = false;
        }
		kraken.gameStarted = false;


        //globalCanvas.finishText.SetActive(true);
        Texture2D texture = new Texture2D(Screen.width, Screen.height/2, TextureFormat.RGB24, true);
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



	public void triggerVictoryScreen(){
        Time.timeScale = 1f;
        //fadeInAnimator.SetBool("fade", true);
        gameOver = true;
		kraken.reset ();
		foreach (playerInput z in players) {
			z.reset ();
            z.followCamera.enabled = false;
		}
        kraken.followCamera.enabled = false;
		screenSplitter.SetActive (false);
		MapObjects map = GameObject.FindObjectOfType<MapObjects> ();
        //Refactor out of map
        
        map.gameOverCamera.gameObject.SetActive(true);
	
        GameOverStatsUI gameOverUI = globalCanvas.gameOverUI;
        gameOverUI.gameObject.SetActive(true);

        List<FreeForAllStatistics> shipStats = new List<FreeForAllStatistics>();
		List<FreeForAllStatistics> krakenStats = new List<FreeForAllStatistics>();

		FreeForAllStatistics winStat = null;
		List<GameObject> losers = new List<GameObject> ();

		if (script is playerInput) {
			playerInput player = ((playerInput)script);
			winStat = player.gameStats;
			shipStats.Add(player.gameStats);
			gameOverUI.winnerText.text = gameOverUI.winnerText.text.Replace ("Replace", player.shipName);
			gameOverUI.winners [0].name.text = player.shipName;
			foreach (playerInput input in players) {
				if (input != player) {
					losers.Add (input.gameObject);
					shipStats.Add (input.gameStats);
				}
			}
			losers.Add (kraken.gameObject);
			krakenStats.Add (kraken.gameStats);
		} else if (script is KrakenInput) {
			KrakenInput player = ((KrakenInput)script);
			winStat = player.gameStats;
			krakenStats.Add(player.gameStats);
			gameOverUI.winnerText.text = gameOverUI.winnerText.text.Replace ("Replace", "Kraken");
			gameOverUI.winners [0].name.text = "Kraken";
			foreach (playerInput input in players) {
				losers.Add (input.gameObject);
				shipStats.Add(input.gameStats);
			}
		}

		//Winner
		script.gameObject.transform.position = new Vector3(map.winnerLoc.transform.position.x,script.gameObject.transform.position.y,map.winnerLoc.transform.position.z);
		script.gameObject.transform.localScale *= 2f;
		script.gameObject.transform.rotation  =Quaternion.Euler (new Vector3 (0f, 180f, 0f));

		//Losers
		losers[0].transform.position = new Vector3(map.loser1loc.transform.position.x,losers[0].transform.position.y,map.loser1loc.transform.position.z);
		losers[0].transform.position = map.loser1loc.transform.position;
		losers [0].transform.rotation = Quaternion.Euler (new Vector3 (0f, 180f, 0f));

		losers[1].transform.position = new Vector3(map.loser2loc.transform.position.x,losers[1].transform.position.y,map.loser2loc.transform.position.z);
		losers[1].transform.rotation  =Quaternion.Euler (new Vector3 (0f, 180f, 0f));


		GameObject titlesPrefab = Resources.Load ("Prefabs/Titles", typeof(GameObject)) as GameObject;
		Titles titles = titlesPrefab.GetComponent<Titles> ();
		titles.calculateTitles (shipStats,krakenStats);

		int num = 0;
		foreach (Title title in  winStat.titles) {
			if (num >= gameOverUI.winners [0].titles.Length) {
				break;
			}
			gameOverUI.winners [0].titles [num].text = title.name;
			gameOverUI.winners [0].titleStats[num].text = title.statsString;
			num++;
		}

		for(int x = 0;x<2;x++){
			num = 0;
			playerInput loserInput = losers [x].GetComponent<playerInput> () ;
			FreeForAllStatistics loserStat = null;
			if (loserInput != null) {
				loserStat = loserInput.gameStats;
				gameOverUI.losers [x].name.text = loserInput.shipName;
			} else {
				KrakenInput krakn = losers [x].GetComponent<KrakenInput> () ;
				loserStat = krakn.gameStats;
				gameOverUI.losers [x].name.text = "Kraken";
			}

			foreach (Title title in  loserStat.titles) {
				if (num >= gameOverUI.winners [0].titles.Length) {
					break;
				}
				gameOverUI.losers [x].titles [num].text = title.name;
				gameOverUI.losers [x].titleStats[num].text = title.statsString;
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
	public void disableWinds(){
		foreach (GameObject obj in GameObject.FindObjectOfType<MapObjects> ().winds) {
			obj.SetActive (false);
		}
		foreach (playerInput inp in players) {
			inp.touchingWind = false;
		}
	}

    override public void exitToCharacterSelect(){
		Destroy (controller.gameObject);
		Destroy (ps.gameObject);
		SceneManager.LoadScene ("start2");
	}
	public void restartCurrentGame(){
		DontDestroyOnLoad (ps);
		DontDestroyOnLoad (controller);
		SceneManager.LoadScene ("free for all_vig");
	}


}