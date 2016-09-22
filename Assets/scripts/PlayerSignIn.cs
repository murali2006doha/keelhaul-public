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
	public Dictionary<string, List<Sprite>> Characters = new Dictionary<string, List<Sprite>>();
	public Sprite AImage;
	public Sprite krakenImage;
	public Sprite BlackbeardShipImage;
	public Sprite AtlanteanShipImage;
	public Sprite ChineseJunkShipImage;
	public Sprite krakenReadyImage;
	public Sprite BlackbeardReadyImage;
	public Sprite AtlanteanReadyImage;
	public Sprite ChineseJunkReadyImage;
	public Sprite krakenLockImage;
	public Sprite BlackbeardLockImage;
	public Sprite AtlanteanLockImage;
	public Sprite ChineseJunkLockImage;
	List<Sprite> images = new List<Sprite>();
	List<Sprite> imagesB = new List<Sprite>();
	List<Sprite> imagesC = new List<Sprite>();
	List<Sprite> imagesD = new List<Sprite>();
	int playersInPlay;

	//Character -> charImage, readyImage, lockImage

	// Use this for initialization
	public string levelName;
	public bool withKeyboard;
	public Transform start;
	public bool signedIn = false;
	//public int assign_index = 0;
	public GameObject mapSelect;
	public ControllerSelect cc;


	void Start () {
		//GameObject.DontDestroyOnLoad (this.gameObject);
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;

		players.Add (player1); 
		players.Add (player2);
		players.Add (player3);

		images.Add (krakenImage);
		images.Add (krakenReadyImage);
		images.Add (krakenLockImage);
		imagesB.Add (BlackbeardShipImage);
		imagesB.Add (BlackbeardReadyImage);
		imagesB.Add (BlackbeardLockImage);
		imagesC.Add (AtlanteanShipImage);
		imagesC.Add (AtlanteanReadyImage);
		imagesC.Add (AtlanteanLockImage);
		imagesD.Add (ChineseJunkShipImage);
		imagesD.Add (ChineseJunkReadyImage);
		imagesD.Add (ChineseJunkLockImage);

		characterStatuses.Add (ShipEnum.Kraken.ToString(), false);
		characterStatuses.Add (ShipEnum.BlackbeardShip.ToString(), false);
		characterStatuses.Add (ShipEnum.AtlanteanShip.ToString(), false);
		characterStatuses.Add (ShipEnum.ChineseJunkShip.ToString(), false);

		Characters.Add (ShipEnum.Kraken.ToString(), images);
		Characters.Add (ShipEnum.BlackbeardShip.ToString(), imagesB);
		Characters.Add (ShipEnum.AtlanteanShip.ToString(), imagesC);
		Characters.Add (ShipEnum.ChineseJunkShip.ToString(), imagesD);

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
				player1.Panel.sprite = AImage;
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
			deIsolateKraken(index);

			return true;
		}

		return false;	
	}


	/*
	 * When a player has chosen a character, remove that character from all the other player lists
	 */
	public void isolateKraken(int index) { 
		print (characterStatuses [getCharacterKeys () [index]]);

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

		if (playersInPlay > 1 && !characterStatuses [ShipEnum.Kraken.ToString()]) {
			foreach (string character in getCharacterKeys()) {
				if (characterStatuses [character]) {
					foreach (CharacterSelect player in players) {
						if (!(player.selectedCharacter == character)) {
							characterStatuses [character] = false;
						}
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

	public void assign(){
		//assign_index++;
	}


	public void LoadScene (string name) {
		Application.LoadLevel (name);
	}

}
