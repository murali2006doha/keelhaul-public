using UnityEngine;
using System.Collections;

public class windtrail : MonoBehaviour {

	public void disableTrail(){
		this.GetComponent<TrailRenderer> ().enabled = false;
	}

	public void enableTrail(){
		this.GetComponent<TrailRenderer> ().enabled = true;
	}
}
