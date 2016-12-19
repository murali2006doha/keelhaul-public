﻿using UnityEngine;
using System.Collections;

public class ShipAnimator : MonoBehaviour {

	public Animator shipAnimator;
	public PlayerInput ship;
	public float respawnTime;
    public SoundCategoryEnum category = SoundCategoryEnum.Generic;
    private float damageFadeSeconds = 0.2f;


    // Use this for initialization
    void Start () {
		this.ship = this.GetComponentInParent<PlayerInput> ();
	}

	public void triggerDeathAnimation(){
		shipAnimator.SetBool ("death", true);
        Invoke("playBubbleSound", 0.5f);
	}

    public void playBubbleSound()
    {
        SoundManager.playSound(SoundClipEnum.SinkWater, SoundCategoryEnum.Generic, transform.position);
    }

	public void triggerRespawnAnimation(){
        SoundManager.playSound(SoundClipEnum.Respawn, category,transform.position);
        shipAnimator.SetBool ("death", false);
	}

	void setupRespawn()
	{
		ship.setupRespawn ();		
	}
	public void onDeathAnimationEnd(int dying){
		Debug.Log ("reaching death animation end");
		if (dying==1 && shipAnimator.GetBool ("death") == true) { //this function is called at end of dying
			Invoke("setupRespawn",respawnTime);
	

		}
		else if (dying==0 && shipAnimator.GetBool("death") == false){
			Debug.Log ("reaching respawn");
			ship.rebirth ();
		}

	}

    public void playDamageAnimation()
    {
        shipAnimator.CrossFade("damageBlink", damageFadeSeconds);
    }

	public void onRespawnAnimationEnd(){
		
	}

	public void resetToIdle(){
		shipAnimator.CrossFade ("idle", 0.1f);
		shipAnimator.SetBool ("death", false);
		CancelInvoke ();
	}
	
	// Update is called once per frame

}
