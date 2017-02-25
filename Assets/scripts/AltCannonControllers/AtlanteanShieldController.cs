using UnityEngine;
using System.Collections;

public class AtlanteanShieldController : MonoBehaviour {

	public GameObject parent;
    public GameObject ampedUpCannonballPrefab;
    public float lifeTime;
	public float powerShieldDuration;
	public bool isReflecting = false;
	PlayerInput ship;
	public Vector3 offset;
	float originalSpeed;
	Quaternion rot;
    GameObject originalCannonballPrefab;
	// Use this for initialization


	void Start () {
		parent = GetComponent<SecondaryFire>().parent;
		ship = parent.GetComponent<PlayerInput>();
        ship.centralCannon.AmpUpCannonball();
		Invoke("DisablePowerShield", powerShieldDuration);
		Invoke("KillSelf", lifeTime);
		rot = Quaternion.Euler(0, 0, -180);
		isReflecting = true;
		//ship.activateInvincibility ();
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.rotation = rot;
		transform.position = parent.transform.position + offset;
	}

	void KillSelf() {
		ship.deactivateInvincibility ();
        ship.centralCannon.DeAmpCannonball();
        Destroy(this.gameObject);
	}

	void DisablePowerShield(){
		isReflecting = false;
	}

}
