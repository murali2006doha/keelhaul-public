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
	//public List<GameObject> charImages = new List<GameObject>();
	public Image Panel;

	public bool isSignedIn = false;
	public Transform upLitArrow;
	public Transform downLitArrow;
	public Transform deselect;
	public string selectedCharacter;
	bool selected;
	int selectedCharacterIndex;
	public int index;			
	public PlayerSignIn ps;
	Color originalColor;

	// Use this for initialization
	void Start () {

		if (this.gameObject != null) {
			this.enabled = true;

			//index = 0;
			selected = false;
			selectedCharacter = "";
			Panel.sprite = ps.AImage;
			originalColor = Panel.color;
		} else {
			this.enabled = false;
		}

	}


	// Update is called once per frame
	void Update () {
		if (Actions != null) {
			renderImage (index);
			characterSelect ();		
		} 
	}


	public int getIndex() {
		return index;
	}


	public void setIndex(int i) {
		index = i;
	}


	/*
	 * navigate the characters selection options and set the character when chosen
	 */
	public void characterSelect () {		

		if (!selected && isSignedIn) {
			lightArrows ();

			if (Actions.Down.WasReleased) {
				index = upAndDown (ps.Characters.Count, index, "down");
				//renderImage (index);
			} 

			if (Actions.Up.WasReleased) {
				index = upAndDown (ps.Characters.Count, index, "up");
				//renderImage(index);
			}

			if (Actions.Green.WasReleased) { //if the player selects the character
				if(ps.lockCharacter(index)) {
					selected = true;
					//renderImage(index);
					selectedCharacterIndex = index;
					selectedCharacter = getCharacterKeys () [selectedCharacterIndex];
				}
			} 

		} else if (selected && Actions.Red.WasReleased) {
			if(ps.unlockCharacter(selectedCharacterIndex)) {
				selected = false;
				//renderImage(index);				
				selectedCharacter = "";
			}
		}
	}



	public void renderImage(int index) {
		Color originalColor = Panel.color;

		if (selected) {
			Panel.sprite = ps.Characters [getCharacterKeys () [index]] [1];  //READY
			deselect.gameObject.SetActive (true);

		} else {
			if (ps.characterStatuses [getCharacterKeys () [index]]) {
				Panel.sprite = ps.Characters [getCharacterKeys () [index]] [2];  //LOCK

			} else {
				Panel.sprite = ps.Characters [getCharacterKeys () [index]] [0];  //CHARACTER
			}
			deselect.gameObject.SetActive (false);

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
	public int upAndDown (int listSize, int i, string direction) {

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
		return ps.getCharacterKeys();
	}

}
