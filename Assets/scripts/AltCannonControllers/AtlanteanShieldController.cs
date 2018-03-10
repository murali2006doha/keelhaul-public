using UnityEngine;
using System.Collections;

public class AtlanteanShieldController : MonoBehaviour {

	public GameObject parent;
    public float lifeTime;
    public bool protecting;
	public bool isReflecting = true;
	PlayerInput ship;
	public Vector3 offset;
	float originalSpeed;
	Quaternion rot;
    	GameObject originalCannonballPrefab;
    // Use this for initialization

    [SerializeField] private float effectDisableDelayTime = 10f;
	public int absorbPercent;


	void Start () {
		Invoke("KillSelf", lifeTime);
        this.protecting = true;
		rot = Quaternion.Euler(0, 0, -180);
		//isReflecting = true;
		PlayerInput.onHitRegister += AddToHealth;
        parent.GetComponent<PlayerInput>().activateInvincibility(false);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.rotation = rot;
		transform.position = parent.transform.position + offset;
	}

	public void KillSelf() {
		PlayerInput.onHitRegister -= AddToHealth;
        this.protecting = false;
        Invoke("DestroyEffect", effectDisableDelayTime);
		ship.deactivateInvincibility ();
  	}

    private void DestroyEffect() {
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
                break;
            }
        }

    }

}
