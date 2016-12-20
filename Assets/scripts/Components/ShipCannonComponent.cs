﻿using UnityEngine;
using System.Collections;

public class ShipCannonComponent : MonoBehaviour
{

    public GameObject cannonBallPrefab;
    public Transform cannonBallPos;

    public int cannonForce = 1000; //Make this public so designers can easily manipulate it
    public int arcCannonForce = 10;
    float dampening = 0.2f;
    public float numberOfCannons = 2;
    public int numOfCannonBalls = 3;
    public float angleOfCannonShots = 30;

    bool canShootRight = true;
    float altTimer;
    float speed = 1;
    ShipStats stats;
    float forceMultiplier = 20f; //Forward facing force multiplier

    Vector3 velocity;
    Transform shipTransform;
    ShipMotorComponent motor;
    GameObject aim;
    PlayerInput input;
    FreeForAllStatistics gameStats;

    internal void Initialize(PlayerInput input, Transform shipTransform, GameObject aim, ShipStats stats,
        FreeForAllStatistics gameStats, ShipMotorComponent motor)
    {
        //this.input = input;
        this.stats = stats;
        this.aim = aim;
        this.shipTransform = shipTransform;
        this.motor = motor;
        this.gameStats = gameStats;
        this.input = input;
    }


    private void Fire()
    {

        var angle = Vector3.Angle(shipTransform.forward, this.transform.forward);
        Vector3 vect = Vector3.zero;
        if (angle > 0 && angle < 45)
        {
            vect = this.transform.forward * GlobalVariables.gameSpeed * forceMultiplier * speed;
        }
        else if (angle > 45 && angle < 100)
        {
            vect = this.transform.forward * GlobalVariables.gameSpeed * forceMultiplier / 2f * speed;
        }
        Vector3 cal = (aim.transform.position - this.transform.position);
        cal.y = 0;
        Vector3 look = MathHelper.setY(this.transform.rotation.eulerAngles, Quaternion.LookRotation(cal).eulerAngles.y);
        Vector3 rot = MathHelper.addY(look, (numOfCannonBalls / 2) * -angleOfCannonShots);

        for (int x = 0; x < numOfCannonBalls; x++)
        {
            Vector3 newRot = MathHelper.addY(rot, x * angleOfCannonShots);
            GameObject cannonBall = (GameObject)Instantiate(cannonBallPrefab, cannonBallPos.position + (velocity * dampening), Quaternion.Euler(newRot));
            cannonBall.transform.rotation = Quaternion.Euler(newRot);
            cannonBall.GetComponent<CannonBall>().setOwner(transform.root);
            Vector3 forwardForce = cannonBall.transform.forward * cannonForce + vect;
            Vector3 upForce = cannonBall.transform.up * arcCannonForce;
            cannonBall.GetComponent<Rigidbody>().AddForce(upForce + forwardForce);
        }

        this.gameStats.numOfShots += this.numOfCannonBalls;
    }


    public void handleShoot()
    {
        this.velocity = shipTransform.forward * motor.getVelocity() * GlobalVariables.gameSpeed;
        this.speed = motor.getVelocity() * GlobalVariables.gameSpeed;
        Vector3 shoot_direction = aim.transform.position - shipTransform.position;
        this.transform.rotation = Quaternion.LookRotation(shoot_direction.normalized);
        if (canShootRight && shoot_direction.magnitude > 0)
        {
            this.Fire();
            input.vibrate(.15f, .25f);
            canShootRight = false;
            Invoke("ResetShotRight", stats.shootDelay);
        }
    }


    public void ResetShotRight()
    {
        canShootRight = true;
    }

}