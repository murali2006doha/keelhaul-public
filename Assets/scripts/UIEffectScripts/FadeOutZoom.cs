using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutZoom : MonoBehaviour {
    public float fadeDuration = 1.0f;
    public float delay = 2.5f;

    Text text;
    // Use this for initialization
    void Start()
    {
        text = this.GetComponent<Text>();
        Invoke("fadeOut", delay);
    }

    void fadeOut()
    {
        text.CrossFadeAlpha(0, fadeDuration, true);
    }

    void Update()
    {

    }
    // Update is called once per frame

}
