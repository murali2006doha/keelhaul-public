﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AbstractAltCannonComponent : MonoBehaviour {

	public GameObject alternateFirePrefab;
	public Transform cannonBallPos;
	public int altCannonForce = 400;

	protected float altTimer;
	protected bool canShootAlt = true;
	protected float alternateShootDelay;
	protected Transform shipTransform;
	protected GameObject aim;
	protected PlayerInput input;
	protected Vector3 shoot_direction;
	protected UIManager uiManager;
	protected ShipStats stats;

	abstract public void alternateFire ();
	abstract public void setupRotation ();

	internal void Initialize(PlayerInput input, Transform shipTransform, GameObject aim, ShipStats stats, UIManager uiManager)
	{
		this.input = input;
		this.stats = stats;
		this.aim = aim;
		this.shipTransform = shipTransform;
		this.uiManager = uiManager;
	}
		

	public void handleShoot(){
		this.setupRotation ();

		//if (canShootAlt && input.Actions.Alt_Fire.RawValue > .5f) {
		if (canShootAlt && shoot_direction.magnitude > 0) {
			altTimer = Time.realtimeSinceStartup;

			this.alternateFire ();

			canShootAlt = false;
			Invoke ("ResetShotAlt", stats.alternateShootDelay);
			input.vibrate (.15f, .25f);
			uiManager.resetAltFireMeter ();

		} else if (!canShootAlt) {
			uiManager.setAltFireMeter ((Time.realtimeSinceStartup - altTimer) / stats.alternateShootDelay);
		}

	}

	public void ResetShotAlt() {
		canShootAlt = true;
	}
}