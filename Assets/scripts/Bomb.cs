﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bomb : MonoBehaviour {

	public Bomb_Controller parent;
	public int blinks = 5;
	public GameObject bombZone;
	public float blinkTime = .5f;
	//these are all the animations involved
	public GameObject shipHit;
	public GameObject krakenHit;
	public GameObject explosion; 
	public float explosion_duration = 1.5f;
	public float damage;


	public IEnumerator ActivateBomb() {


		GameObject smallZone = gameObject.transform.Find ("bomb zone").gameObject;
		GameObject model = gameObject.transform.Find ("bomb_model").gameObject;
		smallZone.SetActive (false);

		GameObject largeBombzone = (GameObject) Instantiate (bombZone, gameObject.transform.position, gameObject.transform.rotation);

		for(int i = 0; i < blinks; i++){ //blinks when activated
			largeBombzone.GetComponent<Renderer> ().material.color = Color.white;
			model.GetComponent<Renderer> ().material.color = Color.white;
			yield return new WaitForSeconds(blinkTime);
			largeBombzone.GetComponent<Renderer> ().material.color = Color.yellow;
			model.GetComponent<Renderer> ().material.color = Color.red;
			yield return new WaitForSeconds(blinkTime);
		}

		//explode!
		Destroy (largeBombzone); 			//destroy the parameter zone
		GameObject exp = explode(); //produces an explosion
		//Invoke ("fadeHalo", .5f);
		yield return new WaitForSeconds(explosion_duration); 

		Destroy(exp);					//destroy the explosion
	}



	void OnTriggerEnter(Collider other) {
		if (LayerMask.LayerToName (other.gameObject.layer).Equals ("playerMesh") && 
			parent.input.gameObject != other.GetComponentInParent<playerInput>().gameObject) {//to activate a bomb
			if (parent.bombList.Contains (other.gameObject) == false) {
				parent.input.gameStats.numOfBombsDetonated+=0.5f;
				StartCoroutine (ActivateBomb ());
			}
		}

		if (LayerMask.LayerToName(other.gameObject.layer).Equals("kraken")) {
			KrakenInput kraken = other.gameObject.GetComponent<KrakenInput> ();
			if (kraken.submerged == false) { //only if not submerged
				kraken.gameStats.numOfBombsDetonated++;
				StartCoroutine(ActivateBomb());
			}
		}

	}



	public void DestroyShip(GameObject exp, Collider col) {
		playerInput controller = col.GetComponentInParent<playerInput> ();
		playerInput ownerPlayer = parent.input.GetComponent<playerInput> ();
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
