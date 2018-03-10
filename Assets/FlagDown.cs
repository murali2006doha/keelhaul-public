using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagDown : MonoBehaviour {
    public RectTransform rect;
    public float origPosition = 70;
    public float newPosition = -45;
    public float moveDown = -2f;
    public float moveUp = 90f;
    public float moveDuration = 0.2f;

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
            LeanTween.moveLocalY(this.gameObject, moveUp, moveDuration).setOnComplete(() => useBlocked = false);
        }
    }

    public void ResetAbility()
    {
        if (!resetBlocked && this.transform.localPosition.y >= origPosition) {
            resetBlocked = true;
            this.transform.localPosition = MathHelper.setY(this.transform.localPosition, origPosition);
            LeanTween.moveLocalY(this.gameObject, moveDown, moveDuration).setOnComplete(() => resetBlocked = false);
        }
    }

}
