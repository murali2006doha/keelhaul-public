using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb_Controller : MonoBehaviour {


	public GameObject bombPrefab;

	public playerInput input;
	public Bomb Bomb;
	public bool canDropBomb = true;

	List<GameObject> bombList = new List<GameObject>();
	List<Vector3> explosionList = new List<Vector3>();

    public GameObject Drop_Bomb() {
		//maybe do an arc to throw bomb?

		GameObject bomb = (GameObject)Instantiate (bombPrefab, transform.position, transform.rotation);
		bomb.transform.rotation = Quaternion.Euler (-90, 0, 0);

		return bomb; //does this make sense? need to return to collect in BombList in playerinput
	}

	public void handleBomb() {
		if (input.Actions.Red && canDropBomb) {
			if (input.bombs != 0) {
				GameObject b = this.Drop_Bomb ();
                SoundManager.playSound("BombDrop", SoundCategoryEnum.Generic, transform.position);
                bombList.Add (b);
				explosionList.Add (b.transform.position); //holds the positions of the bombs. used in the on trigger enter function.
				input.uiManager.decrementBomb ();
				input.bombs = input.bombs - 1;
				float currentbombs = (float)(input.bombs);
				input.gameStats.numOfBombsPlanted++;
				input.uiManager.setBombBar (currentbombs / 3.0f);
				canDropBomb = false;

			} else {
				Debug.Log ("No More Bombs!");
			}
			Invoke ("ResetBomb", 0.5f);
		}
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
	public void handleTrigger(Collider other){
		if (LayerMask.LayerToName (other.gameObject.layer).Equals ("bomb")) {//to activate a bomb
			if (bombList.Contains (other.gameObject) == false) {
				Debug.Log ("triggering bomb explosion");
				Debug.Log (other.gameObject);
				input.gameStats.numOfBombsDetonated+=0.5f;
				StartCoroutine (Bomb.ActivateBomb (other.gameObject));
			}
		}

		if (LayerMask.LayerToName (other.gameObject.layer).Equals ("explosion") && !input.invincible) {//check if ship is in range when a bomb is exploding
			Debug.Log ("HITTING bomb explosion");
			Debug.Log (other.gameObject);
			bool shouldGetHit = true;
			foreach (Vector3 explosion in explosionList) {	//makes sure that the explosion is not coming from a bomb dropped by this ship
				if (explosion != null) {
					if (other.gameObject.transform.position == explosion) {
						shouldGetHit = false;
					}
				} 
			}
			if (shouldGetHit) {
				Debug.Log ("hit by explosion");
				Collider cl;
				cl = input.gameObject.GetComponent<Collider>();
				Bomb.DestroyShip (other.gameObject, cl); 
			}
		}
	}

	public void activateAllBombs(){
		foreach (GameObject bomb in bombList) { //destroy all bombs on field
			if (bomb != null) {
				StartCoroutine (Bomb.ActivateBomb (bomb));
			}
		}
	}

}
