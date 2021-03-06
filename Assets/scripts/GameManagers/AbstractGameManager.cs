﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public abstract class AbstractGameManager : MonoBehaviour {

    // Use this for initialization
    [HideInInspector]
    public GameObject screenSplitter;
    public int minPlayersRequiredToStartGame = 2;
    public bool gameStarted;
    public void Start()
    {
        var listeners = FindObjectsOfType<AudioListener>();

        if (listeners.Length > 1)
        {
            listeners[0].enabled = true;

            for(int x = 1; x < listeners.Length; x++)
            {
                listeners[x].enabled = false;
            }
        }
    }

    protected void RunStartUpActions() {
        Physics.gravity = new Vector3(0f, -0.1f, 0f);
        Application.targetFrameRate = -1; //Unlocks the framerate at start
        Resources.UnloadUnusedAssets();
    }

    public virtual void acknowledgeBarrelScore(PlayerInput player, GameObject barrel)
    {

    }
    public virtual void acknowledgeKill(StatsInterface attacker, StatsInterface victim)
    {

    }


    public virtual void ExitToCharacterSelect() 
        {
        Time.timeScale = 1;
        PhotonNetwork.LeaveRoom();
        Destroy(FindObjectOfType<ControllerSelect>().gameObject);
        Destroy(FindObjectOfType<PlayerSelectSettings>().gameObject);
        SceneManager.LoadScene("Start");
    } 
     
    public abstract void respawnPlayer(PlayerInput player, Vector3 startingPoint, Quaternion startingRotation);

    virtual public Dictionary<string, int> getGamepoints()
    {

        return null;
    }

    virtual public int getPlayerPointsToWIn()
    {
        return 0;
    }

    virtual public void respawnKraken(KrakenInput player, Vector3 startingPoint)
    {

    }

    protected void demoScript(cameraFollow[] cams)
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

    public abstract bool isGameOver();
    internal abstract int getNumberOfTeams();
    public abstract List<PlayerInput> getPlayers();
    public virtual string getShipById(int id)
    {
        return "";
    }

    internal PlayerInput getPlayerWithId(int id)
    {
        var players = getPlayers();
        foreach(PlayerInput player in players)
        {
            if (player.GetId() == id)
            {
                return player;
            }
        }
        return players[0];
    }
}
