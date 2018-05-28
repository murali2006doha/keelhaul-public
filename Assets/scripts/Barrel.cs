using UnityEngine;
using System.Collections;
using System;

public class Barrel : MonoBehaviour
{
    public GameObject owner;
    public LightPillar pillar;
    public GameObject explosion;
    PlayerInput currentPlayer;
    bool shouldDenotate = true;
    internal bool isScoring = false;

    [HideInInspector]
    public bool autoHook = true;

    // Use this for initialization
    void Start()
    {
        pillar = GetComponentInChildren<LightPillar>();
        currentPlayer = FindObjectOfType<PlayerInput>();
        if (autoHook)
        {
            Destroy(GetComponent<CharacterJoint>());
        }
    }

    public void activatePillar()
    {
        pillar.activatePillar();
    }


    public void explodeBarrel()
    {
        if (shouldDenotate)
        {
            GameObject exp = (GameObject)Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity);
        }

    }

    internal void disableDetonate()
    {
        shouldDenotate = false;
    }
}