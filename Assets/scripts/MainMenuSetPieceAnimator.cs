using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSetPieceAnimator : MonoBehaviour {

    [SerializeField]
    Animator mainMenuAnimator;

    [SerializeField]
    Animator krakenAnimator;



    [SerializeField]
    SkinnedMeshRenderer krakenMesh;
    [SerializeField]
    SeagullAnimator[] seagulls;


    [SerializeField]
    GameObject bubbles;


    [SerializeField]
    GameObject mainMenu;

    public void Start() {
        System.Random rand = new System.Random();
        foreach (SeagullAnimator gull in seagulls) {
            gull.Initialize(rand);
        }
    }

    public void SubmergeKraken()
    {
        krakenMesh.enabled = false;
        this.krakenAnimator.SetBool("submerge", true);
    }

    public void EmergeKraken() {

        krakenAnimator.SetFloat("speed", 0.5f);
        krakenMesh.enabled = true;
        this.krakenAnimator.SetBool("underShip", true);
        this.krakenAnimator.SetBool("submerge", false);
    }

    public void BubblesAndSeagulls() {
        bubbles.SetActive(true);
        foreach (SeagullAnimator seagull in seagulls)
        {
            seagull.RandomTakeOff();
        }
    }

}
