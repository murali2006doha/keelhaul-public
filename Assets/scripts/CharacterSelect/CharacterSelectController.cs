using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class CharacterSelectController : MonoBehaviour {


    public List<Text> texts;
    [SerializeField]
    private List<CharacterPanel> panels;

    [SerializeField]
    private MapSelectView mapView;

    [SerializeField]
    private ControllerSelect controllerSelect;

    [SerializeField]
    private PlayerSelectSettings playerSelectSettings;

    [SerializeField]
    private SpriteDictionary characterDMPanelSprites;

    [SerializeField]
    private SpriteDictionary characterSABPanelSprites;

    [SerializeField]
    private SpriteDictionary characterTARPanelSprites;

    [SerializeField]
    private GameObject playableStatus;

    int numPlayers = 4;
    public List<PlayerActions> players = new List<PlayerActions>();
    Dictionary<PlayerActions, int> playerToPos = new Dictionary<PlayerActions, int>();
    Dictionary<CharacterPanel,PlayerActions> panelToPlayer = new Dictionary<CharacterPanel,PlayerActions>();
    private bool playable;
    public UnityAction onTranstionToMainMenu;
    private bool onCharacterSelect = true;
    private GameTypeEnum gameType;
    PlayerActions allPlayers;
    int panelCount = 4;

    private void Start()
    {
        var inputDevice = InputManager.ActiveDevice;
        //last device to
        controllerSelect.SetOnJoin(AddToPlayers);
        controllerSelect.listening = true;
        allPlayers = PlayerActions.CreateAllControllerBinding();
    }


    public void Initialize()
    {

        this.gameType = FindObjectOfType<PlayerSelectSettings>().gameType;
        Reset();

        if (gameType == GameTypeEnum.DeathMatch)
        {
            this.panels[this.panels.Count - 1].gameObject.SetActive(true);
            this.panels.ForEach(panel => panel.Initialize(this.characterDMPanelSprites, GlobalVariables.CharactersForDeathMatch()));
            this.mapView.Initialize(this.gameType, mapEnum =>
            {
                this.BuildDMPlayerSettings(mapEnum);
                SceneManager.LoadScene("Game");
            });
        }
        else if (gameType == GameTypeEnum.Sabotage)
        {
            panelCount = 3;
            this.panels[this.panels.Count - 1].gameObject.SetActive(false);
            this.panels.ForEach(panel => panel.Initialize(this.characterSABPanelSprites, GlobalVariables.CharactersForSabotage()));
            this.mapView.Initialize(this.gameType, mapEnum =>
            {
                this.BuildSABPlayerSettings(mapEnum);
                SceneManager.LoadScene("Game");
            });
        }
        else if (gameType == GameTypeEnum.Targets)
        {
            panelCount = 1;
            for (int x = 1;x < 4; x++)
            {
                this.panels[x].gameObject.SetActive(false);
            }
            this.panels.ForEach(panel => panel.Initialize(this.characterTARPanelSprites, GlobalVariables.CharactersForTargets()));
            this.mapView.Initialize(this.gameType, mapEnum => {
                this.BuildTARPlayerSettings(mapEnum);
                SceneManager.LoadScene("Game");
            });
        }


    }

    private void Reset()
    {
        playerToPos.Clear();
        panelToPlayer.Clear();
        players.Clear();
        foreach (CharacterPanel p in panels)
        {
            p.SignOut();
        }
        for (int x = 1; x < 4; x++)
        {
            this.panels[x].gameObject.SetActive(true);
        }
        controllerSelect.ClearPlayers();
        
    }
    private void Update () {
        foreach (PlayerActions player in players)
        {
            UpdatePlayerController(player);
        }
        if(panelToPlayer.Count == 0 && allPlayers.Red.WasPressed)
        {
            this.TransitionToMainMenu();
        }
    }

    private void UpdatePlayerController(PlayerActions player)
    {

        if (this.onCharacterSelect) {
            this.UpdateCharacterSelect(player);
        } else
        {
            this.UpdateMapSelect(player);
        }
    }

    private void UpdateCharacterSelect(PlayerActions player) {

        if (panelToPlayer.ContainsValue(player))
        {

            if (player.Start.WasReleased && this.playable)
            {
                this.TransitionToMapSelect();
            }

            int playerPosition = playerToPos[player];
            var panel = this.panels[playerPosition];
            int playerIndex = this.players.IndexOf(player);

            if (player.Left.WasReleased)
            {
                panel.ToggleHost(playerIndex, false);
                playerToPos[player] = playerToPos[player] - 1 < 0 ? panelCount-1 : playerToPos[player] - 1;
                this.panels[playerToPos[player]].ToggleHost(playerIndex, true);
            }
            else if (player.Right.WasReleased)
            {

                panel.ToggleHost(playerIndex, false);
                playerToPos[player] = Math.Abs(playerToPos[player] + 1) % panelCount;
                this.panels[playerToPos[player]].ToggleHost(playerIndex, true);
            }



            if (player.Blue.WasReleased)
            {
                if (!this.panels[playerPosition].SignedIn)
                {
                    //panels[playerToPos[player]].SignIn(false, playerIndex);
                    panels[playerToPos[player]].BotSignIn(playerIndex, GetFirstAvailablePanel());
                }


            }

            if ((panelToPlayer.ContainsKey(panel) && player == panelToPlayer[panel]) || (!panel.IsPlayer && panel.SignedIn))
            {
                if (player.Red.WasReleased)
                {
                    if (panel.CharacterSelected)
                    {
                        panel.CharacterSelected = false;
                        UnlockCharactersForSabotage(panel);
                        this.UpdatePlayableStatus();
                    }
                    else
                    {
                        if (panel.IsPlayer)
                        {
                            panelToPlayer.Remove(panel);
                            playerToPos.Remove(player);
                        }

                        panel.SignOut();


                    }
                }

                if (player.Yellow.WasReleased && gameType == GameTypeEnum.DeathMatch)
                {
                    panel.ChangeTeam();
                }

                if (player.Green.WasReleased & (!(panel.OnLockedKraken() || panel.OnLockedShip()))) 
                {
                    panel.CharacterSelected = true;
                    LockCharactersForSabotage();
                    this.UpdatePlayableStatus();

                }

                if (player.Up.WasReleased)
                {
                    panel.ChangeCharacter(-1);
                }

                if (player.Down.WasReleased)
                {
                    panel.ChangeCharacter(1);
                }
            }
        }

        else if (player.Green.WasReleased)
        {
            SignIn(player);
            LockCharactersForSabotage();

        } else if (player.Red.WasReleased) {
            this.TransitionToMainMenu();
        }
    }

    private void UpdateMapSelect(PlayerActions player) {
        if (player.Red.WasReleased)
        {
            this.TransitionToCharacterSelect();
        }
        else if (player.Up.WasReleased)
        {
            this.mapView.ChangeMap(1);
        }
        else if (player.Down.WasReleased)
        {
            this.mapView.ChangeMap(-1);
        }

        else if (player.Green.WasReleased) {
            this.mapView.StartGame();
        }
    }


    public void SignIn(PlayerActions player)
    {

        if (!playerToPos.ContainsKey(player) && playerToPos.Count < numPlayers)
        {
            int firstAvailablePanelIndex = GetFirstAvailablePanel();
            if (firstAvailablePanelIndex == -1 && playerToPos.Count < panelCount)
            {
                firstAvailablePanelIndex = GetFirstAvailableBotPanel();
                panels[firstAvailablePanelIndex].SignOut();
            }
           
            playerToPos.Add(player, firstAvailablePanelIndex);
            panelToPlayer.Add(panels[firstAvailablePanelIndex], player);
            panels[firstAvailablePanelIndex].SignIn(true, playerToPos.Count-1);
        }


    }


    public void AddToPlayers(PlayerActions player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }

    private void UpdatePlayableStatus() {
        
        this.playable = CheckRestraints();
        this.playableStatus.SetActive(this.playable);
    }


    private bool CheckRestraints() {

        bool ready = false;

        if (gameType == GameTypeEnum.DeathMatch)
        {
            HashSet<int> teamsInGame = new HashSet<int>();
            foreach (CharacterPanel panel in this.panels.Filter(panel => panel.CharacterSelected))
            {
                teamsInGame.Add(panel.SelectedTeam);
            }

            bool minplayers = this.panels.Filter(panel => panel.CharacterSelected).Count >= 2;
            bool minteams = teamsInGame.Count >= 2;
            bool containsPlayers = this.panels.Exists(panel => panel.IsPlayer && panel.CharacterSelected);

            ready = minplayers & minteams & containsPlayers;
        }
        else if (gameType == GameTypeEnum.Sabotage)
        {
            bool containsKraken = this.panels.Filter(panel => panel.IsKraken).Count == 1;
            bool minplayers = this.panels.Filter(panel => panel.CharacterSelected).Count == 3;
            bool containsPlayers = this.panels.Exists(panel => panel.IsPlayer && panel.CharacterSelected);

            ready = containsKraken & minplayers & containsPlayers;
        }

        else if (gameType == GameTypeEnum.Targets)
        {
            ready = this.panels.Filter(panel => panel.CharacterSelected).Count == 1;
        }
        return ready;
    }


    private void LockCharactersForSabotage()
    {
        //lock kraken
        bool krakenSelected = this.panels.Filter(p => (p.CharacterSelected && p.IsKraken)).Count == 1;
        if (krakenSelected)
        {
            CharacterPanel kraken = this.panels.Filter(p => (p.CharacterSelected && p.IsKraken))[0];
            foreach (CharacterPanel p in panels)
            {
                if (p != kraken)
                {
                    p.KrakenLock = true;
                }
            }

        }
        //lock ships
        bool shipsSelected = this.panels.Filter(p => (p.CharacterSelected && !p.IsKraken)).Count == 2;
        if (shipsSelected)
        {
            foreach(CharacterPanel p in this.panels.Filter(p2 => (!p2.CharacterSelected))) {
                p.ShipLock = true; 
            };
        }
            
    }

    private void UnlockCharactersForSabotage(CharacterPanel panel)
    {
        if (panel.IsKraken)
        {
            foreach (CharacterPanel p in panels)
            {
                if (p != panel)
                {
                    p.KrakenLock = false;
                }
            }
        }

        int ships = this.panels.Filter(p => (p.CharacterSelected && !p.IsKraken)).Count;
        if (ships < 2)
        {
            foreach (CharacterPanel temp in this.panels.Filter(p2 => (!p2.CharacterSelected)))
            {
                temp.ShipLock = false;
            };
        }
    }


    private void TransitionToMainMenu() {
        this.gameObject.SetActive(false);
        onTranstionToMainMenu();
        this.controllerSelect.ClearPlayers();
    }

    private void TransitionToMapSelect() {
        this.onCharacterSelect = false;
        this.mapView.gameObject.SetActive(true);
    }


    private void TransitionToCharacterSelect()
    {
        this.onCharacterSelect = true;
        this.mapView.gameObject.SetActive(false);
    }

    private int GetFirstAvailablePanel()
    {
       return this.panels.FindIndex(panel => !panel.SignedIn);
    }

    private int GetFirstAvailableBotPanel()
    {
        return this.panels.FindIndex(panel => !panel.IsPlayer);
    }

    private void BuildDMPlayerSettings(MapEnum mapEnum) {

        var ps = GameObject.FindObjectOfType<PlayerSelectSettings>();
        var characterSelections = new List<CharacterSelection>();

        foreach (var panel in this.panels) {
            if (panel.CharacterSelected) {
                characterSelections.Add(
                    new CharacterSelection(
                        panel.GetSelectedCharacter(),
                        this.panelToPlayer.ContainsKey(panel) ? this.panelToPlayer[panel] : null,
                        panel.SelectedTeam,
                        !panel.IsPlayer));
            }
        }

        ps.players = characterSelections;
        ps.map = mapEnum;
    }

    private void BuildSABPlayerSettings(MapEnum mapEnum)
    {

        var ps = GameObject.FindObjectOfType<PlayerSelectSettings>();
        var characterSelections = new List<CharacterSelection>();

        foreach (var panel in this.panels)
        {
            if (panel.CharacterSelected & panel.IsKraken)
            {
                characterSelections.Add(
                    new CharacterSelection(
                        panel.GetSelectedCharacter(),
                        this.panelToPlayer.ContainsKey(panel) ? this.panelToPlayer[panel] : null,
                        panel.SelectedTeam,
                        !panel.IsPlayer));
            } else if (panel.CharacterSelected & !panel.IsKraken)
            {
                characterSelections.Add(
                    new CharacterSelection(
                        panel.GetSelectedCharacter(),
                        this.panelToPlayer.ContainsKey(panel) ? this.panelToPlayer[panel] : null,
                        panel.SelectedTeam,
                        !panel.IsPlayer));
            }
        }

        ps.players = characterSelections;
        ps.map = mapEnum;
    }

    private void BuildTARPlayerSettings(MapEnum mapEnum)
    {
        var ps = GameObject.FindObjectOfType<PlayerSelectSettings>();
        var characterSelections = new List<CharacterSelection>();

        foreach (var panel in this.panels)
        {
           
            if (panel.CharacterSelected & !panel.IsKraken)
            {
                characterSelections.Add(
                    new CharacterSelection(
                        panel.GetSelectedCharacter(),
                        this.panelToPlayer.ContainsKey(panel) ? this.panelToPlayer[panel] : null,
                        panel.SelectedTeam,
                        false));
            }
        }

        ps.players = characterSelections;
        ps.map = mapEnum;
    }
}
