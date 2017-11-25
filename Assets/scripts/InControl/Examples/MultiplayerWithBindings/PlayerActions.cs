using System;
using InControl;
using UnityEngine;

public class PlayerActions : PlayerActionSet
{
	static string saveData;
	public PlayerAction Green;
	public PlayerAction Red;
	public PlayerAction Blue;
	public PlayerAction Yellow;
	public PlayerAction Boost;
	public PlayerAction Left;
	public PlayerAction Right;
	public PlayerAction Fire;
	public PlayerAction Bomb;
	public PlayerAction Alt_Fire;
	public PlayerAction Fire_Hook;
	public PlayerAction Up;
	public PlayerAction Down;
	public PlayerAction R_Up;
	public PlayerAction R_Down;
	public PlayerAction R_Left;
	public PlayerAction R_Right;
	public PlayerAction Start;
	public PlayerAction Select;

	public PlayerTwoAxisAction Rotate;   //left analog stick
	public PlayerTwoAxisAction R_Rotate;    //right analog stick


	public PlayerActions() {
		Green = CreatePlayerAction("Green");
		Red = CreatePlayerAction("Red");
		Blue = CreatePlayerAction("Blue");
		Yellow = CreatePlayerAction("Yellow");
		Left = CreatePlayerAction("Left");
		Right = CreatePlayerAction("Right");
		Up = CreatePlayerAction("Up");
		Down = CreatePlayerAction("Down");
		Rotate = CreateTwoAxisPlayerAction(Left, Right, Down, Up);

		R_Left = CreatePlayerAction("R_Left");
		R_Right = CreatePlayerAction("R_Right");
		R_Up = CreatePlayerAction("R_Up");
		R_Down = CreatePlayerAction("R_Down");
		R_Rotate = CreateTwoAxisPlayerAction(R_Left, R_Right, R_Down, R_Up);
		Boost = CreatePlayerAction("Boost");
		Bomb = CreatePlayerAction("Bomb");
		Fire = CreatePlayerAction("Fire");
		Alt_Fire = CreatePlayerAction("Alt_Fire");
		Fire_Hook = CreatePlayerAction("Fire_Hook");
		Start = CreatePlayerAction("Start");
		Select = CreatePlayerAction("Select");
	}


	public static PlayerActions CreateWithKeyboardBindings_1() {
		var actions = new PlayerActions();

		actions.Up.AddDefaultBinding(Key.UpArrow);
		actions.Down.AddDefaultBinding(Key.DownArrow);
		actions.Left.AddDefaultBinding(Key.LeftArrow);
		actions.Right.AddDefaultBinding(Key.RightArrow);
		actions.Fire.AddDefaultBinding(Key.RightShift);
		actions.Fire_Hook.AddDefaultBinding(Key.M);
		actions.Boost.AddDefaultBinding(Key.X);
		actions.Boost.AddDefaultBinding(Key.Return);
		actions.Bomb.AddDefaultBinding(Key.Space);
		actions.Green.AddDefaultBinding(Key.Alt);
		actions.Yellow.AddDefaultBinding(Key.Z);

		return actions;
	}

	public static PlayerActions CreateWithKeyboardBindings_2() {
		var actions = new PlayerActions();

		actions.Up.AddDefaultBinding(Key.W);
		actions.Down.AddDefaultBinding(Key.S);
		actions.Left.AddDefaultBinding(Key.A);
		actions.Right.AddDefaultBinding(Key.D);

		actions.Fire.AddDefaultBinding(Mouse.LeftButton);
		actions.Fire_Hook.AddDefaultBinding(Mouse.RightButton);
		//actions.Green.AddDefaultBinding(Key.R);
		actions.Green.AddDefaultBinding(Key.Return);
		actions.Alt_Fire.AddDefaultBinding(Key.LeftControl);
		actions.Red.AddDefaultBinding(Key.Space);
		actions.Boost.AddDefaultBinding(Key.Shift);
		actions.Bomb.AddDefaultBinding(Key.Space);
		actions.Green.AddDefaultBinding(Key.Alt);
		actions.R_Up.AddDefaultBinding(Mouse.PositiveY);
		actions.R_Down.AddDefaultBinding(Mouse.NegativeY);
		actions.R_Left.AddDefaultBinding(Mouse.NegativeX);
		actions.R_Right.AddDefaultBinding(Mouse.PositiveX);

		actions.Start.AddDefaultBinding(Key.Escape);
		actions.Select.AddDefaultBinding(Key.Tab);

        setListeningOptionsForKeyboard(actions);
		
        return actions;
	}

	public static PlayerActions CreateWithKeyboardBindings() {
		var actions = new PlayerActions();

		actions.Green.AddDefaultBinding(Key.A);
		actions.Red.AddDefaultBinding(Key.S);
		actions.Blue.AddDefaultBinding(Key.D);
		actions.Yellow.AddDefaultBinding(Key.F);
		actions.Boost.AddDefaultBinding(Key.Return);
		actions.Fire.AddDefaultBinding(Key.RightShift);
		actions.Fire_Hook.AddDefaultBinding(Key.M);
		actions.Boost.AddDefaultBinding(Key.X);
		actions.Bomb.AddDefaultBinding(Key.Space);
		actions.Up.AddDefaultBinding(Key.UpArrow);
		actions.Down.AddDefaultBinding(Key.DownArrow);
		actions.Left.AddDefaultBinding(Key.LeftArrow);
		actions.Right.AddDefaultBinding(Key.RightArrow);
		actions.Fire.AddDefaultBinding(Mouse.LeftButton);

		return actions;
	}

    public static PlayerActions CreateAllControllerBinding()
    {
        var actions = new PlayerActions();

        actions.Up.AddDefaultBinding(Key.W);
        actions.Down.AddDefaultBinding(Key.S);
        actions.Left.AddDefaultBinding(Key.A);
        actions.Right.AddDefaultBinding(Key.D);

        actions.Up.AddDefaultBinding(Key.UpArrow);
        actions.Down.AddDefaultBinding(Key.DownArrow);
        actions.Left.AddDefaultBinding(Key.LeftArrow);
        actions.Right.AddDefaultBinding(Key.RightArrow);

        actions.Fire.AddDefaultBinding(Mouse.LeftButton);
        actions.Fire_Hook.AddDefaultBinding(Mouse.RightButton);
        actions.Green.AddDefaultBinding(Key.Return);
        actions.Alt_Fire.AddDefaultBinding(Key.LeftControl);
        actions.Red.AddDefaultBinding(Key.Space);
        actions.Boost.AddDefaultBinding(Key.Shift);
        actions.Bomb.AddDefaultBinding(Key.Space);
        actions.Green.AddDefaultBinding(Key.Alt);
        actions.R_Up.AddDefaultBinding(Mouse.PositiveY);
        actions.R_Down.AddDefaultBinding(Mouse.NegativeY);
        actions.R_Left.AddDefaultBinding(Mouse.NegativeX);
        actions.R_Right.AddDefaultBinding(Mouse.PositiveX);

        actions.Start.AddDefaultBinding(Key.Escape);
        actions.Select.AddDefaultBinding(Key.Tab);


        actions.Green.AddDefaultBinding(InputControlType.Action1);
        actions.Red.AddDefaultBinding(InputControlType.Action2);
        actions.Blue.AddDefaultBinding(InputControlType.Action3);
        actions.Yellow.AddDefaultBinding(InputControlType.Action4);

        actions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        actions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
        actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        actions.Right.AddDefaultBinding(InputControlType.LeftStickRight);

        actions.R_Up.AddDefaultBinding(InputControlType.RightStickUp);
        actions.R_Down.AddDefaultBinding(InputControlType.RightStickDown);
        actions.R_Left.AddDefaultBinding(InputControlType.RightStickLeft);
        actions.R_Right.AddDefaultBinding(InputControlType.RightStickRight);

        actions.Up.AddDefaultBinding(InputControlType.DPadUp);
        actions.Down.AddDefaultBinding(InputControlType.DPadDown);
        actions.Left.AddDefaultBinding(InputControlType.DPadLeft);
        actions.Right.AddDefaultBinding(InputControlType.DPadRight);

        actions.Fire.AddDefaultBinding(InputControlType.RightTrigger);
        actions.Alt_Fire.AddDefaultBinding(InputControlType.RightBumper);
        actions.Fire_Hook.AddDefaultBinding(InputControlType.LeftTrigger);
        actions.Boost.AddDefaultBinding(InputControlType.LeftBumper);
        actions.Bomb.AddDefaultBinding(InputControlType.Action2);

        actions.Start.AddDefaultBinding(InputControlType.Start);
        actions.Start.AddDefaultBinding(InputControlType.Options);
        actions.Select.AddDefaultBinding(InputControlType.Select);

        actions.ListenOptions.IncludeUnknownControllers = true;
        actions.ListenOptions.MaxAllowedBindings = 4;
        actions.ListenOptions.MaxAllowedBindingsPerType = 4;
        actions.ListenOptions.AllowDuplicateBindingsPerSet = true;
        actions.ListenOptions.UnsetDuplicateBindingsOnSet = false;
        actions.ListenOptions.IncludeMouseButtons = true;
        
        foreach(PlayerAction action in actions.Actions)
        {
            action.RepeatDelay = 0.2f;
            action.FirstRepeatDelay = 0.2f;
            action.Sensitivity = 0.9f;
        }
        actions.Green.FirstRepeatDelay = 0.8f;
        actions.Green.RepeatDelay = 0.8f;

        return actions;
    }


    public static PlayerActions CreateWithJoystickBindings() {
		var actions = new PlayerActions();

		actions.Green.AddDefaultBinding(InputControlType.Action1);
		actions.Red.AddDefaultBinding(InputControlType.Action2);
		actions.Blue.AddDefaultBinding(InputControlType.Action3);
		actions.Yellow.AddDefaultBinding(InputControlType.Action4);

		actions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
		actions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
		actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
		actions.Right.AddDefaultBinding(InputControlType.LeftStickRight);

		actions.R_Up.AddDefaultBinding(InputControlType.RightStickUp);
		actions.R_Down.AddDefaultBinding(InputControlType.RightStickDown);
		actions.R_Left.AddDefaultBinding(InputControlType.RightStickLeft);
		actions.R_Right.AddDefaultBinding(InputControlType.RightStickRight);

		actions.Up.AddDefaultBinding(InputControlType.DPadUp);
		actions.Down.AddDefaultBinding(InputControlType.DPadDown);
		actions.Left.AddDefaultBinding(InputControlType.DPadLeft);
		actions.Right.AddDefaultBinding(InputControlType.DPadRight);

		actions.Fire.AddDefaultBinding(InputControlType.RightTrigger);
		actions.Alt_Fire.AddDefaultBinding(InputControlType.RightBumper);
		actions.Fire_Hook.AddDefaultBinding(InputControlType.LeftTrigger);
		actions.Boost.AddDefaultBinding(InputControlType.LeftBumper);
		actions.Bomb.AddDefaultBinding(InputControlType.Action2);

		actions.Start.AddDefaultBinding(InputControlType.Start);
		actions.Start.AddDefaultBinding(InputControlType.Options);
		actions.Select.AddDefaultBinding(InputControlType.Back);
        actions.Select.AddDefaultBinding(InputControlType.Share);

     

        return actions;
	}



	static void setListeningOptionsForKeyboard(PlayerActions actions) {

		LoadBindings(actions);

		actions.ListenOptions.IncludeUnknownControllers = true;
		actions.ListenOptions.MaxAllowedBindings = 4;
		actions.ListenOptions.MaxAllowedBindingsPerType = 4;
		actions.ListenOptions.AllowDuplicateBindingsPerSet = true;
		actions.ListenOptions.UnsetDuplicateBindingsOnSet = false;
		actions.ListenOptions.IncludeMouseButtons = true;

		actions.ListenOptions.OnBindingFound = (action, binding) => {
			if (binding == new KeyBindingSource(Key.Escape)) {
				action.StopListeningForBinding();
		return false;
			}
			return true;
		};

		actions.ListenOptions.OnBindingAdded += (action, binding) => {
			Debug.Log("Binding added... " + binding.DeviceName + ": " + binding.Name);
		};

		actions.ListenOptions.OnBindingRejected += (action, binding, reason) => {
			Debug.Log("Binding rejected... " + reason);
		};
	}



    	public static void Listen(PlayerAction action) {
            foreach (BindingSource binding in action.Bindings) {
                action.ListenForBindingReplacing(binding);
            }
    	}


    public static string GetName(PlayerAction action) {
        String name = "";
        foreach (BindingSource binding in action.Bindings) {
            name = binding.Name;
        }
        return action.Bindings[0].Name;
    }


    public static bool IsBindingDone(PlayerAction action) {
        return !action.IsListeningForBinding;
    }


	public static void SaveBindings(PlayerActions actions) {
		saveData = actions.Save();
		PlayerPrefs.SetString("Bindings", saveData);
	}


	public static void LoadBindings(PlayerActions actions) {
		if (PlayerPrefs.HasKey("Bindings")) {
			saveData = PlayerPrefs.GetString("Bindings");
			actions.Load(saveData);
		}
	}
	//resets to controls set in default
	public static void Reset(PlayerActions actions) {
		actions.Reset();
        SaveBindings(actions);
	}
}


