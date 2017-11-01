using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FadeLeanTween : MonoBehaviour {

    public float alpha;
    public float time;
	void Start () {
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), alpha, time);
	}
	
}
