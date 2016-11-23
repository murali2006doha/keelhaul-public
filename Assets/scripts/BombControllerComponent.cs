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

	public Bomb bomb;
	public bool canDropBomb = true;

	int bombCount = 3;
	int i = 0;
	bool bombPressed = false;

	List<GameObject> bombList = new List<GameObject>();

	internal void Initialize(CharacterController characterController, ShipStats stats, Transform shipTransform, 
		UIManager uiManager, FreeForAllStatistics gameStats)
	{
		this.cc = characterController;
		this.shipTransform = shipTransform;
		this.input = shipTransform.gameObject.GetComponent<PlayerInput> ();
		this.uiManager = uiManager;
		this.gameStats = gameStats;
		this.stats = stats;
		this.bombCount = stats.max_bombs;
	}


	void Update() {

		handleBomb ();

	}


	public void handleBomb() {
		if (bombPressed && canDropBomb) {
			if (bombCount != 0) {
				GameObject b = this.spawnBomb ();
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


	internal void UpdateInput(bool isPressed) {
		bombPressed = isPressed; 
	}


	private GameObject spawnBomb() {
		//maybe do an arc to throw bomb?
		GameObject bombObject = Instantiate (bomb.gameObject, transform.position, transform.rotation);
		bombObject.GetComponent<Bomb>().Initialize (this, this.input);
		bombObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		bombList.Add (bombObject);

		return bombObject; //does this make sense? need to return to collect in BombList in playerinput
	}


	public void activateAllBombs(){
		foreach (GameObject bomb in bombList) { //destroy all bombs on field
			if (bomb != null) {
				StartCoroutine(bomb.GetComponent<Bomb> ().ActivateBomb ());
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

	public List<GameObject> getBombs(){
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
