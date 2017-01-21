using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathVariables {
    
    internal static string titlesPath = "Prefabs/Titles";
    internal static string krakenPath = "Prefabs/Kraken 1";

    //Cameras
    internal static string topDownCameraPath = "Prefabs/Cameras/TopdownCamera";
    internal static string gameOverCameraPath = "Prefabs/Cameras/GameOverCamera";

    //UIs
    internal static string krakenUIPath = "Prefabs/UI/KrakenUI";
    internal static string shipUIPath = "Prefabs/UI/shipUI";
    internal static string ffaCanvasPath = "Prefabs/UI/Global Canvas FFA";

    //Managers
    internal static string sabotageManager = "Prefabs/GameManagers/SabotageGameManager";
    internal static string krakenHuntManager = "Prefabs/GameManagers/KrakenGameManager";
    internal static string deathMatchManager = "Prefabs/GameManagers/DeathMatchGameManager";


    //cannonball
    internal static string cannonBallPath = "Prefabs/Cannon Shots/team cannonball";
    internal static string atlantisCannonBallPath = "Prefabs/Cannon Shots/atlantis cannonball";
    internal static string chineseCannonBallPath = "Prefabs/Cannon Shots/chinese cannonball";
    internal static string blackbeardCannonBallPath = "Prefabs/Cannon Shots/blackbeard cannonball";


    //alternate shots

    internal static string alternateChineseShot = "Prefabs/Alt Cannon shots/Firework Shot";
    internal static string alternateAtlantisShot = "Prefabs/Alt Cannon shots/Firework Shot";
    internal static string alternateBlackbeardShot = "Prefabs/Alt Cannon shots/Firework Shot";

    //Ship Effects
    internal static string shipInvincibility = "Prefabs/Effects/Star Power";


    public static string GetAssociatedCannonballForShip(ShipEnum shipType) {
        if (shipType == ShipEnum.AtlanteanShip)
        {
            return atlantisCannonBallPath;
        }
        else if (shipType == ShipEnum.BlackbeardShip)
        {
            return blackbeardCannonBallPath;
        }
        else if (shipType == ShipEnum.ChineseJunkShip) {
            return chineseCannonBallPath;
        }

        return string.Empty;
    }
}
