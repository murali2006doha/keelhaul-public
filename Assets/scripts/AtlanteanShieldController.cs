using UnityEngine;
using System.Collections;

public class AtlanteanShieldController : MonoBehaviour {

	public GameObject parent;
	public float lifeTime;
	public float powerShieldDuration;
	public bool isReflecting = false;
	playerInput ship;
	public Vector3 offset;
	float originalSpeed;
	Quaternion rot;
	// Use this for initialization


	void Start () {
		parent = GetComponent<SecondaryFire>().parent;
		ship = parent.GetComponent<playerInput>();
		Invoke("DisablePowerShield", powerShieldDuration);
		Invoke("KillSelf", lifeTime);
		rot = this.transform.rotation;
		isReflecting = true;
		ship.activateInvincibility ();
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.rotation = rot;
		transform.position = parent.transform.position + offset;
	}

	void KillSelf() {
		ship.deactivateInvincibility ();
		Destroy(this.gameObject);
	}

	void DisablePowerShield(){
		isReflecting = false;
	}

}
