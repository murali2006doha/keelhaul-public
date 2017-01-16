using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;

public class AbstractCharacterSelectController : MonoBehaviour {



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


	void initializePanel() {
		Object panel = Resources.Load(CharacterSelectModel.CSPanelPrefab, typeof(GameObject));
		GameObject csPanel = Instantiate(panel, GameObject.Find ("Container").transform.position, GameObject.Find ("Container").transform.rotation) as GameObject;
		Vector3 localscale = csPanel.transform.localScale;
		csPanel.transform.SetParent(GameObject.Find ("Container").transform);
		csPanel.transform.position = GameObject.Find ("Container").transform.position;
		csPanel.transform.rotation = GameObject.Find ("Container").transform.rotation;
		csPanel.transform.localScale = localscale;

		csPanel.gameObject.GetComponent<CharacterSelectPanel> ().setCharacterList (characters);

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

					if (!loadingScene)
					{
						loadingScreen.SetActive(true);  
						StartCoroutine(LoadNewScene());
						loadingScene = true;
					}

					//this.gameObject.SetActive(false);
					//mapSelect.gameObject.SetActive(true);
				}
			} else {
				start.gameObject.SetActive (false);
			}
		}
	}


	public void signIn() {
		var inputDevice = InputManager.ActiveDevice;

		if (cc.players.Count == 1) {
			players [0].Actions = (PlayerActions)cc.players [0];
			if (players [0].Actions.Green.WasReleased) {
				players [0].isSignedIn = true;
				players [0].gameObject.GetComponent<CharacterSelectPanel> ().enabled = true;

			}
		} else if (cc.players.Count == 2) {
			players [1].Actions = (PlayerActions)cc.players [1];
			if (players [1].Actions.Green.WasReleased) {
				players [1].isSignedIn = true;
				players [1].gameObject.GetComponent<CharacterSelectPanel> ().enabled = true;

			}
		} else if (players.Count > 2) {
			if (cc.players.Count == 3) {
				players [2].Actions = (PlayerActions)cc.players [2];
				if (players [2].Actions.Green.WasReleased) {
					players [2].isSignedIn = true;
					players [2].gameObject.GetComponent<CharacterSelectPanel> ().enabled = true;

				}
			} 	
		}
	}












	public bool lockCharacter(int index) {
		if (!characterStatuses [getCharacterKeys() [index]]) {

			//character is locked
			characterStatuses [getCharacterKeys() [index]] = true;
			playersInPlay--;

			//lock character for all players
			if (characterStatuses.ContainsKey (ShipEnum.Kraken.ToString ())) {
				isolateKraken (index);
			}

			return true;
		}

		return false;
	}

	public bool unlockCharacter(int index) {

		if (characterStatuses [getCharacterKeys() [index]]) {

			//character is unlocked
			characterStatuses [getCharacterKeys() [index]] = false;
			playersInPlay++;

			//unlock character for all players
			if (characterStatuses.ContainsKey (ShipEnum.Kraken.ToString ())) {
				deIsolateKraken (index);
				isolateKraken (index);
			}

			return true;
		}

		return false;	
	}


	/*
	 * When a player has chosen a character, remove that character from all the other player lists
	 */
	public void isolateKraken(int index) { 
		if (characterStatuses.ContainsKey (ShipEnum.Kraken.ToString ())) {
			if (playersInPlay == 1 && !characterStatuses [ShipEnum.Kraken.ToString ()]) {
				foreach (string character in getCharacterKeys()) {
					if (!characterStatuses [character] && character != ShipEnum.Kraken.ToString ()) {
						characterStatuses [character] = true;
					}
				}
			} 
		}
	}


	public void deIsolateKraken(int index) {


		List<string> charactersSelected = new List<string>();
		if (playersInPlay > 1 && !characterStatuses [ShipEnum.Kraken.ToString ()]) {
			foreach (CharacterSelectPanel player in players) {
				if (player.selectedCharacter != "") {
					charactersSelected.Add (player.selectedCharacter);
				}
			}

			foreach (string character in getCharacterKeys()) {
				if (characterStatuses [character]) {
					if (!charactersSelected.Contains (character)) {
						characterStatuses [character] = false;
					}
				}
			}
		}
	}




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
		

}
