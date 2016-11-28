using System;
using UnityEngine;
using InControl;


public abstract class AbstractInputManager : MonoBehaviour {
 
	public PlayerActions actions { get; set; }
	public Action<Vector3> onRotateChanged { get; set; }
    	public Action<Vector3> onRightRotateChanged { get; set; }
    	public Action onRedButtonPress { get; set; }
    	public Action onLeftBumperDown { get; set; }
	public Action onLeftTriggerDown { get; set; }
	public Action onRightBumperDown { get; set; }
	public Action onRightTriggerDown { get; set; }
}
