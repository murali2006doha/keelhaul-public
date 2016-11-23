using System;
using UnityEngine;

public class ShipInput : AbstractInputManager
{        

    // Use this for initialization
    void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
        
		if (actions == null) {
            return;
        }
        onRotateChanged(new Vector3(actions.Rotate.X, 0f, actions.Rotate.Y));
		onBombPress (actions.Red);
    }
}
