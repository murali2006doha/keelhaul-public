using UnityEngine;
using System.Collections;

public abstract class AbstractGameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void acknowledgeBarrelScore(playerInput player, GameObject barrel)
    {

    }
    public virtual void acknowledgeKill(StatsInterface attacker, StatsInterface victim)
    {

    }

    public abstract void exitToCharacterSelect();
    
    public abstract void respawnPlayer(playerInput player, Vector3 startingPoint);
    virtual public void respawnKraken(KrakenInput player, Vector3 startingPoint)
    {

    }

    public abstract bool isGameOver();
    internal abstract int getNumberOfTeams();
}
