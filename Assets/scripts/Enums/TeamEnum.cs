using UnityEngine;
using System.Collections;


public enum TeamEnum
{
    Red, Blue, Green, Yellow
}


public class TeamColorHelper
{

    public static TeamEnum GetColor(int val)
    {
        switch (val)
        {
            case 0:
                return TeamEnum.Red;
            case 1:
                return TeamEnum.Blue;
            case 2:
                return TeamEnum.Green;
            case 3:
                return TeamEnum.Yellow;
        }

        return TeamEnum.Red;
    }


    public static int GetValue(TeamEnum color)
    {
        switch (color)
        {
            case TeamEnum.Red:
                return 0;
            case TeamEnum.Blue:
                return 1;
            case TeamEnum.Green:
                return 2;
            case TeamEnum.Yellow:
                return 3;
        }

        return 0;
    }
}