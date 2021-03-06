﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class CannonBall : Photon.MonoBehaviour {

  protected Transform owner;
  private float timeAlive;
  public GameObject shipHit;
  public float lifeTime = 4f;
  public GameObject krakenHit;
  public GameObject normalHit;
  public GameObject shieldHit;
  public GameObject splash;
  public KrakenInput kraken;
  public float gravity = -1.5f;
  public float force;
  public float reflectForce = 100;
  public int reflectMult = 1;
  public float damage = 1;
  protected bool splashed = false;
  public float pushMagnitude = 0f;
  public bool reflected = false;

	private Vector3 initialForce;
  void Start() {
    Invoke("destroySelf", lifeTime);
  }

  public void OnTriggerEnter(Collider collider) {

    if (!GetComponent<PhotonView>().isMine) {
      return;
    }
    if (collider.transform.root != owner) {
      if (collider.transform.root.gameObject.name.Contains("Force")) {
        var shield = collider.transform.root.gameObject.GetComponent<AtlanteanShieldController>();
        var parent = shield.parent;
        Debug.Log("hitting shield");

        if (parent.transform == owner) {
          return;
        } else if (shield.isReflecting) {
          CancelInvoke();

          reflectMult++;
          var opponent = owner;
          owner = parent.transform;
          reflected = true;
          //	this.GetComponent<Rigidbody> ().transform.Rotate (0, 180, 0);

          this.transform.LookAt(opponent);
          this.GetComponent<Rigidbody>().velocity = new Vector3();
					this.GetComponent<Rigidbody>().AddForce((new Vector3(initialForce.x * -1, initialForce.y, initialForce.z * -1) * reflectMult));
          Invoke("destroySelf", lifeTime);

          parent.GetComponent<PlayerInput>().gameStats.numOfReflectedShots++;
        } else {



          if (!kraken) {
            ShipMeshPhysicsComponent mesh = shield.parent.GetComponent<PlayerInput>().shipMeshComponent;
            //ShipMeshPhysicsComponent meshb = shield.GetComponent<ShipMeshPhysicsComponent>();

            PlayerInput player = owner.GetComponent<PlayerInput>();
            player.gameStats.numOfShotHits++;

            mesh.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, damage, player.GetId());
            destroySelf();

          } else {
            kraken.gameStats.numOfShotHits++;
          }
          if (shield.protecting) {
            Instantiate(shieldHit, transform.position, transform.rotation);
          } else {
            Instantiate(normalHit, transform.position, transform.rotation);
          }

          destroySelf();
        }
      } else {
        if (LayerMask.LayerToName(collider.gameObject.layer).Contains("playerMesh")) {
          ShipMeshPhysicsComponent mesh = collider.GetComponent<ShipMeshPhysicsComponent>();

          Instantiate(shipHit, transform.position, transform.rotation);

          if (kraken) {
            kraken.gameStats.numOfShotHits++;
            if (mesh != null) {
              //controller.hit(damage * reflectMult, kraken);
              //controller.addPushForce(this.GetComponent<Rigidbody>().velocity.normalized, pushMagnitude);
              mesh.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, damage);
            }
          } else {
            PlayerInput player = owner.GetComponent<PlayerInput>();
            player.gameStats.numOfShotHits++;
            if (mesh != null) {
              List<float> valuesToSend = new List<float>(new float[] { damage, (float)player.playerId });
              mesh.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, damage, player.GetId());
            }
          }



        } else if (LayerMask.LayerToName(collider.gameObject.layer).Equals("kraken") && !kraken) {
          PlayerInput player = owner.GetComponent<PlayerInput>();
          player.gameStats.numOfShotHits++;
          KrakenInput controller = collider.GetComponentInParent<KrakenInput>();
          Instantiate(krakenHit, transform.position, transform.rotation);
          if (controller != null) {
            controller.hit(damage * reflectMult, player.GetId());
          }

        } else {
          Instantiate(normalHit, transform.position, transform.rotation);
        }
        destroySelf();


      }

    }
  }

  internal Vector3 GetVelocity() {
    return GetComponent<Rigidbody>().velocity;
  }

  virtual public void destroySelf() {
    Destroy(this.gameObject);
  }

  void FixedUpdate() {
    this.GetComponent<Rigidbody>().AddForce(new Vector3(0, gravity, 0));
    CheckSplash();
  }

  public virtual void CheckSplash() {
    if (this.transform.position.y < 0.01f && !splashed) {
      Instantiate(splash, transform.position, transform.rotation);
      splashed = true;
      Invoke("destroySelf", lifeTime);
    }
  }

  [PunRPC]
  public void AddForce(Vector3 force) {
		this.initialForce = force;
    GetComponent<Rigidbody>().AddForce(force);
  }
  public void setOwner(Transform owner) {
    this.owner = owner;
  }

  public PlayerInput getOwner() {
    return owner.GetComponent<PlayerInput>();
  }
}