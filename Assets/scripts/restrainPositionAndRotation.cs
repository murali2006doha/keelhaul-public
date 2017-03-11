using UnityEngine;
using System.Collections;

public class restrainPositionAndRotation : MonoBehaviour {

    public bool restrainRotation;
    public bool restrainPosition;
    
    Vector3 distance;

    Quaternion rotation;
    // Use this for initialization
    void Start () {

        distance = transform.parent.position - transform.position;
        rotation = transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        if (restrainPosition) {
            transform.position = transform.parent.position + distance;
        }
        if (restrainRotation)
        {
            transform.rotation = rotation;
        }
    }
}
