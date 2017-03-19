using UnityEngine;
using System.Collections;
[System.Serializable]
public class CharacterSelection {

    public ShipEnum selectedCharacter;
    public int team; //make this part of the constructor so team id can be passed along with player

    public CharacterSelection(string selectedCharacter, PlayerActions actions)
    {
        this.selectedCharacter =(ShipEnum) System.Enum.Parse(typeof(ShipEnum),selectedCharacter,true) ;
        Actions = actions;
    }

    public PlayerActions Actions { get; set; }

}
