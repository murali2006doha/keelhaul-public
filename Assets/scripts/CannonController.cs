using UnityEngine;
using System.Collections;

public class CannonController : MonoBehaviour {

	public GameObject cannonBallPrefab;
	public GameObject alternateFirePrefab;
	public Transform cannonBallPos;

	public int cannonForce = 1000; //Make this public so designers can easily manipulate it
	public int altCannonForce = 400;
	public int arcCannonForce = 10;
	public Vector3 velocity;
	public float dampening = 1f;
	private float passedTime;
	public GameObject point_to;
    public float numberOfCannons = 2;
	bool canShootAlt = true, canShootRight = true;
	public GameObject aim;
	public PlayerInput input;
    public int numOfCannonBalls = 3;
    public float angleOfCannonShots = 30;
	float altTimer;
	public float speed = 1;
	float shootDelay, alternateShootDelay = 0.1f;
	float forceMultiplier = 20f; //Forward facing force multiplier


	public void setDelays(float shootDelay, float alternateShootDelay){
		this.shootDelay = shootDelay;
		this.alternateShootDelay = alternateShootDelay;
	}

	public void Fire () {
		//
		var angle = Vector3.Angle (this.transform.parent.forward, this.transform.forward);
		Vector3 vect = Vector3.zero;
		if (angle > 0 && angle < 45) {
			vect = this.transform.forward * GlobalVariables.gameSpeed * forceMultiplier * speed;
		} else if(angle >45 && angle < 100) {
			vect = this.transform.forward * GlobalVariables.gameSpeed * forceMultiplier/2f * speed;
		}
        Vector3 cal = (aim.transform.position - this.transform.position);
        cal.y = 0;
        Vector3 look = MathHelper.setY(this.transform.rotation.eulerAngles,Quaternion.LookRotation(cal).eulerAngles.y);
        Vector3 rot = MathHelper.addY(look, (numOfCannonBalls/2)*-angleOfCannonShots);
        
        for (int x = 0; x < numOfCannonBalls; x++)
        {
            Vector3 newRot = MathHelper.addY(rot, x * angleOfCannonShots);
            GameObject cannonBall = (GameObject)Instantiate(cannonBallPrefab, cannonBallPos.position + (velocity * dampening), Quaternion.Euler(newRot));
            cannonBall.transform.rotation = Quaternion.Euler(newRot);
            cannonBall.GetComponent<CannonBall>().setOwner(transform.root);
            cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.forward * cannonForce + vect);
            cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.up * arcCannonForce);
        }
       
		input.gameStats.numOfShots += this.numOfCannonBalls;
        
	}


    public void alternateFire() {
		
		GameObject instantiated = (GameObject) Instantiate(alternateFirePrefab, cannonBallPos.position, this.transform.rotation);
        instantiated.GetComponent<SecondaryFire>().parent = transform.root.gameObject;
		instantiated.GetComponent<SecondaryFire>().force = altCannonForce;
		input.gameStats.numOfAlternateShots++;

    }
				

	public void Fire (Vector3 position, Vector3 direction){

		GameObject cannonBall = (GameObject)Instantiate (cannonBallPrefab, position +(velocity * dampening) , cannonBallPos.rotation);
		cannonBall.GetComponent<CannonBall> ().setOwner (transform.root);
		cannonBall.transform.rotation = Quaternion.LookRotation (direction);
		cannonBall.GetComponent<Rigidbody> ().AddForce (cannonBall.transform.forward * cannonForce);
		cannonBall.GetComponent<CannonBall> ().gravity = 0f;

	}

	public void handleShoot(Vector3 velocity,float speed){
		this.velocity = velocity;
		this.speed = speed;
		Vector3 shoot_direction = aim.transform.position - transform.parent.position;
		this.transform.rotation = Quaternion.LookRotation(shoot_direction.normalized);
		if (canShootRight && (input.Actions.Fire.RawValue > .5f)) {			
			if (shoot_direction.magnitude > 0) {
				this.Fire ();
				input.vibrate (.15f, .25f);
				canShootRight = false;
				Invoke ("ResetShotRight", shootDelay);
			}
		}
		if (canShootAlt && input.Actions.Alt_Fire.RawValue > .5f) {
			if (shoot_direction.magnitude > 0) {
				altTimer = Time.realtimeSinceStartup;
				if (input.shipName.Equals ("Blackbeard Ship")) {
					BroadsideCannonController broadSideCannons = input.GetComponentInChildren<BroadsideCannonController> ();
					broadSideCannons.fireBroadside ();
				} else {
					this.alternateFire ();
				}
				canShootAlt = false;
				Invoke ("ResetShotAlt", alternateShootDelay);
				input.vibrate (.15f, .25f);
				input.uiManager.resetAltFireMeter ();
			}
		} else if (!canShootAlt) {
			input.uiManager.setAltFireMeter ((Time.realtimeSinceStartup - altTimer) / alternateShootDelay);
		}
	}

	void ResetShotAlt() {
		canShootAlt = true;
	}


	void ResetShotRight() {
		canShootRight = true;
	}

	public void ResetShots(){
		canShootAlt = true;
		canShootRight = true;
	}
}