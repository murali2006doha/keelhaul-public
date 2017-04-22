using UnityEngine;
using System.Collections;

public class PrefabSound : MonoBehaviour {

	public void StartSound() {
		StartCoroutine (PlaySound ());

	}



	public IEnumerator PlaySound() {
		this.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (this.GetComponent<AudioSource> ().clip.length);
		Destroy (this.transform.gameObject);
	}

}

