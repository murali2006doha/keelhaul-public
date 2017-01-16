﻿using UnityEngine;
using System.Collections.Generic;
public class CannonBall : Photon.MonoBehaviour {

	private Transform owner;
	private float timeAlive;
	public GameObject shipHit;
	public float lifeTime = 2.5f;
	public GameObject krakenHit;
	public GameObject normalHit;
	public GameObject splash;
    public KrakenInput kraken;
	public float gravity = -1.5f;
	public float force;
	public float reflectForce = 100;
	public int reflectMult = 1;
	public float damage = 1;
	bool splashed = false;
    public float pushMagnitude =0f;
	public bool reflected = false;
    PhotonView view;
	void Start(){
		Invoke ("destroySelf", lifeTime);
        view = GetComponent<PhotonView>();
	}

	public void OnTriggerEnter(Collider collider){

        if (!view.isMine) {
            return;
        }
		if (collider.transform.root != owner) {
			if (collider.transform.root.gameObject.name.Contains ("Force")) {
				var shield = collider.transform.root.gameObject.GetComponent<AtlanteanShieldController> ();
				var parent = shield.parent;
				if (parent.transform == owner) {
					return;
				} else if (shield.isReflecting) {
					CancelInvoke ();

					reflectMult++;
                    var opponent = owner;
					owner = parent.transform;
					reflected = true;
                    //	this.GetComponent<Rigidbody> ().transform.Rotate (0, 180, 0);
                    
                    this.transform.LookAt(opponent);
					this.GetComponent<Rigidbody> ().velocity = new Vector3 ();
					this.GetComponent<Rigidbody> ().AddForce (this.transform.forward * reflectForce * reflectMult);
					Invoke ("destroySelf", lifeTime);
					parent.GetComponent<PlayerInput> ().gameStats.numOfReflectedShots++;
				} else {
                    if (!kraken)
                    {
                        PlayerInput player = owner.GetComponent<PlayerInput>();
                        player.gameStats.numOfShotHits++;
                    } else {
                        kraken.gameStats.numOfShotHits++;
                    }
					
					Instantiate (normalHit, transform.position, transform.rotation);
					Destroy (gameObject);
				}
			} else {
				if (LayerMask.LayerToName (collider.gameObject.layer).Contains ("playerMesh")) {
                    ShipMeshPhysicsComponent mesh = collider.GetComponent<ShipMeshPhysicsComponent>();
                   
                    Instantiate(shipHit, transform.position, transform.rotation);

                    if (kraken)
                    {
                        kraken.gameStats.numOfShotHits++;
                        if (mesh != null)
                        {
                            //controller.hit(damage * reflectMult, kraken);
                            //controller.addPushForce(this.GetComponent<Rigidbody>().velocity.normalized, pushMagnitude);
                            mesh.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, damage);
                        }
                    }
                    else {
                        PlayerInput player = owner.GetComponent<PlayerInput>();
                        player.gameStats.numOfShotHits++;
                        if (mesh != null)
                        {
                            List<float> valuesToSend = new List<float>(new float[] {damage, (float)player.playerId });
                            mesh.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, damage, player.playerId);
                        }
                    } 
					
				

				} else if (LayerMask.LayerToName (collider.gameObject.layer).Equals ("kraken") && !kraken) {
					PlayerInput player = owner.GetComponent<PlayerInput> ();
					player.gameStats.numOfShotHits++;
					KrakenInput controller = collider.GetComponentInParent<KrakenInput> ();
					Instantiate (krakenHit, transform.position, transform.rotation);
					if (controller != null) {
						controller.hit (damage * reflectMult,player);
					}

				} else {
					Instantiate (normalHit, transform.position, transform.rotation);
				}

                PhotonNetwork.Destroy(GetComponent<PhotonView>());

			}

		}
	}
	void destroySelf(){
        PhotonNetwork.Destroy(GetComponent<PhotonView>());
    }

	void FixedUpdate(){
		this.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, gravity, 0));
		if (this.transform.position.y < 0.01f && !splashed) {
			Instantiate (splash, transform.position, transform.rotation);
			splashed = true;
			Invoke ("destroySelf", lifeTime);
		}
	}

	public void setOwner(Transform owner) {
		this.owner = owner;
	}
}