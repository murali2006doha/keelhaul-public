using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipMeshPhysicsComponent : MonoBehaviour {

	ShipStats stats;
	UIManager uiManager;
	PlayerInput input;
	
	AbstractGameManager manager;
	HookshotComponent hookshotComponent;
	BombControllerComponent bombController;
    Action<float,int, bool> takeDamageAction { get; set; }
    [Header("Scene Variables")]
	public GameObject rammingSprite;
    public GameObject scoreDestination;

    internal void Initialize(
        PlayerInput input, ShipStats stats, UIManager uiManager, 
		HookshotComponent hookshotComponent, AbstractGameManager manager, BombControllerComponent bombController, Action<float,int,bool> takeDamage) {
		this.input = input;
		this.hookshotComponent = hookshotComponent;
		this.manager = manager;;
		this.uiManager = uiManager;
		this.stats = stats;
		this.bombController = bombController;
        this.takeDamageAction = takeDamage;
	}



	void OnTriggerStay(Collider other) {
		
		if (input != null && input.gameStarted) {

			if ( scoreDestination!=null && other.transform == scoreDestination.transform && hookshotComponent.isHooked() && other.gameObject.tag.Equals("ScoringZone")) {

				handleScoringZone ();

			}

            if (other.transform.GetComponent<IceSpike>()!=null)
            {
                var iceSpike = other.GetComponent<IceSpike>();
                var parentId = iceSpike.parentId;
                if(parentId != input.GetId())
                {
                    takeDamageAction(iceSpike.damage* Time.deltaTime, parentId, false);
                }
            }
        }
	}
		

	//checks collisions for kraken attachs, bombs, and barrel destination
	void OnTriggerEnter(Collider other) {
		
		if (input != null && input.gameStarted) {
            if (LayerMask.LayerToName(other.gameObject.layer).Equals("kraken_arm") && !input.invincible) {

				handleKrakenArm (other);
			}

            if (LayerMask.LayerToName (other.gameObject.layer).Equals ("explosion") && !input.invincible) {
				handleBombExplosion (other);
			}

			if (other.name == "nose") {  
				handleShipNose(other);
			}

			if (other.name == "krakenNose") {
				handleKrakenNose (other);
			}

			if (other.name == "KrakenBubbles") {
				uiManager.triggerShipAlert();
			}

           
        }
	}


	void handleScoringZone () {
		Vector3 newPos = Vector3.Lerp (hookshotComponent.getBarrelPosition(), scoreDestination.transform.position, Time.time);
		hookshotComponent.setBarrelPosition (newPos);
        SabotageGameManager sabManager = (SabotageGameManager) manager;
        sabManager.GetComponent<PhotonView>().RPC("IncrementPoint", PhotonTargets.All,input.GetId());
        input.SendBarrelScoreToKillFeed();
        uiManager.targetBarrel ();
		LightPillar pillar = scoreDestination.transform.parent.GetComponentInChildren<LightPillar> ();
		if (pillar != null) {
			pillar.activatePillar ();
		}
    }

    [PunRPC]
    public void TakeDamage(float damage, int id)
    {
        if (!GetComponent<PhotonView>().isMine) {
            return; 
        }
        this.takeDamageAction(damage, id,false);
    }

void handleBombExplosion (Collider other) {
		
		bool shouldGetHit = true;
		bool stopHitting = false;
		foreach (GameObject bomb in bombController.getBombList ()) {
			//makes sure that the explosion is not coming from a bomb dropped by this ship
			if (bomb != null) { 
				if (other.gameObject.transform.position == bomb.transform.position) {
					shouldGetHit = false;
				}
			}
		}
		if (shouldGetHit && !stopHitting) {
            Explosion exp = other.GetComponent<Explosion>();
            if (exp.id != input.GetId()) {
                this.TakeDamage(exp.damage, exp.id);
                other.enabled = false;
                stopHitting = true;
            }
            
		}
	}


	void handleKrakenNose (Collider other) {
		KrakenInput kraken = other.transform.root.GetComponent<KrakenInput> ();
		if (kraken.animator.isCurrentAnimName ("headbash")) {
			kraken.velocity = 0;
			input.addPushForce (kraken.cc.velocity, kraken.getCurrentWeight () - stats.weight);
		}
	}


	void handleShipNose (Collider other) {
		PlayerInput otherPlayer = other.transform.root.GetComponent<PlayerInput> ();
		if (otherPlayer.velocity > otherPlayer.stats.maxVelocity) {
			Instantiate (rammingSprite, other.transform.position, Quaternion.identity);
			input.addPushForce (otherPlayer.cc.velocity.normalized, Mathf.Max (otherPlayer.stats.weight - stats.weight, 0f));
			otherPlayer.velocity = 0;
		}
	}


	void handleKrakenArm (Collider other) {
		KrakenInput kraken = other.gameObject.transform.root.GetComponent<KrakenInput> ();
	    input.hit (stats.kraken_damage,kraken.id,true);
		other.gameObject.transform.root.GetComponent<KrakenInput> ().vibrate (.5f, .5f);
	}
}
