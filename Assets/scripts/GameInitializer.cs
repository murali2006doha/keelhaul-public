using UnityEngine;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using InControl;
using System;
using UnityEngine.EventSystems;


public class GameInitializer : MonoBehaviour {


    // Use this for initialization
    cameraFollow[] cams;


    int numOfShips = 2;
    int numOfKrakens;
    public List<CharacterSelection> shipSelections = new List<CharacterSelection>();
    public bool includeKraken = true;
    public bool isTeam;
    public bool isFirstControllerKraken = false;

    List<PlayerInput> players = new List<PlayerInput>();


    public MapEnum map;
    public GameTypeEnum gameType;
    GlobalCanvas globalCanvas;
    GameObject screenSplitter;

    PlayerSelectSettings ps;
    GameModeSelectSettings gs;
    Dictionary<int,int> teamNums = new Dictionary<int,int>();
    public bool isMaster;
    public int playerId;
    public Action onGameManagerCreated;

    AbstractGameManager manager;

    void Start()
    {
        PhotonNetwork.offlineMode = true;
        RoomOptions ro = new RoomOptions() { isVisible = true, maxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom("localRoom", ro, TypedLobby.Default);
        this.Activate();


    }

    public void Activate() {

        Cursor.visible = false;
        ps = GameObject.FindObjectOfType<PlayerSelectSettings>();
        this.map = ps.map;
        setGameTypeAndSettings();
        InstantiateMap();
        InstantiateEventSystem();

        if (shipSelections.Count == 0)
        {
            shipSelections.Add(new CharacterSelection(ShipEnum.AtlanteanShip.ToString(), null));
            shipSelections.Add(new CharacterSelection(ShipEnum.ChineseJunkShip.ToString(), null));
        }
        numOfKrakens = includeKraken ? 1 : 0;
        MapObjects mapObjects = GameObject.FindObjectOfType<MapObjects>();

        if (!isTeam)
        {
            numOfShips = Math.Min(Math.Min(mapObjects.shipStartingLocations.Length, 4 - numOfKrakens), shipSelections.Count);
            shipSelections.RemoveRange(numOfShips - 1, shipSelections.Count - numOfShips);
        }
        else
        {
            int numOfTeams = getNumberOfTeams();
            removeTeams(numOfTeams - mapObjects.shipStartingLocations.Length);
            numOfShips = Math.Min(4 - numOfKrakens, shipSelections.Count);
        }

        initializeGlobalCanvas();
        initializePlayerCameras();

        globalCanvas = GameObject.FindObjectOfType<GlobalCanvas>();
        screenSplitter = globalCanvas.splitscreenImages;
        globalCanvas.setUpSplitScreen(ps ? ps.players.Count : numOfShips + numOfKrakens);


        //spawning players and attaching player input to objects.
        SoundManager.initLibrary();

        MapObjects map = GameObject.FindObjectOfType<MapObjects>();
        if (isMaster)
        {
            createGameManager(() => createPlayersAndMapControllers(map));
        }

    }

    void InstantiateEventSystem() {
		if (!FindObjectOfType<EventSystem>())
		{
			GameObject obj = new GameObject("EventSystem");
            obj.AddComponent<EventSystem>().sendNavigationEvents = false;
			obj.AddComponent<StandaloneInputModule>().forceModuleActive = true;
		}
    }

    public void SimplyInstantiateManager(GameTypeEnum mode) {
        GameObject manager = PhotonNetwork.InstantiateSceneObject(PathVariables.deathMatchManager, transform.position, transform.rotation, 0, null);
    }

    private void InstantiateMap() {
        print(gameType.ToString() + map.ToString());
        GameObject mapToInstantiate = Resources.Load(PathVariables.GetMapForMode(gameType, map)) as GameObject;
        Instantiate(mapToInstantiate);
    }

    private void setGameTypeAndSettings() {

        if (ps) {
            gameType = ps.gameType;

            if (gameType == GameTypeEnum.Sabotage) {
                this.isTeam = true;
            }

            includeKraken = ps.includeKraken;
            shipSelections.Clear();
            foreach(CharacterSelection selection in ps.players)
            {
                if (!(selection.selectedCharacter == ShipEnum.Kraken))
                {
                    shipSelections.Add(selection);
                }
                else
                {
                    includeKraken = true; //Refactor
                }
            }
        }
    }


    private void removeTeams(int v)
    {
        if (v == 0)
        {
            return;
        }
        Dictionary<int, List<CharacterSelection>> teamToList = new Dictionary<int, List<CharacterSelection>>();
        foreach (CharacterSelection selection in shipSelections)
        {
            if (!teamToList.ContainsKey(selection.team))
            {
                teamToList.Add(selection.team, new List<CharacterSelection>());
            }
            teamToList[selection.team].Add(selection);
        }
        for(int x = 0; x < v; x++)
        {
            shipSelections.RemoveAll(z => teamToList[x].Contains(z));
        }
    }

    private int getNumberOfTeams()
    {
        HashSet<int> teams = new HashSet<int>();
        foreach(CharacterSelection selection in shipSelections)
        {
            teams.Add(selection.team);
        }
        return teams.Count;
    }

    private void createGameManager(Action onInitialize)
    {
        if (gameType == GameTypeEnum.Sabotage)
        {

            SabotageGameManager sabManager= null;
            if ((!PhotonNetwork.offlineMode && PhotonNetwork.isMasterClient) || (PhotonNetwork.offlineMode))
            {
                GameObject manager = PhotonNetwork.Instantiate(PathVariables.sabotageManager, transform.position,Quaternion.identity,0);
                sabManager = manager.GetComponent<SabotageGameManager>();
            }
            else
            {
                sabManager = FindObjectOfType<SabotageGameManager>();
            }

            sabManager.cams = cams;
            sabManager.ps = ps;
            sabManager.isTeam = isTeam;
            sabManager.players = players;
            sabManager.includeKraken = includeKraken;
            sabManager.countDown = globalCanvas.countDownTimer;
            sabManager.globalCanvas = globalCanvas;
            sabManager.screenSplitter = globalCanvas.splitscreenImages;
            sabManager.fadeInAnimator = globalCanvas.fadePanelAnimator;
            if (!isTeam)
            {
                for (int x = 0; x < players.Count; x++)
                {
                    sabManager.shipPoints.Add(0);
                }
            }
            else
            {
                for (int x = 0; x < teamNums.Count; x++)
                {
                    sabManager.shipPoints.Add(0);
                }
            }

            sabManager.onInitialize = onInitialize;
            manager = sabManager;
            manager.enabled = true;



        } else if (gameType == GameTypeEnum.DeathMatch)
        {

            DeathMatchGameManager deathMatchManager = GameObject.FindObjectOfType<DeathMatchGameManager>();

            if (PhotonNetwork.offlineMode)
            {
                GameObject manager = PhotonNetwork.InstantiateSceneObject(PathVariables.deathMatchManager, transform.position, transform.rotation, 0, null);
                deathMatchManager = manager.GetComponent<DeathMatchGameManager>();

            }


            deathMatchManager.cams = cams;
            deathMatchManager.ps = ps;
            deathMatchManager.isTeam = isTeam;
            deathMatchManager.players = players;
            deathMatchManager.includeKraken = includeKraken;
            deathMatchManager.countDown = globalCanvas.countDownTimer;
            deathMatchManager.globalCanvas = globalCanvas;
            deathMatchManager.screenSplitter = globalCanvas.splitscreenImages;
            deathMatchManager.fadeInAnimator = globalCanvas.fadePanelAnimator;
            if (!isTeam)
            {
                for (int x = 0; x < players.Count; x++)
                {
                    deathMatchManager.shipPoints.Add(0);
                }
            }
            else
            {
                for (int x = 0; x < teamNums.Count; x++)
                {
                    deathMatchManager.shipPoints.Add(0);
                }
            }
            deathMatchManager.onInitialize = onInitialize;
            manager = deathMatchManager;
            manager.enabled = true;
        }
        else if (gameType == GameTypeEnum.KrakenHunt) {
            GameObject manager = Instantiate(Resources.Load(PathVariables.krakenHuntManager, typeof(GameObject)), this.transform.parent) as GameObject;
            KrakenHuntGameManager krakenHuntManager = manager.GetComponent<KrakenHuntGameManager>();
            krakenHuntManager.cams = cams;
            krakenHuntManager.ps = ps;
            krakenHuntManager.players = players;
            krakenHuntManager.includeKraken = includeKraken;
            krakenHuntManager.countDown = globalCanvas.countDownTimer;
            krakenHuntManager.globalCanvas = globalCanvas;
            krakenHuntManager.screenSplitter = globalCanvas.splitscreenImages;
            krakenHuntManager.fadeInAnimator = globalCanvas.fadePanelAnimator;
            for (int x = 0; x < players.Count; x++)
            {
                krakenHuntManager.shipPoints.Add(0);
            }
        }

    }

    private int createPlayersAndMapControllers(MapObjects map)
    {
        int num = 0;
        if (ps == null || ps.players.Count == 0) //Default behaviour if didn't come from character select screen.
        {
            num = createPlayersWithoutCharacterSelection(map, num);

        }
        else // Easy case, create kraken or ships with selection
        {

            foreach (CharacterSelection player in ps.players)
            {

                if (player.selectedCharacter == ShipEnum.Kraken)
                {
                    createKraken(map, player.Actions);
                    manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.All, ps.players.Count);

                }
                else
                {

                    num = createShipWithName(num, player);

                    manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.AllBuffered, PhotonNetwork.offlineMode?num: playerId);

                }
            }

        }

        return num;
    }

    private int createPlayersWithoutCharacterSelection(MapObjects map, int num)
    {
        int numDevices = 0;
        num = GetRightShipIndex(num);
        int shipIndex = GetRightShipSelection(num);

        if (InputManager.Devices != null && InputManager.Devices.Count > 0)
        {


            List<InputDevice> devices = new List<InputDevice>();
            foreach (InputDevice device in InputManager.Devices)
            {

                //add only controllers?
                if (device.Name.ToLower().Contains("controller") || device.Name.ToLower().Contains("joy") || device.IsKnown)
                {
                    devices.Add(device);

                }

            }
            bool createdKraken = false;
            numDevices = devices.Count;
            if (devices.Count > 0)
            {
                PlayerActions action = PlayerActions.CreateWithJoystickBindings();
                action.Device = devices[0];
                if (isFirstControllerKraken && includeKraken)
                {
                    createKraken(map, action);
                    createdKraken = true;
                }
                else
                {
                    Debug.Log(num.ToString() + "  nummm");
                    shipSelections[shipIndex].Actions = action;

                    num = createShipWithName(num, shipSelections[shipIndex]);
                    shipIndex++;
                    if (PhotonNetwork.offlineMode)
                    {

                        manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.All, num);
                    }
                    else
                    {
                        manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.AllBuffered, playerId);
                    }

                }

            }
            // Create joystick bindings for kraken and ships

            for (int n = 1; n< devices.Count;n++)
            {
                PlayerActions action = PlayerActions.CreateWithJoystickBindings();
                action.Device = devices[n];
                if (!createdKraken && includeKraken)
                {

                    createKraken(map, action);
                    createdKraken = true;
                }
                else if (num < shipSelections.Count)
                {
                    shipSelections[shipIndex].Actions = action;
                    num = createShipWithName(num, shipSelections[shipIndex]);
                    shipIndex++;
                    if (PhotonNetwork.offlineMode)
                    {
                        manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.All, num);
                    }
                    else
                    {
                        manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.AllBuffered, playerId);
                    }
                }
                else
                {
                    break;
                }

            }

            // Create keyboard bindings for remaining ships
            if (!createdKraken && includeKraken)
            {
                createKraken(map, PlayerActions.CreateWithKeyboardBindings());
            }
            for (int z = num; z < shipSelections.Count; z++)
            {
                shipSelections[z].Actions = PlayerActions.CreateWithKeyboardBindings_2();
                num = createShipWithName(num, shipSelections[z]);
                if (PhotonNetwork.offlineMode)
                {
                    manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.All, num);
                }
                else
                {
                    manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.AllBuffered, playerId);
                }
            }


        }

        if (numDevices == 0)
        {

            if (includeKraken)
            {
                createKraken(map, PlayerActions.CreateWithKeyboardBindings());
            }

            for (int z = 0; z < shipSelections.Count; z++)
            {
                shipSelections[z].Actions = PlayerActions.CreateWithKeyboardBindings_2();
                num = createShipWithName(GetRightShipSelection(num), shipSelections[z]);
                if (PhotonNetwork.offlineMode)
                {
                    manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.All, num);
                }
                else
                {
                    manager.GetComponent<PhotonView>().RPC("AddPlayer", PhotonTargets.AllBuffered, playerId);
                }
            }

        }

        return num;
    }

    private void createKraken(MapObjects map, PlayerActions action)
    {
        GameObject krakenObj = Instantiate(Resources.Load(PathVariables.krakenPath, typeof(GameObject)), this.transform.parent) as GameObject;
        krakenObj.transform.position = map.krakenStartPoint.transform.position;

        KrakenInput kraken = krakenObj.GetComponent<KrakenInput>();
        if (action.Device == null) {

            action = PlayerActions.CreateWithKeyboardBindings();
        }
        kraken.Actions = action;
    }

    private void initializePlayerCameras()
    {
        UnityEngine.Object camera = Resources.Load(PathVariables.topDownCameraPath, typeof(GameObject));
        if (ps)
        {
            intializeCameraWithCharacterSelections(camera);

        }
        else //Default behaviour use global variables to initialize
        {
            InitializeCameraWithoutCharacterSelections(camera);

        }
    }



    private void intializeCameraWithCharacterSelections(UnityEngine.Object camera)
    {
        bool foundKraken = false;
        // Look for kraken
        cams = new cameraFollow[ps.players.Count];
        int camCount = 0;
        foreach (CharacterSelection player in ps.players)
        {

            if (player.selectedCharacter == ShipEnum.Kraken)
            {
                UnityEngine.Object krakenUI = Resources.Load(PathVariables.krakenUIPath, typeof(GameObject));
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
                    newCamera.GetComponent<cameraFollow>().SetRectsOfCameras(new Rect(0.5f, 0.5f, 0.5f, 0.5f));
                }
                else if (ps.players.Count == 1) {
                    newCamera.GetComponent<cameraFollow>().SetRectsOfCameras(new Rect(0f, 0f, 1f, 1f));
                }
                else
                {
                    newCamera.GetComponent<cameraFollow>().SetRectsOfCameras(new Rect(0, 0.5f, 1, 0.5f));
                }
                foundKraken = true;
                break;
            }

        }
        UnityEngine.Object shipUI = Resources.Load(PathVariables.shipUIPath, typeof(GameObject));

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
                setUpCameraPositions(foundKraken, shipCount, ps.players.Count, newCamera);
                var camera1 = newCamera.GetComponentInChildren<Camera>();
                setUpCameraOnCanvas(instantiatedUI, camera1);
                shipCount++;
            }

        }
    }

    private void InitializeCameraWithoutCharacterSelections(UnityEngine.Object camera)
    {
        cams = new cameraFollow[numOfKrakens + shipSelections.Count];

        UnityEngine.Object shipUI = Resources.Load(PathVariables.shipUIPath, typeof(GameObject));
        int camCount = 0;
        if (includeKraken)
        {
            UnityEngine.Object krakenUI = Resources.Load(PathVariables.krakenUIPath, typeof(GameObject));
            GameObject newCamera = Instantiate(camera, this.transform.parent) as GameObject;
            newCamera.name = "Kraken Screen";
            cams[camCount] = newCamera.GetComponent<cameraFollow>();
            GameObject instantiatedUI = Instantiate(krakenUI, newCamera.transform) as GameObject;

            var camera1 = newCamera.GetComponentInChildren<Camera>();
            setUpCameraOnCanvas(instantiatedUI, camera1);

            camera1.GetComponentInParent<cameraFollow>().SetRectsOfCameras(new Rect(0f, 0f, 1f, 1f));
            if (numOfKrakens + shipSelections.Count >= 4)
            {
                camera1.GetComponentInParent<cameraFollow>().SetRectsOfCameras(new Rect(0.5f, 0.5f, 0.5f, 0.5f));
            }
            else if (shipSelections.Count == 1)
            {
                camera1.GetComponentInParent<cameraFollow>().SetRectsOfCameras(new Rect(0f, 0f, 1f, 1f));
            }
            else
            {
                camera1.GetComponentInParent<cameraFollow>().SetRectsOfCameras(new Rect(0, 0.5f, 1, 0.5f));
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
            setUpCameraPositions(includeKraken, x, numOfKrakens + shipSelections.Count, newCamera);

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

    private void setUpCameraPositions(bool includeKraken, int shipCount, int playerCount, GameObject newCamera)
    {
        var camera1 = newCamera.GetComponentInChildren<cameraFollow>();

        if (playerCount == 1) {
            camera1.SetRectsOfCameras(new Rect(0f, 0f, 1f, 1f));
        }
        else if (playerCount == 2)
        {

            if (includeKraken)
            {
                camera1.SetRectsOfCameras(new Rect(0f, 0f, 1f, 0.5f));
            }
            else
            {
                camera1.SetRectsOfCameras(new Rect(0f, 0.5f * (1 - shipCount), 1f, 0.5f));
            }
        }
        else if (playerCount == 3)
        {
            if (includeKraken)
            {
                camera1.SetRectsOfCameras(new Rect(0.5f * shipCount, 0f, 0.5f, 0.5f));
            }
            else if (shipCount == 0)
            {
                camera1.SetRectsOfCameras(new Rect(0f, 0.5f, 1f, 0.5f));
            }
            else
            {
                camera1.SetRectsOfCameras(new Rect(0.5f * (shipCount - 1), 0f, 0.5f, 0.5f));
            }
        }
        else
        {
            if (includeKraken)
            {
                if (shipCount == 0)
                {
                    camera1.SetRectsOfCameras(new Rect(0, 0.5f, 0.5f, 0.5f));
                }
                else
                {
                    camera1.SetRectsOfCameras(new Rect(0.5f * (shipCount - 1), 0f, 0.5f, 0.5f));
                }
            }
            else
            {
                camera1.SetRectsOfCameras(new Rect(0.5f * (shipCount % 2), 0.5f * (shipCount > 1 ? 1 : 0), 0.5f, 0.5f));
            }
        }
    }

    private void initializeGlobalCanvas()
    {

        GameObject canvas = Instantiate(Resources.Load(PathVariables.ffaCanvasPath, typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
        globalCanvas = canvas.GetComponent<GlobalCanvas>();
    }

    private int createShipWithName(int num, CharacterSelection ship)
    {
        num = createPlayerShip(num, ship);
        return num;
    }

    private int createPlayerShip(int num, CharacterSelection player)
    {
        var otherShips = FindObjectsOfType<PlayerInput>();
        GameObject newShip = null;
        string path = GlobalVariables.shipToPrefabLocation[player.selectedCharacter.ToString()];
        if (path != null)
        {
            newShip = PhotonNetwork.Instantiate(path, Vector3.zero, Quaternion.identity, 0);
        }
        if (newShip != null)
        {
            PlayerInput input = newShip.GetComponent<PlayerInput>();

            input.playerId = playerId;

            if (!player.bot && player.Actions.Device == null) {
                Cursor.visible = true;
                player.Actions = PlayerActions.CreateWithKeyboardBindings_2();
            }

            if (player.bot) {
                input.TurnOnAi();
            }
            input.Actions = player.Actions;
            input.shipNum = num+1;
            int altSkinCount = 1;
            foreach(PlayerInput otherShip in otherShips)
            {
                if(otherShip.type == input.type)
                {
                    altSkinCount++;
                }
            }
            if (altSkinCount > 1)
            {
                //input.GetComponent<PhotonView>().RPC("ChangeSkin", PhotonTargets.AllBuffered, altSkinCount);
            }

            if (isTeam)
            {
                if (!teamNums.ContainsKey(player.team))
                {
                    teamNums.Add(player.team, 0);
                }
                teamNums[player.team] = teamNums[player.team] + 1;

                input.teamNo = player.team;
                input.placeInTeam = teamNums[player.team] - 1;
                input.teamGame = true;
            }

            players.Add(input);
            num++;
        }
        return num;
    }

    private int GetRightShipSelection(int num) {
        return PhotonNetwork.offlineMode ? num : 0;
    }

    private int GetRightShipIndex(int num)
    {
        return PhotonNetwork.offlineMode ? num : playerId - 1;
    }

}
