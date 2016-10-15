using UnityEngine;
using System.Collections;

public class barrel : MonoBehaviour {
	public GameObject owner;
	public LightPillar pillar;
	public GameObject explosion; 
	playerInput currentPlayer;

	// Use this for initialization
	void Start () {
		pillar = GetComponentInChildren<LightPillar>();
		currentPlayer = FindObjectOfType<playerInput> ();
	}

	public void activatePillar()
	{
		pillar.activatePillar();
	}


	public void explodeBarrel() {
		GameObject exp = (GameObject) Instantiate (explosion, this.gameObject.transform.position, Quaternion.identity);
	

	}

}
