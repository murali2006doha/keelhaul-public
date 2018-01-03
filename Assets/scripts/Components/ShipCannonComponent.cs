using UnityEngine;
using System.Collections;

public class ShipCannonComponent : MonoBehaviour
{

    public Transform cannonBallPos;

    public int cannonForce = 1000; //Make this public so designers can easily manipulate it
    public int arcCannonForce = 10;
    protected float dampening = 0.2f;
    public int numOfCannonBalls = 3;
    public float angleOfCannonShots = 30;

    protected bool canShootRight = true;
    protected float altTimer;
    protected float speed = 1;
    protected ShipStats stats;
    protected float forceMultiplier = 20f; //Forward facing force multiplier

    protected Vector3 velocity;
    protected Transform shipTransform;
    protected ShipMotorComponent motor;
    protected GameObject aim;
    protected PlayerInput input;
    protected FreeForAllStatistics gameStats;
    public string cannonballPath;

    internal void Initialize(PlayerInput input, Transform shipTransform, GameObject aim, ShipStats stats,
        FreeForAllStatistics gameStats, ShipMotorComponent motor, string path)
    {
        this.stats = stats;
        this.aim = aim;
        this.shipTransform = shipTransform;
        this.motor = motor;
        this.gameStats = gameStats;
        this.input = input;
        this.cannonballPath = path;
    }


    public virtual void Fire()
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
            GameObject cannonBall = PhotonNetwork.Instantiate(cannonballPath, cannonBallPos.position + (velocity * dampening), Quaternion.Euler(newRot), 0);
            cannonBall.transform.rotation = Quaternion.Euler(newRot);
            cannonBall.GetComponent<CannonBall>().setOwner(transform.root);
            Vector3 forwardForce = cannonBall.transform.forward * cannonForce + vect;
            Vector3 upForce = cannonBall.transform.up * arcCannonForce;
            cannonBall.GetComponent<PhotonView>().RPC("AddForce", PhotonTargets.All, upForce + forwardForce);
        }

        this.gameStats.numOfShots += this.numOfCannonBalls;
    }	


    public void handleShoot()
    {
        this.velocity = shipTransform.forward * motor.getVelocity() * GlobalVariables.gameSpeed;
        this.speed = motor.getVelocity() * GlobalVariables.gameSpeed;
        Vector3 shoot_direction = aim.transform.position - shipTransform.position;
        if (canShootRight && shoot_direction.magnitude > 0)
        {
            Fire();
            input.vibrate(.3f, .3f);
            canShootRight = false;
            Invoke("ResetShotRight", stats.shootDelay);
        }
    }

    public void AmpUpCannonball() {
        this.cannonballPath += "Amp";
    }

    public void DeAmpCannonball()
    {
        this.cannonballPath = this.cannonballPath.Substring(0,this.cannonballPath.IndexOf("Amp"));
    }

    public void ResetShotRight()
    {
        canShootRight = true;
    }

}
