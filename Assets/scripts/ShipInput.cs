using System;
using UnityEngine;

public class ShipInput : AbstractInputManager {

    void Update () {
        
		if (actions == null) 
		{
		    return;
		}
        if (onRotateChanged !=null)
        {
		    onRotateChanged(new Vector3(actions.Rotate.X, 0f, actions.Rotate.Y));
        }
        if (onRightRotateChanged != null)
        {
            if(actions.Device == null)
            {
                onRightRotateChanged(Input.mousePosition);
            }
            else {
                onRightRotateChanged(new Vector3(actions.R_Rotate.X, 0f, actions.R_Rotate.Y));
            }
        }

        if (actions.Red.WasPressed && onRedButtonPress != null) {
			onRedButtonPress ();
		}

		if (actions.Boost.WasPressed &&  onLeftBumperDown != null)
		{
		    onLeftBumperDown();
		}
        if (actions.Fire_Hook.WasPressed && onLeftTriggerDown != null)
        {
            onLeftTriggerDown();
        }
		if (actions.Fire.RawValue > .5f && onRightTriggerDown != null)
		{
			onRightTriggerDown();
		}
		if (actions.Alt_Fire.RawValue > .5f && onRightBumperDown != null)
		{
			onRightBumperDown();
		}

        // with controller
    }
}
