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
	public List<CharacterSelect> players = new List<CharacterSelect>(); //to pass on to 

	public Dictionary<string, bool> characters = new Dictionary<string, bool>();
	public Dictionary<CharacterSelect, bool> players_in_play = new Dictionary<CharacterSelect, bool>();

	// Use this for initialization
	public string levelName;
	public bool withKeyboard;
	public Transform start;
	public GameObject pressAtoJoin1;
	public GameObject pressAtoJoin2;
	public GameObject pressAtoJoin3;
	public GameObject pressAtoJoin4;
	public bool signedIn = false;
	//public int assign_index = 0;
	public GameObject mapSelect;
	public ControllerSelect cc;


	void Start () {
		//GameObject.DontDestroyOnLoad (this.gameObject);
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;

		players_in_play.Add (player1, false);
		players_in_play.Add (player2, false);
		players_in_play.Add (player3, false);
		characters.Add ("kraken", false);
		characters.Add ("atlantean", false);
		characters.Add ("blackbeard", false);
		characters.Add ("chinese", false);
		players.Add (player1); 
		players.Add (player2);
		players.Add (player3);

		player1.setIndex(0);
		player2.setIndex (0);
		player3.setIndex (0);

		pressAtoJoin1.SetActive (true);
		pressAtoJoin2.SetActive (true);
		pressAtoJoin3.SetActive (true);
	}


	void Update(){ //need to make controllers work in menu
		var inputDevice = InputManager.ActiveDevice; //last device to

		if (this.gameObject.activeSelf) {
			cc.listening = true;
			signIn ();
			selectCharacter ();
			unSelectCharacter ();
			addLocks ();
		}

		if (this.gameObject != null) {

			if (players.TrueForAll(p => players_in_play [p])) {

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
			if (pressAtoJoin1 != null) {
				pressAtoJoin1.SetActive (true);
			}
		}

		else if (cc.players.Count == 1) {
			player1.Actions = (PlayerActions)cc.players [0];
			if (player1.Actions.Green.IsPressed) {
				pressAtoJoin1.SetActive (false);
				player1.isSignedIn = true;
			}
		}

		else if (cc.players.Count == 2) {
			player2.Actions = (PlayerActions)cc.players [1];
			if (player2.Actions.Green.WasReleased) {
				pressAtoJoin2.SetActive (false);
				player2.isSignedIn = true;
			}

		} else if (cc.players.Count == 3) {
			player3.Actions = (PlayerActions)cc.players [2];
			if (player3.Actions.Green.WasReleased) {
				pressAtoJoin3.SetActive (false);
				player3.isSignedIn = true;
			}
		} 			
	}




	/*
	 * When a player has chosen a character, remove that character from all the other player lists
	 */
	public void selectCharacter() { 

		foreach (CharacterSelect player in players) {
			if (!players_in_play[player]) {  //player has not selected yet
				if (player.Actions != null && player.Actions.Green.WasReleased && player.isSelected()) { 	
					if (!characters [player.selectedCharacter]) {
						characters [player.selectedCharacter] = true;	
						players_in_play [player] = true;
					}
				}
			}
		}

		if (characters.ContainsKey ("kraken")) {
			if (numberOfPlayersInPlay () == 1 && !characters ["kraken"]) {
				isolateKraken ();
			}
		}
	}


	public void addLocks() {
		foreach (CharacterSelect player in players) {
			int i = 0;
			foreach (string ch in characters.Keys) {
				if ((characters [ch]) && (player.selectedCharacter != ch)) {
					player.characterList [i].transform.GetChild (1).gameObject.SetActive (true);
				} else if (!characters [ch]) {
					player.characterList [i].transform.GetChild (1).gameObject.SetActive (false);
				}
				i++;
			}
		}
	}


	public int numberOfPlayersInPlay() {
		int i = 0;
		foreach (KeyValuePair<CharacterSelect, bool> entry in players_in_play) {
			if(!entry.Value) {
				i++;
			}
		}
		return i;
	}


	public void unSelectCharacter() {

		foreach (CharacterSelect player in players) {

			if (players_in_play [player]) {
				if (player.Actions.Red.WasReleased && !player.isSelected ()) { 		
					if (characters [player.selectedCharacter]) {
						player.selectedCharacter = getCharacterKeys () [player.getIndex ()];
						players_in_play [player] = false;
						if (characters.ContainsKey ("kraken")) {							
							deIsolateKraken ();
						}
						characters [player.selectedCharacter] = false;
						player.selectedCharacter = "";
					}
				}
			}
		}
	}



	void isolateKraken() {

		foreach (string key in getCharacterKeys()) {
			if(key != "kraken") {
				characters[key] = true;
			}
		}
		//make the lock screen appear on each character other than krakenrrr
	}


	void deIsolateKraken() {

		foreach (string key in getCharacterKeys()) {
			if (characters [key] = true && key != "kraken") {
				if (!players.Exists (p => p.selectedCharacter == key)) {
					characters [key] = false;
				}
			}
		}
	}


	public List<string> getCharacterKeys() {
		List<string> keys = new List<string> (characters.Keys);
		return keys;
	}

	public List<CharacterSelect> getPlayersKeys() {
		List<CharacterSelect> keys = new List<CharacterSelect> (players_in_play.Keys);
		return keys;
	}

	public void assign(){
		//assign_index++;
	}


	public void LoadScene (string name) {
		Application.LoadLevel (name);
	}

}
