using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMotorComponent : MonoBehaviour
{

    CharacterController cc;
    ShipStats stats;
    Transform shipTransform;

    [Header("Scene Variables")]
    public GameObject wake;


    private float velocity = 0f;
    Vector3 directionVector = Vector3.zero;
    bool boosted;
    private float speedModifier = 1;


    internal void Initialize(CharacterController characterController, ShipStats stats, Transform shipTransform)
    {
        this.cc = characterController;
        this.stats = stats;
        this.shipTransform = shipTransform;
    }

    void Update()
    {
        UpdateShipPosition();
        UpdateWake();
    }

    void UpdateShipPosition() {
        if ((directionVector.magnitude == 0 && velocity != 0f) || (velocity * speedModifier > stats.maxVelocity))
        {
            velocity = Mathf.Max(0f, (velocity * speedModifier - (stats.deAccelerationSpeed * (Time.deltaTime * GlobalVariables.gameSpeed)))); 
        }

        shipTransform.position = new Vector3(shipTransform.position.x, 0f, shipTransform.position.z); //Clamp Y


        if (directionVector.magnitude > 0f && velocity * speedModifier <= stats.maxVelocity)
        {
            velocity = Mathf.Min(stats.maxVelocity, velocity * speedModifier + (directionVector.magnitude * stats.moveSpeed * (Time.deltaTime * GlobalVariables.gameSpeed)));
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

    void Boost(float[] values)
    {
        if (!boosted)
        {
            boosted = true;
            velocity = stats.boostVelocity;
            Invoke("ResetBoost", stats.boostResetTime);
        }

    }

    void ResetBoost()
    {
        boosted = false;
    }

}
