using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipViewToggler : Photon.MonoBehaviour {
    void Start()
    { 
        if (photonView.isMine)
        {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            playerInput.enabled = true;
            playerInput.altCannonComponent.enabled = true;
            playerInput.motor.enabled = true;
            var audioListener = GetComponent<AudioListener>();
            if (audioListener != null) {
                audioListener.enabled = true;
            }
        }
    }

}
