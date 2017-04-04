using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlanteanAltCannonComponent : AbstractAltCannonComponent {

    public GameObject instantiated;

    public override void alternateFire () {

		instantiated = (GameObject) PhotonNetwork.Instantiate(PathVariables.alternateAtlantisShot, cannonBallPos.position, this.transform.rotation, 0);
        instantiated.GetComponent<PhotonView>().RPC("SetUpParent", PhotonTargets.All,this.shipTransform.GetComponent<PlayerInput>().GetId());
		input.gameStats.numOfAlternateShots++;

	}

	public override void setupRotation() {
		shoot_direction = aim.transform.position - shipTransform.position;
		this.transform.rotation = Quaternion.LookRotation (shoot_direction.normalized);
	}

	public PlayerInput getInput() {

		return this.input;
	}

    public override void ResetShotAlt()
    {
        base.ResetShotAlt();
        if (instantiated)
        {
            instantiated.GetComponent<AtlanteanShieldController>().CancelInvoke();
            instantiated.GetComponent<AtlanteanShieldController>().KillSelf();
        }

    }

}
