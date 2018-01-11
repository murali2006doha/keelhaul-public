using System;
using UnityEngine;
using System.Collections.Generic;
using InControl;



	// This example iterates on the basic multiplayer example by using action sets with
	// bindings to support both joystick and keyboard players. It would be a good idea
	// to understand the basic multiplayer example first before looking a this one.
	//
	public class NetworkedPlayerManager : MonoBehaviour
	{
	
	//	public  gameManager manager;
		const int maxPlayers = 4;
		public bool listening = false;
		List<Vector3> playerPositions = new List<Vector3>() {
			new Vector3( -1, 1, -10 ),
			new Vector3( 1, 1, -10 ),
			new Vector3( -1, -1, -10 ),
			new Vector3( 1, -1, -10 ),
		};

	public List<GameObject> players = new List<GameObject>( maxPlayers );
		PlayerActions keyboardListener;
		PlayerActions joystickListener;


		void OnEnable()
		{
			InputManager.OnDeviceDetached += OnDeviceDetached;
			keyboardListener = PlayerActions.CreateWithKeyboardBindings();
			joystickListener = PlayerActions.CreateWithJoystickBindings();
		}


		void OnDisable()
		{
			InputManager.OnDeviceDetached -= OnDeviceDetached;
			joystickListener.Destroy();
			keyboardListener.Destroy();
		}


		void Update()
		{
		if (listening) {
			if (JoinButtonWasPressedOnListener( joystickListener ))
			{
				var inputDevice = InputManager.ActiveDevice;

				if (ThereIsNoPlayerUsingJoystick( inputDevice ))
				{
					AssignListener (null, inputDevice);
					//listening = false;
				//	manager.assign ();
				}
			}

			if (JoinButtonWasPressedOnListener( keyboardListener ))
			{
				if (ThereIsNoPlayerUsingKeyboard())
				{
					AssignListener (keyboardListener, null);
					//listening = false;
					//manager.assign ();
				}
			}

		}
			

	}

		bool JoinButtonWasPressedOnListener( PlayerActions actions )
		{
		return actions.Green.WasPressed;
		}


	GameObject FindPlayerUsingJoystick( InputDevice inputDevice )
		{
			var playerCount = players.Count;
			for (int i = 0; i < playerCount; i++)
			{
				var player = players[i];

			if (player.GetComponent<PlayerInput> () != null) {
				PlayerInput temp = player.GetComponent<PlayerInput> ();
				if (temp.Actions != null && temp.Actions.Device == inputDevice)
				{
					return player;
				}
			} 
			else {
				KrakenInput temp = player.GetComponent<KrakenInput> ();
				if (temp.Actions != null && temp.Actions.Device == inputDevice)
				{
					return player;
				}
			}
		
			}

			return null;
		}


		bool ThereIsNoPlayerUsingJoystick( InputDevice inputDevice )
		{
			return FindPlayerUsingJoystick( inputDevice ) == null;
		}

		void AssignListener(PlayerActions listener, InputDevice inputDevice){
			var playerCount = players.Count;
			for (int i = 0; i < playerCount; i++)
			{
				var player = players[i];
			if (player.GetComponent<PlayerInput> () != null) {
				PlayerInput temp = player.GetComponent<PlayerInput> ();
				if (temp.Actions == null && inputDevice != null) {
					var actions = PlayerActions.CreateWithJoystickBindings ();
					actions.Device = inputDevice;

					temp.Actions = actions;
					temp.joinVibrate ();
					break;
				} 
				else if (temp.Actions == null && inputDevice == null) {
					temp.Actions = listener;
					break;

				}
			} 
			else {
				KrakenInput temp = player.GetComponent<KrakenInput> ();
				if (temp.Actions == null && inputDevice != null) {
					var actions = PlayerActions.CreateWithJoystickBindings ();
					actions.Device = inputDevice;

					temp.Actions = actions;
					temp.joinVibrate ();
					break;
				} 
				else if (temp.Actions == null && inputDevice == null) {
					temp.Actions = listener;
					break;

				}
			}
				
			
			}
		}


	GameObject FindPlayerUsingKeyboard()
		{
			var playerCount = players.Count;
			for (int i = 0; i < playerCount; i++)
			{
				var player = players[i];
			if (player.GetComponent<PlayerInput> () != null) {
				PlayerActions temp = player.GetComponent<PlayerInput> ().Actions;
				if (temp == keyboardListener) {
					return player;
				}
			} else {
				PlayerActions temp = player.GetComponent<KrakenInput> ().Actions;
				if (temp == keyboardListener) {
					return player;
				}
				
			}


			}

			return null;
		}


		bool ThereIsNoPlayerUsingKeyboard()
		{
			return FindPlayerUsingKeyboard() == null;
		}


		void OnDeviceDetached( InputDevice inputDevice )
		{
			var player = FindPlayerUsingJoystick( inputDevice );
			if (player != null)
			{
				RemovePlayer( player );
			}
		}


	void RemovePlayer( GameObject player )
		{
			playerPositions.Insert( 0, player.transform.position );
			players.Remove( player );
		if (player.GetComponent<PlayerInput> () != null)
			player.GetComponent<PlayerInput> ().Actions = null;
		else {
			player.GetComponent<KrakenInput> ().Actions = null;
		}
			Destroy( player.gameObject );
		}


		void OnGUI()
		{
		
		}
	}
