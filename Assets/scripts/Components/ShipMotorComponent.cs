using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMotorComponent : MonoBehaviour {

  protected CharacterController cc;
  protected ShipStats stats;
  protected Transform shipTransform;
  protected Action onBoost, onBoostRecharge, onBoostFinish;
  [SerializeField]
  private float ramRotationSpeed = 1f;

  [Header("Scene Variables")]
  [SerializeField]
  public GameObject wake;
  [SerializeField]
  private GameObject boostParticle;


  [SerializeField]
  protected float velocity = 0f;
  [SerializeField]
  protected bool boosted, boosting;
  [SerializeField]
  protected float speedModifier = 1;
  protected Vector3 directionVector = Vector3.zero;
  protected Vector3 oldEulerAngles;
  protected Quaternion originalRotation;
  protected Quaternion originalRotationValue;
  protected bool keyboardControls;
  public bool aiControls;
  public bool sinking = false;
  public bool locked;

  private Vector3 rammedDirection;
  private float rammedForce;
  private bool underRam;
  private PlayerInput input;

  internal void Initialize(PlayerInput input, CharacterController characterController, ShipStats stats, Transform shipTransform, Action onBoost, Action onBoostFinish, Action onBoostRecharge, bool keyboardControls) {
    this.input = input;
    this.cc = characterController;
    this.stats = stats;
    this.shipTransform = shipTransform;
    this.onBoost = onBoost;
    this.onBoostFinish = onBoostFinish;
    this.onBoostRecharge = onBoostRecharge;
    this.keyboardControls = keyboardControls;
  }

  public virtual void Update() {
    if (shipTransform) {
      UpdateShipPosition();
      UpdateWake();
    }
  }

  protected void UpdateShipPosition() {
    if (sinking) {
      cc.Move(transform.up * -2 * stats.sinkSpeed * (Time.deltaTime * GlobalVariables.gameSpeed));
      return;
    }

    shipTransform.position = new Vector3(shipTransform.position.x, 0f, shipTransform.position.z); //Clamp Y

    if (locked) {
      return;
    }

    if (underRam) {
      rammedForce = Mathf.Max(0f, rammedForce - (stats.deAccelerationSpeed * Time.deltaTime * GlobalVariables.gameSpeed));
      shipTransform.Rotate(Vector3.up, ramRotationSpeed * cc.velocity.magnitude, Space.World);
      if (rammedForce == 0f) {
        this.input.stopPushForce();
        underRam = false;

      }

      cc.Move(rammedDirection.normalized * rammedForce * Time.deltaTime * GlobalVariables.gameSpeed);
      return;
    }
    //Must be in Boost?
    if ((directionVector.magnitude == 0 || (keyboardControls && !aiControls) && directionVector.z <= 0) && velocity != 0f || (velocity > stats.maxVelocity)) {
      velocity = Mathf.Max(0f, (velocity - (stats.deAccelerationSpeed * (Time.deltaTime * GlobalVariables.gameSpeed))));
    }




    if (directionVector.magnitude > 0f && velocity <= stats.maxVelocity) {
      if (keyboardControls && !aiControls) {
        if (directionVector.z > 0) {
          velocity = Mathf.Min(stats.maxVelocity, velocity + (directionVector.z * stats.moveSpeed * (Time.deltaTime * GlobalVariables.gameSpeed)));
        }
      } else {
        velocity = Mathf.Min(stats.maxVelocity, velocity + (directionVector.magnitude * stats.moveSpeed * (Time.deltaTime * GlobalVariables.gameSpeed)));
      }

      if (boosting) {
        Debug.Log("reaching boost finish");
        boosting = false;
        this.boostParticle.SetActive(false);
        this.onBoostFinish();
      }
    }

    RotateBoat();

    cc.Move(shipTransform.forward * velocity * speedModifier * (Time.deltaTime * GlobalVariables.gameSpeed));

    if (this.shipTransform.position.y != 0) {
      shipTransform.position = MathHelper.setY(shipTransform.position, Mathf.Lerp(shipTransform.position.y, 0, 1));
    }
    directionVector = Vector3.zero;
  }

  void RotateBoat() {
    if (directionVector.magnitude > 0f) {
      if (keyboardControls && !aiControls) {
        if (directionVector.x != 0) {
          shipTransform.Rotate(0f, directionVector.x * stats.turnSpeed * Time.deltaTime * GlobalVariables.gameSpeed, 0f);
        }
      } else {
        Quaternion wanted_rotation = Quaternion.LookRotation(directionVector);
        shipTransform.rotation = Quaternion.RotateTowards(shipTransform.rotation, wanted_rotation, stats.turnSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));
      }
    }
  }

  void UpdateWake() {
    wake.transform.rotation = shipTransform.rotation;
  }

  internal void UpdateInput(Vector3 input) {
    directionVector = input;

  }

  public void AddPushForce(float magnitude, Vector3 direction) {
    this.rammedDirection = direction;
    this.rammedForce = this.stats.maxVelocity * magnitude;
    this.underRam = true;
  }

  public void StopPushForce() {
    this.underRam = false;
  }

  public void Brake() {
    this.velocity = 0;
  }

  public virtual void Boost() {
    if (!boosted && shipTransform != null && onBoost != null) {
      this.onBoost();
      this.boostParticle.SetActive(true);
      boosted = true;
      boosting = true;
      velocity = stats.boostVelocity;
      Invoke("ResetBoost", stats.boostResetTime);
    }
  }

  void tiltBoat() {

    Quaternion wanted_rotation = directionVector.Equals(Vector3.zero) ? Quaternion.identity : Quaternion.LookRotation(directionVector); // get the rotation

    if (oldEulerAngles == transform.rotation.eulerAngles) { //if not rotating, go back to original rotation

      shipTransform.localRotation = Quaternion.Lerp(shipTransform.localRotation, originalRotation, stats.tiltSpeed * (Time.deltaTime * GlobalVariables.gameSpeed));

    } else {

      float angle_difference = (wanted_rotation.eulerAngles.y - transform.rotation.eulerAngles.y);

      if (angle_difference < 0) {
        angle_difference = angle_difference + 360f;
      }


      if ((angle_difference < 180) && (angle_difference > 0)) { //when tilting right

        Quaternion newRotation = Quaternion.Euler(stats.tiltAngle, 0f, 0f);
        shipTransform.localRotation = Quaternion.Lerp(shipTransform.localRotation, newRotation, stats.tiltSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));

      } else if ((angle_difference > 180) && (angle_difference < 360)) {
        Quaternion newRotation = Quaternion.Euler(-stats.tiltAngle, 0f, 0f);
        shipTransform.localRotation = Quaternion.Lerp(shipTransform.localRotation, newRotation, stats.tiltSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));
      }

      oldEulerAngles = transform.rotation.eulerAngles; //set new oldEulerAngles

    }
  }

  void ResetBoost() {
    if (boosted) {
      boosted = false;
      this.onBoostRecharge();
    }

  }


  public float getVelocity() {
    return velocity;
  }

  internal void reset() {
    ResetBoost();
    velocity = 0;
    speedModifier = 1;
    directionVector = Vector3.zero;
  }

  public void setSpeedModifier(float val) {
    this.speedModifier = val;
  }

  public float getSpeedModifier() {
    return this.speedModifier;
  }

  public bool isBoosting() {
    return boosting;
  }

  internal void StartSinking() {
    sinking = true;
    Invoke("StopSinking", 2f);

  }

  public void StopSinking() {
    sinking = false;
  }
}
