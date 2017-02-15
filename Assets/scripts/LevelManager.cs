using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using InControl;

public class LevelManager : MonoBehaviour {

	public PlayerActions Actions { get; set; }
	public Transform startScreen;
	public Transform mainMenu;
	public Transform selectmode;
	//public Transform optionmenu;
	public Transform characterselectDM;
	public Transform characterselectPlunder;

	//public CharacterSelect player1;
	public Button select;
	public Button options;
	public Button threevone;
	public Button plunder;
	Button[] menuOptions = new Button[2];
	Button[] modeOptions = new Button[2];
	int index = 0;
    public List<Pulsate> pulsaters;
	ControllerSelect cc;


	void Start () {

		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.listening = false;

		menuOptions[0] = select;
		menuOptions[1] = options;
		modeOptions [0] = threevone;
		modeOptions [1] = plunder;

		startScreen.gameObject.SetActive (true); //True
		mainMenu.gameObject.SetActive (false);
		//optionmenu.gameObject.SetActive (false);
		characterselectDM.gameObject.SetActive (false);
		characterselectPlunder.gameObject.SetActive (false);
		selectmode.gameObject.SetActive (false);
	}


	void Update () {

		escape ();
		if (startScreen.gameObject.active == true) {
			if (InputManager.ActiveDevice.Action1.WasReleased || InputManager.AnyKeyIsPressed) {

				selectmode.gameObject.SetActive(true);
                foreach(Pulsate p in pulsaters)
                {
                    p.fadeInDelay = 0;
                    p.fadeOutDelay = 0;
                    p.duration = 0.2f;
                    p.changePulsate();
                    p.Invoke("removePulsate", 1f);
                }
                //Invoke("activateCharSelect", 1.5f);
			}
		}

		if (mainMenu.gameObject.active == true) { //navigation for main menu
			navigateScreen (menuOptions);
		}

		if (selectmode.gameObject.active == true) { //navigation for mode select
			navigateScreen (modeOptions);
		}

	}



    void activateCharSelect() {
		characterselectPlunder.gameObject.SetActive(true);
		//characterselectDM.gameObject.SetActive(true);
        startScreen.gameObject.GetComponent<Fade>().enabled = true;
		startScreen.gameObject.SetActive (false);
    }


	public void escape() { //going back a page
		if (Input.GetKeyDown (KeyCode.Escape) || InputManager.ActiveDevice.Action4.WasPressed) {
			if (mainMenu.gameObject.active == false && startScreen.gameObject.active == true) {
				Application.Quit ();
			} else if (mainMenu.gameObject.active == true) {
				mainMenu.gameObject.SetActive (false);
				startScreen.gameObject.SetActive (true);
				LogAnalyticsUI.MainMenuBackSelected ("startscreen");
			} else if (selectmode.gameObject.active == true) {
				selectmode.gameObject.SetActive (false);
				startScreen.gameObject.SetActive (true);
				LogAnalyticsUI.MainMenuBackSelected ("startscreen");
			} else if (characterselectPlunder.gameObject.active == true) {
				characterselectPlunder.gameObject.SetActive (false);
				selectmode.gameObject.SetActive (true);
				LogAnalyticsUI.MainMenuBackSelected ("selectmode");
			} else if (characterselectDM.gameObject.active == true) {
				characterselectDM.gameObject.SetActive (false);
				selectmode.gameObject.SetActive (true);
				LogAnalyticsUI.MainMenuBackSelected ("selectmode");
			}
		}
	}


	public void selectMode(bool clicked) {	//clicking on select mode
		if (clicked == true) {
			select.Select ();

			selectmode.gameObject.SetActive (clicked);
			mainMenu.gameObject.SetActive (false);
		} else {
			selectmode.gameObject.SetActive (clicked);
			mainMenu.gameObject.SetActive (true);
		}
	}


	public void characterSelectFFA(bool clicked) { //clicking on 3 vs 1
		if (clicked == true) {
			threevone.Select ();

			characterselectDM.gameObject.SetActive (clicked);
			selectmode.gameObject.SetActive (false);

			LogAnalyticsUI.MainMenuGameModeSelected (GameTypeEnum.DeathMatch);
		} else {
			characterselectDM.gameObject.SetActive (clicked);
			selectmode.gameObject.SetActive (true);
		}
	}


	public void characterSelect1v1(bool clicked) { //clicking on 3 vs 1
		if (clicked == true) {
			plunder.Select ();

			characterselectPlunder.gameObject.SetActive (clicked);
			selectmode.gameObject.SetActive (false);

			LogAnalyticsUI.MainMenuGameModeSelected (GameTypeEnum.Sabotage);

		} else {
			characterselectPlunder.gameObject.SetActive (clicked);
			selectmode.gameObject.SetActive (true);
		}
	}


	public void optionMenu(bool clicked) { //clicking on options; no options page for now
		if (clicked == true) {
			//optionmenu.gameObject.SetActive (clicked);
			//mainMenu.gameObject.SetActive (false);
		} else {
			//optionmenu.gameObject.SetActive (clicked);
			//mainMenu.gameObject.SetActive (true);
		}
	}


	public void LoadScene (string name) {
		Application.LoadLevel (name);
	}



	/*-------------------------------------------------------------------------------*/

	//This is all the navigation code used for the Keyboard and controller arrow keys. 

	public int getPositionIndex (Button[] items, int item, string direction) {
		if (direction == "up") {
			if (item == 0) {
				item = items.Length - 1;
			} else {
				item -= 1;
			}
		}

		if (direction == "down") {
			if (item == items.Length - 1) {
				item = 0;
			} else {
				item += 1;
			}
		}

		return item;
	}
		

	public void navigateScreen (Button[] menuScreen) {	//navigating main menu		
		if (Input.GetKeyDown (KeyCode.DownArrow) || InputManager.ActiveDevice.DPadDown.WasReleased) {
			index = getPositionIndex (menuScreen, index, "down");
			menuScreen [index].Select ();
		}

		if (Input.GetKeyDown (KeyCode.UpArrow) || InputManager.ActiveDevice.DPadUp.WasReleased) {
			index = getPositionIndex (menuScreen, index, "up");
			menuScreen [index].Select ();
		}

		if (InputManager.ActiveDevice.Action1.IsPressed) {
			menuScreen [index].onClick.Invoke();
		}
	}
		

}