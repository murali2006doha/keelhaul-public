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
    private SpriteDictionary characterPanelSprites;


    [SerializeField]
    private GameObject playableStatus;

    int numPlayers = 4;
    public List<PlayerActions> players = new List<PlayerActions>();
    Dictionary<PlayerActions, int> playerToPos = new Dictionary<PlayerActions, int>();
    Dictionary<CharacterPanel,PlayerActions> panelToPlayer = new Dictionary<CharacterPanel,PlayerActions>();
    private bool playable;
    public UnityAction onTranstionToMainMenu;
    private bool onCharacterSelect = true;
    private void Start()
    {
        var inputDevice = InputManager.ActiveDevice;
        //last device to
        controllerSelect.SetOnJoin(AddToPlayers);
        controllerSelect.listening = true;

        this.panels.ForEach(panel => panel.Initialize(this.characterPanelSprites));

        this.mapView.Initialize(GameTypeEnum.DeathMatch, mapEnum => {
            this.BuildPlayerSettings(mapEnum);
            SceneManager.LoadScene("Game");
        });

    }

    private void Update () {
        foreach (PlayerActions player in players)
        {
            UpdatePlayerController(player);
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
                playerToPos[player] = playerToPos[player] - 1 < 0 ? 3 : playerToPos[player] - 1;
                this.panels[playerToPos[player]].ToggleHost(playerIndex, true);
            }
            else if (player.Right.WasReleased)
            {

                panel.ToggleHost(playerIndex, false);
                playerToPos[player] = Math.Abs(playerToPos[player] + 1) % numPlayers;
                this.panels[playerToPos[player]].ToggleHost(playerIndex, true);
            }



            if (player.Blue.WasReleased)
            {
                if (!this.panels[playerPosition].SignedIn)
                {
                    this.panels[this.playerToPos[player]].SignIn(false, playerIndex);
                }


            }

            if ((panelToPlayer.ContainsKey(panel) && player == panelToPlayer[panel]) || (!panel.IsPlayer && panel.SignedIn))
            {
                if (player.Red.WasReleased)
                {
                    if (panel.CharacterSelected)
                    {
                        panel.CharacterSelected = false;
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

                if (player.Yellow.WasReleased)
                {
                    panel.ChangeTeam();
                }

                if (player.Green.WasReleased)
                {
                    panel.CharacterSelected = true;
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
            playerToPos.Add(player, firstAvailablePanelIndex);
            panelToPlayer.Add(panels[firstAvailablePanelIndex], player);
            panels[firstAvailablePanelIndex].SignIn(true, firstAvailablePanelIndex);
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

        HashSet<int> teamsInGame = new HashSet<int>();
        foreach (CharacterPanel panel in this.panels.Filter(panel => panel.CharacterSelected))
        {
            teamsInGame.Add(panel.SelectedTeam);
        }

        bool minplayers = this.panels.Filter(panel => panel.CharacterSelected).Count >= 2;
        bool minteams = teamsInGame.Count >= 2;
        bool containsPlayers = this.panels.Exists(panel => panel.IsPlayer);

        return minplayers & minteams & containsPlayers;
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

    private void BuildPlayerSettings(MapEnum mapEnum) {

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
}
