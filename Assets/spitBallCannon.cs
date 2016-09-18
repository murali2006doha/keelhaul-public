using UnityEngine;
using System.Collections;

public class spitBallCannon : MonoBehaviour
{

    public GameObject cannonBallPrefab;
    public Transform cannonBallPos;
    public int cannonForce = 1000; //Make this public so designers can easily manipulate it
    public int arcCannonForce = 10;
    public Vector3 velocity;
    public float dampening = 1f;
    private float passedTime;
    public float spitCharge = 1f;
    public float spitTime = 1f;
    public Vector4 spitScale;
    public GameObject spit;
    public bool canShoot = true;
    public KrakenInput input;
    public float angleOfCannonShots = 30;
    float altTimer;
    public float speed = 1;
    public int baseSpitDamage = 1;
    public int spitDamage = 1;
    public float maxSpitDamage = 4;
    public float maxSpitDistance = 4;
    float shootDelay, alternateShootDelay = 0.1f;
    float forceMultiplier = 20f; //Forward facing force multiplier
    public float maxSpitScale = 4f;

    public void setDelays(float shootDelay, float alternateShootDelay)
    {
        this.shootDelay = shootDelay;
    }

    public void spawnSpit() {
        spit = (GameObject)Instantiate(cannonBallPrefab, cannonBallPos.position + (velocity * dampening), transform.rotation);
        spit.transform.parent = cannonBallPos.transform;
        spitScale = spit.transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        if (spit) {
            float chargeMagnitude = Time.realtimeSinceStartup - spitTime;
            if (input.Actions.Device)
                input.Actions.Device.Vibrate(chargeMagnitude/5);
            if (spit.transform.localScale.magnitude < maxSpitScale)
                 spit.transform.localScale = spitScale * (spitScale.magnitude + (chargeMagnitude));
            if (spitDamage < maxSpitDamage)
                spitDamage = (int)(baseSpitDamage + chargeMagnitude);  
        }
    }

    public void Fire()
    {
        if (canShoot) {
            float chargeMagnitude = Mathf.Min((Time.realtimeSinceStartup - spitTime), maxSpitDistance);
            input.vibrate(chargeMagnitude/5 * 2, .1f);
            spit.transform.parent = null;
            spit.GetComponent<CannonBall>().setOwner(transform.root);
            spit.GetComponent<CannonBall>().kraken = input;
            spit.GetComponent<Rigidbody>().isKinematic = false;
            spit.GetComponent<CannonBall>().enabled = true;
            spit.GetComponent<CannonBall>().damage = spitDamage;
            spit.GetComponent<Collider>().enabled = true;
            spit.GetComponent<Rigidbody>().AddForce(spit.transform.forward * cannonForce * chargeMagnitude);
            spit.GetComponent<Rigidbody>().AddForce(spit.transform.up * arcCannonForce);
            canShoot = false;
            spit = null;
            spitTime = 0;
        }
      

    }




    public void Fire(Vector3 position, Vector3 direction)
    {

        GameObject cannonBall = (GameObject)Instantiate(cannonBallPrefab, position + (velocity * dampening), cannonBallPos.rotation);
        cannonBall.GetComponent<CannonBall>().setOwner(transform.root);
        cannonBall.transform.rotation = Quaternion.LookRotation(direction);
        cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.forward * cannonForce);
        cannonBall.GetComponent<CannonBall>().gravity = 0f;

    }

    public void handleShoot(Vector3 velocity, float speed)
    {
        this.velocity = velocity;
        this.speed = speed;
		Vector3 shoot_direction = transform.forward;
        this.transform.rotation = Quaternion.LookRotation(shoot_direction.normalized);
		if (canShoot && (input.Actions.Fire.RawValue > .5f))
        {
            if (shoot_direction.magnitude > 0)
            {
                this.Fire();
                input.vibrate(.15f, .25f);
                canShoot = false;
                Invoke("ResetShotRight", shootDelay);
            }
        }

    }

    public void ResetShots()
    {
        canShoot = true;
    }
}