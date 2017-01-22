﻿using UnityEngine;
using System.Collections;
using InControl;

public class ControllerSelect : MonoBehaviour {

	// Use this for initialization
	public ArrayList players = new ArrayList();
	public bool withKeyboard;
	PlayerActions keyboardListener;
	PlayerActions keyboardListener_1;
	PlayerActions keyboardListener_2;
	PlayerActions joystickListener;
	public bool listening = true;

	void Start () {
        Application.targetFrameRate = -1;
		GameObject.DontDestroyOnLoad (this.gameObject);

	}



	void Awake() {

		GameObject.DontDestroyOnLoad (this.gameObject);

	}

	// Update is called once per frame
	void Update () {
		if (listening && players.Count < 4) {
			if (JoinButtonWasPressedOnListener (joystickListener)) {
				var inputDevice = InputManager.ActiveDevice;
				if (ThereIsNoPlayerUsingJoystick (inputDevice)) {
					AssignListener (inputDevice);
				}
			}

			if (withKeyboard) {
				if (JoinButtonWasPressedOnListener (keyboardListener_1)) {
				
					AssignListener (keyboardListener_1);

				} else if (JoinButtonWasPressedOnListener (keyboardListener_2)) {
				
					AssignListener (keyboardListener_2);

				}
			}
		}


	}

	bool ThereIsNoPlayerUsingJoystick(InputDevice device){
		foreach (PlayerActions action in players) {
			if (action.Device == device) {
				return false;
			}
		}
		return true;
	}

	bool ThereIsNoPlayerUsingKeyboard(PlayerActions action){
		foreach (PlayerActions actions in players) {
			if (actions == action) {
				return false;
			}
		}
		return true;
	}

	void AssignListener(InputDevice inputDevice){
		var playerCount = players.Count;
		var actions = PlayerActions.CreateWithJoystickBindings ();
		actions.Device = inputDevice;
		players.Add (actions);
		print ("Assigned Player " + (playerCount + 1));

	}

	void AssignListener(PlayerActions action){
		var playerCount = players.Count;
		players.Add (action);
		print ("Assigned Player " + (playerCount + 1));

	}
	bool JoinButtonWasPressedOnListener( PlayerActions actions )
	{
		return actions.Green.WasPressed;
	}

	void OnEnable()
	{
		InputManager.OnDeviceDetached += OnDeviceDetached;
		keyboardListener = PlayerActions.CreateWithKeyboardBindings();
		keyboardListener_1 = PlayerActions.CreateWithKeyboardBindings_1();
		keyboardListener_2 = PlayerActions.CreateWithKeyboardBindings_2();
		joystickListener = PlayerActions.CreateWithJoystickBindings();
	}


	void OnDisable()
	{
		InputManager.OnDeviceDetached -= OnDeviceDetached;
		joystickListener.Destroy();
		keyboardListener_1.Destroy ();
		keyboardListener_2.Destroy ();
		keyboardListener.Destroy();
	}

	void OnDeviceDetached( InputDevice inputDevice )
	{
		if (ThereIsNoPlayerUsingJoystick(inputDevice))
		{
			RemovePlayer( inputDevice );
		}
	}

	void RemovePlayer(InputDevice device){

	}
}
