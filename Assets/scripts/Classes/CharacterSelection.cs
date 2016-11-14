using UnityEngine;
using System.Collections;
[System.Serializable]
public class CharacterSelection {

    public ShipEnum selectedCharacter;
    public int team;

    public CharacterSelection(string selectedCharacter, PlayerActions actions)
    {
        this.selectedCharacter =(ShipEnum) System.Enum.Parse(typeof(ShipEnum),selectedCharacter,true) ;
        Actions = actions;
    }

    public PlayerActions Actions { get; set; }

}
