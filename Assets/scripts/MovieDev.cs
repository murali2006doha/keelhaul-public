/*
 (C) 2015
 your R&D lab
 */
 
 using UnityEngine;
 using UnityEngine.UI;
 using System.Collections;
 using UnityEngine.Events;
 
 public class MovieDev:MonoBehaviour
     {
     
     MovieTexture mt;
     RectTransform rt;
     Vector2 origPos;
     
     void Awake()
         {
         rt = GetComponent<RectTransform>();
         origPos = rt.anchoredPosition;
         
         
         RawImage rim = GetComponent<RawImage>();
         mt = (MovieTexture)rim.mainTexture;
         }
     
     void Update()
         {
         
         if (Input.GetButtonDown("Jump"))
             {
             if (mt.isPlaying)
                 {
                 mt.Stop();
                 }
             else
                 {
                 mt.Stop();
                 mt.Play();
                 }
             }
         
         if (Input.GetKeyDown(KeyCode.W))
             rt.Translate(0f, 1f, 0f);
         if (Input.GetKeyDown(KeyCode.X))
             rt.Translate(0f, -1f, 0f);
             
         if (Input.GetKeyDown(KeyCode.A))
             rt.Translate(-1f, 0f, 0f);
         if (Input.GetKeyDown(KeyCode.D))
             rt.Translate(1f, 0f, 0f);
         
         if (Input.GetKeyDown(KeyCode.S))
             {
             rt.anchoredPosition = origPos;
             rt.eulerAngles = Vector3.zero;
             rt.localScale = new Vector3( 1f, 1f, 1f );
             }
         
         if (Input.GetKeyDown(KeyCode.Q))
             rt.Rotate(0f, 0f, .25f);
         if (Input.GetKeyDown(KeyCode.E))
             rt.Rotate(0f, 0f, -.25f);
         
         if (Input.GetKeyDown(KeyCode.Z))
             {
             float scaleNow = rt.localScale.x;
             if (scaleNow < 0.5f ) return;
             scaleNow = scaleNow - 0.01f;
             rt.localScale = new Vector3( scaleNow, scaleNow, 1f );
             }
         if (Input.GetKeyDown(KeyCode.C))
             {
             float scaleNow = rt.localScale.x;
             if (scaleNow > 1.5f ) return;
             scaleNow = scaleNow + 0.01f;
             rt.localScale = new Vector3( scaleNow, scaleNow, 1f );
             }
         
         }
 }