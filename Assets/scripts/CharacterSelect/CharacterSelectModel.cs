using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectModel : MonoBehaviour {

	//Others
    internal static string CSPanelPrefab = "Prefabs/UI/CSPanelPrefab";
    internal static string controllerUnselect = "Sprites/B";
    internal static string keyboardUnselect = "Sprites/ESC";
    internal static string keyboardNext = "Sprites/ENTER";

	public static Dictionary<ShipEnum, string> ShipToImage = new Dictionary<ShipEnum, string>(){
		{ ShipEnum.AtlanteanShip, "character_portraits_assets/angria"},
		{ ShipEnum.BlackbeardShip, "character_portraits_assets/blackbeard"},
		{ ShipEnum.ChineseJunkShip, "character_portraits_assets/gung"},
        { ShipEnum.VikingShip, "character_portraits_assets/viking"},
        { ShipEnum.Kraken, "character_portraits_assets/kraken"} 
	};


	public static Dictionary<ShipEnum, string> ShipToReadyImage = new Dictionary<ShipEnum, string>(){
		{ ShipEnum.AtlanteanShip, "character_portraits_assets/angria_ready"},
		{ ShipEnum.BlackbeardShip, "character_portraits_assets/blackbeard_ready"},
		{ ShipEnum.ChineseJunkShip, "character_portraits_assets/gung_ready"},
        { ShipEnum.VikingShip, "character_portraits_assets/viking_ready"},
        { ShipEnum.Kraken, "character_portraits_assets/kraken_ready"} 
	};


	public static Dictionary<ShipEnum, string> ShipToLockImage = new Dictionary<ShipEnum, string>(){
		{ ShipEnum.AtlanteanShip, "character_portraits_assets/angria_lock"},
		{ ShipEnum.BlackbeardShip, "character_portraits_assets/blackbeard_lock"},
		{ ShipEnum.ChineseJunkShip, "character_portraits_assets/gung_lock"},
        { ShipEnum.VikingShip, "character_portraits_assets/viking_lock"},
        { ShipEnum.Kraken, "character_portraits_assets/kraken_lock"} 
	};
		


	public string getCSPanel() {
		return CSPanelPrefab;
	}

}
