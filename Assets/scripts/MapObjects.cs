using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapObjects : MonoBehaviour {

	public GameObject[] scoringZones;
	public GameObject[] shipStartingLocations;
	public GameObject krakenStartPoint;
	public List<ShipEnum> enums;
	public Camera gameOverCamera;
	public GameObject winnerLoc;
	public GameObject loser1loc;
	public GameObject loser2loc;
	public IslandManager[] islands;

	public GameObject[] winds;
    public GameObject gameOverStatPrefab;

}
