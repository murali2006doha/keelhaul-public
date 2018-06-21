using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtlanteanAltCannonComponent : AbstractAltCannonComponent {

    public GameObject instantiated;
  [SerializeField] private float blinkDistance;
  [SerializeField] private float damage;
  [SerializeField] private float buildUpTime;
  [SerializeField] private TrailRenderer trailer;
  [SerializeField] private GameObject particleEffect;
  [SerializeField] private Animator shipAnim;

  public override void alternateFire () {
    this.input.SetUpForTeleport();
    this.trailer.enabled = true;
    this.particleEffect.SetActive(true);
    this.particleEffect.transform.SetParent(null, true);
    this.shipAnim.SetTrigger("ethereal");
    Invoke("Blink", buildUpTime);
  }

  private void Blink() {
    this.particleEffect.SetActive(false);
    this.particleEffect.transform.SetParent(this.trailer.transform, true);
    this.setupRotation();
    this.shipAnim.SetTrigger("dethereal");
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
	
		List<PlayerInput> hitInputs = new List<PlayerInput> ();
    foreach (var hit in hits) {
      var enemy = hit.collider.transform.root.GetComponent<PlayerInput>();
			if (enemy != null && !hitInputs.Contains(enemy)) {
        enemy.hit(damage, this.input.GetId());
				hitInputs.Add (enemy);
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

  public override void CancelPower() {

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
