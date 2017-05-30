using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpike : MonoBehaviour {
    public int parentId;
    public float damage = 0.1f;
	// Use this for initialization
	void Start () {
        LeanTween.scale(this.gameObject,new Vector3(30,40,30),0.5f); //MagIC numbers
        LeanTween.moveY(gameObject, 0.3f, 0.5f); //MaGiC
	}

    [PunRPC]
    public void SetSpikeParent(int id)
    {
        parentId = id;
    }

}
