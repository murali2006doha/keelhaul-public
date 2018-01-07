using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathVariables {
    
    internal static string soundPrefab = "Prefabs/SoundPrefab";
    internal static string titlesPath = "Prefabs/Titles";
    internal static string krakenPath = "Prefabs/Kraken 1";

    //Cameras
    internal static string topDownCameraPath = "Prefabs/Cameras/TopdownCamera";
    internal static string gameOverCameraPath = "Prefabs/Cameras/GameOverCamera";

    //UIs
    internal static string krakenUIPath = "Prefabs/UI/KrakenUI";
    internal static string shipUIPath = "Prefabs/UI/shipUI";
    internal static string ffaCanvasPath = "Prefabs/UI/Global Canvas FFA";
    internal static string playerStatsPath = "Prefabs/UI/PlayerStatsPanel";

    //Managers
    internal static string sabotageManager = "Prefabs/GameManagers/SabotageGameManager";
    internal static string krakenHuntManager = "Prefabs/GameManagers/KrakenGameManager";
    internal static string deathMatchManager = "Prefabs/GameManagers/DeathMatchGameManager";


    //cannonball
    internal static string atlantisCannonBallPath = "Prefabs/Cannon Shots/atlantis cannonball";
    internal static string chineseCannonBallPath = "Prefabs/Cannon Shots/chinese cannonball";
    internal static string blackbeardCannonBallPath = "Prefabs/Cannon Shots/blackbeard cannonball";
    internal static string vikingCannonBallPath = "Prefabs/Cannon Shots/Viking arrow";


    //Bombs
    internal static string atlantisBombPath = "Prefabs/Bombs/atlantis bomb";
    internal static string chineseBombPath = "Prefabs/Bombs/gung bomb";
    internal static string blackbeardBombPath = "Prefabs/Bombs/atlantis bomb";


    //alternate shots

    internal static string alternateChineseShot = "Prefabs/Alt Cannon shots/Firework Shot";
    internal static string alternateAtlantisShot = "Prefabs/Alt Cannon shots/Force Field";
    internal static string alternateBlackbeardShot = "Prefabs/Cannon Shots/centre blackbeard cannonball";
    internal static string alternateVikingShot = "Prefabs/Alt Cannon shots/Viking Field";

    internal static string explosionPath = "Prefabs/Explosion";
    internal static string bombExplosionPath = "Prefabs/BombExplosion";
    
    //Ship Effects
    internal static string shipInvincibility = "Prefabs/Effects/Star Power";

    //Maps
    internal static string genericMapPath = "Prefabs/Maps/@/";


    //Alternate Textures
    internal static string blackbeardAltTexturePath = "Ship/Alternate_Textures/blackbeard_texture";
    internal static string chineseAltTexturePath = "Ship/Alternate_Textures/junk_with_emit_and_gloss";
    internal static string atlanteanAltTexturePath = "Ship/Alternate_Textures/atlantis_diffuse_and_AO";
    internal static string spike = "Prefabs/Alt Cannon shots/viking_iceberg";

    //ship portrait paths
    internal static string blackbeardPortraitPath = "character_portraits_assets/blackbeardPortrait";
    internal static string chinesePortratPath = "character_portraits_assets/gungPortrait";
    internal static string atlanteanPortraitPath = "character_portraits_assets/atlantisPortrait";
    internal static string vikingPortraitPath = "character_portraits_assets/blackbeardPortrait";

    //ship portrait background paths
    internal static string blackbeardPortraitBackgroundPath = "character_portraits_assets/blackbeardPortraitBackground";
    internal static string chinesePortratBackgroundPath = "character_portraits_assets/gungPortraitBackground";
    internal static string atlanteanPortraitBackgroundPath = "character_portraits_assets/atlantisPortraitBackground";
    internal static string vikingPortraitBackgroundPath = "character_portraits_assets/blackbeardPortraitBackground";


    //keyboard and controller button sprites
    internal static string BBack = "Sprites/B";
    internal static string ESCBack = "Sprites/ESC";
    internal static string ANext = "Sprites/A";
    internal static string EnterNext = "Sprites/ENTER";
    //internal static string AorEnterNext = "Sprites/ENTER";
    //internal static string BorESCBack = "Sprites/ENTER";



    //Networked cs view
    internal static string selectedCharacterView = "networked_character_select/characterView";
    public static string GetAssociatedCannonballForShip(ShipEnum shipType)
    {
        if (shipType == ShipEnum.AtlanteanShip)
        {
            return atlantisCannonBallPath;
        }
        else if (shipType == ShipEnum.BlackbeardShip)
        {
            return blackbeardCannonBallPath;
        }
        else if (shipType == ShipEnum.ChineseJunkShip)
        {
            return chineseCannonBallPath;
        }
        else if(shipType == ShipEnum.VikingShip)
        {
            return vikingCannonBallPath;
        }

        return string.Empty;

    }

    public static string GetMapForMode(GameTypeEnum mode, MapEnum map) {
        var actualMode = mode == GameTypeEnum.KrakenHunt ? GameTypeEnum.Sabotage : mode;
    Debug.Log(actualMode);
        return genericMapPath.Replace("@", actualMode.ToString()) + map.ToString();   
    }

    public static string GetAssociatedBombForShip(ShipEnum shipType)
    {
        if (shipType == ShipEnum.AtlanteanShip)
        {
            return atlantisBombPath;
        }
        else if (shipType == ShipEnum.BlackbeardShip)
        {
            return blackbeardBombPath;
        }
        else if (shipType == ShipEnum.ChineseJunkShip)
        {
            return chineseBombPath;
        }
        else if (shipType == ShipEnum.VikingShip)
        {
            return blackbeardBombPath;
        }

        return string.Empty;

    }

    public static string GetAssociatedTextureSkinPath(ShipEnum shipType,int altSkinNum)
    {
        if (shipType == ShipEnum.AtlanteanShip)
        {
            return atlanteanAltTexturePath + altSkinNum;
        }
        else if (shipType == ShipEnum.BlackbeardShip)
        {
            return blackbeardAltTexturePath + altSkinNum;
        }
        else if (shipType == ShipEnum.ChineseJunkShip)
        {
            return chineseAltTexturePath + altSkinNum;
        }
        else if (shipType == ShipEnum.VikingShip)
        {
            return blackbeardAltTexturePath + altSkinNum;
        }

        return  string.Empty;
    }

    public static string GetAssociatedPortraitPath(ShipEnum shipType)
    {
        if (shipType == ShipEnum.AtlanteanShip)
        {
            return atlanteanPortraitPath;
        }
        else if (shipType == ShipEnum.BlackbeardShip)
        {
            return blackbeardPortraitPath;
        }
        else if (shipType == ShipEnum.ChineseJunkShip)
        {
            return chinesePortratPath;
        }
        else if (shipType == ShipEnum.VikingShip)
        {
            return vikingPortraitPath;
        }

        return string.Empty;
    }

    public static string GetAssociatedPortraitBackgroundPath(ShipEnum shipType)
    {
        if (shipType == ShipEnum.AtlanteanShip)
        {
            return atlanteanPortraitBackgroundPath;
        }
        else if (shipType == ShipEnum.BlackbeardShip)
        {
            return blackbeardPortraitBackgroundPath;
        }
        else if (shipType == ShipEnum.ChineseJunkShip)
        {
            return chinesePortratBackgroundPath;
        }
        else if (shipType == ShipEnum.VikingShip)
        {
            return vikingPortraitBackgroundPath;
        }

        return string.Empty;
    }

}
