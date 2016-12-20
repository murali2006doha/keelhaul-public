using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipViewToggler : Photon.MonoBehaviour {

    void Start()
    { 
        if (photonView.isMine)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<ShipMotorComponent>().enabled = true;
        }
    }

}
