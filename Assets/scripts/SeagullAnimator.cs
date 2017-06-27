using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullAnimator : MonoBehaviour {

    [SerializeField]
    Animator _animator;

    [SerializeField]
    float magnitude;

    [SerializeField]
    float _time;

    [SerializeField]
    int delayMin;

    [SerializeField]
    int delayMax;


    [SerializeField]
    int takeOffDelay;
    private System.Random rand;

    public void Initialize (System.Random rand) {
        this.rand = rand;
        if (rand.Next(0,2) == 0) {
            this._animator.SetTrigger("IdleOne"); 
        } else
        {
            this._animator.SetTrigger("IdleTwo");
        }

        
        this._animator.SetFloat("IdleSpeed", rand.Next(7, 20)/10f);
	}

    public void RandomTakeOff()
    {
        Invoke("TakeOff", rand.Next(takeOffDelay) / 100);

    }

    public void TakeOff () {
        this._animator.SetTrigger("TakeOff");
    }

    public void OnTakeOffComplete() {
        this.StartFlying();
    }
    public void StartFlying() {
        LeanTween.moveLocal(this.gameObject, ((transform.right * -1) + transform.up) * magnitude, _time);
    }
}
