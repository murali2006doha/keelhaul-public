using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using InControl;
using System;

public class GameInitializer : MonoBehaviour {


    // Use this for initialization
    cameraFollow[] cams;


    int numOfShips = 2;
    int numOfKrakens;
    public List<CharacterSelection> shipSelections = new List<CharacterSelection>();
    public bool includeKraken = true;
    public bool isTeam;

    List<playerInput> players = new List<playerInput>();
    
    public GameTypeEnum gameType;
    GlobalCanvas globalCanvas;
    GameObject screenSplitter;

    PlayerSelectSettings ps;

    


    void Start()
    {
        
       
        if (shipSelections.Count == 0)
        {
            shipSelections.Add(new CharacterSelection(ShipEnum.AtlanteanShip.ToString(), null));
            shipSelections.Add(new CharacterSelection(ShipEnum.ChineseJunkShip.ToString(), null));
        }
        numOfKrakens = includeKraken ? 1 : 0;
        numOfShips = Math.Min(4 - numOfKrakens, shipSelections.Count);

        ps = GameObject.FindObjectOfType<PlayerSelectSettings>();

        initializeGlobalCanvas();

        initializePlayerCameras();

        globalCanvas = GameObject.FindObjectOfType<GlobalCanvas>();
        screenSplitter = globalCanvas.splitscreenImages;
        globalCanvas.setUpSplitScreen(ps ? ps.players.Count : numOfShips + numOfKrakens);


        //spawning players and attaching player input to objects.
        SoundManager.initLibrary();

        MapObjects map = GameObject.FindObjectOfType<MapObjects>();
        createPlayersAndMapControllers(map);

        createGameManager();

    }

    private void createGameManager()
    {
        if(gameType == GameTypeEnum.Sabotage)
        {
           GameObject manager =  Instantiate(Resources.Load(PathVariables.sabotageManager, typeof(GameObject)), this.transform.parent) as GameObject;
           SabotageGameManager sabManager = manager.GetComponent<SabotageGameManager>();
            sabManager.cams = cams;
            sabManager.ps = ps;
            sabManager.isTeam = isTeam;
            sabManager.players = players;
            sabManager.includeKraken = includeKraken;
            sabManager.countDown = globalCanvas.countDownTimer;
            sabManager.globalCanvas = globalCanvas;
            sabManager.screenSplitter = globalCanvas.splitscreenImages;
            sabManager.fadeInAnimator = globalCanvas.fadePanelAnimator;
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
                    createKraken(map, PlayerActions.CreateWithKeyboardBindings());
                }
                else
                {
                    num = createPlayerShip(num, player);
                }
            }

        }

        return num;
    }

    private int createPlayersWithoutCharacterSelection(MapObjects map, int num)
    {
        int numDevices = 0;

        this.GetComponent<InControlManager>().enabled = true;

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

            // Create joystick bindings for kraken and ships
            foreach (InputDevice device in devices)
            {
                if (num > shipSelections.Count)
                {
                    break;
                }
                PlayerActions action = PlayerActions.CreateWithJoystickBindings();
                action.Device = device;
                if (numDevices == 0 && numOfKrakens > 0)
                {
                    createKraken(map, action);
                }
                else
                {
                    num = createShipWithName(num, action, shipSelections[num].selectedCharacter.ToString());
                }
                numDevices++;

            }
            // Create keyboard bindings for remaining ships
            for (int z = numDevices - numOfKrakens; z < shipSelections.Count; z++)
            {
                num = createShipWithName(num, PlayerActions.CreateWithKeyboardBindings_2(), shipSelections[z].selectedCharacter.ToString());
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
                num = createShipWithName(num, PlayerActions.CreateWithKeyboardBindings_2(), shipSelections[z].selectedCharacter.ToString());
            }

        }

        return num;
    }

    private void createKraken(MapObjects map, PlayerActions action)
    {
        GameObject krakenObj = Instantiate(Resources.Load(PathVariables.krakenPath, typeof(GameObject)), this.transform.parent) as GameObject;
        krakenObj.transform.position = map.krakenStartPoint.transform.position;

        KrakenInput kraken = krakenObj.GetComponent<KrakenInput>();
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
                    newCamera.GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                else
                {
                    newCamera.GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);
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


            if (numOfKrakens + shipSelections.Count >= 4)
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
        var camera1 = newCamera.GetComponentInChildren<Camera>();
        if (playerCount == 2)
        {

            if (includeKraken)
            {
                camera1.rect = new Rect(0f, 0f, 1f, 0.5f);
            }
            else
            {
                camera1.rect = new Rect(0f, 0.5f * (1 - shipCount), 1f, 0.5f);
            }
        }
        else if (playerCount == 3)
        {
            if (includeKraken)
            {
                camera1.rect = new Rect(0.5f * shipCount, 0f, 0.5f, 0.5f);
            }
            else if (shipCount == 0)
            {
                camera1.rect = new Rect(0f, 0.5f, 1f, 0.5f);
            }
            else
            {
                camera1.rect = new Rect(0.5f * (shipCount - 1), 0f, 0.5f, 0.5f);
            }
        }
        else
        {
            if (includeKraken)
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

        GameObject canvas = Instantiate(Resources.Load(PathVariables.ffaCanvasPath, typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
        globalCanvas = canvas.GetComponent<GlobalCanvas>();
    }

    private int createShipWithName(int num, PlayerActions action, string name)
    {
        CharacterSelection shipOne = new CharacterSelection(name, action);

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

}
