using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;



public class BroadsideCannonController : MonoBehaviour {

	public Transform ParentLeftCannon;
	public Transform ParentRightCannon;
	public GameObject cannonBallPrefab;

	public int cannonBallsNumber = 3;
	List<GameObject> leftCannons = new List<GameObject>();
	List<GameObject> rightCannons = new List<GameObject>();

	public Transform cannonBallPosL;
	public Transform cannonBallPosR;
	public int cannonForce = 400; //Make this public so designers can easily manipulate it
	public int arcCannonForce = 10;
	public Vector3 velocity;
	public float dampening = 1f;
	private float passedTime;
	public PlayerInput input;
	float altTimer;
	public float speed = 1;
	public float minDelay = 0.2f;
	public float maxDelay = 0.5f;


	public IEnumerator fireBroadside() {

		for (int x = 0; x < cannonBallsNumber; x++) {
			float wait_time = Random.Range (minDelay, maxDelay);
			//GameObject CannonBallL = leftCannons [0];
			LeftFire ();
			//leftCannons.RemoveAt (0);

			//GameObject CannonBallR = rightCannons [0];
			RightFire ();
			//rightCannons.RemoveAt (0);
			yield return new WaitForSeconds (wait_time);
		}

		input.gameStats.numOfAlternateShots++;
	}

	void LeftFire () {
		GameObject cannonBall = (GameObject)Instantiate(cannonBallPrefab, cannonBallPosL.position + (velocity * dampening), this.transform.rotation);
		cannonBall.transform.rotation = ParentLeftCannon.rotation;

		cannonBall.GetComponent<CannonBall>().setOwner(transform.root);
		cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.forward * cannonForce);
		cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.up * arcCannonForce);

	}


	void RightFire () {
		GameObject cannonBall = (GameObject)Instantiate(cannonBallPrefab, cannonBallPosR.position + (velocity * dampening), this.transform.rotation);
		cannonBall.transform.rotation = ParentRightCannon.rotation;

		cannonBall.GetComponent<CannonBall>().setOwner(transform.root);
		cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.forward * cannonForce);
		cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.up * arcCannonForce);

	}
}