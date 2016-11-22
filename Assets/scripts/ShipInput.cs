using System;
using UnityEngine;
using InControl;


public class ShipInput : MonoBehaviour {


    public PlayerActions actions { get; set; }
    public Action<Vector3> onRotateChanged;
    // Use this for initialization
    void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
        if (actions == null) {
            return;
        }
        onRotateChanged(new Vector3(actions.Rotate.X, 0f, actions.Rotate.Y));
       

    }
}
