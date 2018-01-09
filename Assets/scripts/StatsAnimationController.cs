using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsAnimationController : MonoBehaviour {

    public Action onAnimEnd;
    public GameObject winner;
    public List<GameObject> fourSlots;
    public List<GameObject> threeSlots;

    public TMPro.TextMeshProUGUI text;

    public List<GameObject> panels;

    public List<Sprite> rankSprites;
    public List<Sprite> characterNames;

    public void HookupCallback(Action onAnimEnd)
    {
        this.onAnimEnd = onAnimEnd;
    }

    public void CallMethod()
    {
        onAnimEnd();
    }

}
