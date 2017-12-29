using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMove : MonoBehaviour {

    public Transform startPos;
    public float val = 2.5f;
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(startPos.position, -Vector3.up, out hit,1000f, LayerMask.GetMask(new string[] { "Water" })))
        {
            
           this.transform.localPosition = MathHelper.setY(this.transform.localPosition, hit.point.y- val);
          
        }

        if (Physics.Raycast(startPos.position, Vector3.up, out hit, 1000f, LayerMask.GetMask(new string[] { "Water" })))
        {
            
            this.transform.localPosition = MathHelper.setY(this.transform.localPosition, hit.point.y - val);
            
        }



    }
}
