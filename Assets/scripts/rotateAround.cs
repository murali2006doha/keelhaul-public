using UnityEngine;
using System.Collections;

public class rotateAround : MonoBehaviour {

    public Transform pivot;
    public float angleSpeed = 10f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(pivot.transform.position, Vector3.up, angleSpeed * Time.deltaTime);
	}
}
