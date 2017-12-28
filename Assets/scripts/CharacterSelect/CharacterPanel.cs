using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{

    [SerializeField]
    private Image characterImage;

    [SerializeField]
    private Text status;

    [SerializeField]
    private Text teamIndicator;

    [SerializeField]
    private Image selected;

    [SerializeField]
    private List<PanelHostHolder> panelHostHolders;

    private SpriteDictionary characterToPanels;


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
    public void Initialize(SpriteDictionary panelSprites)
    {
        this.characterReferences = GlobalVariables.CharactersForDeathMatch();
        this.SignOut();
        this.characterToPanels = panelSprites;
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
        this.characterImage.gameObject.SetActive(false);
        this.status.text = string.Empty;
        this.SignedIn = false;
        this.IsPlayer = false;
        this.characterIndex = 0;
    }

    public void ToggleHost(int playerIndex, bool isAdding)
    {

        if (isAdding)
        {
            this.panelHostHolders[playerIndex].Decorate(playerIndex);
        }
        else
        {
            this.panelHostHolders[playerIndex].Hide();
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
            this.characterImage.sprite = this.characterToPanels.Get(this.characterReferences[this.characterIndex]);
        }


    }


    private void DecorateSignedIn(int playerIndex)
    {
        this.characterImage.gameObject.SetActive(true);
        this.ToggleHost(playerIndex, true);
        this.status.text = this.IsPlayer ? ("Player " + playerIndex) : "Bot";
        this.ChangeCharacter(0);
    }
}
