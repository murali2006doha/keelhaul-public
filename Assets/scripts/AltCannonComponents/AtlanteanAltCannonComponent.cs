﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlanteanAltCannonComponent : AbstractAltCannonComponent {

    public GameObject instantiated;
  [SerializeField] private float blinkDistance;
  [SerializeField] private float damage;
  [SerializeField] private float buildUpTime;
  [SerializeField] private TrailRenderer trailer;

  public override void alternateFire () {
    this.input.SetUpForTeleport();
    this.trailer.enabled = true;
    Invoke("Blink", buildUpTime);
  }

  private void Blink() {
    this.setupRotation();
    Ray ray = new Ray(this.input.shipMesh.transform.position, this.shoot_direction);
    RaycastHit hitInfo;
    this.input.motor.locked = true;
    var environmentLayerMask = 1 << LayerMask.NameToLayer("environment");

    var startingPosition = this.input.transform.position;
    if (Physics.Raycast(ray, out hitInfo, blinkDistance, environmentLayerMask, QueryTriggerInteraction.UseGlobal)) {
      this.input.transform.SetPositionAndRotation(new Vector3(hitInfo.point.x, this.input.transform.position.y, hitInfo.point.z), this.input.transform.rotation);
    } else {
      this.input.transform.position += this.shoot_direction * blinkDistance;
    }


    var directionVector = (this.input.transform.position - startingPosition);
    Ray damageRay = new Ray(startingPosition, directionVector.normalized);
    var hits = Physics.RaycastAll(damageRay, directionVector.magnitude, 1 << LayerMask.NameToLayer("player"));

    foreach (var hit in hits) {
      var enemy = hit.collider.transform.root.GetComponent<PlayerInput>();
      if (enemy != null) {
        enemy.hit(damage, this.input.GetId());
      }
    }

    input.gameStats.numOfAlternateShots++;
    this.input.ResetFromTeleport();
    Invoke("TurnOffRenderer", 1f);
  }

  public void TurnOffRenderer() {
    this.trailer.enabled = false;
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
