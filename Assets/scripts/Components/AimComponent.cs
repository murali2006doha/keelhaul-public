using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimComponent : MonoBehaviour {


    int maxDistance = 2;
    float minDistance = 0.5f;
    float moveSpeed = 10f;
    float mouseClamp = 0.005f;

    public GameObject aim;
    public GameObject line;
    private Transform shipTransform;
    private bool keyboardControls;
    private Camera cam;

    public void AimAt(Vector3 moveVector)
    {
        if (keyboardControls)
        {
            AimWithMouseAt(new Vector2(moveVector.x, moveVector.y));
        }
        else if (moveVector.magnitude >= minDistance)
        {
            aim.transform.position = Vector3.MoveTowards(aim.transform.position, (shipTransform.position) + (moveVector * maxDistance), moveSpeed);
            var pos = aim.transform.position;
            pos.y = 0;
            line.transform.LookAt(pos);
        }
        
    }

    void AimWithMouseAt(Vector2 mousePos)
    {
        //TODO: Instead of screen to world - use delta mouse x,y

        /*Requires z to calculate cam to world point: given by 134f to get to middle of screen +/- y pos of mouse relative to middle of screen*/
        var ray = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane+34f+ (mousePos.y - Screen.height / 2) / (Screen.height / 2)*6));

        aim.transform.position = new Vector3(ray.x,0.1f,ray.z);
        var pos = aim.transform.position;
        pos.y = 0;
        line.transform.LookAt(pos);
        //Aim clamp
        if ((aim.transform.position - this.shipTransform.position).magnitude > maxDistance)
        {
            var newVec = Vector3.ClampMagnitude((aim.transform.position - this.shipTransform.position), maxDistance);
            aim.transform.position = (shipTransform.position) + newVec;
        }
    
    }




    internal void Initialize(Transform shipTransform,bool keyboardControls,Camera cam)
    {
        this.shipTransform = shipTransform;
        this.keyboardControls = keyboardControls;
        this.cam = cam;
    }

    
}
