using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class VideoPlayer : MonoBehaviour {

	public RawImage [] movieList;
	MovieTexture currentMovieTexture;
	int textureIndex = 0;
	public bool showIsOver = false;
	// Use this for initialization
	void Start () {
		foreach (RawImage movie in movieList) {
			movie.gameObject.SetActive(false);
		} 
		playMovie ();
	}

	void playMovie() {
		movieList [textureIndex].gameObject.SetActive(true);
		currentMovieTexture = (MovieTexture)(movieList [textureIndex].GetComponent<RawImage> ().mainTexture);
		currentMovieTexture.Play ();
	}

	void stopMovie() {
		currentMovieTexture.Stop ();
		movieList [textureIndex].gameObject.SetActive(false);
	}
	// Update is called once per frame
	void Update () {
		if (!currentMovieTexture.isPlaying) {
			stopMovie ();
			if ((textureIndex + 1) < movieList.Length) {
				textureIndex++;
				playMovie ();
			} else {
				showIsOver = true;
			}
		}
	}
}
