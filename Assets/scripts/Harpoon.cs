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

  private List<PlayerInput> victims = new List<PlayerInput>();
  private PlayerInput owner;
  private UnityAction<List<PlayerInput>> onFinish;

  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


  public void OnTriggerEnter(Collider other) {
    var otherPlayer = other.transform.root.GetComponent<PlayerInput>();

    if (otherPlayer == null) {
      return;
    }

    Debug.Log("reaching here");
    otherPlayer.SetLockedStatus(true);
    if (!this.victims.Contains(otherPlayer)) {
      this.victims.Add(otherPlayer);
    }
  }

  public void Initialize(UnityAction<List<PlayerInput>> onFinish, PlayerInput owner) {
    this.onFinish = onFinish;
    this.owner = owner;
    this.rigidbody.AddForce(this.transform.forward * force);
    Invoke("PullBack", this.lifeTime);
  }

  private void PullBack() {
    this.onFinish(this.victims);
    Destroy(this.gameObject);
  }
}
