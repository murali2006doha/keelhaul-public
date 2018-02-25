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
    private Image botStatus;

    [SerializeField]
    private Image teamColor;

    [SerializeField]
    private SpriteDictionary characterTypeImages;

    [SerializeField]
    private SpriteDictionary characterReadyImages;

    [SerializeField]
    private SpriteDictionary characterLockImages;

    [SerializeField]
    private Animator characterSelectAnimator;

    [SerializeField] 
    private SpriteDictionary colors;

    private SpriteDictionary characterToPanels;

    private List<string> characterReferences;

    public int characterIndex { get; set; }
    private int selectedTeam = 0;
    private bool characterSelected;
    private int playerIndex;

    public bool IsPlayer { get; set; }
    public bool IsKraken { get; set; }
    public bool SignedIn { get; set; }
    public bool KrakenLock { get; set; }
    public bool ShipLock { get; set; }

    void Update()
    {
        if (GameTypeEnum.Sabotage == FindObjectOfType<PlayerSelectSettings>().gameType)
        {
            if (GetSelectedCharacter() == "Kraken") {
                IsKraken = true;
                if (SignedIn) SelectedTeam = 0;
            } else {
                IsKraken = false;
                if (SignedIn) SelectedTeam = 1;
            }
        }

        if(GameTypeEnum.Targets == FindObjectOfType<PlayerSelectSettings>().gameType)
        {
        
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
            this.DecorateTeam();
        }
    }


    void DecorateTeam() {
        //these things change along with changing teams
        if (GameTypeEnum.Targets != FindObjectOfType<PlayerSelectSettings>().gameType)
        {
            this.teamColor.sprite = this.colors.Get(TeamColorHelper.GetColor(selectedTeam).ToString());
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


    public bool OnLockedShip()
    {

        if (!IsKraken & ShipLock)
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    public string GetSelectedCharacter()
    {
        return this.characterReferences[this.characterIndex];
    }


    public void SetSelectedCharacter(int index)
    {
        this.characterIndex = index;
    }
    public void SignIn(bool isPlayer, int team)
    {
        this.playerIndex = playerIndex;
        this.IsPlayer = isPlayer;
        this.SelectedTeam = team;
        this.SignedIn = true;
        this.DecorateSignedIn();
    }

    public void BotSignIn(int team)
    {
        this.IsPlayer = false;
        this.SelectedTeam = team;
        this.SignedIn = true;
        this.DecorateSignedIn();
    }


    public void SignOut()
    {
        this.characterImage.gameObject.SetActive(false);
        this.characterText.gameObject.SetActive(false);
        this.teamColor.gameObject.SetActive(false);
        this.botStatus.enabled = false;
        this.SignedIn = false;
        this.CharacterSelected = false;
        this.IsPlayer = false;
        this.IsKraken = false;
        this.KrakenLock = false;
        this.ShipLock = false;
        this.characterIndex = 0;
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
            if (this.characterIndex + direction == numCharacters)
            {
                this.characterIndex = 0;
            }
            else if (this.characterIndex + direction == -1)
            {
                this.characterIndex = numCharacters - 1;
            }
            else
            {
                this.characterIndex = this.characterIndex + direction;
            }

            AnimateArrows(direction);
        }

    }

    private void AnimateArrows(int direction)
    {
        if (direction == -1)
        {
            characterSelectAnimator.SetBool("up-arrow", true);
        }
        else if (direction == 1)
        {
            characterSelectAnimator.SetBool("down-arrow", true);
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
            if (OnLockedKraken() || OnLockedShip())
            {
                this.characterText.sprite = this.characterLockImages.Get(this.GetSelectedCharacter());;
            }
            else
            {
                this.characterText.sprite = this.characterTypeImages.Get(this.GetSelectedCharacter());
            }
        };
    }

    private void DecorateSignedIn()
    {
        this.teamColor.gameObject.SetActive(true);
        this.DecorateTeam();
        this.characterText.gameObject.SetActive(true);
        this.characterImage.gameObject.SetActive(true);
        this.botStatus.enabled = false;
        this.ChangeCharacter(0);
    }


    public void EnableBotStatus() {
        this.botStatus.enabled = true;
    }
}
