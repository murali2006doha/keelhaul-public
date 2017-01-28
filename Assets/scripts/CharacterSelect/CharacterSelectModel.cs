using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectModel : MonoBehaviour {

	//Others
	internal static string pressA = "sprites/character_portraits_assets/press_A";
	internal static string pressB = "sprites/Character Select UI/B";
	internal static string deselect = "sprites/Character Select UI/deselect";
	internal static string CSPanelPrefab = "Prefabs/UI/CSPanelPrefab";

	public static Dictionary<ShipEnum, string> ShipToImage = new Dictionary<ShipEnum, string>(){
		{ ShipEnum.AtlanteanShip, "character_portraits_assets/angria"},
		{ ShipEnum.BlackbeardShip, "character_portraits_assets/blackbeard"},
		{ ShipEnum.ChineseJunkShip, "character_portraits_assets/gung"},
		{ ShipEnum.Kraken, "character_portraits_assets/kraken"} 
	};


	public static Dictionary<ShipEnum, string> ShipToReadyImage = new Dictionary<ShipEnum, string>(){
		{ ShipEnum.AtlanteanShip, "character_portraits_assets/angria_ready"},
		{ ShipEnum.BlackbeardShip, "character_portraits_assets/blackbeard_ready"},
		{ ShipEnum.ChineseJunkShip, "character_portraits_assets/gung_ready"},
		{ ShipEnum.Kraken, "character_portraits_assets/kraken_ready"} 
	};


	public static Dictionary<ShipEnum, string> ShipToLockImage = new Dictionary<ShipEnum, string>(){
		{ ShipEnum.AtlanteanShip, "character_portraits_assets/angria_lock"},
		{ ShipEnum.BlackbeardShip, "character_portraits_assets/blackbeard_lock"},
		{ ShipEnum.ChineseJunkShip, "character_portraits_assets/gung_lock"},
		{ ShipEnum.Kraken, "character_portraits_assets/kraken_lock"} 
	};
		


	public string getCSPanel() {
		return CSPanelPrefab;
	}

}
