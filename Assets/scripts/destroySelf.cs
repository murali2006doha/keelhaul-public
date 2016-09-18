using UnityEngine;
using System.Collections;

public class destroySelf : MonoBehaviour {

    public float destroyTime;
	// Use this for initialization
	void Start () {
        Invoke("killSelf", destroyTime);
	}
	
	// Update is called once per frame
	void killSelf () {
        Destroy(this.gameObject);
	}
}
