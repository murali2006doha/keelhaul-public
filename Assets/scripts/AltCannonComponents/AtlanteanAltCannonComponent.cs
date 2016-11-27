using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlanteanAltCannonComponent : AbstractAltCannonComponent {

	public override void alternateFire () {

		GameObject instantiated = (GameObject) Instantiate(alternateFirePrefab, cannonBallPos.position, this.transform.rotation);
		instantiated.GetComponent<SecondaryFire>().parent = transform.root.gameObject;
		instantiated.GetComponent<SecondaryFire>().force = altCannonForce;
		input.gameStats.numOfAlternateShots++;

	}

	public override void setupRotation() {
		shoot_direction = aim.transform.position - shipTransform.position;
		this.transform.rotation = Quaternion.LookRotation (shoot_direction.normalized);

	}
}
