using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenMouth : MonoBehaviour {

    public Rigidbody rb;
    bool hittingBarrel = false;
    GameObject barrel;
	// Use this for initialization
	void Start () {
        barrel = GameObject.FindObjectOfType<Barrel>().gameObject;
    }

    public void hook() {
        if (hittingBarrel) {
            if (barrel.GetComponent<Barrel>().owner)
            {
                barrel.GetComponent<Barrel>().owner.GetComponent<HookshotComponent>().UnHook();
            }

            barrel.GetComponent<CharacterJoint>().connectedBody = rb;
            barrel.GetComponent<Barrel>().owner = this.gameObject;
        }
    }


    public bool isHooked() {
//        return (barrel.GetComponent<Barrel>().owner == this.gameObject);
		return false;
    }

    public void UnHook() {
        if (isHooked()) {
            barrel.GetComponent<CharacterJoint>().connectedBody = null;
            barrel.GetComponent<Barrel>().owner = null;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("barrel"))
        {

            hittingBarrel = true;
            GetComponent<Renderer>().material.color = Color.yellow;
            barrel = other.gameObject;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("barrel"))
        {
            hittingBarrel = false;
            GetComponent<Renderer>().material.color = Color.white;

        }
    }

}
