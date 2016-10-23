using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pulsate : MonoBehaviour {

    Graphic   graphic;
    public float duration;
    public float fadeOutDelay;
    public float fadeInDelay;
    void Start () {
        graphic = GetComponent<Graphic>();
        fadeOut();
	}
	
	// Update is called once per frame
    void fadeIn()
    {
        graphic.CrossFadeAlpha(1, duration, false);
        Invoke("fadeOut", duration + fadeOutDelay);
    }

    void fadeOut()
    {
        graphic.CrossFadeAlpha(0, duration, false);
        Invoke("fadeIn", duration + fadeInDelay);
    }

    public void changePulsate()
    {
        CancelInvoke();
        graphic.CrossFadeAlpha(0, 0f, false);
        fadeIn();
    }

    public void removePulsate()
    {
        graphic.CrossFadeAlpha(0, 0.2f, false);
        CancelInvoke();
    }
}
