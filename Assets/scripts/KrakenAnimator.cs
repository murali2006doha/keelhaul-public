using UnityEngine;
using System.Collections;
using System;

//Animating the Kraken is quite complicated.Since there might be an AI component, this class is a list of functions both 
//the input and AI can call.
public class KrakenAnimator : MonoBehaviour {


	// Use this for initialization
	public KrakenInput kraken;
	public Animator animKraken; //An array of the different kraken parts's animator components
	public GameObject smashTentacle1,smashTentacle2;
	public GameObject sinkableShip;
	public ParticleSystem[] splashParticles;
    public ParticleSystem[] emergeSplashParticles;
    public float respawnTime;
 

    void Start () {

	}

    public int getState()
    {
        return animKraken.GetCurrentAnimatorStateInfo(0).fullPathHash; // passing in 0 as there will only ever be one layer
    }
    void resetAnimationVars() {
        animKraken.SetBool("submerge", false);
        animKraken.SetBool("smash", false);
        animKraken.SetBool("underShip", false);
    
    }

    public void chargeHeadbash() {
        animKraken.SetBool("headbashCharge", true);
        animKraken.SetBool("headbashFinish", false);
    }

    public void launchHeadbash()
    {
        animKraken.SetBool("headbashCharge", false);
    }

    public void finishHeadbash()
    {
        animKraken.SetBool("headbashFinish", true);
    }

    public void resetUnderShip(){
		animKraken.SetBool ("underShip", false);
		kraken.resetEmerge ();

	}
	public void setSubmerging(){
		kraken.resetSubmerging ();
	}

	public void toggleCollider(){
		smashTentacle1.GetComponent<BoxCollider>().enabled = !smashTentacle1.GetComponent<BoxCollider>().enabled;
		//smashTentacle1.GetComponent<AudioSource>().enabled = !smashTentacle1.GetComponent<AudioSource>().enabled;
		smashTentacle2.GetComponent<BoxCollider>().enabled = !smashTentacle2.GetComponent<BoxCollider>().enabled;
		//smashTentacle2.GetComponent<AudioSource>().enabled = !smashTentacle2.GetComponent<AudioSource>().enabled;

	}

    public void disableCollider()
    {
        smashTentacle1.GetComponent<BoxCollider>().enabled = false;
        smashTentacle2.GetComponent<BoxCollider>().enabled = false;
        

    }
    public void resetSmash(){
		kraken.resetSmash ();
	}
	public void setSmash(bool Bool){
		animKraken.SetBool ("smash", Bool);
	}

    public void setSpeedParameter(float speed) {
        animKraken.SetFloat("speed", speed);
    }

	public bool isSmashing(){
		return true;
		//return smashTentacle.GetCurrentAnimatorStateInfo (0).IsName ("smash");
	}
	//The kraken needs to emerge
	public void emergeKraken(){
        SoundManager.playSound(SoundClipEnum.KrakenEmerge, SoundCategoryEnum.KrakenStageOne, transform.position);
        animKraken.SetBool("submerge", false);

	}

    public void cancelEmergeKraken()
    {
        animKraken.SetBool("submerge", false);

    }

    public void sinkShip(){
		if (sinkableShip!=null)
			sinkableShip.GetComponent<PlayerInput> ().sinkToYourDeath ();
		sinkableShip = null;
	}
	public void emergeKrakenAttack(GameObject ship){
        SoundManager.playSound(SoundClipEnum.KrakenEmergeAttack, SoundCategoryEnum.KrakenStageOne, transform.position);
        sinkableShip = ship;
		animKraken.SetBool("submerge", false);
		animKraken.SetBool ("underShip", true);
		kraken.attacking = true;
		Invoke ("resetAttack", 2f);

	}

	//The Kraken needs to submerge
	public void submergeKraken(){
		animKraken.SetBool("submerge", true);
	}

	public void submergeKraken(float speed){
		animKraken.SetFloat ("speed",speed);
		animKraken.SetBool("submerge", true);
	}

	public void executeSmash(){
        SoundManager.playSound(SoundClipEnum.KrakenSwipe, SoundCategoryEnum.KrakenStageOne,transform.position);
        animKraken.SetBool("smash", true);

	
	}

	public void setSpeed(float speed){
		animKraken.SetFloat ("speed",speed);
	}

	void resetAttack(){
		kraken.attacking = false;
	}
	public void triggerDeathAnimation(){
		animKraken.SetBool ("death", true);
		animKraken.SetBool ("underShip", false);
	//	animKraken.SetBool ("submerge", false);
	//	animKraken.SetBool ("smash", false);

	}


	public void triggerRespawnAnimation(){

		animKraken.SetBool ("death", false);
	}

	void setupRespawn(){
        SoundManager.playSound(SoundClipEnum.KrakenDrum, SoundCategoryEnum.Generic, transform.position);
        SoundManager.playSound(SoundClipEnum.KrakenRespawn, SoundCategoryEnum.KrakenStageOne,transform.position);
        kraken.setupRespawn ();
	}
	public void onDeathAnimationEnd(int dying){

		if (dying==1 && animKraken.GetBool ("death") == true) { //this function is called at end of dying
			kraken.attacking = false;
			kraken.disableCollisions();
			Invoke("setupRespawn",respawnTime);

		}
		else if (dying==0 && animKraken.GetBool("death") == false){
            resetAnimationVars();
            kraken.rebirth ();
		}

	}

	public bool isCurrentAnimName(string tag){
		return animKraken.GetCurrentAnimatorStateInfo(0).IsTag(tag);
	}

	public void causeSplash(int dying){
		if (this.splashParticles != null) {
			foreach (ParticleSystem system in this.splashParticles) {
				system.Play ();
			}
		}
	}

    public void startFire() {
        animKraken.SetBool("spitCharge", true);
        kraken.spitter.spawnSpit();
        kraken.spitter.spitTime = Time.realtimeSinceStartup;
    }

    public void stopCharging() {
        animKraken.SetBool("spitCharge", false);
    }

    public void actuallyFire() {
        kraken.spitter.Fire();
    }

    public void resetFire() {
        animKraken.SetBool("spit", false);
		kraken.spitter.ResetShots ();   
    }

    public void causeEmergeSplash(int dying)
    {
        if (this.emergeSplashParticles != null)
        {
            foreach (ParticleSystem system in this.emergeSplashParticles)
            {
                system.Play();
            }
        }
    }

    public void resetToIdle(){
		animKraken.CrossFade ("idle", 0.1f);
		animKraken.SetBool("submerge", false);
		animKraken.SetBool ("underShip", false);
		animKraken.SetBool ("death", false);
		CancelInvoke ();
	}


}
