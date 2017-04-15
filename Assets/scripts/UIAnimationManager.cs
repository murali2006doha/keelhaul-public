using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIAnimationManager : MonoBehaviour {

    public Animator anim;
    public AnimationClip boost;
    public AnimationClip boostRecharged;
    public AnimationClip healthLoss;
    public AnimationClip respawn;
    public AnimationClip alternateFireRecharged;
    public AnimationClip alternateFire;
    public AnimationClip death;
    public AnimationClip bomb;
    public AnimationClip bombExplosion;
    public AnimationClip kill;
    private UnityAction<Sprite> setPortraitAction;
    private ShipEnum shipType;      
    private bool dying;

    private Sprite normalFace;
    private Sprite hitFace;
    private Sprite hurtFace;
    private Sprite pointFace;

    public void Initialize(UnityAction<Sprite> setPortraitAction, ShipEnum type, Sprite normalFace, Sprite hitFace, Sprite hurtFace, Sprite pointFace) {
        this.setPortraitAction = setPortraitAction;
        this.shipType = type;
        this.normalFace = normalFace;
        this.hitFace = hitFace;
        this.hurtFace = hurtFace;
        this.pointFace = pointFace;
    }


    public void onBoost()
    {

        anim.SetTrigger("BoostUse");
    }

    public void onBoostRecharged()
    {
        anim.SetTrigger("BoostReady");
    }

    public void onHit()
    {
        anim.Play(healthLoss.name, 1);
        if (!dying) {
            this.setPortraitAction(this.hitFace);
        }
        
    }

    public void OnHitCompleteMecanim()
    {
        if (!dying) {
            this.setPortraitAction(this.normalFace);
        }
        
    }


    public void OnScore()
    {
        Debug.Log(shipType);
        if (shipType != null) {
            this.setPortraitAction(this.pointFace);
            Invoke("OnScoreCompleteMecanim", 1f);
        }
    }

    public void OnScoreCompleteMecanim()
    {
        this.setPortraitAction(this.normalFace);
    }

    public void onAlternateFire()
    {
        anim.SetBool("AlternateFireUsable", false);
    }

    public void onAlternateFireRecharged()
    {
        anim.SetBool("AlternateFireUsable", true);
    }

    public void onDeath()
    {
        dying = true;
        this.setPortraitAction(this.hurtFace);
        anim.SetBool("dying", true);
    }

    public void OnRespawn() {
        dying = false;
        this.setPortraitAction(this.normalFace);
        anim.SetBool("dying", false);
    }
    public void onBomb()
    {
        anim.Play(bomb.name, 4);
    }

    public void onBombExplosion()
    {
        anim.Play(bombExplosion.name, 5);
    }

    public void onKill()
    {
        anim.Play(kill.name, 6);
    }
}
