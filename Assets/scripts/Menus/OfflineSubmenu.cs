using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineSubmenu : AbstractMenu {

    public ActionButton deathMatchOffline;
    public ActionButton sabotageOffline;
    public ActionButton targetsOffline;

    [SerializeField]
    private CharacterSelectController csController;

    protected override void SetActions()
    {
        this.csController.onTranstionToMainMenu = this.TransitionOutOfCharacterSelect;

        deathMatchOffline.SetAction(() => {
            FindObjectOfType<PlayerSelectSettings>().gameType = GameTypeEnum.DeathMatch;
            this.TransitionToCharacterSelect();

        });

        sabotageOffline.SetAction(() => {
            FindObjectOfType<PlayerSelectSettings>().gameType = GameTypeEnum.Sabotage;
            this.TransitionToCharacterSelect();
        });

        targetsOffline.SetAction(() => {
            FindObjectOfType<PlayerSelectSettings>().gameType = GameTypeEnum.Targets;
            this.TransitionToCharacterSelect();
        });

    }




    protected override void SetActionSelectables()
    {
        actionSelectables.Add(deathMatchOffline.gameObject);       
        actionSelectables.Add(sabotageOffline.gameObject);
        actionSelectables.Add(targetsOffline.gameObject);
    }



    private void TransitionToCharacterSelect()
    {
        this.csController.gameObject.SetActive(true);
        this.csController.Initialize();
        this.gameObject.SetActive(false);
    }

    private void TransitionOutOfCharacterSelect()
    {
        this.gameObject.SetActive(true);
    }
}
