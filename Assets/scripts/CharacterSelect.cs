using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

using UnityEngine.UI;
using InControl;


/*
 * Attached to each player gameobject
 */ 
public class CharacterSelect : MonoBehaviour {

	public PlayerActions Actions { get; set; }
	public GameObject kraken; 
	public GameObject blue;
	public GameObject red;
	public GameObject green;
	public List<GameObject> characterList = new List<GameObject>(); //contains the texts

	public bool isSignedIn = false;
	public Transform upLitArrow;
	public Transform downLitArrow;
	public Transform deselect;

	public string selectedCharacter;
	public bool selected = false;
	public int index;			//indicates which character is shown from the available characters
	public PlayerSignIn ps;


	// Use this for initialization
	void Start () {

		if (Actions != null) {
			characterList[0].SetActive (true);
		}

		characterList.Add (kraken);
		characterList.Add (blue);
		characterList.Add (red);
		characterList.Add (green);
		selectedCharacter = "";
	}


	// Update is called once per frame
	void Update () {
		if (Actions != null) {
			characterSelect ();
			if (selected != true) {
				characterList[index].SetActive (true);
			}
		}
		ps = GameObject.FindObjectOfType<PlayerSignIn> ();

	}


	public List<GameObject> getCharacterList() {
		return characterList;
	}


	public int getIndex() {
		return index;
	}


	public void setIndex(int i) {
		index = i;
	}


	public bool isSelected() {
		return selected;
	}

	/*
	 * navigate the characters selection options and set the character when chosen
	 */
	public void characterSelect () {		
		if (!selected && isSignedIn) {

			lightArrows ();

			if (Actions.Down.WasPressed) {
				characterList [index].SetActive (false);
				index = upAndDown (characterList, index, "down");
			} 

			if (Actions.Up.WasPressed) {
				characterList [index].SetActive (false);
				index = upAndDown (characterList, index, "up");
			}


			if (Actions.Green.WasPressed) { //if the player selects the character
				if(!ps.characters[ps.getCharacterKeys()[index]]) {
					selected = true;
					characterList [index].SetActive (true);
					characterList [index].transform.GetChild (0).gameObject.SetActive (true);
					selectedCharacter = ps.getCharacterKeys () [index];
					deselect.gameObject.SetActive (true);
				}
			} 

		} else if (selected && Actions.Red.WasPressed) {
			if (ps.characters [ps.getCharacterKeys () [index]]) {
				selected = false;
				characterList [index].transform.GetChild (0).gameObject.SetActive (false);
				characterList [index].SetActive (true);	
				deselect.gameObject.SetActive (false);
			}
		}
	}


	public void lightArrows() {

		if (Actions.Down.WasPressed) {
			downLitArrow.gameObject.SetActive(true);
			Invoke ("turnOffDownArrow", 0.15f);

		} if (Actions.Up.WasPressed) {
			upLitArrow.gameObject.SetActive(true);
			Invoke ("turnOffUpArrow", 0.15f);
		} 

	}

	public void turnOffDownArrow() {

		downLitArrow.gameObject.SetActive (false);
	}

	public void turnOffUpArrow() {

		upLitArrow.gameObject.SetActive (false);
	}


	/*
	 * cycles through a list 
	 */ 
	public int upAndDown (List<GameObject> items, int i, string direction) {

		if (direction == "up") {
			if (i == 0) {
				i = items.Count - 1;
			} else {
				i -= 1;
			}
		}
		if (direction == "down") {
			if (i == items.Count - 1) {
				i = 0;
			} else {
				i += 1;
			}
		}

		return i;
	}

}
