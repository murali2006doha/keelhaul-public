using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using InControl;
using System;

public class InputBindingsMenu : AbstractMenu {

    public ActionButton moveForward;
    public ActionButton rotateLeft;
    public ActionButton rotateRight;
    public ActionButton fire;
    public ActionButton altFire;
    public ActionButton boost;
    public ActionButton hookshot;
    public ActionButton dropBomb;
    public ActionButton save;
    public ActionButton reset;
    public Text moveForwardText;
    public Text rotateLeftText;
    public Text rotateRightText;
    public Text fireText;
    public Text altFireText;
    public Text boostText;
    public Text hookshotText;
    public Text dropBombText;
    public Text setBinding;
    private bool checkingBinding = false;
    private PlayerAction actionToBind;


    public override void Navigate() {
        if (!checkingBinding) {

            canReturn = true;

            if (this.actions.Device == null || this.actions.Device.Name == "None") {

                moveForwardText.text = PlayerActions.GetName(this.actions.Up);
                rotateLeftText.text = PlayerActions.GetName(this.actions.Left);
                rotateRightText.text = PlayerActions.GetName(this.actions.Right);
                fireText.text = PlayerActions.GetName(this.actions.Fire);
                altFireText.text = PlayerActions.GetName(this.actions.Alt_Fire);
                boostText.text = PlayerActions.GetName(this.actions.Boost);
                hookshotText.text = PlayerActions.GetName(this.actions.Fire_Hook);
                dropBombText.text = PlayerActions.GetName(this.actions.Bomb);
            }

            setBinding.gameObject.SetActive(false);

            if (actionSelectables.Count > 0) {
                index = NavigateModal(this.actions, actionSelectables.ToArray(), index);
                //NavigateModal(actionSelectables.ToArray());
            }
            index = NavigateModalWithMouse(actionSelectables, index);
            //NavigateModalWithMouse();
       
        } else {

            canReturn = false;
            setBinding.gameObject.SetActive(true);
            CheckIfBindingIsComplete(actionToBind);

            if (AnyInputBackWasReleased()) {
                checkingBinding = false;
            }
        }
    }

    protected override void SetActions() {
        if (this.actions.Device == null || this.actions.Device.Name == "None") {
            moveForward.SetAction(() => {
                checkingBinding = true;
                actionToBind = this.actions.Up;
                PlayerActions.Listen(actionToBind);

            });
            rotateLeft.SetAction(() => {
                checkingBinding = true;
                actionToBind = this.actions.Left;
                PlayerActions.Listen(actionToBind);
            });
            rotateRight.SetAction(() => {
                checkingBinding = true;
                actionToBind = this.actions.Right;
                PlayerActions.Listen(actionToBind);
            });
            fire.SetAction(() => {
                checkingBinding = true;
                actionToBind = this.actions.Fire;
                PlayerActions.Listen(actionToBind);
            });
            altFire.SetAction(() => {
                checkingBinding = true;
                actionToBind = this.actions.Alt_Fire;
                PlayerActions.Listen(actionToBind);
            });
            boost.SetAction(() => {
                checkingBinding = true;
                actionToBind = this.actions.Boost;
                PlayerActions.Listen(actionToBind);
            });
            hookshot.SetAction(() => {
                checkingBinding = true;
                actionToBind = this.actions.Fire_Hook;
                PlayerActions.Listen(actionToBind);
            });
            dropBomb.SetAction(() => {
                checkingBinding = true;
                actionToBind = this.actions.Bomb;
                PlayerActions.Listen(actionToBind);
            });



            save.SetAction(() => {
                PlayerActions.SaveBindings(this.actions);
            });

            Dictionary<ModalActionEnum, Action> modalActions = new Dictionary<ModalActionEnum, Action>();
            modalActions.Add(ModalActionEnum.onOpenAction, () => {
                this.enabled = false;
                ToggleSelectables();
                canReturn = false;
            });
            modalActions.Add(ModalActionEnum.onCloseAction, () => {
                this.enabled = true;
                ToggleSelectables();
                canReturn = true;
                index = actionSelectables.IndexOf(reset.gameObject);
            });

            reset.SetAction(() => {

                ModalStack.InitializeModal(this.actions, ModalsEnum.notificationDoubleModal, modalActions);
                FindObjectOfType<NotificationDoubleModal>().Spawn(NotificationImages.quitConfirm,           
                                                            NotificationImages.yes, 
                                                        NotificationImages.no,
                                                            () => {
                    PlayerActions.Reset(this.actions);
                }, () => {
                    reset.ButtonComponent.Select();
                });
            });

        } else {
            moveForward.SetAction(() => {});
            rotateLeft.SetAction(() => {});
            rotateRight.SetAction(() => {});
            fire.SetAction(() => {});
            altFire.SetAction(() => {});
            boost.SetAction(() => {});
            hookshot.SetAction(() => {});
            dropBomb.SetAction(() => {});

        }
    }

    protected override void SetActionSelectables() {

        actionSelectables.Add(moveForward.gameObject);
    	actionSelectables.Add(rotateLeft.gameObject);
    	actionSelectables.Add(rotateRight.gameObject);
    	actionSelectables.Add(fire.gameObject);
    	actionSelectables.Add(altFire.gameObject);
    	actionSelectables.Add(boost.gameObject);
        actionSelectables.Add(hookshot.gameObject);
        actionSelectables.Add(dropBomb.gameObject);
        actionSelectables.Add(save.gameObject);
        actionSelectables.Add(reset.gameObject);

    }


    void KeyboardOrController() {

    }


    void CheckIfBindingIsComplete(PlayerAction action) {
        if (PlayerActions.IsBindingDone(action)) {
            checkingBinding = false;
        }
    }


    public void ResetPage() {
        checkingBinding = false;
        canReturn = true;
        setBinding.gameObject.SetActive(false);
        index = 0;
        PlayerActions.LoadBindings(actions);

        moveForwardText.text = PlayerActions.GetName(this.actions.Up);
        rotateLeftText.text = PlayerActions.GetName(this.actions.Left);
        rotateRightText.text = PlayerActions.GetName(this.actions.Right);
        fireText.text = PlayerActions.GetName(this.actions.Fire);
        altFireText.text = PlayerActions.GetName(this.actions.Alt_Fire);
        boostText.text = PlayerActions.GetName(this.actions.Boost);
        hookshotText.text = PlayerActions.GetName(this.actions.Fire_Hook);
        dropBombText.text = PlayerActions.GetName(this.actions.Bomb);
    }

}
