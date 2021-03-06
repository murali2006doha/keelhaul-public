﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public abstract class AbstractCharacterSelectController : MonoBehaviour {

    public PlayerActions Actions { get; set; }
    public AsyncOperation asyncLoad;
    public bool loadingScene;
    public bool Offline;

    // Use this for initialization
    public bool withKeyboard;
    public Transform start;
    public int numPlayers;
    public List<ShipEnum> characters;
    public GameObject loadingScreen; //put this in mainMenuModel later

    protected Dictionary<string, bool> characterStatuses = new Dictionary<string, bool>();
    protected List<CharacterSelectPanel> players = new List<CharacterSelectPanel>(); //to pass on to 
    protected ControllerSelect cc;
    protected int playersInPlay;
    protected bool started = false;
    protected Action onSelectCharacter;
    protected GameTypeEnum  mode;

    [SerializeField] CharacterSelectDescriptionView descriptionView;

    public void OnSelectCharacterAction(Action action)  {
        this.onSelectCharacter = action;
    }


    // Use this for initialization
    void Start () {
        
        //GameObject.DontDestroyOnLoad (this.gameObject);
        cc = GameObject.FindObjectOfType<ControllerSelect> ();
        cc.withKeyboard = withKeyboard;
        cc.listening = false;

        for(int i = 0; i < numPlayers; i++) {
            initializePanel ();
        }               
        foreach (ShipEnum character in characters) {
            characterStatuses.Add (character.ToString(), false);
        }
            
        playersInPlay = players.Count;
    }


    public void initializePanel() {
        var suffix = numPlayers == 1 ? "" : "Offline";
        
        GameObject csPanel = Instantiate(Resources.Load(CharacterSelectModel.CSPanelPrefab + suffix, typeof(GameObject)), GameObject.Find ("Container").transform.position, GameObject.Find ("Container").transform.rotation) as GameObject;
        Vector3 localscale = csPanel.gameObject.transform.localScale;
        csPanel.gameObject.GetComponent<CharacterSelectPanel> ().InitializePanel (this, characters, Actions);

        csPanel.gameObject.transform.SetParent(GameObject.Find ("Container").transform);
        csPanel.gameObject.transform.localScale = localscale;
        if (this.descriptionView) {
            csPanel.gameObject.GetComponent<CharacterSelectPanel>().updateCharacterDescription = this.descriptionView.OnCharacterSelectChange;
        }

        players.Add (csPanel.gameObject.GetComponent<CharacterSelectPanel>());

    }


    // Update is called once per frame
    public void Update ()
    {
        var inputDevice = InputManager.ActiveDevice;
        //last device to
        if (this.gameObject.activeSelf) {
            cc.listening = true;
            signIn ();
        }
        else {
            cc.listening = false;
        }
        if (this.gameObject != null) {
            if (playersInPlay == 0) {
                start.gameObject.SetActive (true);
                //change to next when there is map selection
                if (!started && players.Exists (p => p.Actions.Green.IsPressed)) {
                    //LogAnalyticsUI.MainMenuGameStartedWithCharacters (mode, GlobalVariables.getMapToLoad().ToString(), players);

                    started = true;
                    this.onSelectCharacter ();
                }
            }
            else {
                start.gameObject.SetActive (false);
            }
        }
    }   


    public void signIn() {
        var inputDevice = InputManager.ActiveDevice;

        int playerCount = cc.players.Count;
        if (playerCount > 0 && playerCount <= numPlayers) {
            players [playerCount - 1].Actions = (PlayerActions)cc.players [playerCount - 1];
            if (players [playerCount - 1].Actions.Green.WasReleased) {
                players [playerCount - 1].isSignedIn = true;
                players [playerCount - 1].gameObject.GetComponent<CharacterSelectPanel> ().enabled = true;
            }
        }
    }


    public abstract bool lockCharacter (int index);

    public abstract bool unlockCharacter (int index);

    public List<string> getCharacterKeys() {
        List<string> keys = new List<string> (characterStatuses.Keys);
        return keys;
    }


    public Dictionary<string, bool> getCharacterStatuses() {
        return characterStatuses;
    }
        

    public bool isStarted() {
        return started;
    }


    public void setPlayerSelectSettings() {

        GameObject.FindObjectOfType<PlayerSelectSettings> ().setPlayerCharacters (players);

    }


    public GameTypeEnum getMode() {
        return mode;
    }

    public PlayerSelectSettings getPlayerSelectSettings() {

        return GameObject.FindObjectOfType<PlayerSelectSettings> ();

    }


}
