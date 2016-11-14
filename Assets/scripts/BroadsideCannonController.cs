using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;



public class BroadsideCannonController : MonoBehaviour {

	public GameObject cannonBallPrefab;
	public List<Transform> rightCannons = new List<Transform>();
	public List<Transform> leftCannons = new List<Transform>();

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
	public float minDelay; 
	public float maxDelay; //will find random time between the two

	List<Transform> tempCannonListL;
	List<Transform> tempCannonListR;


	void Start() {

		tempCannonListL = new List<Transform>(leftCannons);
		tempCannonListR = new List<Transform>(rightCannons);

	}


	public void fireBroadside() {

		shuffle (rightCannons);
		shuffle (leftCannons);

		StartCoroutine (fireAll(leftCannons));
		StartCoroutine (fireAll(rightCannons));

		input.gameStats.numOfAlternateShots++;

		resetAllCannons ();
	}


	IEnumerator fireAll(List<Transform> cannonList) {

		int count = cannonList.Count;
		for (int x = 0; x < count; x++) {
			float random_delay = Random.Range (minDelay, maxDelay);
			Transform cannonShot = cannonList [0];			
			fireShot (cannonShot);
			cannonList.RemoveAt (0);

			yield return new WaitForSeconds (random_delay);
		}
	}


	void shuffle(List<Transform> cannonList) {

		for (int i = 0; i < cannonList.Count; i++) {

			Transform temp = cannonList [i];
			int randomCannon = Random.Range (0, cannonList.Count);
			cannonList [i] = cannonList [randomCannon];
			cannonList [randomCannon] = temp;

		}
	}


	void fireShot (Transform cannonPos) {
		GameObject cannonBall = (GameObject)Instantiate(cannonBallPrefab, cannonPos.position + (velocity * dampening), this.transform.rotation);
		cannonBall.transform.rotation = cannonPos.rotation;

		cannonBall.GetComponent<CannonBall>().setOwner(transform.root);
		cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.forward * cannonForce);
		cannonBall.GetComponent<Rigidbody>().AddForce(cannonBall.transform.up * arcCannonForce);

	}


	void resetAllCannons() {

		leftCannons = new List<Transform>(tempCannonListL);
		rightCannons = new List<Transform>(tempCannonListR);

	}
}