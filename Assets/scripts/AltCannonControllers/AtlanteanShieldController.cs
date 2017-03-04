using UnityEngine;
using System.Collections;

public class AtlanteanShieldController : MonoBehaviour {

	public GameObject parent;
    public float lifeTime;
	public float powerShieldDuration;
	public bool isReflecting = false;
	PlayerInput ship;
	public Vector3 offset;
	float originalSpeed;
	Quaternion rot;
    GameObject originalCannonballPrefab;
	// Use this for initialization
	public int absorbPercent;


	void Start () {
		Invoke("DisablePowerShield", powerShieldDuration);
		Invoke("KillSelf", lifeTime);
		rot = Quaternion.Euler(0, 0, -180);
		isReflecting = true;
		PlayerInput.onHitRegister += AddToHealth;
		parent.GetComponent<PlayerInput> ().invincible = true;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.rotation = rot;
		transform.position = parent.transform.position + offset;
	}

	void KillSelf() {

        ship.centralCannon.DeAmpCannonball();
		PlayerInput.onHitRegister -= AddToHealth;
		parent.GetComponent<PlayerInput> ().invincible = false;
        PhotonNetwork.Destroy(GetComponent<PhotonView>());

  	}

	void AddToHealth() {
		parent.GetComponent<PlayerInput> ().AddToHealth ((absorbPercent / 100f));
	}

	void DisablePowerShield(){
		isReflecting = false;

	}

    [PunRPC]
    public void SetUpParent(int id) {
        var players = FindObjectsOfType<PlayerInput>();
        foreach(PlayerInput player in players)
        {
            if(player.GetId() == id)
            {
                parent = player.gameObject;
                ship = player;
                ship.centralCannon.AmpUpCannonball();
                break;
            }
        }

    }

}
