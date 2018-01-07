using UnityEngine;
using System;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {
	public Text timer;
	float time;
	public bool done = false;
    Action callBack;
	// Use this for initialization
	void Start () {
		time = Time.realtimeSinceStartup;
	}


    public void AddCallBack(Action callBack)
    {
        this.callBack = callBack;
    }

    public void resetTime()
    {
        done = false;
        time = Time.realtimeSinceStartup;
    }
	
	// Update is called once per frame
	void Update () {
	
		timer.text = Mathf.Ceil (3 -(Time.realtimeSinceStartup - time)).ToString();
		if (Time.realtimeSinceStartup - time > 3 && !done) {
			timer.text = "GO!";
			done = true;
            callBack();

        }

	}
}
