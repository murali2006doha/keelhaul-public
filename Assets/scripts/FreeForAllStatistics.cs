using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FreeForAllStatistics {
	
	public float healthLost;//Done
	public int hookshotNum;//Done
	public int hookshotMisses;//Done
	public int numOfShots;//Done
	public int numOfShotHits;//Done
	public int numOfAlternateShots;//Done
	public int numOfReflectedShots;//Done
	public float damageGivenToChinese;//Done
	public float damageTakenFromChinese;//Done
	public float damageGivenToBlackBeard;//Done
	public float damageTakenFromBlackBeard;//Done
	public float damageGivenToAtlantis;//Done
	public float damageTakenFromAtlantis;//Done
	public float damageTakenFromKraken;//Done
	public float damageGivenToKraken;//Done
	public int numOfBarrelSteals;//Done
	public int numOfBarrelsLost;//Done
	public int numOfTimesSubmergedByKraken;//Done
	public float timeSpentCarryingBarrel;//Done
	public float numOfBombsPlanted;//Done
	public float numOfBombsDetonated;//Done
	public float timeSpentSubmerged;//Done
	public float timeSpentUnderShips;//Done
	public int numOfDeaths;//Done
	public int numOfKills;//Done
	public int numOfBoosts;//Done
	public int numOfKrakenSmash;//Done

    [System.NonSerialized]
    public List<Title> titles = new List<Title>();

    
	public void addGivenDamage(string type, float damage){
		if (type == ShipEnum.AtlanteanShip.ToString ()) {
			damageGivenToAtlantis += damage;
		} else if (type == ShipEnum.ChineseJunkShip.ToString ()) {
			damageGivenToChinese += damage;
		} else if (type == ShipEnum.BlackbeardShip.ToString ()) {
			damageGivenToBlackBeard += damage;
		} else {
			damageGivenToKraken += damage;
		}
	}

	public void addTakenDamage(string type, float damage){
		if (type == ShipEnum.AtlanteanShip.ToString ()) {
			damageTakenFromAtlantis += damage;
		} else if (type == ShipEnum.ChineseJunkShip.ToString ()) {
			damageTakenFromChinese += damage;
		} else if (type == ShipEnum.BlackbeardShip.ToString ()) {
			damageTakenFromBlackBeard += damage;
		} else {
			damageTakenFromKraken += damage;
		}
	}

	public void getTitlesByWeight(){
		
	}
}
