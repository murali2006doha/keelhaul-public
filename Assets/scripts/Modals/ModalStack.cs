using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

/*
 * ALWAYS INITIALIZE MODALS WITH THIS!
 * this will be attached to the main modal, whether its the pause screen or whatever. 
 * buttons and methods get initialized in modal component, but they get executed here
 */
public class ModalStack : MonoBehaviour {

    public static Stack<AbstractModalComponent> workingModals = new Stack<AbstractModalComponent>();

    //ordered
    static AbstractModalComponent selectedModal;        
     

    /// <summary>
    /// ModalActionEnum.onCloseAction is called when a modal is closed, which is why it is added to the popAction
    /// ModalActionEnum.onOpenAction is called when the modal is first initialized
    /// </summary>

    public static void InitializeModal(PlayerActions actions, ModalsEnum modalType, Dictionary<ModalActionEnum, Action> ModalActions) {


        UnityEngine.Object modalPrefab = Resources.Load(Modals.typeToModalPrefab [modalType]);
        GameObject modalObject = (GameObject) GameObject.Instantiate (modalPrefab, Vector3.zero, Quaternion.identity);
        AbstractModalComponent modalComponent = (AbstractModalComponent) modalObject.GetComponent<AbstractModalComponent> ();

        modalComponent.SetupModal (actions);
        modalComponent.pushAction += Push;
        modalComponent.popAction += Pop;
        modalComponent.popAction += ModalActions [ModalActionEnum.onCloseAction];
        workingModals.Push (modalComponent);

        ModalActions [ModalActionEnum.onOpenAction]();  //gets called as soon as the object is initialized

        selectedModal = getActiveModal(); //always 0th item in the list
        SetModalStatus ();

    }


    static void Push () {

        //if an another NEW modal opens 
        if (FindObjectOfType<AbstractModalComponent> () != selectedModal) {
            workingModals.Push (FindObjectOfType<AbstractModalComponent> ());
        }   
    }

    static void Pop () {

        if (workingModals.ToArray().Length > 0) {
            workingModals.Pop ();
            selectedModal.enabled = false;
        }

        //if only pause
        if (workingModals.ToArray().Length == 0) {
            ClearStack ();
        } else {
            selectedModal = getActiveModal(); 
            SetModalStatus ();
        }


    }


    static void SetModalStatus() {
        getActiveModal().isActive = true;
        getActiveModal().gameObject.SetActive (true);

        for(int i = 1; i < workingModals.ToArray().Length; i++) {
            workingModals.ToArray () [i].isActive = false;
        }

    }

    //returns top most modal (active) of the list
    static AbstractModalComponent getActiveModal() { 
        return workingModals.ToArray()[0];
    }



    static void ClearStack() {
        workingModals = new Stack<AbstractModalComponent> ();
        DestroyObject (selectedModal.transform.gameObject);
        selectedModal = null;
    }
}

