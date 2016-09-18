using UnityEngine;
using System.Collections;

public class LightPillar : MonoBehaviour {
	bool up = false;
	bool active = false;
	// Use this for initialization
	void Start () {
		this.GetComponent<MeshRenderer> ().enabled = false;
		this.gameObject.transform.localPosition = new Vector3 (this.gameObject.transform.localPosition.x, -1.2f, this.gameObject.transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			if (up && this.transform.localPosition.y <= -0.24f) {
				this.transform.Translate(new Vector3(0f,3f*Time.deltaTime*GlobalVariables.gameSpeed,0f));
			} else if (!up && this.transform.localPosition.y >= -1.2f) {
				this.transform.Translate(new Vector3(0f,-3f*Time.deltaTime*GlobalVariables.gameSpeed,0f));
			}
		}
	}

	public void activatePillar(){
		this.GetComponent<MeshRenderer> ().enabled = true;
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, -1.2f, this.transform.localPosition.z);
		up = true;
		active = true;
		Invoke ("lowerPillar", 1f);
	}

	public void lowerPillar(){
		up = false;
		Invoke ("deActivatePillar", 1f);
	}

	public void deActivatePillar(){
		this.GetComponent<MeshRenderer> ().enabled = false;
		up = false;
		active = false;
	}
}
