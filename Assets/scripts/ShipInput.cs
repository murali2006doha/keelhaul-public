using System;
using UnityEngine;

public class ShipInput : AbstractInputManager {        
	
	void Update () {
        
		if (actions == null) 
		{
		    return;
		}

		onRotateChanged(new Vector3(actions.Rotate.X, 0f, actions.Rotate.Y));
		onRedButtonPress (actions.Red);

		if (actions.Boost.WasPressed)
		{
		    onLeftBumperDown();
		}
    	}
}
