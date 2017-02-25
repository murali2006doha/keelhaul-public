using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingFreeze : MonoBehaviour {

    public GameObject parent;
    public float lifeTime;
    PlayerInput ship;
    public Vector3 offset;
    float originalSpeed;
    Quaternion rot;

    void Start()
    {
        Invoke("KillSelf", lifeTime);
        rot = Quaternion.Euler(0, 0, -180);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = rot;
        transform.position = parent.transform.position + offset;
    }

    void KillSelf()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            ship.centralCannon.DeAmpCannonball();
            PhotonNetwork.Destroy(GetComponent<PhotonView>());

        }
    }

    [PunRPC]
    public void ActivateFreeze(int id)
    {
        var players = FindObjectsOfType<PlayerInput>();
        foreach (PlayerInput player in players)
        {
            if (player.GetId() == id)
            {
                parent = player.gameObject;
                ship = player;
                ship.centralCannon.AmpUpCannonball();
                break;
            }
        }

    }
}
