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
    string bombPath;
	int bombCount = 3;
	List<GameObject> bombList = new List<GameObject>();

	public bool canDropBomb = true;

	


	internal void Initialize(ShipStats stats, PlayerInput input, UIManager uiManager, FreeForAllStatistics gameStats, string bombPath)
	{
		this.input = input;
        this.bombPath = bombPath;
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
        GameObject bombObject = PhotonNetwork.Instantiate(bombPath, transform.position, transform.rotation, 0);
		bombObject.GetComponent<BombComponent>().Initialize (this, this.input);
		bombList.Add (bombObject);
	}


	public void activateAllBombs(){
        uiManager.animManager.onBombExplosion();
        foreach (GameObject bomb in bombList) { //destroy all bombs on field

			if (bomb != null) {
				StartCoroutine(bomb.GetComponent<BombComponent> ().ActivateBomb ());
			}
		}
	}


	private void decrementBomb() {

		uiManager.decrementBomb ();
		bombCount = bombCount - 1;
		uiManager.setBombBar ((float)(bombCount));

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
		





}
