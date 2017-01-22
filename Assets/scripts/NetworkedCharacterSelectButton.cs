using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NetworkedCharacterSelectButton : MonoBehaviour {

    public Button button;
    [SerializeField] public CharacterSelection character;
    public string shipType;
    public  Action<ShipEnum> onClickAction;

    public void Start()
    {

    }

    public void OnClickAction() { 
        this.onClickAction(character.selectedCharacter);

    }
    
}
