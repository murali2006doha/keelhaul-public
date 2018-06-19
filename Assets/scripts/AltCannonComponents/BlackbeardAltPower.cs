using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class BlackbeardAltPower : AbstractAltCannonComponent {

  ShipCannonComponent cannonComponent;
  bool canMultiplyDamage = false;
  float damageMultiplier = 1f;

  [SerializeField]
  public Harpoon harpoonPrefab;

  [SerializeField]
  private float harpoonSpeed;

  public AudioClip soundClip;

  public void setDamageMultiplier(float damageMultiplier) {
    this.damageMultiplier = damageMultiplier;
  }

  public void resetDamageMultiplier() {
    this.damageMultiplier = 1f;
  }

  private void OnHarpoonEnd(PlayerInput victim) {
    Debug.Log("reaching here for " + victim.GetId());
    victim.MoveToHarpoonLocation(this.transform.position, harpoonSpeed);
  }

  public override void alternateFire() {
    var instantiatedHarpoon = GameObject.Instantiate(harpoonPrefab, this.cannonBallPos.position, this.transform.rotation);
    instantiatedHarpoon.Initialize(this.OnHarpoonEnd, this.input);
  }

  public override void setupRotation() {
    shoot_direction = aim.transform.position - shipTransform.position;
    this.transform.rotation = Quaternion.LookRotation(shoot_direction.normalized);
  }
}
