using UnityEngine;
using System.Collections;

public abstract class AbstractGameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract void exitToCharacterSelect();
    public abstract void incrementPoint(playerInput player, GameObject barrel);
    public abstract void respawnPlayer(playerInput player, Vector3 startingPoint);


}
