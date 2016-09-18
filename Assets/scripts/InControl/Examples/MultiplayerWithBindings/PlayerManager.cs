using System;
using UnityEngine;
using System.Collections.Generic;
using InControl;



	// This example iterates on the basic multiplayer example by using action sets with
	// bindings to support both joystick and keyboard players. It would be a good idea
	// to understand the basic multiplayer example first before looking a this one.
	//
	public class PlayerManager : MonoBehaviour
	{
		public GameObject playerPrefab;
		public  gameManager manager;
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
		PlayerActions keyboardListener_1;
		PlayerActions keyboardListener_2;
		PlayerActions joystickListener;


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
					listening = false;
					manager.assign ();
				}
			}

			if (JoinButtonWasPressedOnListener( keyboardListener_1 ))
			{
				if (ThereIsNoPlayerUsingKeyboard_1())
				{
					AssignListener (keyboardListener_1, null);
					listening = false;
					manager.assign ();
				}
			}
			if (JoinButtonWasPressedOnListener( keyboardListener_2 ))
			{
				if (ThereIsNoPlayerUsingKeyboard_2())
				{
					AssignListener (keyboardListener_2, null);
					listening = false;
					manager.assign ();
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

			if (player.GetComponent<playerInput> () != null) {
				playerInput temp = player.GetComponent<playerInput> ();
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
			if (player.GetComponent<playerInput> () != null) {
				playerInput temp = player.GetComponent<playerInput> ();
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
			return null;
		}

	GameObject FindPlayerUsingKeyboard_1()
		{
			var playerCount = players.Count;
			for (int i = 0; i < playerCount; i++)
			{
				var player = players[i];
			if (player.GetComponent<playerInput> () != null) {
				PlayerActions temp = player.GetComponent<playerInput> ().Actions;
				if (temp == keyboardListener_1) {
					return player;
				}
			} else {
				PlayerActions temp = player.GetComponent<KrakenInput> ().Actions;
				if (temp == keyboardListener_1) {
					return player;
				}
				
			}


			}

			return null;
		}
	GameObject FindPlayerUsingKeyboard_2()
		{
			var playerCount = players.Count;
			for (int i = 0; i < playerCount; i++)
			{
				var player = players[i];
				if (player.GetComponent<playerInput> () != null) {
					PlayerActions temp = player.GetComponent<playerInput> ().Actions;
					if (temp == keyboardListener_1) {
					return player;
					}
				} else {
					PlayerActions temp = player.GetComponent<KrakenInput> ().Actions;
					if (temp == keyboardListener_1) {
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

		bool ThereIsNoPlayerUsingKeyboard_1()
		{
			return FindPlayerUsingKeyboard_1() == null;
		}


		bool ThereIsNoPlayerUsingKeyboard_2()
		{
			return FindPlayerUsingKeyboard_2() == null;
		}

		void OnDeviceDetached( InputDevice inputDevice )
		{
			var player = FindPlayerUsingJoystick( inputDevice );
			if (player != null)
			{
				RemovePlayer( player );
			}
		}


	GameObject CreatePlayer( InputDevice inputDevice )
		{
			if (players.Count < maxPlayers)
			{
				// Pop a position off the list. We'll add it back if the player is removed.
				var playerPosition = playerPositions[0];
				playerPositions.RemoveAt( 0 );

				var gameObject = (GameObject) Instantiate( playerPrefab, playerPosition, Quaternion.identity );
				 
				if (gameObject.GetComponent < playerInput> () != null) {
					var player = gameObject.GetComponent<playerInput>();

					if (inputDevice == null)
					{
						// We could create a new instance, but might as well reuse the one we have
						// and it lets us easily find the keyboard player.
						player.Actions = keyboardListener;
					}
					else
					{
						// Create a new instance and specifically set it to listen to the
						// given input device (joystick).
						var actions = PlayerActions.CreateWithJoystickBindings();
						actions.Device = inputDevice;

						player.Actions = actions;
					}

				players.Add( gameObject );

				return gameObject;
				}
				else  {

					var player = gameObject.GetComponent<KrakenInput>();

					if (inputDevice == null)
					{
						// We could create a new instance, but might as well reuse the one we have
						// and it lets us easily find the keyboard player.
						player.Actions = keyboardListener;
					}
					else
					{
						// Create a new instance and specifically set it to listen to the
						// given input device (joystick).
						var actions = PlayerActions.CreateWithJoystickBindings();
						actions.Device = inputDevice;

						player.Actions = actions;
					}

					players.Add( gameObject );

				return gameObject;
				}
				

			}

			return null;
		}


	void RemovePlayer( GameObject player )
		{
			playerPositions.Insert( 0, player.transform.position );
			players.Remove( player );
		if (player.GetComponent<playerInput> () != null)
			player.GetComponent<playerInput> ().Actions = null;
		else {
			player.GetComponent<KrakenInput> ().Actions = null;
		}
			Destroy( player.gameObject );
		}


		void OnGUI()
		{
		
		}
	}
