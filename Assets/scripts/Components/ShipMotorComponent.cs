using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMotorComponent : MonoBehaviour
{

    protected CharacterController cc;
    protected ShipStats stats;
    protected Transform shipTransform;
    protected Action onBoost, onBoostFinish;

    [Header("Scene Variables")]
    public GameObject wake;


    [SerializeField] protected float velocity = 0f;
    [SerializeField] protected bool boosted, boosting;
    [SerializeField] protected float speedModifier = 1;
    protected Vector3 directionVector = Vector3.zero;
    protected Vector3 oldEulerAngles;
    protected Quaternion originalRotation;
    protected Quaternion originalRotationValue;


    internal void Initialize(CharacterController characterController, ShipStats stats, Transform shipTransform, Action onBoost, Action onBoostFinish)
    {
        this.cc = characterController;
        this.stats = stats;
        this.shipTransform = shipTransform;
        this.onBoost = onBoost;
        this.onBoostFinish = onBoostFinish;
    }

    public virtual void Update()
    {
       
        UpdateShipPosition();
        UpdateWake();
    }

    protected void UpdateShipPosition()
    {

        //Must be in Boost?
        if ((directionVector.magnitude == 0 && velocity != 0f) || (velocity > stats.maxVelocity))
        {
            velocity = Mathf.Max(0f, (velocity - (stats.deAccelerationSpeed * (Time.deltaTime * GlobalVariables.gameSpeed))));
        }

        shipTransform.position = new Vector3(shipTransform.position.x, 0f, shipTransform.position.z); //Clamp Y


        if (directionVector.magnitude > 0f && velocity <= stats.maxVelocity)
        {
            velocity = Mathf.Min(stats.maxVelocity, velocity + (directionVector.magnitude * stats.moveSpeed * (Time.deltaTime * GlobalVariables.gameSpeed)));

            if (boosting)
            {
                Debug.Log("reaching boost finish");
                boosting = false;
                this.onBoostFinish();
            }
        }

        RotateBoat();

        cc.Move(shipTransform.forward * velocity * speedModifier * (Time.deltaTime * GlobalVariables.gameSpeed));

        if (this.shipTransform.position.y != 0)
        {
            shipTransform.position = MathHelper.setY(shipTransform.position, Mathf.Lerp(shipTransform.position.y, 0, 1));
        }
        directionVector = Vector3.zero;
    }

    void RotateBoat()
    {
        if (directionVector.magnitude > 0f)
        {
            Quaternion wanted_rotation = Quaternion.LookRotation(directionVector);
            shipTransform.rotation = Quaternion.RotateTowards(shipTransform.rotation, wanted_rotation, stats.turnSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));
        }
    }

    void UpdateWake()
    {
        wake.transform.rotation = shipTransform.rotation;
    }

    internal void UpdateInput(Vector3 input)
    {
        directionVector = input;

    }

    public virtual void Boost()
    {
        if (!boosted) {
            this.onBoost();
            boosted = true;
            boosting = true;
            velocity = stats.boostVelocity;
            Invoke("ResetBoost", stats.boostResetTime);
        }
    }

    void tiltBoat()
    {

        Quaternion wanted_rotation = directionVector.Equals(Vector3.zero) ? Quaternion.identity : Quaternion.LookRotation(directionVector); // get the rotation

        if (oldEulerAngles == transform.rotation.eulerAngles)
        { //if not rotating, go back to original rotation

            shipTransform.localRotation = Quaternion.Lerp(shipTransform.localRotation, originalRotation, stats.tiltSpeed * (Time.deltaTime * GlobalVariables.gameSpeed));

        }
        else
        {

            float angle_difference = (wanted_rotation.eulerAngles.y - transform.rotation.eulerAngles.y);

            if (angle_difference < 0)
            {
                angle_difference = angle_difference + 360f;
            }


            if ((angle_difference < 180) && (angle_difference > 0))
            { //when tilting right

                Quaternion newRotation = Quaternion.Euler(stats.tiltAngle, 0f, 0f);
                shipTransform.localRotation = Quaternion.Lerp(shipTransform.localRotation, newRotation, stats.tiltSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));

            }
            else if ((angle_difference > 180) && (angle_difference < 360))
            {
                Quaternion newRotation = Quaternion.Euler(-stats.tiltAngle, 0f, 0f);
                shipTransform.localRotation = Quaternion.Lerp(shipTransform.localRotation, newRotation, stats.tiltSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));
            }

            oldEulerAngles = transform.rotation.eulerAngles; //set new oldEulerAngles

        }
    }

    void ResetBoost()
    {
        boosted = false;
    }


    public float getVelocity()
    {
        return velocity;
    }

    internal void reset()
    {
        ResetBoost();
        velocity = 0;
        speedModifier = 1;
        directionVector = Vector3.zero;
    }

    public void setSpeedModifier(float val)
    {
        this.speedModifier = val;
    }
}
