using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;


/*
 * Handles the player sign in and character selection for all the players
 * false means not selected and true means selected
 */ 
public class PlayerSignIn : MonoBehaviour {  

	public PlayerActions Actions { get; set; }
	public List<CharacterSelect> players = new List<CharacterSelect>(); //to pass on to 
    public AsyncOperation asyncLoad;
    public bool loadingScene;
	public Dictionary<string, bool> characterStatuses = new Dictionary<string, bool>();
	public Dictionary<string, List<Sprite>> CharacterItems = new Dictionary<string, List<Sprite>>();
	public Sprite AImage;
	int playersInPlay;
    bool started = false;
    // Use this for initialization
    public string levelName;
	public bool withKeyboard;
	public Transform start;
	public GameObject mapSelect;
	ControllerSelect cc;

    public GameObject loadingScreen;
	[System.Serializable]
	public struct CharacterMenuItems
	{
		public ShipEnum character;
		public Sprite charImage;
		public Sprite readyImage;
		public Sprite lockImage;
	}

	[SerializeField]
	public List<CharacterMenuItems> characters;


	void Start () {
		//GameObject.DontDestroyOnLoad (this.gameObject);
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;

		foreach (CharacterMenuItems charInfo in characters) {

			characterStatuses.Add (charInfo.character.ToString(), false);

			List<Sprite> images = new List<Sprite>();
			images.Add(charInfo.charImage);
			images.Add(charInfo.readyImage);
			images.Add(charInfo.lockImage);

			CharacterItems.Add (charInfo.character.ToString (), images);

		}

		playersInPlay = players.Count;

	}


	void Update(){ //need to make controllers work in menu

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

		if (cc.players.Count == 0) {
		
		
		} else if (cc.players.Count == 1) {
			players [0].Actions = (PlayerActions)cc.players [0];
			if (players [0].Actions.Green.WasReleased) {
				players [0].isSignedIn = true;
			}
		} else if (cc.players.Count == 2) {
			players [1].Actions = (PlayerActions)cc.players [1];
			if (players [1].Actions.Green.WasReleased) {
				players [1].isSignedIn = true;
			}
		} else if (players.Count > 2) {
			if (cc.players.Count == 3) {
				players [2].Actions = (PlayerActions)cc.players [2];
				if (players [2].Actions.Green.WasReleased) {
					players [2].isSignedIn = true;
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
			foreach (CharacterSelect player in players) {
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


	public List<string> getCharacterKeys() {
		List<string> keys = new List<string> (characterStatuses.Keys);
		return keys;
	}


    IEnumerator LoadNewScene()
    {
        //To do: move this logic out of playerSignIn, make it more generic
        AsyncOperation async = SceneManager.LoadSceneAsync(levelName);
        while (!async.isDone)
        {
            yield return null;
        }

    }

}
