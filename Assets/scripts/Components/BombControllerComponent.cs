using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombControllerComponent : MonoBehaviour {

	FreeForAllStatistics gameStats;
	CharacterController cc;
	ShipStats stats;
	Transform shipTransform;
	UIManager uiManager;
	PlayerInput input;

	int bombCount = 3;
	List<GameObject> bombList = new List<GameObject>();

	public bool canDropBomb = true;

	[Header("Scene Variables")]
	public BombComponent bomb;


	internal void Initialize(ShipStats stats, PlayerInput input, UIManager uiManager, FreeForAllStatistics gameStats)
	{
		this.input = input;
		this.uiManager = uiManager;
		this.gameStats = gameStats;
		this.stats = stats;
		this.bombCount = stats.max_bombs;
	}


	public void handleBomb() {
		if (canDropBomb) {
			if (bombCount != 0) {
				spawnBomb ();
                SoundManager.playSound(SoundClipEnum.BombDrop, SoundCategoryEnum.Generic, transform.position);
				gameStats.numOfBombsPlanted++;
				decrementBomb ();
				canDropBomb = false;

			} else {
				Debug.Log ("No More Bombs!");
			}
			Invoke ("ResetBomb", 0.5f);
		}
	}


	private void spawnBomb() {
		//maybe do an arc to throw bomb?
		GameObject bombObject = Instantiate (bomb.gameObject, transform.position, transform.rotation);
		bombObject.GetComponent<BombComponent>().Initialize (this, this.input);
		bombObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		bombList.Add (bombObject);
	}


	public void activateAllBombs(){
		foreach (GameObject bomb in bombList) { //destroy all bombs on field
			if (bomb != null) {
				StartCoroutine(bomb.GetComponent<BombComponent> ().ActivateBomb ());
			}
		}
	}


	private void decrementBomb() {

		uiManager.decrementBomb ();
		bombCount = bombCount - 1;
		uiManager.setBombBar ((float)(bombCount) / 3.0f);

	}

	public void resetBombs(){
		bombList = new List<GameObject>();
		input.uiManager.resetBomb ();
		this.bombCount = stats.max_bombs;
	}

	private void ResetBomb() {
		canDropBomb = true;
	}

	public List<GameObject> getBombList(){
		return bombList;
	}
		

	public void handleTrigger(Collider other){
		bool shouldGetHit = true;
		bool stopHitting = false;
		if (LayerMask.LayerToName (other.gameObject.layer).Equals ("explosion") && !input.invincible) {//check if ship is in range when a bomb is exploding
			foreach (GameObject bomb in bombList) {	//makes sure that the explosion is not coming from a bomb dropped by this ship
				if (bomb != null) {
					if (other.gameObject.transform.position == bomb.transform.position) {
						shouldGetHit = false;
					}
				} 
			}
			if (shouldGetHit && !stopHitting) {
				Collider cl = input.gameObject.GetComponent<Collider>();
				bomb.DestroyShip (other.gameObject, cl);
				other.enabled = false;
				stopHitting = true;
			}
		}
	}



}
