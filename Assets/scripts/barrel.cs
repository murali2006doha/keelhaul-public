using UnityEngine;
using System.Collections;

public class Barrel : MonoBehaviour {
	public GameObject owner;
	public LightPillar pillar;
	public GameObject explosion; 
	PlayerInput currentPlayer;

	// Use this for initialization
	void Start () {
		pillar = GetComponentInChildren<LightPillar>();
		currentPlayer = FindObjectOfType<PlayerInput> ();
	}

	public void activatePillar()
	{
		pillar.activatePillar();
	}


	public void explodeBarrel() {
		GameObject exp = (GameObject) Instantiate (explosion, this.gameObject.transform.position, Quaternion.identity);
	

	}

}
