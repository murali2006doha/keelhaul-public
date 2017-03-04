using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class BlackbeardCannonComponent : ShipCannonComponent {

    ShipCannonComponent cannonComponent;
    bool canMultiplyDamage = false;
    float damageMultiplier = 1f;

    [Serializable]
    public struct cannonToCannonBall {
        public String cannonBallPath;
    }

    public cannonToCannonBall[] prefabs;

    public override void Fire() {

        var angle = Vector3.Angle(shipTransform.forward, this.transform.forward);
        Vector3 vect = Vector3.zero;
        if (angle > 0 && angle < 45)
        {
            vect = this.transform.forward * GlobalVariables.gameSpeed * forceMultiplier * speed;
        }
        else if (angle > 45 && angle < 100)
        {
            vect = this.transform.forward * GlobalVariables.gameSpeed * forceMultiplier / 2f * speed;
        }
        Vector3 cal = (aim.transform.position - this.transform.position);
        cal.y = 0;
        Vector3 look = MathHelper.setY(this.transform.rotation.eulerAngles, Quaternion.LookRotation(cal).eulerAngles.y);
        Vector3 rot = MathHelper.addY(look, (numOfCannonBalls / 2) * -angleOfCannonShots);

        for (int x = 0; x < prefabs.Length; x++) {
            cannonToCannonBall prefab = prefabs [x];
            Vector3 newRot = MathHelper.addY (rot, x * angleOfCannonShots);
            GameObject cannonBall = PhotonNetwork.Instantiate (prefab.cannonBallPath, cannonBallPos.position + (velocity * dampening), Quaternion.Euler (newRot), 0);
            cannonBall.transform.rotation = Quaternion.Euler (newRot);
            cannonBall.GetComponent<CannonBall> ().setOwner (transform.root);
            cannonBall.GetComponent<CannonBall> ().damage = cannonBall.GetComponent<CannonBall> ().damage * damageMultiplier;

            Vector3 forwardForce = cannonBall.transform.forward * cannonForce + vect;
            Vector3 upForce = cannonBall.transform.up * arcCannonForce;
            cannonBall.GetComponent<PhotonView> ().RPC ("AddForce", PhotonTargets.All, upForce + forwardForce);
        }

        this.gameStats.numOfShots += this.numOfCannonBalls;

    }



    public void setDamageMultiplier(float damageMultiplier) {
        this.damageMultiplier = damageMultiplier;
    }


	public void resetDamageMultiplier() {
		this.damageMultiplier = 1f;
	}


        
}
