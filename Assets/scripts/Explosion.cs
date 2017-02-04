using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float damage;
    public int id;

    [PunRPC]
    public void Initialize(float damage, int id) {
        this.damage = damage;
        this.id = id;
    }

}
