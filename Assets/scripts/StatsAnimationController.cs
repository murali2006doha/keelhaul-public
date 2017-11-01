using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsAnimationController : MonoBehaviour {

    public Action onAnimEnd;

    public void HookupCallback(Action onAnimEnd)
    {
        this.onAnimEnd = onAnimEnd;
    }

    public void CallMethod()
    {
        onAnimEnd();
    }

}
