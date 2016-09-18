using UnityEngine;
using System.Collections;

public class signInCamera : MonoBehaviour {
	public Transform kraken_pos, ship_pos1, ship_pos2,ship_pos3;
	public Transform target;
	public float moveSpeed = 100f;
	public bool reached = true;
    public float distance;
    public Vector3 offset, _distance;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!reached && target != null) {
			moveToTarget ();
		}
	}
	void moveToTarget(){
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position + offset, moveSpeed * (Time.deltaTime * GlobalVariables.gameSpeed));
		_distance = ((target.transform.position + offset) - transform.position);
		if (_distance.magnitude <= .01f) {
			reached = true;

		}
	}
}
