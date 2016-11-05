using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb_Controller : MonoBehaviour {


	//public GameObject bombPrefab;

	public playerInput input;
	public Bomb bombComponent;
	public bool canDropBomb = true;
	public int bombCount = 3;
	int i = 0;

	public List<GameObject> bombList = new List<GameObject>();
	List<Vector3> explosionList = new List<Vector3>();


	public void handleBomb() {
		if (input.Actions.Red && canDropBomb) {
			if (bombCount != 0) {
				GameObject b = this.spawnBomb ();
                SoundManager.playSound(SoundClipEnum.BombDrop, SoundCategoryEnum.Generic, transform.position);
                bombList.Add (b);
				explosionList.Add (b.transform.position); //holds the positions of the bombs. used in the on trigger enter function.
				input.uiManager.decrementBomb ();
				float currentbombs = (float)(bombCount);
				input.gameStats.numOfBombsPlanted++;
				input.uiManager.setBombBar (currentbombs / 3.0f);
				canDropBomb = false;
				decrementBombCount ();

			} else {
				Debug.Log ("No More Bombs!");
			}
			Invoke ("ResetBomb", 0.5f);
		}
	}



	private GameObject spawnBomb() {
		//maybe do an arc to throw bomb?
		GameObject bomb = Instantiate (bombComponent.gameObject, transform.position, transform.rotation);
		bomb.GetComponent<Bomb> ().parent = this;
		bomb.transform.rotation = Quaternion.Euler (-90, 0, 0);

		return bomb; //does this make sense? need to return to collect in BombList in playerinput
	}


	public void decrementBombCount() {

		bombCount = bombCount - 1;

	}

	public void resetBombs(){
		bombList = new List<GameObject>();
		input.uiManager.resetBomb ();
		explosionList = new List<Vector3>();
	}

	void ResetBomb() {
		canDropBomb = true;
	}

	public List<GameObject> getBombs(){
		return bombList;
	}

	public List<Vector3>  getExplosions(){
		return explosionList;
	}


	//called in triggerEnter in playerInput
	public void handleTrigger(Collider other){
		bool shouldGetHit = true;
		bool stopHitting = false;
		if (LayerMask.LayerToName (other.gameObject.layer).Equals ("explosion") && !input.invincible) {//check if ship is in range when a bomb is exploding
			Debug.Log ("HITTING bomb explosion");
			foreach (Vector3 explosion in explosionList) {	//makes sure that the explosion is not coming from a bomb dropped by this ship
				if (explosion != null) {
					if (other.gameObject.transform.position == explosion) {
						shouldGetHit = false;
					}
				} 
			}
			if (shouldGetHit && !stopHitting) {
				Debug.Log ("hit by explosion");
				Collider cl = input.gameObject.GetComponent<Collider>();
				bombComponent.DestroyShip (other.gameObject, cl);
				other.enabled = false;
				stopHitting = true;
			}
		}
	}

	public void activateAllBombs(){
		foreach (GameObject bomb in bombList) { //destroy all bombs on field
			if (bomb != null) {
				StartCoroutine(bomb.GetComponent<Bomb> ().ActivateBomb ());
			}
		}
	}

}
