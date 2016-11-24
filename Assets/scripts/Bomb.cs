using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bomb : MonoBehaviour {

	BombControllerComponent parentCannon;
	PlayerInput player;

	int blinks = 5;
	float blinkTime;

	public GameObject largeBombZone;
	public GameObject smallBombZone;
	public GameObject bombModel;

	//these are all the animations involved
	public GameObject shipHit;
	public GameObject krakenHit;
	public GameObject explosion; 
	public float explosion_duration = 1.5f;
	public float damage;
	public float waitTimeToExplode;


	void Start() {

		blinkTime = (waitTimeToExplode / (float) blinks);

	}


	internal void Initialize(BombControllerComponent parent, PlayerInput input) {
		this.parentCannon = parent;
		this.player = input;
	}


	public IEnumerator ActivateBomb() {

		smallBombZone.SetActive (false);
		largeBombZone.SetActive (true);

		for(int i = 0; i < blinks; i++){ //blinks when activated
			largeBombZone.GetComponent<Renderer> ().material.color = Color.white;
			bombModel.GetComponent<Renderer> ().material.color = Color.white;
			yield return new WaitForSeconds(blinkTime);
			largeBombZone.GetComponent<Renderer> ().material.color = Color.yellow;
			bombModel.GetComponent<Renderer> ().material.color = Color.red;
			yield return new WaitForSeconds(blinkTime);
		}

		//explode!
		Destroy (largeBombZone); 			//destroy the parameter zone
		GameObject exp = explode(); //produces an explosion
		//Invoke ("fadeHalo", .5f);
		yield return new WaitForSeconds(explosion_duration); 

		Destroy(exp);					//destroy the explosion
	}



	void OnTriggerEnter(Collider other) {

		if ((other.gameObject.name).Equals ("playerMesh") && 
			player.gameObject != other.GetComponentInParent<PlayerInput>().gameObject) {//to activate a bomb
			if (parentCannon.getBombs().Contains (other.gameObject) == false) {
				player.gameStats.numOfBombsDetonated+=0.5f;
				StartCoroutine (ActivateBomb ());
			}
		}

		if ((other.gameObject.name).Equals("krakenMesh")) {
			KrakenInput kraken = other.gameObject.GetComponent<KrakenInput> ();
			if (kraken.submerged == false) { //only if not submerged
				kraken.gameStats.numOfBombsDetonated++;
				StartCoroutine(ActivateBomb());
			}
		}

	}



	public void DestroyShip(GameObject exp, Collider col) {

		PlayerInput controller = col.GetComponentInParent<PlayerInput> ();
		PlayerInput ownerPlayer = player;

		Instantiate (shipHit, exp.transform.position, exp.transform.rotation);
		if (controller != null) {
			controller.hit (damage,ownerPlayer);
		}
	}

 
	private GameObject explode(){
		if (gameObject != null) {
			GameObject exp = (GameObject) Instantiate (explosion, gameObject.transform.position, Quaternion.identity);
			Destroy (gameObject);
			return exp;
		}
		return null;
	}


	private void fadeHalo() {
		Behaviour halo = (Behaviour) explosion.GetComponent("Halo");
		halo.enabled = false;
	}


}
