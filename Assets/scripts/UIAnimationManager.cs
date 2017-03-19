using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void onBoost()
    {
        anim.Play(boost.name);
    }

    public void onBoostRecharged()
    {
        anim.Play(boostRecharged.name);
    }

    public void onHit()
    {
        anim.Play(healthLoss.name, 1);
    }

    public void onAlternateFire()
    {
        anim.Play(respawn.name,2);
    }

    public void onAlternateFireRecharged()
    {
        anim.Play(respawn.name, 2);
    }

    public void onDeath()
    {
        anim.Play(death.name, 3);
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
