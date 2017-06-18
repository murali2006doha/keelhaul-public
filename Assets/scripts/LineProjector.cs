using UnityEngine;

public class LineProjector : MonoBehaviour {

    public string[] layersMask = new string[] { "environment","playerMesh" };
   
    public GameObject projector;
    public GameObject line;
    private float zscale = 1f;

    void FixedUpdate () {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f, LayerMask.GetMask(layersMask)))
        {
           projector.SetActive(true);
            var point = hit.point;
            point.y = 1f;
            point.y = 0;
            var distance = point - this.transform.position;
           this.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (distance.magnitude / 2.3f) * zscale);
            projector.transform.position = point;
        }
        else
        {
            this.transform.localScale = Vector3.Lerp(transform.localScale,new Vector3(transform.localScale.x, transform.localScale.y, zscale),0.5f);
            projector.SetActive(false);
        }
        

    }

    
}
