using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Analytics;

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
    CanvasGroup canvasGroup;
    public float fadeSpeed = 0.5f;
    public bool startFading = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();


//		Analytics.CustomEvent("Match Ended", new Dictionary<string, object>
//			{
//				{ "Mode", gameType.ToString() },
//				{ "Map", map.ToString() },
//				{ "Local", PhotonNetwork.offlineMode },
//				{ "Characters", "ASD"},//Dictionary<players[0]., players[0].ToString()> },
//				{ "time_to_match", globalCanvas.countDownTimer}
//			});
    }

    void Update()
    {
        if(canvasGroup.alpha < 1 && startFading)
        {
            canvasGroup.alpha += fadeSpeed * Time.deltaTime * GlobalVariables.gameSpeed;
          
        }
    }

}
