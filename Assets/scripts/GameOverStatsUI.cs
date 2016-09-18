using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverStatsUI : MonoBehaviour {

	[System.Serializable]
	public class Stats{
		public Text name;
		public Text[] titles;
		public Text[] titleStats;
	}
		

	public Text winnerText;
	public Stats[] winners;
	public Stats[] losers;

}
