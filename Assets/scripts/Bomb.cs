using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bomb : MonoBehaviour {

	public Transform owner;
	public int blinks = 5;
	public GameObject bombZone;
	public float blinkTime = .5f;
	//these are all the animations involved
	public GameObject shipHit;
	public GameObject krakenHit;
	public GameObject explosion; 
	public float explosion_duration = 1.5f;


	void Start()  {
	} 


	void Update() {
	}


	public void OnTriggerEnter(Collider collider) {

		//bomb needs to explode if it is in the range of another explosion. TO DO
		if (LayerMask.LayerToName (collider.gameObject.layer).Equals ("explosion")) { 
		
		}
	}


	public IEnumerator ActivateBomb(GameObject bomb) {

		GameObject smallZone = bomb.transform.Find ("bomb zone").gameObject;
		GameObject model = bomb.transform.Find ("bomb_model").gameObject;
		smallZone.SetActive (false);

		GameObject largeBombzone = (GameObject) Instantiate (bombZone, bomb.transform.position, bomb.transform.rotation);

		for(int i = 0; i < blinks; i++){ //blinks when activated
			if (bomb!=null){
				largeBombzone.GetComponent<Renderer> ().material.color = Color.white;
				model.GetComponent<Renderer> ().material.color = Color.white;
				yield return new WaitForSeconds(blinkTime);
					
			}
			if (bomb != null) {
				largeBombzone.GetComponent<Renderer> ().material.color = Color.yellow;
				model.GetComponent<Renderer> ().material.color = Color.red;
				yield return new WaitForSeconds(blinkTime);
			}

		}

		//explode!
		Destroy (largeBombzone); 			//destroy the parameter zone
		GameObject exp = explode(bomb); //produces an explosion
		//Invoke ("fadeHalo", .5f);
		yield return new WaitForSeconds(explosion_duration); 

		Destroy(exp);					//destroy the explosion
	}

 
	public GameObject explode(GameObject bomb){
		if (bomb != null) {
			GameObject exp = (GameObject) Instantiate (explosion, bomb.transform.position, Quaternion.identity);
			Destroy (bomb);
			return exp;
		}
		return null;
	}


	public void fadeHalo() {
		Behaviour halo = (Behaviour) explosion.GetComponent("Halo");
		halo.enabled = false;
	}


		

	public void DestroyShip(GameObject exp, Collider col) {
		playerInput controller = col.GetComponentInParent<playerInput> ();
		playerInput ownerPlayer = owner.GetComponent<playerInput> ();
		Instantiate (shipHit, exp.transform.position, exp.transform.rotation);
		if (controller != null) {
			controller.hit (1,ownerPlayer);
		}
	}
		



}
