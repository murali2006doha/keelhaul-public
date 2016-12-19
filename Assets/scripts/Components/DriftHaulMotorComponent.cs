using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftHaulMotorComponent : ShipMotorComponent
{

    public float rotationSpeed = 1f;
    public Vector3 acceleration;
    public Vector3 shipVelocity;

    public bool isPushed;
    float pushMagnitude;
    Vector3 pushDirection;
    public float deAccelerationSpeed = 2f;
    public float maxVelocity = 3f;
    public float boostVelocity = 5f;
    public float moveSpeed = 2f;
    public float turnSpeed = 120f;



    public override void Update()
    {
        UpdateShipPosition();
        UpdateWake();
    }

    new protected void UpdateShipPosition()
    {
        
        if (shipVelocity.sqrMagnitude > 0)
        {

            acceleration = shipVelocity.normalized * Time.deltaTime * deAccelerationSpeed * -1;

        }


        transform.position = new Vector3(transform.position.x, 0f, transform.position.z); //clamp y

        if (isPushed)
        {

            directionVector = pushDirection;
            transform.Rotate(Vector3.up, rotationSpeed * cc.velocity.magnitude, Space.World);
            cc.Move(pushDirection * velocity * (Time.deltaTime * GlobalVariables.gameSpeed));
            if (velocity <= 0 || cc.velocity.magnitude <= 0.2f)
            {
                stopPushForce();
            }
            return;
        }


        if (directionVector.magnitude > 0f && shipVelocity.magnitude <= maxVelocity)
        {
            acceleration += (transform.forward * directionVector.magnitude * moveSpeed * (Time.deltaTime * GlobalVariables.gameSpeed));
        }

        if (directionVector.magnitude > 0f)
        {
            Quaternion wanted_rotation = Quaternion.LookRotation(directionVector); // get the rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, wanted_rotation, turnSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));
        }

        shipVelocity = Vector3.ClampMagnitude(shipVelocity + acceleration, getMaxVelocity());

        if (boosted && boosting && shipVelocity.magnitude <= maxVelocity) {
            this.onBoostFinish();
            boosting = false;
        }

        cc.Move(shipVelocity * this.speedModifier *  (Time.deltaTime * GlobalVariables.gameSpeed));

        if (this.transform.position.y != 0)
        {
            transform.position = MathHelper.setY(transform.position, Mathf.Lerp(transform.position.y, 0, 1));
        }

        directionVector = Vector3.zero;
    }

    public float getMaxVelocity()
    {
        if (boosted)
        {
            return boostVelocity;
        }
        return maxVelocity;
    }

    void RotateBoat()
    {
        if (directionVector.magnitude > 0f)
        {
            Quaternion wanted_rotation = Quaternion.LookRotation(directionVector);
            shipTransform.rotation = Quaternion.RotateTowards(shipTransform.rotation, wanted_rotation, turnSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));
        }
    }

    void UpdateWake()
    {
        wake.transform.rotation = shipTransform.rotation;
    }

    public override void Boost()
    {
        if (!boosted)
        {
            this.onBoost();
            boosted = true;
            boosting = true;
            shipVelocity = Vector3.ClampMagnitude(transform.forward * boostVelocity + shipVelocity, boostVelocity);
            Invoke("ResetBoost", stats.boostResetTime);
        }
    }

    void stopPushForce()
    {
        isPushed = false;
        pushDirection = Vector3.zero;
        pushMagnitude = 0f;
       

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



    internal new void reset()
    {
        ResetBoost();
        isPushed = false;
        pushDirection = Vector3.zero;
        pushMagnitude = 0f;
        velocity = 0;
        speedModifier = 1;
        directionVector = Vector3.zero;
    }

}
