using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour {

    public float fadeDuration = 1.0f;
    public float fadeAlpha = 0.5f;
    public float delay = 2.5f;

    Image image;
    // Use this for initialization
    void Start()
    {
        image = this.GetComponent<Image>();
        image.CrossFadeAlpha(fadeAlpha>0?0:1, 0, true);
        Invoke("fade", delay);
    }

    void fade()
    {
        image.CrossFadeAlpha(fadeAlpha, fadeDuration, true);
    }
}
