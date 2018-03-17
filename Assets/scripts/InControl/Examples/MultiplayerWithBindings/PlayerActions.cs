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
  public PlayerAction L_Up;
  public PlayerAction L_Right;
  public PlayerAction L_Left;
  public PlayerAction L_Down;
  public PlayerAction Start;
  public PlayerAction Pause;
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

    L_Up = CreatePlayerAction("L_Up");
    L_Left = CreatePlayerAction("L_Left");
    L_Right = CreatePlayerAction("L_Right");
    L_Down = CreatePlayerAction("L_Down");
    Rotate = CreateTwoAxisPlayerAction(L_Left, L_Right, L_Down, L_Up);

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
    Pause = CreatePlayerAction("Pause");
		Select = CreatePlayerAction("Select");
	}


	public static PlayerActions CreateWithKeyboardBindings() {

    var actions = new PlayerActions();

    actions = CreateKeyboardUniversalBindings(actions);
    actions = CreateKeyboardMenuBindings(actions);
    actions = CreateKeyboardGameBindings(actions);

    return actions;
	}


  public static PlayerActions CreateWithJoystickBindings()
  {
    var actions = new PlayerActions();

    actions = CreateJoystickUniversalBindings(actions);
    actions = CreateJoystickMenuBindings(actions);
    actions = CreateJoystickGameBindings(actions);

    return actions;
  }

 
  private static PlayerActions CreateKeyboardUniversalBindings(PlayerActions actions)
  {

    actions.Select.AddDefaultBinding(Key.Tab);
    actions.Start.AddDefaultBinding(Key.Return);

    return actions;
  }

  private static PlayerActions CreateKeyboardMenuBindings(PlayerActions actions) {
    actions.Up.AddDefaultBinding(Key.UpArrow);
    actions.Down.AddDefaultBinding(Key.DownArrow);
    actions.Left.AddDefaultBinding(Key.LeftArrow);
    actions.Right.AddDefaultBinding(Key.RightArrow);

    actions.Up.AddDefaultBinding(Key.W);
    actions.Down.AddDefaultBinding(Key.S);
    actions.Left.AddDefaultBinding(Key.A);
    actions.Right.AddDefaultBinding(Key.D);

    actions.Green.AddDefaultBinding(Key.Return);
    actions.Red.AddDefaultBinding(Key.Escape);
    actions.Blue.AddDefaultBinding(Key.E);
    actions.Yellow.AddDefaultBinding(Key.Tab);

    return actions;
  }

  private static PlayerActions CreateKeyboardGameBindings(PlayerActions actions)
  {

    actions.Fire.AddDefaultBinding(Mouse.LeftButton);
    actions.Fire_Hook.AddDefaultBinding(Mouse.RightButton);
    actions.Alt_Fire.AddDefaultBinding(Key.LeftControl);

    actions.Boost.AddDefaultBinding(Key.Shift);
    actions.Bomb.AddDefaultBinding(Key.Space);

    actions.L_Up.AddDefaultBinding(Key.W);
    actions.L_Down.AddDefaultBinding(Key.S);
    actions.L_Left.AddDefaultBinding(Key.A);
    actions.L_Right.AddDefaultBinding(Key.D);

    actions.R_Up.AddDefaultBinding(Mouse.PositiveY);
    actions.R_Down.AddDefaultBinding(Mouse.NegativeY);
    actions.R_Left.AddDefaultBinding(Mouse.NegativeX);
    actions.R_Right.AddDefaultBinding(Mouse.PositiveX);

    actions.Pause.AddDefaultBinding(Key.Escape);

    return actions;
  }
    

  private static PlayerActions CreateJoystickUniversalBindings(PlayerActions actions)
  {

    actions.Start.AddDefaultBinding(InputControlType.Start);
    actions.Start.AddDefaultBinding(InputControlType.Options);
    actions.Start.AddDefaultBinding(InputControlType.Menu);
    actions.Select.AddDefaultBinding(InputControlType.Back);
    actions.Select.AddDefaultBinding(InputControlType.Share);

    return actions;
  }

  private static PlayerActions CreateJoystickMenuBindings(PlayerActions actions)
  {
    actions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
    actions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
    actions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
    actions.Right.AddDefaultBinding(InputControlType.LeftStickRight);

    actions.Up.AddDefaultBinding(InputControlType.DPadUp);
    actions.Down.AddDefaultBinding(InputControlType.DPadDown);
    actions.Left.AddDefaultBinding(InputControlType.DPadLeft);
    actions.Right.AddDefaultBinding(InputControlType.DPadRight);

    actions.Green.AddDefaultBinding(InputControlType.Action1);
    actions.Red.AddDefaultBinding(InputControlType.Action2);
    actions.Blue.AddDefaultBinding(InputControlType.Action3);
    actions.Yellow.AddDefaultBinding(InputControlType.Action4);

    return actions;
  }


  private static PlayerActions CreateJoystickGameBindings(PlayerActions actions)
  {
    actions.Fire.AddDefaultBinding(InputControlType.RightTrigger);
    actions.Alt_Fire.AddDefaultBinding(InputControlType.RightBumper);
    actions.Fire_Hook.AddDefaultBinding(InputControlType.LeftTrigger);
    actions.Boost.AddDefaultBinding(InputControlType.LeftBumper);
    actions.Bomb.AddDefaultBinding(InputControlType.Action2);

    actions.L_Up.AddDefaultBinding(InputControlType.LeftStickUp);
    actions.L_Left.AddDefaultBinding(InputControlType.LeftStickLeft);
    actions.L_Down.AddDefaultBinding(InputControlType.LeftStickDown);
    actions.L_Right.AddDefaultBinding(InputControlType.LeftStickRight);

    actions.R_Up.AddDefaultBinding(InputControlType.RightStickUp);
    actions.R_Down.AddDefaultBinding(InputControlType.RightStickDown);
    actions.R_Left.AddDefaultBinding(InputControlType.RightStickLeft);
    actions.R_Right.AddDefaultBinding(InputControlType.RightStickRight);

    actions.Pause.AddDefaultBinding(InputControlType.Start);
    actions.Pause.AddDefaultBinding(InputControlType.Options);
    actions.Pause.AddDefaultBinding(InputControlType.Menu);

    return actions;
  }


  public static PlayerActions CreateAllControllerBinding()
  {
    var actions = new PlayerActions();

    actions = CreateKeyboardUniversalBindings(actions);
    actions = CreateKeyboardMenuBindings(actions);
    actions = CreateKeyboardGameBindings(actions);

    actions = CreateJoystickUniversalBindings(actions);
    actions = CreateJoystickMenuBindings(actions);
    actions = CreateJoystickGameBindings(actions);

    setListeningOptions (actions);

    return actions;
  }


	static void setListeningOptions(PlayerActions actions) {

    LoadBindings(actions);

    actions.ListenOptions.IncludeUnknownControllers = true;
    actions.ListenOptions.MaxAllowedBindings = 4;
    actions.ListenOptions.MaxAllowedBindingsPerType = 4;
    actions.ListenOptions.AllowDuplicateBindingsPerSet = true;
    actions.ListenOptions.UnsetDuplicateBindingsOnSet = false;
    actions.ListenOptions.IncludeMouseButtons = true;
    actions.ListenOptions.IncludeControllers = false;
    actions.ListenOptions.IncludeKeys = true;

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

    foreach(PlayerAction action in actions.Actions)
    {
      action.RepeatDelay = 0.2f;
      action.FirstRepeatDelay = 0.2f;
      action.Sensitivity = 0.9f;
    }
    actions.Green.FirstRepeatDelay = 0.8f;
    actions.Green.RepeatDelay = 0.8f;
	}
   

	public static void Listen(PlayerAction action) {

    foreach (BindingSource binding in action.Bindings) {
      if (binding.BindingSourceType == BindingSourceType.KeyBindingSource ||
        binding.BindingSourceType == BindingSourceType.MouseBindingSource )
      {
        action.ListenForBindingReplacing (binding);
      }
    }
  }


  public static string GetKeyboardBindingName(PlayerAction action) {
    String name = "";
    foreach (BindingSource binding in action.Bindings) {
      if (binding.BindingSourceType == BindingSourceType.KeyBindingSource ||
        binding.BindingSourceType == BindingSourceType.MouseBindingSource )
      {
        name = binding.Name; 
      }
    }
    return name;
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


