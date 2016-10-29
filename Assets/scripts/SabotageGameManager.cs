using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using InControl;
using System;

public class SabotageGameManager : AbstractGameManager {
	//Use this script for general things like managing the state of the game, tracking players and so on.

	public float respawnTimer;
	// Use this for initialization
	cameraFollow  [] cams;
	GameObject[] barrels;
	Vector3[] barrels_start_pos;
	Vector3 barrel_start_pos;

    int defaultShipNum = 2;
    public List<CharacterSelection> shipSelections = new List<CharacterSelection>();
    public int defaultKrakenNum = 1;
    public bool team;
	
	public List<playerInput> players = new List<playerInput>();
	public KrakenInput kraken;
	public int playerWinPoints = 3;
	public int krakenWinPoints = 5;
	public GameObject pressAtoJoin;
	public bool signedIn = false;
	int assign_index =0;
	public bool freeForAll = true;
	
	
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

        MapObjects map = GameObject.FindObjectOfType<MapObjects>();
        GameObject[] winds = map.winds;

        if (winds != null && winds.Length > 0)
        {
            foreach (GameObject obj in winds)
            {
                obj.SetActive(false);
            }
        }

        ps = GameObject.FindObjectOfType<PlayerSelectSettings>();
        controller = GameObject.FindObjectOfType<ControllerSelect>();


        defaultKrakenNum = Math.Min(defaultKrakenNum, 1);
        if (shipSelections.Count == 0)
        {
            shipSelections.Add(new CharacterSelection(ShipEnum.AtlanteanShip.ToString(), null));
            shipSelections.Add(new CharacterSelection(ShipEnum.ChineseJunkShip.ToString(), null));
        }
        
        defaultShipNum = Math.Min(4 - defaultKrakenNum, shipSelections.Count);


        initializeGlobalCanvas();
        
        initializePlayerCameras();

        globalCanvas = GameObject.FindObjectOfType<FFAGlobalCanvas>();
        screenSplitter = globalCanvas.splitscreenImages;
        globalCanvas.setUpSplitScreen(ps ? ps.players.Count : defaultShipNum + defaultKrakenNum);
        fadeInAnimator = globalCanvas.fadePanelAnimator;
        countDown = globalCanvas.countDownTimer;

		gameOver = false;
        
        
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
            if (defaultKrakenNum > 0)
            {
                GameObject k = Instantiate(Resources.Load("Prefabs/Kraken 1", typeof(GameObject)), this.transform.parent) as GameObject;
                k.transform.position = map.krakenStartPoint.transform.position;

                kraken = k.GetComponent<KrakenInput>();
                
            }
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

                // Create joystick bindings for kraken and ships
                foreach (InputDevice device in devices)
                {
                    if (num > shipSelections.Count)
                    {
                        break;
                    }
                    PlayerActions action = PlayerActions.CreateWithJoystickBindings();
                    action.Device = device;
                    if (numDevices == 0)
                    {
                        if(defaultKrakenNum > 0) {
                            kraken.Actions = action;
                        }
                    }
                    else
                    {
                        num = createShipWithName(num, action, shipSelections[num].selectedCharacter.ToString());
                    }
                    numDevices++;

                }
                // Create keyboard bindings for remaining ships
                for (int z = numDevices-defaultKrakenNum; z < shipSelections.Count; z++)
                {
                    num = createShipWithName(num, PlayerActions.CreateWithKeyboardBindings_2(), shipSelections[z].selectedCharacter.ToString());
                }
                    
                
            }
            
            if(numDevices == 0)
            {
                print("no devices or characters selected - adding default");
                if (defaultKrakenNum > 0)
                {
                    print("Adding kraken");
                    kraken.Actions = PlayerActions.CreateWithKeyboardBindings();
                  
                }
                print("Adding " + shipSelections.Count + " Ships");
                for (int z = 0; z < shipSelections.Count; z++)
                {
                    num = createShipWithName(num, PlayerActions.CreateWithKeyboardBindings_2(), shipSelections[z].selectedCharacter.ToString());
                }
				
            }
           
        }
        else
        {
            foreach (CharacterSelection player in ps.players)
            {

				if (player.selectedCharacter == ShipEnum.Kraken)
                {
                    
                    GameObject k = Instantiate(Resources.Load("Prefabs/Kraken 1", typeof(GameObject)), this.transform.parent) as GameObject;
                    kraken = k.GetComponent<KrakenInput>();
                    k.transform.position = map.krakenStartPoint.transform.position;
                    kraken.Actions = player.Actions;
                    
                }
                else
                {
                    if (num < maxNoOfShips)
                    {
                        num = createPlayerShip(num, player);
                    }
                }
            }

        }
	}

    private void initializePlayerCameras()
    {
        UnityEngine.Object camera = Resources.Load("Prefabs/Cameras/TopdownCamera", typeof(GameObject));
        if (ps)
        {
            bool foundKraken = false;
            // Look for kraken
            cams = new cameraFollow[ps.players.Count];
            int camCount = 0;
            foreach (CharacterSelection player in ps.players)
            {

                if (player.selectedCharacter == ShipEnum.Kraken)
                {
                    UnityEngine.Object krakenUI = Resources.Load("Prefabs/UI/KrakenUI", typeof(GameObject));
                    GameObject newCamera = Instantiate(camera, this.transform.parent) as GameObject;
                    newCamera.name = "Kraken Screen";
                    cams[camCount] = newCamera.GetComponent<cameraFollow>();
                    GameObject instantiatedUI = Instantiate(krakenUI, newCamera.transform) as GameObject;
                  
                    var camera1 = newCamera.GetComponentInChildren<Camera>();
                    setUpCameraOnCanvas(instantiatedUI, camera1);
                    camCount++;
                    //Only case where screen is small
                    if (ps.players.Count == 4)
                    {
                        newCamera.GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    }
                    else {
                        newCamera.GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);
                    }
                    foundKraken = true;
                    break;
                }
              
            }
            UnityEngine.Object shipUI = Resources.Load("Prefabs/UI/shipUI", typeof(GameObject));

            int shipCount = 0;
            //Look for ships
            foreach (CharacterSelection player in ps.players)
            {

                if (player.selectedCharacter != ShipEnum.Kraken)
                {
                    GameObject newCamera = Instantiate(camera, this.transform.parent) as GameObject;
                    newCamera.name = "Player " + (camCount + 1) + " Screen";
                    cams[camCount] = newCamera.GetComponent<cameraFollow>();
                    camCount++;
                    GameObject instantiatedUI = Instantiate(shipUI, newCamera.transform) as GameObject;
                    //Wide Screen Case 1
                    setUpCameraPositions(foundKraken, shipCount,ps.players.Count, newCamera);
                    var camera1 = newCamera.GetComponentInChildren<Camera>();
                    setUpCameraOnCanvas(instantiatedUI, camera1);
                    shipCount++;
                }

            }



        } else //Default behaviour use global variables to initialize
        {
            cams = new cameraFollow[defaultKrakenNum + shipSelections.Count];
            bool foundKraken = defaultKrakenNum>0;
            UnityEngine.Object shipUI = Resources.Load("Prefabs/UI/shipUI", typeof(GameObject));
            int camCount = 0;
            if (defaultKrakenNum > 0)
            {
                UnityEngine.Object krakenUI = Resources.Load("Prefabs/UI/KrakenUI", typeof(GameObject));
                GameObject newCamera = Instantiate(camera, this.transform.parent) as GameObject;
                newCamera.name = "Kraken Screen";
                cams[camCount] = newCamera.GetComponent<cameraFollow>();
                GameObject instantiatedUI = Instantiate(krakenUI, newCamera.transform) as GameObject;
                
                var camera1 = newCamera.GetComponentInChildren<Camera>();
                setUpCameraOnCanvas(instantiatedUI, camera1);
                
               
                if (defaultKrakenNum + shipSelections.Count >= 4)
                {
                    camera1.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                else
                {
                    camera1.rect = new Rect(0, 0.5f, 1, 0.5f);
                }
                camCount++;
            }
            
            for (int x = 0; x < shipSelections.Count; x++)
            {
                GameObject newCamera = Instantiate(camera, this.transform.parent) as GameObject;
                newCamera.name = "Player " + (camCount + 1) + " Screen";
                cams[camCount] = newCamera.GetComponent<cameraFollow>();

                camCount++;
                GameObject instantiatedUI = Instantiate(shipUI, newCamera.transform) as GameObject;
                var camera1 = newCamera.GetComponentInChildren<Camera>();
                setUpCameraOnCanvas(instantiatedUI, camera1);
                //Wide Screen Case 1
                setUpCameraPositions(foundKraken, x, defaultKrakenNum + shipSelections.Count, newCamera);
               
            }

        }
    }

    private static void setUpCameraOnCanvas(GameObject instantiatedUI, Camera camera1)
    {
        Canvas[] canvas = instantiatedUI.GetComponentsInChildren<Canvas>();
        foreach (Canvas can in canvas)
        {
            can.worldCamera = camera1;
        }
    }

    private void setUpCameraPositions(bool foundKraken, int shipCount, int playerCount, GameObject newCamera)
    {
        var camera1 = newCamera.GetComponentInChildren<Camera>();
        if (playerCount == 2)
        {
            
            if (foundKraken)
            {
                print("yes");
                camera1.rect = new Rect(0f, 0f, 1f, 0.5f);
                print(camera1.rect);

            }
            else
            {
               
                camera1.rect = new Rect(0f, 0.5f * (1 - shipCount), 1f, 0.5f);
            }
        }
        else if (playerCount == 3)
        {
            if (foundKraken)
            {
                camera1.rect = new Rect(0.5f * shipCount, 0f, 0.5f, 0.5f);
            }
            else if (shipCount == 0)
            {
                camera1.rect = new Rect(0f, 0.5f, 1f, 0.5f);
            }
            else
            {
                camera1.rect = new Rect(0.5f * (shipCount-1), 0f, 0.5f, 0.5f);
            }
        }
        else
        {
            if (foundKraken)
            {
                if (shipCount == 0)
                {
                    camera1.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                }
                else
                {
                    camera1.rect = new Rect(0.5f * (shipCount - 1), 0f, 0.5f, 0.5f);
                }
            }
            else
            {
                camera1.rect = new Rect(0.5f * (shipCount % 2), 0.5f * (shipCount > 1 ? 1 : 0), 0.5f, 0.5f);
            }
        }
    }

    private void initializeGlobalCanvas()
    {
        
        GameObject canvas = Instantiate(Resources.Load(GlobalVariables.ffaCanvasPath, typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
        globalCanvas = canvas.GetComponent<FFAGlobalCanvas>();
    }

    private int createShipWithName(int num, PlayerActions action, string name)
    {
        CharacterSelection shipOne = new CharacterSelection(name,action);
        
        num = createPlayerShip(num, shipOne);
        return num;
    }

    private int createPlayerShip(int num, CharacterSelection player)
    {
        GameObject newShip = null;
        string path = GlobalVariables.shipToPrefabLocation[player.selectedCharacter.ToString()];
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
		
		foreach (playerInput player in players) {
			player.gameStarted = true;
		}
        if (kraken) {
		    kraken.gameStarted = true;
        }
	}

	void destroyCountDown(){
		Destroy (countDown);
	}


    void Update(){

		//puts the camera in the starting positions as soon as the game starts
		if (!done) {
			if (!countDown.gameObject.activeSelf) {
				
				countDown.SetActive (true);
				screenSplitter.SetActive (true);

				foreach (cameraFollow k  in cams) {
					k.camera.gameObject.SetActive (true);
				}
			}

			if (countDown.GetComponent<CountDown> ().done) {
				gameStart ();
				done = true;
				
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
	override public void respawnKraken(KrakenInput player, Vector3 startingPoint){

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

		player.uiManager.decrementEnemyHealth (); //set in UIManager itself

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

    public override bool isGameOver()
    {
        return gameOver;
    }

    override public void incrementPoint(KrakenInput kraken){
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