using UnityEngine;
using System.Collections;

public class colorSwapper : MonoBehaviour {
    public float green_min;
    public float red_min;
    public float blue_min;
    public float green_max;
    public float red_max;
    public float blue_max;

    public Color col;
    // Use this for initialization
    void Start () {
        float i = Random.Range (0, 9);
        float green = Random.Range (green_min, green_max);
        float red = Random.Range (red_min, red_max);
        float blue = Random.Range(blue_min,blue_max);

        col = new Color (green/255f, red/255f, blue/255f);
        GetComponent<Renderer>().material.SetColor("_TintColor", col);
    }
    
  
}
