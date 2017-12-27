using UnityEngine;
using System.Collections;
[System.Serializable]
public class CharacterSelection {

    public ShipEnum selectedCharacter;
    public int team; //make this part of the constructor so team id can be passed along with player
    public bool Bot;
    public CharacterSelection(string selectedCharacter, PlayerActions actions, int team = 0, bool bot = false)
    {
        Debug.Log(bot);
        Debug.Log(selectedCharacter);
        this.selectedCharacter =(ShipEnum) System.Enum.Parse(typeof(ShipEnum),selectedCharacter,true) ;
        Actions = actions;
        this.team = team;
        this.Bot = bot;
    }

    public PlayerActions Actions { get; set; }

}
