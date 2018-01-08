using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{

    [SerializeField]
    private Image characterImage;

    [SerializeField]
    private Image characterText;

    [SerializeField]
    private Text status;

    [SerializeField]
    private Text teamIndicator;

    [SerializeField]
    private GameObject teamHolder;

    [SerializeField]
    private List<PanelHostHolder> panelHostHolders;

    [SerializeField]
    private SpriteDictionary characterTypeImages;

    [SerializeField]
    private SpriteDictionary characterReadyImages;

    [SerializeField]
    private SpriteDictionary characterLockImages;

    private SpriteDictionary characterToPanels;

    private List<string> characterReferences;

    private int characterIndex = 0;
    private int selectedTeam = 0;
    private bool characterSelected;

    public bool IsPlayer { get; set; }
    public bool IsKraken { get; set; }
    public bool SignedIn { get; set; }
    public bool KrakenLock { get; set; }

    void Update()
    {
        if (GameTypeEnum.Sabotage == FindObjectOfType<PlayerSelectSettings>().gameType)
        {
            this.teamHolder.gameObject.SetActive(false);

            if (GetSelectedCharacter() == "Kraken") { IsKraken = true; }
            else { IsKraken = false; }
        }

        RenderImages();

    }


    public void Initialize(SpriteDictionary panelSprites, List<string> characterReferences)
    {
        this.characterReferences = characterReferences;
        this.SignOut();
        this.characterToPanels = panelSprites;
    }

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

    public bool CharacterSelected
    {
        get
        {
            return this.characterSelected;
        }

        set
        {
            this.characterSelected = value;
        }
    }

    public bool OnLockedKraken() {

        if(IsKraken & KrakenLock) {
            return true;
        } else {
            return false;
        }

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
        this.panelHostHolders.ForEach(panel => panel.Hide());
        this.characterImage.gameObject.SetActive(false);
        this.characterText.gameObject.SetActive(false);
        this.teamHolder.gameObject.SetActive(false);
        this.status.text = string.Empty;
        this.SignedIn = false;
        this.CharacterSelected = false;
        this.IsPlayer = false;
        this.IsKraken = false;
        this.KrakenLock = false;
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
        int numCharacters = characterReferences.Count;

        if (!this.characterSelected)
        {
            if (this.characterIndex + direction == numCharacters) {
                this.characterIndex = 0;
            } else if (this.characterIndex + direction == -1) {
                this.characterIndex = numCharacters - 1;
            } else {
                this.characterIndex = this.characterIndex + direction;
            }
        }

    }

    private void RenderImages()
    {
        this.characterImage.sprite = this.characterToPanels.Get(GetSelectedCharacter());

        if (characterSelected)
        {
            this.characterText.sprite = this.characterReadyImages.Get(this.GetSelectedCharacter());
        }
        else if (!characterSelected)
        {
            if (OnLockedKraken())
            {
                this.characterText.sprite = this.characterLockImages.Get(this.GetSelectedCharacter());;
            }
            else
            {
                this.characterText.sprite = this.characterTypeImages.Get(this.GetSelectedCharacter());
            }
        };
    }

    private void DecorateSignedIn(int playerIndex)
    {
        this.characterText.gameObject.SetActive(true);
        this.characterImage.gameObject.SetActive(true);
        this.teamHolder.gameObject.SetActive(true);
        this.ToggleHost(playerIndex, true);
        this.status.text = this.IsPlayer ? ("Player " + (playerIndex+1)) : "Bot";
        this.ChangeCharacter(0);
    }
}
