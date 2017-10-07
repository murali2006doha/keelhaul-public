using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * This class attempts to force VERT- Field of view scaling.
 * By default, Unity uses the HOR+ technique.
 * 
 * http://en.wikipedia.org/wiki/Field_of_view_in_video_games#Scaling_methods
 */

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class FOVScript : MonoBehaviour
{

    private float prevWidth;    
    Dictionary<Camera, float> camToDefaultVal = new Dictionary<Camera, float>();
    public float horizontalResolution = 1920;

    void Start()
    {

        foreach (Camera cam in FindObjectsOfType(typeof(Camera)))
        {
            camToDefaultVal.Add(cam, cam.orthographicSize);
        }
        prevWidth = horizontalResolution;
    }

    void Update()
    {
       
        if (Screen.width != prevWidth)
        {

            float currentAspect = (float)Screen.width / (float)Screen.height;
            
            float desiredAspect = 16f / 9f;
           
           
            print("Screen resolution change" + Screen.width + "-" + Screen.height);

            foreach (Camera cam in FindObjectsOfType(typeof(Camera)))
            {
                float cameraHeight = camToDefaultVal[cam];
                float aspect = cam.aspect;
                float ratio = desiredAspect / aspect;
                cam.orthographicSize = cameraHeight * ratio;
            }

            //Comment to always run
            prevWidth = Screen.width;
            
        }

    }
   
        

    

}