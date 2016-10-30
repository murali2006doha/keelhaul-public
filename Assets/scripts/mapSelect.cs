using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

using UnityEngine.UI;
using InControl;

public class MapSelect : MonoBehaviour {

	public PlayerActions Actions { get; set; }
	public GameObject chineseMap;
	public GameObject tropicalMap;
	public GameObject rightLitArrow;
	public GameObject leftLitArrow;
	public GameObject deselect;
	public GameObject start;
	List<GameObject> mapList = new List<GameObject>(); //contains the texts
	bool selected = false;
	int index = 0;
	GameObject selectedMap;
	PlayerSelectSettings ps;
	List<CharacterSelection> players;

	// Use this for initialization
	void Start () {
		ps = GameObject.FindObjectOfType<PlayerSelectSettings> ();
		players = ps.players;
		selectedMap = chineseMap;
		mapList.Add (chineseMap);
		mapList.Add (tropicalMap);
	}

	IEnumerator wait()
	{
		yield return new WaitForSeconds(5f);
	}
	
	// Update is called once per frame
	void Update () {
		selectMap ();

		if (selected) {
			if (players.Exists (p => p.Actions.Start)) {
				Text text = start.GetComponent<Text> ();
				text.color = Color.yellow;
				LoadScene ("TropicalMap");
			}
		} else {
			if (players.Exists (p => p.Actions.Red.IsPressed)) {
				this.gameObject.SetActive (false);
				GameObject menu = GameObject.Find ("SelectMode");
				menu.SetActive (true);
			}
		}
	}



	void selectMap() {
		if (!selected) {

			lightArrows ();

			if (players.Exists (p => p.Actions.Left.WasPressed)) {
				mapList [index].SetActive (false);
				index = upAndDown (mapList, index, "down");
				selectedMap = mapList[index];
				mapList [index].SetActive (true);

			} 

			if (players.Exists (p => p.Actions.Right.WasPressed)) {
				mapList [index].SetActive (false);
				index = upAndDown (mapList, index, "up");
				selectedMap = mapList[index];
				mapList [index].SetActive (true);

			}


			if (players.Exists (p => p.Actions.Green.WasPressed)) { //if the player selects the character
				selected = true;
				mapList [index].transform.GetChild (0).gameObject.SetActive (true);
				selectedMap = mapList [index];
				start.SetActive (true);
				//deselect.SetActive (true);
			} 

		} else if (selected && players.Exists (p => p.Actions.Red.WasPressed)) {
			selected = false;
			mapList [index].transform.GetChild (0).gameObject.SetActive (false);
			start.SetActive (false);
			//deselect.SetActive (false);
		}
	}


	void lightArrows() {

		if (players.Exists (p => p.Actions.Left.IsPressed)) {
			leftLitArrow.gameObject.SetActive(true);
			Invoke ("turnOffDownArrow", 0.15f);

		} if (players.Exists (p => p.Actions.Right.IsPressed)) {
			rightLitArrow.gameObject.SetActive(true);
			Invoke ("turnOffUpArrow", 0.15f);
		} 
	}

	public void turnOffDownArrow() {

		leftLitArrow.gameObject.SetActive (false);
	}

	public void turnOffUpArrow() {

		rightLitArrow.gameObject.SetActive (false);
	}



	/*
	 * cycles through a list 
	 */ 
	int upAndDown (List<GameObject> items, int i, string direction) {

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

	
	void LoadScene (string name) {
		Application.LoadLevel (name);
	}


}
