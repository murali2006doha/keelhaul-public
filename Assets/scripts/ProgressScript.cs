using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressScript : MonoBehaviour {
	Text text;
	public string name;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();

		text.CrossFadeAlpha (0.0f,0.1f,false);
	}

	public void activatePopup(string text, string player, string type){
		
		if (player.Equals (name)) {
			text = text.Replace (player, "You");
			text = text.Replace ("The", "");
			text = text.Replace ("Needs", "Need");
		}
		//Replace type with enum.
		if (type.Equals ("Kraken")) {
			text = text.Replace ("Point", "Kill");
		}
		this.text.text = text;
		Invoke ("fadeIn", 1);
		Invoke ("fadeOut", 7);
	}

	void fadeIn(){
		text.CrossFadeAlpha (1.0f,0.5f,false);
	}

	void fadeOut(){
		text.CrossFadeAlpha (0.0f,0.5f,false);
	}
}
