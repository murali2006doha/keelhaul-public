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
		int initialLayer = LayerMask.NameToLayer ("p2_ui");
		if (num == 1) {
			int newLayer = LayerMask.NameToLayer ("p1_ui");
			for (int i = 0; i < ship.gameObject.transform.GetChildCount (); i++) {
				GameObject child = ship.gameObject.transform.GetChild (i).gameObject;
				if (child.layer == initialLayer) {
					child.layer = newLayer;
				}
			}
		}



		ship.scoreDestination = mapObjects.scoringZones[num-1];


	
		ship.startingPoint = mapObjects.shipStartingLocations[num-1].transform.position;
		ship.transform.position =  mapObjects.shipStartingLocations[num-1].transform.position;
			

		GameObject[] uis = GameObject.FindGameObjectsWithTag ("UIManagers");
		foreach(GameObject ui in uis){
			if(ui.name.Contains(num+"")){
				ship.uiManager = ui.GetComponent<UIManager>();
                print("Yes");
				ship.uiManager.altFireBar.gameObject.transform.GetChild(1).gameObject.GetComponentInChildren<Image> ().sprite = info.altFireSprite;
				ship.uiManager.altFireBar.gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Image> ().sprite = info.altFireOutline;
				ship.followCamera = ui.GetComponentInParent<cameraFollow> ();
				ship.followCamera.target = ship.gameObject;
				ship.followCamera.ready = true;
				break;
			}
		}

		GameObject splash = (GameObject) Instantiate (splashParticle, Vector3.zero, Quaternion.identity);
		ship.GetComponent<Hookshot>().splashParticle = splash;
				
		ship.victoryScreen = mapObjects.victoryScreens[mapObjects.enums.IndexOf(type)];	
		ship.cullingMask = "p" + num + "_ui";
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
