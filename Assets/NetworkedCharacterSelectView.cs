using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedCharacterSelectView : MonoBehaviour {


    public NetworkedCharacterSelectButton [] buttons;
    // Use this for initialization
   public void Initialize(Action<ShipEnum> onClick) {
        foreach (NetworkedCharacterSelectButton button in buttons) {
            button.onClickAction = onClick;
        }
    }
}
