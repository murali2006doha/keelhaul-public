using UnityEngine;
using System.Collections;
using System;

public class explosiveEnvironment : MonoBehaviour {

    public GameObject volumetricExplosion;
    public string[] destructibleLayers = { "playerMesh", "cannonball", "kraken", "kraken_arm", "explosion" };
    public float delayTime = 0.5f;
    bool dying = false;
    void OnTriggerEnter(Collider collider)
    {
        string colliderLayer = LayerMask.LayerToName(collider.gameObject.layer);
        if (Array.IndexOf(destructibleLayers, LayerMask.LayerToName(collider.gameObject.layer)) > -1 && !dying) {
            if (colliderLayer == "explosion")
            {
                dying = true;
                Invoke("instantiateExplosion", delayTime);
            }
            else {
                dying = true;
                instantiateExplosion();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Array.IndexOf(destructibleLayers, LayerMask.LayerToName(collision.collider.gameObject.layer)) > -1 && !dying)
        {
            dying = true;
            instantiateExplosion();
        }
    }

    void instantiateExplosion() {
        Instantiate(volumetricExplosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
