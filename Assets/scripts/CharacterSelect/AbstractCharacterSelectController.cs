using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;

public abstract class AbstractCharacterSelectController : MonoBehaviour {

	public PlayerActions Actions { get; set; }
	public AsyncOperation asyncLoad;
	public bool loadingScene;

	// Use this for initialization
	public bool withKeyboard;
	public Transform start;
	public int numPlayers;
	public List<ShipEnum> characters;
	public GameObject loadingScreen; //put this in mainMenuModel later

	protected Dictionary<string, bool> characterStatuses = new Dictionary<string, bool>();
	protected List<CharacterSelectPanel> players = new List<CharacterSelectPanel>(); //to pass on to 
	protected ControllerSelect cc;
	protected int playersInPlay;
	protected bool started = false;

	// Use this for initialization
	void Start () {
		
		//GameObject.DontDestroyOnLoad (this.gameObject);
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;


		for(int i = 0; i < numPlayers; i++) {
			initializePanel ();
		}	
			
		foreach (ShipEnum character in characters) {
			characterStatuses.Add (character.ToString(), false);
		}
			
		playersInPlay = players.Count;
	}


	public void initializePanel() {
		Object panel = Resources.Load(CharacterSelectModel.CSPanelPrefab, typeof(GameObject));

		GameObject csPanel = Instantiate(panel, GameObject.Find ("Container").transform.position, GameObject.Find ("Container").transform.rotation) as GameObject;
		Vector3 localscale = csPanel.gameObject.transform.localScale;
		csPanel.gameObject.GetComponent<CharacterSelectPanel> ().initializePanel (this, characters, Actions);

		csPanel.gameObject.transform.SetParent(GameObject.Find ("Container").transform);
		csPanel.gameObject.transform.localScale = localscale;


		players.Add (csPanel.gameObject.GetComponent<CharacterSelectPanel>());

	}


	// Update is called once per frame
	void Update () {

		var inputDevice = InputManager.ActiveDevice; //last device to

		if (this.gameObject.activeSelf) {
			cc.listening = true;
			signIn ();
		} else {
			cc.listening = false;
		}

		if (this.gameObject != null) {

			if (playersInPlay == 0) {

				start.gameObject.SetActive (true); //change to next when there is map selection
				if (!started && players.Exists(p => p.Actions.Green.IsPressed)) {
					started = true;
					GameObject.FindObjectOfType<PlayerSelectSettings> ().setPlayerCharacters (players);

					if (!loadingScene) {
						loadingScreen.SetActive(true);  
						StartCoroutine(LoadNewScene());
						loadingScene = true;
					}
				}
			} else {
				start.gameObject.SetActive (false);
			}
		}
	}
		


	public void signIn() {
		var inputDevice = InputManager.ActiveDevice;

		int playerCount = cc.players.Count;
		if (playerCount > 0 && playerCount <= numPlayers) {

			//if (playerCount < players.Count) {
				players [playerCount - 1].Actions = (PlayerActions)cc.players [playerCount - 1];
				if (players [playerCount - 1].Actions.Green.WasReleased) {
					players [playerCount - 1].isSignedIn = true;
					players [playerCount - 1].gameObject.GetComponent<CharacterSelectPanel> ().enabled = true;
				}
			//}
		}
	}


	public abstract bool lockCharacter (int index);

	public abstract bool unlockCharacter (int index);



	/*
	 * cycles through a list 
	 */ 
	public int getIndexPosition (int listSize, int i, string direction) {

		if (direction == "up") {
			if (i == 0) {
				i = listSize - 1;
			} else {
				i -= 1;
			}
		}
		if (direction == "down") {
			if (i == listSize - 1) {
				i = 0;
			} else {
				i += 1;
			}
		}

		return i;
	}



	public List<string> getCharacterKeys() {
		List<string> keys = new List<string> (characterStatuses.Keys);
		return keys;
	}


	IEnumerator LoadNewScene() {
		//To do: move this logic out of playerSignIn, make it more generic
		AsyncOperation async = SceneManager.LoadSceneAsync(GlobalVariables.getMapToLoad());
		while (!async.isDone) {
			yield return null;
		}

	}


	public Dictionary<string, bool> getCharacterStatuses() {
		return characterStatuses;
	}



	public ShipEnum getShipType(int playerNum) {
		
		ShipEnum type = (ShipEnum)System.Enum.Parse (typeof(ShipEnum), players [playerNum].selectedCharacter, true);

		return type;
	}

	public bool isStarted() {
		return started;
	}
		

}
