using UnityEngine;
using System.Collections;

public class SecondaryFire:MonoBehaviour {

    public GameObject parent;
    public float force;
    public SecondaryFire(GameObject parent) {
        this.parent = parent;
    }

    [PunRPC]
    public void AddForce(float force)
    {
        this.force = force;
    }
}
