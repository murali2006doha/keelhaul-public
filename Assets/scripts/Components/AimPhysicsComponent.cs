using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPhysicsComponent : MonoBehaviour {


    GameObject barrel;
    public GameObject aim;
    bool isAimOnBarrel;
    bool changeColor = true;

    public bool isAimTouchingBarrel()
    {
        return isAimOnBarrel;
    }

    public void enableColorChange(bool enable)
    {
        changeColor = enable;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("barrel"))
        {
            if (changeColor)
            {
                aim.GetComponent<Renderer>().material.color = Color.yellow;
            }
            isAimOnBarrel = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("barrel"))
        {

            aim.GetComponent<Renderer>().material.color = Color.white;
            isAimOnBarrel = false;
        }
    }
}
