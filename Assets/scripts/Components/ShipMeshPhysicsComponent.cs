using System;
using UnityEngine;

public class ShipMeshPhysicsComponent : MonoBehaviour {

	ShipStats stats;
	UIManager uiManager;
	PlayerInput input;
	GameObject scoreDestination;
	AbstractGameManager manager;
	HookshotComponent hookshotComponent;
	BombControllerComponent bombController;
    Action<float> takeDamageAction { get; set; }
    [Header("Scene Variables")]
	public GameObject rammingSprite;

	internal void Initialize(
        PlayerInput input, ShipStats stats, UIManager uiManager, GameObject scoreDestination, 
		HookshotComponent hookshotComponent, AbstractGameManager manager, BombControllerComponent bombController, Action<float> takeDamage) {
		this.input = input;
		this.scoreDestination = scoreDestination;
		this.hookshotComponent = hookshotComponent;
		this.manager = manager;;
		this.uiManager = uiManager;
		this.stats = stats;
		this.bombController = bombController;
        this.takeDamageAction = takeDamage;
	}



	void OnTriggerStay(Collider other) {
		
		if (input != null && input.gameStarted) {

			if (other.transform == scoreDestination.transform && hookshotComponent.isHooked() && other.gameObject.tag.Equals("ScoringZone")) {

				handleScoringZone ();

			}
		}
	}
		

	//checks collisions for kraken attachs, bombs, and barrel destination
	void OnTriggerEnter(Collider other) {
		
		if (input != null && input.gameStarted) {

			if (LayerMask.LayerToName(other.gameObject.layer).Equals("kraken_arm") && !input.invincible) {
				print (other);

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
		manager.acknowledgeBarrelScore (input, hookshotComponent.barrel);
		//manager.incrementPoint (this, hook_component.barrel);
		uiManager.targetBarrel ();
		LightPillar pillar = scoreDestination.transform.parent.GetComponentInChildren<LightPillar> ();
		if (pillar != null) {
			pillar.activatePillar ();
		}
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        this.takeDamageAction(damage);
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
			Collider cl = input.gameObject.GetComponent<Collider> ();
			bombController.bomb.DestroyShip (other.gameObject, cl);
			other.enabled = false;
			stopHitting = true;
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
	//	input.hit (stats.kraken_damage);
		other.gameObject.transform.root.GetComponent<KrakenInput> ().vibrate (.5f, .5f);
	}
}
