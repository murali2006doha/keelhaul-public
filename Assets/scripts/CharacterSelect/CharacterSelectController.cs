﻿using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterSelectController : MonoBehaviour {

    
    public List<Text> texts;
    [SerializeField]
    ControllerSelect controllerSelect;
    int numPlayers = 4;
    public List<PlayerActions> players = new List<PlayerActions>();
    Dictionary<PlayerActions, int> playerToPos = new Dictionary<PlayerActions, int>();
    Dictionary<int,PlayerActions> panelToPlayer = new Dictionary<int,PlayerActions>();
    String[] panelCharacter = new String[4];
    bool[] panelBot = new bool[4];
    List<String> characters = new List<String>{ "Chiense", "BlackBeard", "Atleatnean", "Viking" };

    void Start()
    {
        var inputDevice = InputManager.ActiveDevice;
        //last device to
        controllerSelect.SetOnJoin(SignIn);
        controllerSelect.listening = true;
       
        

    }

    // Update is called once per frame
    void Update () {

        foreach(Text text in texts)
        {
            text.text = "Players:";
        }
        foreach(PlayerActions player in playerToPos.Keys)
        {
            texts[playerToPos[player]].text += "P" + players.IndexOf(player) + ",";
        }
        foreach (Text text in texts)
        {
            text.text += " Owner:";
        }

        foreach (int pos in panelToPlayer.Keys)
        {
            texts[pos].text += "P" + players.IndexOf(panelToPlayer[pos]);
        }

        foreach (Text text in texts)
        {
            text.text += " Character:";
        }

        foreach (Text text in texts)
        {
            text.text += panelCharacter[texts.IndexOf(text)];
        }

        foreach (Text text in texts)
        {
            text.text += " IS BOT:";
        }

        foreach (Text text in texts)
        {
            text.text += panelBot[texts.IndexOf(text)];
        }

        foreach (PlayerActions player in players)
        {
            UpdatePlayerController(player);
        }

    }

    private void UpdatePlayerController(PlayerActions player)
    {
        if (panelToPlayer.ContainsValue(player))
        {
            if (player.Left.WasReleased)
            {
                playerToPos[player] = playerToPos[player] - 1 < 0 ? 3 : playerToPos[player] - 1;
            }
            else if (player.Right.WasReleased)
            {
                playerToPos[player] = Math.Abs(playerToPos[player] + 1) % numPlayers;
            }

            if (panelToPlayer.ContainsKey(playerToPos[player]) && player == panelToPlayer[playerToPos[player]])
            {
                 if (player.Red.WasReleased)
                {
                   
                        panelToPlayer.Remove(playerToPos[player]);
                        playerToPos.Remove(player);
                        panelCharacter[playerToPos[player]] = "";
                        panelBot[playerToPos[player]] = false;
                   
                }

                if (player.Blue.WasReleased)
                {
                    panelBot[playerToPos[player]] = !panelBot[playerToPos[player]];
                }

                if (player.Up.WasReleased)
                {
                    var index = characters.IndexOf(panelCharacter[playerToPos[player]]);
                    panelCharacter[playerToPos[player]] = characters[index - 1 < 0 ? 3 : index - 1];
                }

                if (player.Down.WasReleased)
                {
                    var index = characters.IndexOf(panelCharacter[playerToPos[player]]);
                    panelCharacter[playerToPos[player]] = characters[(index + 1) % 4];
                }

            }



        }
        else if (player.Green.WasReleased)
        {
            SignIn(player);
        
        }

       
    }


    public void SignIn(PlayerActions player)
    {
        Debug.Log(player);
        if (!players.Contains(player))
        {
            players.Add(player);
        }
        if (!playerToPos.ContainsKey(player) && playerToPos.Count < numPlayers)
        {
            var firstAvailablePanelIndex = GetFirstAvailablePanel();
            playerToPos.Add(player, firstAvailablePanelIndex);
            panelToPlayer.Add(firstAvailablePanelIndex, player);
            panelCharacter[firstAvailablePanelIndex] = characters[0];
        }
          
        
    }

    private int GetFirstAvailablePanel()
    {
        for(int x= 0; x < 4; x++)
        {
            if (!panelToPlayer.ContainsKey(x))
            {
                return x;
            }
        }
        return 0;
    }
}
