using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class ShipInstantiator : MonoBehaviour {

	[System.Serializable]
	public class ShipInformation
	{
		public ShipEnum type;
		public ShipStats stats;
		public GameObject ship;
		public string name;
		public GameObject altFirePrefab;
		public Sprite altFireSprite;
		public Sprite altFireOutline;
        public Sprite portrait;

    }

	public ShipInformation[] ships;
	public GameObject splashParticle;


	public void setupShipNames(PlayerInput ship, ShipEnum type, int num, int numOfBases, int id){
		MapObjects mapObjects = GameObject.FindObjectOfType<MapObjects> ();

		ShipInformation info = getShip (type);
		ship.shipName = info.name;
		ship.stats = info.stats;
		if (ship.shipMesh == null) {
			InstantiateShipMesh (info ,ship);
		}


        //Refactor later

        int numKraken = GameObject.FindObjectOfType<KrakenInput>() ? 1 : 0;

        int newLayer = LayerMask.NameToLayer("p" + (num + numKraken) + "_ui");
        for (int i = 0; i < ship.gameObject.transform.childCount; i++)
        {
            GameObject child = ship.gameObject.transform.GetChild(i).gameObject;
            if (LayerMask.LayerToName(child.layer).Contains("_ui"))
            {
                child.layer = newLayer;
            }
        }
        //TODO: Ship instantiator is too tied to sabotage game mode. Refactor out.
        Debug.Log("num of bases :" + numOfBases.ToString());
        ship.scoreDestination = mapObjects.scoringZones[(ship.teamGame ? (ship.teamNo + 1) : num) % numOfBases];
        mapObjects.islands[(ship.teamGame ? (ship.teamNo + 1) : num) % numOfBases].enemyShips.Add(ship);

        if (ship.teamGame)
        {
            //Refactor later
            ship.startingPoint = mapObjects.shipStartingLocations[(ship.teamNo % numOfBases)].transform.GetChild(ship.placeInTeam).position;
            ship.transform.position = ship.startingPoint;
        }
        else
        {
            ship.startingPoint = mapObjects.shipStartingLocations[num-1].transform.position;
            ship.transform.position = ship.startingPoint;
        }


        GameObject[] uis = GameObject.FindGameObjectsWithTag("UIManagers");

        GameObject ui;

        if (!PhotonNetwork.offlineMode)
        {
            ui = uis[0];

        }
        else {
            ui = uis[num - 1];
        }

        ship.uiManager = ui.GetComponent<UIManager>();

        ship.uiManager.altFireBar.gameObject.transform.GetChild(1).gameObject.GetComponentInChildren<Image>().sprite = info.altFireSprite;
        ship.uiManager.altFireBar.gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = info.altFireOutline;
        ship.uiManager.transform.GetChild(0).Find("Portrait").GetComponent<Image>().sprite = info.portrait;

        ship.followCamera = ui.GetComponentInParent<cameraFollow>();
        ship.followCamera.target = ship.gameObject;
        ship.followCamera.ready = true;
        ship.followCamera.camera.cullingMask |= (1 << newLayer);

        LayerHelper.setLayerRecursively(ship.uiManager.worldSpace, LayerMask.NameToLayer("p" + (num + numKraken) + "_ui"));


        GameObject splash = (GameObject)Instantiate(splashParticle, Vector3.zero, Quaternion.identity);
        if (ship.hookshotComponent)
        {
            ship.hookshotComponent.splashParticle = splash;
        }

        ship.cullingMask = "p" + (num + numKraken) + "_ui";
        Destroy(this.gameObject);
    }

    public ShipInformation getShip(ShipEnum type)
    {
        foreach (ShipInformation ship in ships)
        {
            if (ship.type == type)
            {
                return ship;
            }
        }
        return null;
    }

    public void InstantiateShipMesh(ShipInformation info, PlayerInput ship)
    {
        GameObject obj = (GameObject)Instantiate(info.ship, Vector3.zero, Quaternion.identity);
        obj.transform.parent = ship.transform;
        obj.transform.localPosition = info.ship.transform.position;
        obj.transform.localRotation = info.ship.transform.rotation;
        obj.transform.localScale = info.ship.transform.localScale;
        ship.shipMesh = obj.GetComponentInChildren<MeshCollider>();
    }
}
