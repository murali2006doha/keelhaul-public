using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class OfflineSubmenu : AbstractMenu {


    [SerializeField] Image panelSprite;
    [SerializeField] Text panelText;

    [SerializeField]
    private SpriteDictionary modeToImage;

    [SerializeField]
    public Animator mapViewAnimator;

    int modeIndex;
    List<GameTypeEnum> modes = new List<GameTypeEnum>();

    [SerializeField]
    private CharacterSelectController csController;

    // Update is called once per frame
    void Update()
    {
        Navigate();
        panelSprite.sprite = modeToImage.Get(modes[modeIndex].ToString());
        panelText.text = modes[modeIndex].ToString();
    }

    public override void Navigate()
    {
        if (AnyInputEnterWasReleased())
        {
            TransitionToCharacterSelect();
        }
        if (AnyInputBackWasReleased() && canReturn)
        {
            modeIndex = 0;
            GoBack();
        }
        if (NavigationUtils.AnyInputUpWasReleased(this.actions))
        {
            ChangeMode(1);
        }
        if (NavigationUtils.AnyInputDownWasReleased(this.actions))
        {
            ChangeMode(-1);
        }
    }

    public void ChangeMode(int direction)
    {
        AnimateArrows(direction);

        this.modeIndex += direction;
        if (this.modeIndex >= this.modes.Count)
        {
            this.modeIndex = 0;
        }
        else if (this.modeIndex < 0)
        {
            this.modeIndex = this.modes.Count - 1;
        }
    }

    private void AnimateArrows(int direction)
    {
        if (direction == -1)
        {
            mapViewAnimator.SetBool("down-arrow", true);
        }
        else if (direction == 1)
        {
            mapViewAnimator.SetBool("up-arrow", true);
        }
    }

    protected override void SetActions()
    {
        this.csController.onTranstionToMainMenu = this.TransitionOutOfCharacterSelect;
    }

    protected override void SetActionSelectables()
    {
        modes.Add(GameTypeEnum.DeathMatch);
        modes.Add(GameTypeEnum.Sabotage);
        modes.Add(GameTypeEnum.Targets);
    }



    private void TransitionToCharacterSelect()
    {
        FindObjectOfType<PlayerSelectSettings>().gameType = modes[modeIndex];
        this.csController.gameObject.SetActive(true);
        this.csController.Initialize();
        this.gameObject.SetActive(false);
    }

    private void TransitionOutOfCharacterSelect()
    {
        this.gameObject.SetActive(true);
    }
}
