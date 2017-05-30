using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineProjector : MonoBehaviour {

    public string[] layersMask = new string[] { "environment","playerMesh" };
   
    public GameObject projector;
    public float zscale = 0.1f;

    void FixedUpdate () {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f))
        {
                projector.SetActive(true);
            var point = hit.point;
            point.y = 0f;
            
            projector.transform.position = point;
            point.y = 0;
            var distance = point - this.transform.position;
            this.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (distance.magnitude / 2.7f) * zscale);
        }
        else
        {
            this.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,zscale);
            projector.SetActive(false);
        }
        

    }

    
}
