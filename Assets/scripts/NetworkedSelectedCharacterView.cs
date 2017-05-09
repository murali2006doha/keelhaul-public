using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkedSelectedCharacterView : MonoBehaviour {
    [SerializeField]
    Text playerInfo;

    [SerializeField]
    TMPro.TextMeshProUGUI selectedCharacter;


    // Use this for initialization
    public void Initialize (string playerId) {
        this.playerInfo.text = "Player is selecting character";
        this.selectedCharacter.text = "<sprite=\"atlas\" name=\"null\"> <size=60%>";
    }

    public void SetCharacterPortrait(string type) {
        this.playerInfo.text = "Player has selected and is waiting";
        this.selectedCharacter.text = "<sprite=\"atlas\" name=\"" + type.ToString() + "\"> <size=60%>";
    }

}
