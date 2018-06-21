using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AbstractAltCannonComponent : MonoBehaviour {

  public int altCannonForce = 400;

  [Header("Scene Variables")]
  public GameObject alternateFirePrefab;
  public Transform cannonBallPos;

  protected float altTimer;
  protected bool canShootAlt = true;
  protected float alternateShootDelay;
  protected Transform shipTransform;
  protected GameObject aim;
  protected PlayerInput input;
  protected Vector3 shoot_direction;
  protected UIManager uiManager;
  protected ShipStats stats;
  public bool disabled;
  abstract public void alternateFire();
  abstract public void setupRotation();
  abstract public void CancelPower();

  internal void Initialize(PlayerInput input, Transform shipTransform, GameObject aim, ShipStats stats, UIManager uiManager) {
    this.input = input;
    this.stats = stats;
    this.aim = aim;
    this.shipTransform = shipTransform;
    this.uiManager = uiManager;
  }

  public void Update() {
    if (!canShootAlt) {
      float val = (Time.realtimeSinceStartup - altTimer) / stats.alternateShootDelay;
      uiManager.setAltFireMeter(val > 1 ? 1f : val);
    }
  }



  public void handleShoot() {
    if (disabled) {
      return;
    }
    this.setupRotation();

    //if (canShootAlt && input.Actions.Alt_Fire.RawValue > .5f) {
    if (canShootAlt && shoot_direction.magnitude > 0) {
      altTimer = Time.realtimeSinceStartup;
      this.alternateFire();
      canShootAlt = false;
      Invoke("ResetShotAlt", stats.alternateShootDelay);
      input.vibrate(.15f, .25f);
      uiManager.resetAltFireMeter();
      uiManager.animManager.onAlternateFire();


    }

  }

  public virtual void ResetShotAlt() {
    canShootAlt = true;
    uiManager.setAltFireMeter(1);
    uiManager.SetAltFireAvailable();
  }
}
