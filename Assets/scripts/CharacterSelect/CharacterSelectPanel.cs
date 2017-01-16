using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

using UnityEngine.UI;
using InControl;


public class CharacterSelectPanel : MonoBehaviour {

	public PlayerActions Actions { get; set; }
	public Image backPanel;
	public Image forePanel;

	public Animator characterPanelAnimator;
	public float swoopSpeed;
	public bool isSignedIn = false;
	public string selectedCharacter;
	public bool selected;
	public Transform upLitArrow;
	public Transform downLitArrow;
	public Transform deselect;

	private Sprite readyImage;
	private Sprite lockImage;
	private Sprite charImage;
	private List<ShipEnum> characters = new List<ShipEnum> ();
	protected int index;
	protected int selectedCharacterIndex;
	AbstractCharacterSelectController csc;


	// Use this for initialization
	void Start () {

		csc = FindObjectOfType<AbstractCharacterSelectController> ();
		selected = false;
		selectedCharacter = "";
		backPanel.sprite = Resources.Load<Sprite>(CharacterSelectModel.ShipToImage [ShipEnum.Kraken]);
		forePanel.GetComponent<Image> ().enabled = false;
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
				index = csc.getIndexPosition (characters.Count, index, "down");
				characterPanelAnimator.SetTrigger("shakeDown");
			} 

			if (Actions.Up.WasReleased) {
				index = csc.getIndexPosition (characters.Count, index, "up");
				characterPanelAnimator.SetTrigger("shakeUp");
			}

			if (Actions.Green.WasReleased) { //if the player selects the character
				if(csc.lockCharacter(index)) {
					selected = true;
					selectedCharacterIndex = index;
					selectedCharacter = characters [selectedCharacterIndex].ToString();
					turnOffArrows();
				}
			} 

		} else if (selected && Actions.Red.WasReleased) {
			if(csc.unlockCharacter(selectedCharacterIndex)) {
				selected = false;
				selectedCharacter = "";
			}
		}
	}




	public void renderImage(int index) {
		if (selected) {

			forePanel.sprite = Resources.Load<Sprite>(CharacterSelectModel.ShipToReadyImage [characters[index]]);
			forePanel.GetComponent<Image> ().enabled = true;
			deselect.gameObject.SetActive (true);

		} else {

			if (csc.getCharacterStatuses() [characters [index].ToString()]) {
				forePanel.sprite = Resources.Load<Sprite>(CharacterSelectModel.ShipToLockImage [characters[index]]);
				forePanel.GetComponent<Image> ().enabled = true;
			} else {
				forePanel.GetComponent<Image> ().enabled = false;
				backPanel.sprite = Resources.Load<Sprite>(CharacterSelectModel.ShipToImage [characters[index]]);
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




	public void setCharacterList(List<ShipEnum> characters) {
		this.characters = characters;
	}




}
