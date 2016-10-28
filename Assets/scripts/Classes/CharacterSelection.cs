using UnityEngine;
using System.Collections;
[System.Serializable]
public class CharacterSelection {

    public string selectedCharacter;

    public CharacterSelection(string selectedCharacter, PlayerActions actions)
    {
        this.selectedCharacter = selectedCharacter;
        Actions = actions;
    }

    public PlayerActions Actions { get; set; }

}
