using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{

    [SerializeField]
    private Text characterName;
    [SerializeField]
    private Text status;
    [SerializeField]
    private Text hosts;

    [SerializeField]
    private Text teamIndicator;

    [SerializeField]
    private Image selected;


    private int characterIndex = 0;
    private int selectedTeam = 0;
    public int SelectedTeam {
        get
        {
            return this.selectedTeam;
        }
        set
        {
            this.selectedTeam = value;
            this.teamIndicator.text = "Team " + value;

        }
    }
    private bool characterSelected;


    public bool IsPlayer { get; set; }
    public bool SignedIn { get; set; }

    public bool CharacterSelected
    {
        get
        {
            return this.characterSelected;
        }

        set
        {
            this.characterSelected = value;
            this.selected.gameObject.SetActive(value);
        }
    }

    private List<string> characterReferences;
    public void Initialize()
    {
        this.characterReferences = GlobalVariables.CharactersForDeathMatch();
        this.SignOut();
    }

    public string GetSelectedCharacter()
    {
        return this.characterReferences[this.characterIndex];
    }

    public void SignIn(bool isPlayer, int playerIndex)
    {
        this.IsPlayer = isPlayer;
        this.DecorateSignedIn(playerIndex);
        this.SelectedTeam = playerIndex;
        this.SignedIn = true;
    }

    public void SignOut()
    {
        this.characterName.text = string.Empty;
        this.status.text = "A to sign in!";
        this.SignedIn = false;
        this.IsPlayer = false;
        this.characterIndex = 0;
    }

    public void ToggleHost(int playerIndex, bool isAdding)
    {
        if (isAdding)
        {
            this.hosts.text += "P" + playerIndex;
        }
        else if (this.hosts.text.IndexOf(playerIndex.ToString()) > -1)
        {
            this.hosts.text.Remove(this.hosts.text.IndexOf(playerIndex.ToString()) - 1, 2);
        }
    }

    public void ChangeTeam() {
        if (this.characterSelected)
        {
            return;
        }

        if (this.SelectedTeam + 1 == 4)
        {
            this.SelectedTeam = 0;
        } else
        {
            this.SelectedTeam++;
        }
        

    }

    public void ChangeCharacter(int direction)
    {
        if (!this.characterSelected)
        {
            this.characterIndex = Mathf.Clamp(this.characterIndex + direction, 0, this.characterReferences.Count - 1);
            this.characterName.text = this.characterReferences[this.characterIndex];
        }
    }


    private void DecorateSignedIn(int playerIndex)
    {
        this.ToggleHost(playerIndex, true);
        this.status.text = this.IsPlayer ? ("Player " + playerIndex) : "Bot";
        this.ChangeCharacter(0);
    }
}
