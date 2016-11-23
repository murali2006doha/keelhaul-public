using UnityEngine;
using System.Collections;

public class destructibleEnvironment : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }


    void OnTriggerEnter(Collider collider) {
        GameObject collidingObject = collider.gameObject;

        if (collidingObject.name == "nose") {
            PlayerInput player = collidingObject.transform.root.GetComponent<PlayerInput>();
            if (player.cc.velocity.magnitude > GlobalVariables.minCcVelocity) {
                Destroy(this.gameObject);
            }
        }
        if (LayerMask.LayerToName(collidingObject.layer).Contains("cannonball")) {
            Destroy(this.gameObject);
        }
    }
}