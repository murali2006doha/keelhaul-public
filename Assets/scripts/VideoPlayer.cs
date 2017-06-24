using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class VideoPlayer : MonoBehaviour {

	public RawImage [] movieList;
    public AudioClip[] sourceAudio;
	MovieTexture currentMovieTexture;
	int textureIndex = 0;
	public bool showIsOver = false;
	// Use this for initialization
	void Start () {
		foreach (RawImage movie in movieList) {
            ((MovieTexture)(movie.mainTexture)).Stop();
		} 
		playMovie ();
	}

	void playMovie() {
        movieList[textureIndex].GetComponent<RawImage>().gameObject.SetActive(true);
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
			if ((textureIndex + 1) < movieList.Length) {
                stopMovie();
                textureIndex++;
				playMovie ();
			} else {
				showIsOver = true;
                SceneManager.LoadScene("Start");
			}
		}
	}
}
