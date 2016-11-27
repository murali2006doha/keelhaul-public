using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AbstractAltCannonComponent : MonoBehaviour {

	public GameObject alternateFirePrefab;
	public Transform cannonBallPos;
	public int altCannonForce = 400;
	public float alternateShootDelay = 0.1f;

	protected float altTimer;
	protected bool canShootAlt = true;

	protected Transform shipTransform;
	protected ShipMotorComponent motor;
	protected GameObject aim;
	protected PlayerInput input;
	protected Vector3 shoot_direction;

	abstract public void alternateFire ();
	abstract public void setupRotation ();

	internal void Initialize(GameObject aim, ShipStats stats, PlayerInput input)
	{
		this.input = input;
		this.alternateShootDelay = stats.alternateShootDelay;
		this.aim = aim;
		this.shipTransform = input.gameObject.transform;
	}
		

	public void handleShoot(){
		this.setupRotation ();

		if (canShootAlt && input.Actions.Alt_Fire.RawValue > .5f) {
			if (shoot_direction.magnitude > 0) {
				altTimer = Time.realtimeSinceStartup;

				this.alternateFire ();

				canShootAlt = false;
				Invoke ("ResetShotAlt", alternateShootDelay);
				input.vibrate (.15f, .25f);
				input.uiManager.resetAltFireMeter ();
			}
		} else if (!canShootAlt) {
			input.uiManager.setAltFireMeter ((Time.realtimeSinceStartup - altTimer) / alternateShootDelay);
		}

	}

	public void ResetShotAlt() {
		canShootAlt = true;
	}
}
