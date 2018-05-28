using UnityEngine;
using System.Collections;

public enum InputEnum
{
	Green, Red, Blue, Yellow, Left, Right, Up, Down, AnyMovement, Rotate, R_Rotate, AnyAiming, Boost, Bomb, Fire, Alt_Fire,
	Fire_Hook, Start, Submerge_emerge
}

public class InputEnumConverter
{
	public static bool wasPressed(InputEnum val, PlayerActions actions) {
		switch (val) {
			case InputEnum.Green:
				return actions.Green.WasPressed;
			case InputEnum.Red:
				return actions.Red.WasPressed;
			case InputEnum.Blue:
				return actions.Blue.WasPressed;
			case InputEnum.Yellow:
				return actions.Yellow.WasPressed;
			case InputEnum.Left:
				return actions.Left.WasPressed;
			case InputEnum.Right:
				return actions.Right.WasPressed;
			case InputEnum.Up:
				return actions.Up.WasPressed;
			case InputEnum.Down:
				return actions.Down.WasPressed;
			case InputEnum.AnyMovement:
				return actions.Rotate.X != 0 || actions.Rotate.Y != 0;
			case InputEnum.AnyAiming:
				return actions.R_Rotate.X != 0 || actions.R_Rotate.Y != 0;
			case InputEnum.Boost:
				return actions.Boost.WasPressed;
			case InputEnum.Bomb:
				return actions.Bomb.WasPressed;
			case InputEnum.Fire:
				return actions.Fire.WasPressed;
			case InputEnum.Alt_Fire:
				return actions.Alt_Fire.WasPressed;
			case InputEnum.Fire_Hook:
				return actions.Fire_Hook.WasPressed;
			case InputEnum.Start:
				return actions.Start.WasPressed;
      case InputEnum.Submerge_emerge:
        return actions.Submerge_emerge.WasPressed;

		}

		return false;
	}
}

