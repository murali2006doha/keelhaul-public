using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadedZoom : MonoBehaviour {
    public float fadeDuration = 1.0f;
    public int finalTextSize = 71;
    public int fontStep = 1;
    Text text;
    // Use this for initialization
    void Start () {
        text = this.GetComponent<Text>();
        text.fontSize = 1;
        text.CrossFadeAlpha(0, 0, true);
        text.CrossFadeAlpha(1, fadeDuration, true);
    }
	
	// Update is called once per frame
	void Update () {
	    if(text.fontSize <finalTextSize){
            text.fontSize+= fontStep;
        }
	}
}
