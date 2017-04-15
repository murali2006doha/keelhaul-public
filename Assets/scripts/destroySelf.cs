using UnityEngine;
using System.Collections;

public class destroySelf : MonoBehaviour {

    public float destroyTime;
    public Object toDestory;
	// Use this for initialization
	void Start () {
        Invoke("killSelf", destroyTime);
	}
	
	// Update is called once per frame
	void killSelf () {
        if (toDestory == null)
        {
            Destroy(this.gameObject);
        }
        else {
            Destroy(toDestory);
        }
        
	}
}
