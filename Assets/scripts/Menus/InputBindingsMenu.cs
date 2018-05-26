using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;
using System;

public class InputBindingsMenu : AbstractMenu
{

  public Text setBindingText;
  public ActionButton moveForward;
  public ActionButton rotateLeft;
  public ActionButton rotateRight;
  public ActionButton fire;
  public ActionButton boost;
  //public ActionButton hookshot;
  public ActionButton dropBomb;
  public ActionButton altFire;
  public ActionButton submerge_emerge;
  public Text moveForwardText;
  public Text rotateLeftText;
  public Text rotateRightText;
  public Text fireText;
  public Text boostText;
  //public Text hookshotText;
  public Text dropBombText;
  public Text altFireText;
  public Text submerge_emergeText;
  private bool checkingBinding = false;
  private PlayerAction actionToBind;

  public override void Navigate()
  {


    if (!checkingBinding)
    {
      canReturn = true;
      if (actionSelectables.Count > 0)
      {
        index = NavigationUtils.NavigateModal(this.actions, actionSelectables.ToArray(), index);
      }

      index = NavigationUtils.NavigateModalWithMouse(actionSelectables, index);

      if (this.actions.Device == null || this.actions.Device.Name == "None")
      {
        moveForwardText.text = PlayerActions.GetKeyboardBindingName(this.actions.L_Up);
        rotateLeftText.text = PlayerActions.GetKeyboardBindingName(this.actions.L_Left);
        rotateRightText.text = PlayerActions.GetKeyboardBindingName(this.actions.L_Right);
        fireText.text = PlayerActions.GetKeyboardBindingName(this.actions.Fire);
        altFireText.text = PlayerActions.GetKeyboardBindingName(this.actions.Alt_Fire);
        boostText.text = PlayerActions.GetKeyboardBindingName(this.actions.Boost);
        //hookshotText.text = PlayerActions.GetKeyboardBindingName(this.actions.Fire_Hook);
        dropBombText.text = PlayerActions.GetKeyboardBindingName(this.actions.Bomb);
        submerge_emergeText.text = PlayerActions.GetKeyboardBindingName(this.actions.Submerge_emerge);
      }

      setBindingText.gameObject.SetActive(false);

      if (AnyInputSaveWasReleased())
      {
        PlayerActions.SaveBindings(this.actions);
        print("SAVED bindings");
      }

      if (AnyInputResetWasReleased())
      {
        Dictionary<ModalActionEnum, Action> modalActions = new Dictionary<ModalActionEnum, Action>();
        modalActions.Add(ModalActionEnum.onOpenAction, () =>
        {
          this.enabled = false;
          ToggleSelectables();
          canReturn = false;
        });
        modalActions.Add(ModalActionEnum.onCloseAction, () =>
        {
          this.enabled = true;
          ToggleSelectables();
          canReturn = true;
        });

        ModalStack.InitializeModal(this.actions, ModalsEnum.notificationDoubleModal, modalActions);
        FindObjectOfType<NotificationDoubleModal>().Spawn(NotificationImages.quitConfirm,
          NotificationImages.yes,
          NotificationImages.no,
          () =>
          {
            PlayerActions.Reset(this.actions);
          }, () =>
          {
          });
      }


    }
    else
    {

      canReturn = false;
      setBindingText.gameObject.SetActive(true);
      CheckIfBindingIsComplete(actionToBind);

      if (AnyInputBackWasReleased())
      {
        checkingBinding = false;
        ToggleSelectables();
      }
    }
  }

  protected override void SetActions()
  {
    if (this.actions.Device == null || this.actions.Device.Name == "None")
    {
      SetAllActions();
    }
    else
    {
      ToggleSelectables();
    }
  }


  void SetAllActions() {
    moveForward.SetAction(() =>
    {
      checkingBinding = true;
      actionToBind = this.actions.L_Up;
      PlayerActions.Listen(actionToBind);
      ToggleSelectables();

    });
    rotateLeft.SetAction(() =>
    {
      checkingBinding = true;
      actionToBind = this.actions.L_Left;
      PlayerActions.Listen(actionToBind);
      ToggleSelectables();
    });
    rotateRight.SetAction(() =>
    {
      checkingBinding = true;
      actionToBind = this.actions.L_Right;
      PlayerActions.Listen(actionToBind);
      ToggleSelectables();
    });
    fire.SetAction(() =>
    {
      checkingBinding = true;
      actionToBind = this.actions.Fire;
      PlayerActions.Listen(actionToBind);
      ToggleSelectables();
    });
    altFire.SetAction(() =>
    {
      checkingBinding = true;
      actionToBind = this.actions.Alt_Fire;
      PlayerActions.Listen(actionToBind);
      ToggleSelectables();
    });
    boost.SetAction(() =>
    {
      checkingBinding = true;
      actionToBind = this.actions.Boost;
      PlayerActions.Listen(actionToBind);
      ToggleSelectables();
    });
    //hookshot.SetAction (() => {
    //  checkingBinding = true;
    //  actionToBind = this.actions.Fire_Hook;
    //  PlayerActions.Listen (actionToBind);
    //});
    dropBomb.SetAction(() =>
    {
      checkingBinding = true;
      actionToBind = this.actions.Bomb;
      PlayerActions.Listen(actionToBind);
      ToggleSelectables();
    });

    submerge_emerge.SetAction(() =>
    {
      checkingBinding = true;
      actionToBind = this.actions.Submerge_emerge;
      PlayerActions.Listen(actionToBind);
      ToggleSelectables();
    });
  }

  public override void GoBack()
  {
    if (!dontDestroy)
    {
      ResetPage();
      this.gameObject.SetActive(false);
    }
    onReturnAction();
  }


  protected override void SetActionSelectables()
  {
    actionSelectables.Add(moveForward.gameObject);
    actionSelectables.Add(rotateLeft.gameObject);
    actionSelectables.Add(rotateRight.gameObject);
    actionSelectables.Add(fire.gameObject);
    actionSelectables.Add(boost.gameObject);
    //actionSelectables.Add (hookshot.gameObject);
    actionSelectables.Add(dropBomb.gameObject);
    actionSelectables.Add(altFire.gameObject);
    actionSelectables.Add(submerge_emerge.gameObject);
  }


  void CheckIfBindingIsComplete(PlayerAction action)
  {
    if (PlayerActions.IsBindingDone(action))
    {
      checkingBinding = false;
      ToggleSelectables();
    }
  }


  void ResetPage()
  {
    checkingBinding = false;
    canReturn = true;
    setBindingText.gameObject.SetActive(false);
    index = 0;
    PlayerActions.LoadBindings(this.actions);

    moveForwardText.text = PlayerActions.GetKeyboardBindingName(this.actions.Up);
    rotateLeftText.text = PlayerActions.GetKeyboardBindingName(this.actions.Left);
    rotateRightText.text = PlayerActions.GetKeyboardBindingName(this.actions.Right);
    fireText.text = PlayerActions.GetKeyboardBindingName(this.actions.Fire);
    altFireText.text = PlayerActions.GetKeyboardBindingName(this.actions.Alt_Fire);
    boostText.text = PlayerActions.GetKeyboardBindingName(this.actions.Boost);
    //hookshotText.text = PlayerActions.GetKeyboardBindingName(this.actions.Fire_Hook);
    dropBombText.text = PlayerActions.GetKeyboardBindingName(this.actions.Bomb);
    submerge_emergeText.text = PlayerActions.GetKeyboardBindingName(this.actions.Submerge_emerge);
  }

}
