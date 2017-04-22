using System;
using InControl;


public class PlayerActions : PlayerActionSet
{
    public PlayerAction Green;
    public PlayerAction Boost;
    public PlayerAction Red; 
    public PlayerAction Blue;
    public PlayerAction Yellow;
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Fire;
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


    public PlayerActions()
    {
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
        Fire = CreatePlayerAction("Fire");
        Alt_Fire = CreatePlayerAction("Alt_Fire");
        Fire_Hook = CreatePlayerAction("Fire_Hook");
        Start = CreatePlayerAction("Start");
		Select = CreatePlayerAction ("Select");
    }


    public static PlayerActions CreateWithKeyboardBindings_1()
    {
        var actions = new PlayerActions();

        actions.Up.AddDefaultBinding(Key.UpArrow);
        actions.Down.AddDefaultBinding(Key.DownArrow);
        actions.Left.AddDefaultBinding(Key.LeftArrow);
        actions.Right.AddDefaultBinding(Key.RightArrow);
        actions.Fire.AddDefaultBinding(Key.RightShift);
        actions.Fire_Hook.AddDefaultBinding(Key.M);
        actions.Boost.AddDefaultBinding(Key.X);
        actions.Boost.AddDefaultBinding(Key.Return);
        actions.Green.AddDefaultBinding(Key.Alt);
        actions.Yellow.AddDefaultBinding(Key.Z);

        return actions;
    }

    public static PlayerActions CreateWithKeyboardBindings_2()
    {
        var actions = new PlayerActions();

        actions.Up.AddDefaultBinding(Key.W);
        actions.Down.AddDefaultBinding(Key.S);
        actions.Left.AddDefaultBinding(Key.A);
        actions.Right.AddDefaultBinding(Key.D);

        actions.Fire.AddDefaultBinding(Mouse.LeftButton);
        actions.Fire_Hook.AddDefaultBinding(Mouse.RightButton);
		actions.Green.AddDefaultBinding(Key.R);     //action
		actions.Green.AddDefaultBinding(Key.Return);     //action
        actions.Alt_Fire.AddDefaultBinding(Key.LeftControl);
        actions.Red.AddDefaultBinding(Key.Space);   //bomb
        actions.Boost.AddDefaultBinding(Key.Shift);
        actions.Green.AddDefaultBinding(Key.Alt);
        actions.R_Up.AddDefaultBinding(Mouse.PositiveY);
        actions.R_Down.AddDefaultBinding(Mouse.NegativeY);
        actions.R_Left.AddDefaultBinding(Mouse.NegativeX);
        actions.R_Right.AddDefaultBinding(Mouse.PositiveX);

		actions.Start.AddDefaultBinding(Key.Escape);
        actions.Select.AddDefaultBinding (Key.Tab);

        return actions;
    }

    public static PlayerActions CreateWithKeyboardBindings()
    {
        var actions = new PlayerActions();

        actions.Green.AddDefaultBinding(Key.A);
        actions.Red.AddDefaultBinding(Key.S);
        actions.Blue.AddDefaultBinding(Key.D);
        actions.Yellow.AddDefaultBinding(Key.F);
        actions.Boost.AddDefaultBinding(Key.Return);
        actions.Fire.AddDefaultBinding(Key.RightShift);
        actions.Fire_Hook.AddDefaultBinding(Key.M);
        actions.Boost.AddDefaultBinding(Key.X);
        actions.Up.AddDefaultBinding(Key.UpArrow);
        actions.Down.AddDefaultBinding(Key.DownArrow);
        actions.Left.AddDefaultBinding(Key.LeftArrow);
        actions.Right.AddDefaultBinding(Key.RightArrow);
        actions.Fire.AddDefaultBinding(Mouse.LeftButton);

        return actions;
    }


    public static PlayerActions CreateWithJoystickBindings()
   {
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

        actions.Start.AddDefaultBinding(InputControlType.Start);
        actions.Start.AddDefaultBinding(InputControlType.Options);
        actions.Select.AddDefaultBinding (InputControlType.Select);

        return actions;
    }
}


