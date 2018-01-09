using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour {

    public bool activated = false;
    bool notMoved = true;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CannonBall>() != null)
        {
            activated = true;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(activated && notMoved)
        {
            LeanTween.moveY(this.gameObject, -1.5f, 1f);
            notMoved = false;
        }
	}
}
