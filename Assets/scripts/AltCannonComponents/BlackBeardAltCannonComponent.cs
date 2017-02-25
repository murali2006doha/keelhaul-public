using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBeardAltCannonComponent : AbstractAltCannonComponent {

	public List<Transform> rightCannons = new List<Transform>();
	public List<Transform> leftCannons = new List<Transform>();

	public int arcCannonForce = 10;
	public Vector3 velocity;
	public float dampening = 1f;
	public float speed = 1;
	public float minDelay; 
	public float maxDelay; //will find random time between the two

	List<Transform> tempCannonListL;
	List<Transform> tempCannonListR;
	Quaternion originalRotation;

	void Start() {
		originalRotation = this.transform.rotation;
	}


	public override void setupRotation() {
		shoot_direction = aim.transform.position - shipTransform.position;
	}


	public override void alternateFire() {
		initializeCannonBalls ();

		shuffle (rightCannons);
		shuffle (leftCannons);

		StartCoroutine (fireAll(rightCannons));
		StartCoroutine (fireAll(leftCannons));

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
		GameObject cannonBall = PhotonNetwork.Instantiate(PathVariables.alternateBlackbeardShot, cannonPos.position + (velocity * dampening), cannonPos.rotation, 0);
        cannonBall.GetComponent<CannonBall>().setOwner(shipTransform);
        cannonBall.GetComponent<PhotonView>().RPC("AddForce", PhotonTargets.All, cannonBall.transform.forward * altCannonForce + cannonBall.transform.up * arcCannonForce);
	}


	void initializeCannonBalls() {

		tempCannonListL = new List<Transform>(leftCannons);
		tempCannonListR = new List<Transform>(rightCannons);

	}


	void resetAllCannons() {

		leftCannons = new List<Transform>(tempCannonListL);
		rightCannons = new List<Transform>(tempCannonListR);

	}
		
}

