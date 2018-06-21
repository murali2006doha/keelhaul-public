using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Harpoon : MonoBehaviour {
  [SerializeField]
  private float lifeTime;

  [SerializeField]
  private float force;

  [SerializeField]
  private Rigidbody rigidbody;

  [SerializeField]
  private float hitDamage = 0.1f  ;

  [SerializeField]
  private float moveBackSpeed = 10f;

  [SerializeField]
  private LineRenderer line;

  private Vector3 originPosition;
  private List<PlayerInput> victims = new List<PlayerInput>();
  private PlayerInput owner;
  private UnityAction<PlayerInput> onFinish;

  [SerializeField]
  private float harpoonDistance = 0.1f;

  [SerializeField]
  private float harpoonDistanceVictim = 0.1f;

  private UnityAction onEnd;

  private bool pullingBack = false;
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    this.line.SetPosition(0, this.originPosition);
    this.line.SetPosition(1, this.transform.position);
    if (this.pullingBack) {
      if (Vector3.Distance(this.transform.position, this.originPosition) < this.harpoonDistance) {
		    onEnd ();
        Destroy(this.gameObject);
      }

      foreach (var victim in this.victims) {
        if (Vector3.Distance(victim.gameObject.transform.position, this.transform.position) < harpoonDistanceVictim) {
          this.victims.Remove(victim);
          this.onFinish(victim);
        }
      }

      this.transform.position = Vector3.MoveTowards(this.transform.position, this.originPosition, this.moveBackSpeed * Time.deltaTime);
      Debug.Log("reaching here for harpoon");
    }

  }


  public void OnTriggerEnter(Collider other) {
    var otherPlayer = other.transform.root.GetComponent<PlayerInput>();

    if (otherPlayer == null || otherPlayer == this.owner) {
        return;
    }

    if (this.pullingBack) {
      if (victims.Contains(otherPlayer)) {
      }
    } else  if (!otherPlayer.invincible) {
      if (otherPlayer.shipName == ShipEnum.BlackbeardShip.ToString()) {
        if (((BlackbeardAltPower)(otherPlayer.altCannonComponent)).hooking) {
          otherPlayer.altCannonComponent.CancelPower();
        }
      }
      otherPlayer.hit(hitDamage, this.owner.GetId());
      otherPlayer.SetLockedStatus(true, true);
      if (!this.victims.Contains(otherPlayer)) {
        this.victims.Add(otherPlayer);
      }

    }
    
  }

	public void Initialize(UnityAction<PlayerInput> onFinish, UnityAction onEnd, PlayerInput owner) {
    this.onFinish = onFinish;
    this.pullingBack = false;
    this.owner = owner;
    this.rigidbody.AddForce(this.transform.forward * force);
    this.originPosition = this.transform.position;
    this.onEnd = onEnd;
    Invoke("PullBack", this.lifeTime);
  }

  public void Cancel() {
    this.victims.ForEach(victim => victim.SetLockedStatus(false));
    Destroy(this.gameObject);
  }
  private void PullBack() {
    this.pullingBack = true;
    this.rigidbody.isKinematic = true;
    this.rigidbody.useGravity = false;
  }
}
