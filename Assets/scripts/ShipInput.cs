using System;
using UnityEngine;

public class ShipInput : AbstractInputManager {

    void Update () {
        
		if (actions == null) 
		{
		    return;
		}

		onRotateChanged(new Vector3(actions.Rotate.X, 0f, actions.Rotate.Y));
        onRightRotateChanged(new Vector3(actions.R_Rotate.X, 0f, actions.R_Rotate.Y));


        if (actions.Red.WasPressed) {
			onRedButtonPress ();
		}

		if (actions.Boost.WasPressed)
		{
		    onLeftBumperDown();
		}
        if (actions.Fire_Hook.WasPressed)
        {
            onLeftTriggerDown();
        }
		if (actions.Fire.RawValue > .5f)
		{
			onRightTriggerDown();
		}
		if (actions.Alt_Fire.RawValue > .5f)
		{
			onRightBumperDown();
		}

        // with controller
    }
}
