using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenInk : MonoBehaviour {

    public float lifeTime;
    public Vector3 startPosition;
    public Transform kraken;
    public float timePassed;
    // Use this for initialization
    void Start () {
        timePassed = Time.realtimeSinceStartup;
        startPosition = transform.position;
	}

}
