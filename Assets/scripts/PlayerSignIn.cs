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
	public CharacterSelect player1,player2,player3; 
	List<CharacterSelect> players = new List<CharacterSelect>(); //to pass on to 

	Dictionary<CharacterSelect, bool> players_in_play = new Dictionary<CharacterSelect, bool>();
	public Dictionary<string, bool> characterStatuses = new Dictionary<string, bool>();
	public Dictionary<string, List<Sprite>> CharacterItems = new Dictionary<string, List<Sprite>>();
	public Sprite AImage;
	int playersInPlay;

	// Use this for initialization
	public string levelName;
	public bool withKeyboard;
	public Transform start;
	public bool signedIn = false;
	public GameObject mapSelect;
	public ControllerSelect cc;

	[System.Serializable]
	public struct CharacterMenuItems
	{
		public ShipEnum character;
		public Sprite charImage;
		public Sprite readyImage;
		public Sprite lockImage;
	}

	[SerializeField]
	public List<CharacterMenuItems> sprites;


	void Start () {
		//GameObject.DontDestroyOnLoad (this.gameObject);
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;

		players.Add (player1); 
		players.Add (player2);
		players.Add (player3);

		foreach (CharacterMenuItems charInfo in sprites) {

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
		}

		if (this.gameObject != null) {

			if (playersInPlay == 0) {

				start.gameObject.SetActive (true); //change to next when there is map selection

				if (players.Exists(p => p.Actions.Green.IsPressed)) {

					GameObject.FindObjectOfType<PlayerSelectSettings> ().setPlayerCharacters (players);

					LoadScene (levelName);
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
				player1.Panel.sprite = AImage;
				player2.Panel.sprite = AImage;
				player3.Panel.sprite = AImage;

		}
		else if (cc.players.Count == 1) {
			player1.Actions = (PlayerActions)cc.players [0];
			if (player1.Actions.Green.WasReleased) {
				player1.isSignedIn = true;
			}
		}

		else if (cc.players.Count == 2) {
			player2.Actions = (PlayerActions)cc.players [1];
			if (player2.Actions.Green.WasReleased) {
				player2.isSignedIn = true;
			}

		} else if (cc.players.Count == 3) {
			player3.Actions = (PlayerActions)cc.players [2];
			if (player3.Actions.Green.WasReleased) {
				player3.isSignedIn = true;
			}
		} 			
	}


	public bool lockCharacter(int index) {
		if (!characterStatuses [getCharacterKeys() [index]]) {

			//character is locked
			characterStatuses [getCharacterKeys() [index]] = true;
			playersInPlay--;
			//lock character for all players
			isolateKraken(index);

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
			if (getCharacterKeys () [index] == ShipEnum.Kraken.ToString ()) {
				deIsolateKraken (index);
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

	public List<CharacterSelect> getPlayersKeys() {
		List<CharacterSelect> keys = new List<CharacterSelect> (players_in_play.Keys);
		return keys;
	}


	public void LoadScene (string name) {
		Application.LoadLevel (name);
	}

}
