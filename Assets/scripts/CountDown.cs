using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {
	public Text timer;
	float time;
	public bool done = false;
	// Use this for initialization
	void Start () {
		time = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
	
		timer.text = Mathf.Ceil (3 -(Time.realtimeSinceStartup - time)).ToString();
		if (Time.realtimeSinceStartup - time > 3) {
			timer.text = "GO!";
			done = true;
		}

	}
}
