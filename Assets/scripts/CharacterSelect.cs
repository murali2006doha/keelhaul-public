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
	public Image forePanel;
	public Image backPanel;

	public Animator characterPanelAnimator;
    public float swoopSpeed;

	public bool isSignedIn = false;
	public Transform upLitArrow;
	public Transform downLitArrow;
	public Transform deselect;
	public string selectedCharacter;
	public bool selected;
	int selectedCharacterIndex;
	public int index;			
	public PlayerSignIn ps;
	Image foreImage;

	// Use this for initialization
	void Start () {
      
		selected = false;
		selectedCharacter = "";
		backPanel.sprite = ps.AImage;

		foreImage = forePanel.GetComponent<Image>();
		foreImage.enabled = false;



    }


	// Update is called once per frame
	void Update () {

        if (characterPanelAnimator.transform.parent.gameObject.activeSelf) {
            characterPanelAnimator.SetFloat("swoopSpeed", swoopSpeed);
        }
		if (Actions != null && this.gameObject.activeSelf) {
			renderImage (index);
            characterSelect ();	
		} 
	}


	/*
	 * navigate the characters selection options and set the character when chosen
	 */
	public void characterSelect () {

        if (!selected && isSignedIn) {
			lightArrows ();

			if (Actions.Down.WasReleased) {
				index = getIndexPosition (ps.CharacterItems.Count, index, "down");
                characterPanelAnimator.SetTrigger("shakeDown");
			} 

			if (Actions.Up.WasReleased) {
				index = getIndexPosition (ps.CharacterItems.Count, index, "up");
                characterPanelAnimator.SetTrigger("shakeUp");
            }

			if (Actions.Green.WasReleased) { //if the player selects the character
				if(ps.lockCharacter(index)) {
					selected = true;
					selectedCharacterIndex = index;
					selectedCharacter = getCharacterKeys () [selectedCharacterIndex];
					turnOffArrows();
				}
			} 

		} else if (selected && Actions.Red.WasReleased) {
			if(ps.unlockCharacter(selectedCharacterIndex)) {
				selected = false;
				selectedCharacter = "";
			}
		}
	}


	public void renderImage(int index) {
		if (selected) {
			
			foreImage.sprite = ps.CharacterItems [getCharacterKeys () [index]] [1];  //READY
			foreImage.enabled = true;
			deselect.gameObject.SetActive (true);

		} else {
			
			if (ps.characterStatuses [getCharacterKeys () [index]]) {
				foreImage.sprite = ps.CharacterItems [getCharacterKeys () [index]] [2];  //LOCK
				foreImage.enabled = true;
			} else {
				foreImage.enabled = false;
				backPanel.sprite = ps.CharacterItems [getCharacterKeys () [index]] [0];  //CHARACTER
			}
			deselect.gameObject.SetActive (false);

		}
	}



	public void lightArrows() {

		if (Actions.Down.IsPressed) {
			downLitArrow.gameObject.SetActive (true);
		}
		if (Actions.Down.WasReleased) {
			downLitArrow.gameObject.SetActive (false);
		}
		if (Actions.Up.IsPressed) {
			upLitArrow.gameObject.SetActive (true);
		}
		if (Actions.Up.WasReleased) {
			upLitArrow.gameObject.SetActive (false);
		}
	}


	void turnOffArrows() {
		upLitArrow.gameObject.SetActive (false);
		downLitArrow.gameObject.SetActive (false);

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
		return ps.getCharacterKeys();
	}

}
