using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingAltCannonComponent : AbstractAltCannonComponent
{


    public override void alternateFire()
    {

        GameObject instantiated = (GameObject)PhotonNetwork.Instantiate(PathVariables.alternateVikingShot, cannonBallPos.position, this.transform.rotation, 0);
        instantiated.GetComponent<PhotonView>().RPC("ActivateFreeze", PhotonTargets.All, this.shipTransform.GetComponent<PlayerInput>().GetId());
        input.gameStats.numOfAlternateShots++;

    }

  public override void CancelPower() {
    throw new NotImplementedException();
  }

  public override void setupRotation()
    {
        shoot_direction = aim.transform.position - shipTransform.position;
        this.transform.rotation = Quaternion.LookRotation(shoot_direction.normalized);
    }
}
