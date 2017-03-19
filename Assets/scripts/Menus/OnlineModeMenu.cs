using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineModeMenu : AbstractMenu {

	public ActionButton deathmatch;
	public ActionButton sabotage;
	//public SabotageGameTypeMenu sabotageGameTypeMenu; 

	// Use this for initialization
	void Start () {

		setButtonsToActions ();

		actionButtons.Add (deathmatch);
		//actionButtons.Add (sabotage);

		sabotage.ButtonComponent.interactable = false;
	}

	public override void setButtonsToActions (){

		deathmatch.SetAction (() => {
			ToggleButtons();
			FindObjectOfType<GameModeSelectSettings>().setGameType(GameTypeEnum.DeathMatch);
			SceneManager.LoadScene("Game");
			//DeathmatchGameTypeMenu.initialize(GameTypeEnum.DeathMatch, isOnline, () => {this.gameObject.SetActive (true);
		});

		sabotage.SetAction (() => {
			ToggleButtons();
			//sabotageGameTypeMenu.initialize(GameTypeEnum.Sabotage, isOnline, () => {this.gameObject.SetActive (true);
		});
	}


}
