using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

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
        //canvasGroup = GetComponent<CanvasGroup>();

    }

    void Update()
    {
        //if(canvasGroup.alpha < 1 && startFading)
        //{
        //    canvasGroup.alpha += fadeSpeed * Time.deltaTime * GlobalVariables.gameSpeed;
          
        //}
    }

    internal void DisableExtraLosers(int count)
    {
        for(int x = count; x < losers.Length; x++)
        {
            losers[x].name.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
