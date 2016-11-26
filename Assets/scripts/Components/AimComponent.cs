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
    private Transform shipTransform;

    public void AimAt(Vector3 moveVector)
    {
 
        if (moveVector.magnitude >= minDistance)
        {
            aim.transform.position = Vector3.MoveTowards(aim.transform.position, (shipTransform.position) + (moveVector * maxDistance), moveSpeed);
        }
        
    }

    void AimWithMouseAt(Vector2 mousePos)
    {
        Vector2 normalized = mousePos.normalized;
        Vector2 mousePosCentered = new Vector2(mousePos.x - Screen.width / 2, mousePos.y - Screen.height / 2);
        float magniture = mousePosCentered.magnitude;
        float ratio = Screen.width / Screen.height;
        aim.transform.position =  new Vector3(normalized.x * Mathf.Max(Mathf.Min(magniture * mouseClamp, maxDistance), minDistance), 0f, normalized.y * Mathf.Max(Mathf.Min(magniture * mouseClamp, maxDistance), minDistance) * ratio);

    }

   
   

    internal void Initialize(Transform shipTransform)
    {
        this.shipTransform = shipTransform;
    }

    
}
