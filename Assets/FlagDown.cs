using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagDown : MonoBehaviour {
    public RectTransform rect;
    float origPosition = 70;
    float newPosition = -45;

    bool resetBlocked = false;
    bool useBlocked = false;

    public void Start()
    {
        this.transform.localPosition = MathHelper.setY(this.transform.localPosition, origPosition);
    } 

    public void UseAbility()
    {
        if (!useBlocked)
        {
            useBlocked = true;
            this.transform.localPosition = MathHelper.setY(this.transform.localPosition, newPosition);
            LeanTween.moveLocalY(this.gameObject, 90f, 0.2f).setOnComplete(() => useBlocked = false);
        }
    }

    public void ResetAbility()
    {
        if (!resetBlocked && this.transform.localPosition.y >= 70) {
            resetBlocked = true;
            this.transform.localPosition = MathHelper.setY(this.transform.localPosition, origPosition);
            LeanTween.moveLocalY(this.gameObject, -3f, 0.2f).setOnComplete(() => resetBlocked = false);
        }
    }

}
