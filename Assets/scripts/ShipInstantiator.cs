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

	}

	public ShipInformation[] ships;
	public GameObject splashParticle;


	public void setupShipNames(playerInput ship, ShipEnum type, int num){
		MapObjects mapObjects = GameObject.FindObjectOfType<MapObjects> ();
		num++;
		ShipInformation info = getShip (type);
		ship.shipName = info.name;
		ship.stats = info.stats;
		if (ship.shipMesh == null) {
			InstantiateShipMesh (info ,ship);
		}
		ship.gameObject.GetComponentInChildren<CannonController> ().alternateFirePrefab = info.altFirePrefab;

        //Refactor later

        int numKraken = GameObject.FindObjectOfType<KrakenInput>() ? 1 : 0;
		
		int newLayer = LayerMask.NameToLayer ("p"+(num+ numKraken) + "_ui");
		for (int i = 0; i < ship.gameObject.transform.childCount; i++) {
			GameObject child = ship.gameObject.transform.GetChild (i).gameObject;
			if (LayerMask.LayerToName(child.layer).Contains("_ui")) {
				child.layer = newLayer;
			}
		}
		
		ship.scoreDestination = mapObjects.scoringZones[((num % 2) + 1) - 1];
		mapObjects.islands [(num%2)+1 - 1].enemyShip = ship;
	
		ship.startingPoint = mapObjects.shipStartingLocations[((num % 2) + 1) - 1].transform.position;
		ship.transform.position =  mapObjects.shipStartingLocations[((num % 2) + 1) - 1].transform.position;
			

		GameObject[] uis = GameObject.FindGameObjectsWithTag ("UIManagers");
        
        GameObject ui = uis[num - 1];

		ship.uiManager = ui.GetComponent<UIManager>();
     
		ship.uiManager.altFireBar.gameObject.transform.GetChild(1).gameObject.GetComponentInChildren<Image> ().sprite = info.altFireSprite;
		ship.uiManager.altFireBar.gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Image> ().sprite = info.altFireOutline;
		ship.followCamera = ui.GetComponentInParent<cameraFollow> ();
		ship.followCamera.target = ship.gameObject;
		ship.followCamera.ready = true;
        ship.followCamera.camera.cullingMask |= (1 << newLayer);

        LayerHelper.setLayerRecursively(ship.uiManager.worldSpace,LayerMask.NameToLayer("p"+ (num + numKraken) + "_ui"));
       
				
		GameObject splash = (GameObject) Instantiate (splashParticle, Vector3.zero, Quaternion.identity);
		ship.GetComponent<Hookshot>().splashParticle = splash;
				
		ship.cullingMask = "p" + (num + numKraken) + "_ui";
		Destroy (this.gameObject);
	}

	public ShipInformation getShip(ShipEnum type){
		foreach (ShipInformation ship in ships) {
			if (ship.type == type) {
				return ship;
			}
		}
		return null;
	}

	public void InstantiateShipMesh(ShipInformation info,playerInput ship){
		GameObject obj = (GameObject)Instantiate (info.ship, Vector3.zero, Quaternion.identity);
		obj.transform.parent = ship.transform;
		obj.transform.localPosition = info.ship.transform.position;
		obj.transform.localRotation = info.ship.transform.rotation;
		obj.transform.localScale = info.ship.transform.localScale;
		ship.shipMesh = obj.GetComponentInChildren<MeshCollider> ();
	}
}
