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
    private UnityAction<string,string> setPortraitAction;
    private ShipEnum shipType;
    private bool dying;
    public void Initialize(UnityAction<string,string> setPortraitAction, ShipEnum type) {
        this.setPortraitAction = setPortraitAction;
        this.shipType = type;
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
            this.setPortraitAction(PathVariables.GetAssociatedPortraitPath(shipType) + "Hit", "");
        }
        
    }

    public void OnHitCompleteMecanim()
    {
        if (!dying) {
            this.setPortraitAction(PathVariables.GetAssociatedPortraitPath(shipType), "");
        }
        
    }


    public void OnScore()
    {
        Debug.Log(shipType);
        if (shipType != null) {
            this.setPortraitAction(PathVariables.GetAssociatedPortraitPath(shipType) + "Point", "");
            Invoke("OnScoreCompleteMecanim", 1f);
        }
    }

    public void OnScoreCompleteMecanim()
    {
        this.setPortraitAction(PathVariables.GetAssociatedPortraitPath(shipType), "");
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
        this.setPortraitAction(PathVariables.GetAssociatedPortraitPath(shipType) + "Hurt", "");
        anim.SetBool("dying", true);
    }

    public void OnRespawn() {
        dying = false;
        this.setPortraitAction(PathVariables.GetAssociatedPortraitPath(shipType), "");
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
