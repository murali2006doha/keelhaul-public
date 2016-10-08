using UnityEngine;
using System.Collections;

public class barrel : MonoBehaviour {
	public GameObject owner;
    public LightPillar pillar;
	// Use this for initialization
	void Start () {
        pillar = GetComponentInChildren<LightPillar>();
    }

    public void activatePillar()
    {
        pillar.activatePillar();
    }

}
