using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackbeardAltPower : AbstractAltCannonComponent {

    public float altPowerLength;
    public int altDamageMultiplier;
    public int altFiringDelayMultiplier;
    public int altSpeedMultiplier;

    float origDamage;
    float origFiringDelay;
    float origSpeed;
    float origCameraSpeed;


    void ChangeStats () {

        origFiringDelay = this.stats.shootDelay;
        origSpeed = this.input.motor.getSpeedModifier ();
        origCameraSpeed = this.input.followCamera.followSpeed;

        this.stats.shootDelay = this.stats.shootDelay / altFiringDelayMultiplier;
        this.input.motor.setSpeedModifier (this.input.motor.getSpeedModifier () * altSpeedMultiplier);
        this.input.followCamera.followSpeed = this.input.followCamera.followSpeed * altSpeedMultiplier;
		this.transform.parent.GetComponentInChildren<BlackbeardCannonComponent>().setDamageMultiplier(altDamageMultiplier);
    }


    public override void setupRotation() {
        shoot_direction = aim.transform.position - shipTransform.position;
        this.transform.rotation = Quaternion.LookRotation (shoot_direction.normalized); 
    }
        

    public override void alternateFire() {
        ChangeStats ();
        Invoke ("ResetStats", altPowerLength);
    }


    void ResetStats() {

        this.stats.shootDelay = origFiringDelay;
        this.input.motor.setSpeedModifier (origSpeed);
        this.input.followCamera.followSpeed = origCameraSpeed;
		this.transform.parent.GetComponentInChildren<BlackbeardCannonComponent>().resetDamageMultiplier();
    }
}
