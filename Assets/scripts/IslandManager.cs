using UnityEngine;
using System.Collections.Generic;

public class IslandManager : MonoBehaviour {

	public GameObject[] smokeSetOne;
	public GameObject[] smokeSetTwo;
	public GameObject[] smokeSetThree;
	public List<PlayerInput> enemyShips = new List<PlayerInput>();


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		manageDamageSmoke ();	
		//print (enemyShip.uiManager.enemyIslandHealthBar.value);
	}

	void manageDamageSmoke() {
        foreach(PlayerInput enemyShip in enemyShips)
        {
            if (enemyShip.uiManager.enemyIslandHealthBar.value == 2f)
            {
                foreach (GameObject smoke in smokeSetOne)
                {
                    smoke.SetActive(true);
                }

            }
            else if (enemyShip.uiManager.enemyIslandHealthBar.value < 1.5f && enemyShip.uiManager.enemyIslandHealthBar.value > 0.5f)
            {
                foreach (GameObject smoke in smokeSetTwo)
                {
                    smoke.SetActive(true);
                }

            }
            else if (enemyShip.uiManager.enemyIslandHealthBar.value == 0f)
            {

                foreach (GameObject smoke in smokeSetThree)
                {
                    smoke.SetActive(true);
                }
            }

        }
		
	}
}
