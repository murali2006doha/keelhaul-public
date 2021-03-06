﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChineseAltCannonComponent : AbstractAltCannonComponent {

	public override void alternateFire () {

		GameObject instantiated = PhotonNetwork.Instantiate(PathVariables.alternateChineseShot, cannonBallPos.position, this.transform.rotation, 0);
        instantiated.GetComponent<CannonBall>().setOwner(transform.root);
        instantiated.GetComponent<PhotonView>().RPC("AddForce", PhotonTargets.All, this.transform.forward * altCannonForce);
		input.gameStats.numOfAlternateShots++;

	}

  public override void CancelPower() {
    throw new NotImplementedException();
  }

  public override void setupRotation() {
		shoot_direction = aim.transform.position - shipTransform.position;
		this.transform.rotation = Quaternion.LookRotation (shoot_direction.normalized);

	}
}

