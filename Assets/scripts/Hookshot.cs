using UnityEngine;
using System.Collections;
using InControl;

public class Hookshot : MonoBehaviour {

	//Ship instantiate
	public GameObject splashParticle;
	public PlayerActions Actions { get; set; }
	public Camera playerCam;

	//Already Present in Prefab and variables to tweak
	public GameObject barrel = null;
	public Transform hook;
	public GameObject aim;

	public GameObject barrel_dest;
	public float tetherSpeed = 10f;
	public int maxDistance = 2;
	public float minDistance = 0.5f;
	public float moveSpeed = 10f;
	public float mouseClamp = 0.005f;
	public float missedTetherSpeed = 4f;
	public float missedTetherRollbackSpeed = 3f;
	public float extraDistance = 1.2f;
	public float stuckTime = 0.5f;
	public bool hookshotActive = false;
	public bool aiHook = false;
	public Transform scoreDestination;
	public FreeForAllStatistics stats;
    public UIManager uiManager;

	ArrowController arrowController;
	Transform ship;
	Rigidbody rb;
	LineRenderer tether; 
	Vector3 missedLocation;
	Vector3 moveVector;
	Vector3 barrel_anchor;
	float distanceCounter = 0;
	bool hittingBarrel = false;
	bool reachedDestination = false;
	bool tempReached = false;
	bool missedBarrel=false;
	bool reverseTether = false;

	float stuckCounter =0f;
    

    void Start () {
		Vector3 inFront = new Vector3 (1f, 0f, 1f);
		aim.transform.position = this.transform.position + inFront;
		this.arrowController = GetComponentInChildren<ArrowController> ();
		this.ship = this.gameObject.transform;
		tether = GetComponent<LineRenderer> ();
		barrel = GameObject.FindObjectOfType<barrel> ().gameObject;
		rb = barrel.GetComponent<Rigidbody> ();
		aiHook = false;
		arrowController.home = scoreDestination;
//        uiManager.targetBarrel();
	}

	// Update is called once per frame
	void Update () {
		if (scoreDestination != null && arrowController.home==null) {
			arrowController.home = scoreDestination;
		}
		if (Actions!= null) { 
            if (Actions.Device != null) 
			    aim.transform.position = (aim.transform.position - this.transform.position).normalized * 0.75f + this.transform.position;
		}
		if (isHooked()) {
			stats.timeSpentCarryingBarrel += Time.deltaTime;
            uiManager.setTarget(scoreDestination.position);
            uiManager.changeCompassColor(Color.green);
            arrowController.TargetHome ();
			aim.GetComponent<Renderer> ().material.color = Color.white;
		}
		if (Actions != null && !ship.GetComponent<playerInput>().dying) {
			if (Actions.R_Rotate.IsPressed == true) { //when aim is moving, make it visible
				aim_reticule ();
			}
			ManageTether ();
		}	
	}


	// moves the aim reticule within a circular perimeter around the player
	void aim_reticule() {
			// with mouse 
		if (Actions.Device == null ) {
            if (playerCam != null) {
                Vector2 mousePos = Input.mousePosition;
                Vector2 mousePosCentered = new Vector2(mousePos.x - Screen.width / 2, mousePos.y - Screen.height / 2);
                Vector2 normalized = mousePosCentered.normalized;
                float magniture = mousePosCentered.magnitude;
                float ratio = Screen.width / Screen.height;
                aim.transform.position = this.transform.position + new Vector3(normalized.x* Mathf.Max(Mathf.Min(magniture* mouseClamp, maxDistance),minDistance), 0f, normalized.y* Mathf.Max(Mathf.Min(magniture * mouseClamp, maxDistance),minDistance)* ratio);
            }
			// with controller
        } else {
				
			moveVector = new Vector3 (Actions.R_Rotate.X, 0f, Actions.R_Rotate.Y);
			if (moveVector.magnitude >= minDistance) { 
				aim.transform.position = Vector3.MoveTowards (aim.transform.position, (this.transform.position) + (moveVector * maxDistance), moveSpeed);
			} 
		}
	}

	public void aim_reticule(Vector3 pos) {
		moveVector = new Vector3 (pos.x, 0f, pos.z);
		aim.transform.position = Vector3.Lerp (aim.transform.position, pos, Time.deltaTime * 10f *GlobalVariables.gameSpeed);
	}


	void PullInBarrel() { 

		// rotates barrel around the ship and pulls it in behind the ship at the same time
		if (barrel != null && rb!=null) {
			Vector3 targetPosition = barrel_dest.transform.position;
			Vector3 barrelPosition = barrel.transform.position;
			Physics.IgnoreCollision(rb.GetComponent<Collider>(), GetComponent<Collider>());


			if (Vector3.Distance (barrelPosition, targetPosition) > .1f) { 
				var barrelJoint = barrel.GetComponent<CharacterJoint> ();
				if (barrelJoint != null) {
					barrel_anchor = barrelJoint.anchor;
					Destroy (barrelJoint);
				}
				//rb.isKinematic = true;
				var relativePoint = transform.InverseTransformPoint (barrel.transform.position); 
				//checks which side the barrel is on

				if (isHooked ()) {

					if (relativePoint.x < 0.0) { 
						//if barrel is on the left side of ship, rotate counter clock-wise
						//barrel.transform.RotateAround (transform.position, Vector3.down, 200 * (Time.deltaTime * GlobalVariables.gameSpeed));	

						MoveTowardsTarget ();

					} else if (relativePoint.x > 0.0) {	
						//if barrel is on the right side of ship, rotate clock-wise
						//barrel.transform.RotateAround (transform.position, Vector3.up, 200 * (Time.deltaTime * GlobalVariables.gameSpeed));	

						MoveTowardsTarget ();
					}
				}

			} else {
				rb.isKinematic = false;
				reachedDestination = true;
				tempReached = false;
				barrel.GetComponent<barrel> ().owner = this.gameObject;
				barrel.AddComponent<CharacterJoint> ();
				barrel.GetComponent<CharacterJoint> ().anchor = barrel_anchor;

				barrel.GetComponent<CharacterJoint> ().connectedBody = barrel_dest.GetComponent<Rigidbody> ();

			}
		}
	}


	private void MoveTowardsTarget() {

		// pulls in the barrel towards the empty game object, which sits directly behind the ship 

		//the speed, in units per second, we want to move towards the target
		float speed = 5;

		Vector3 targetPosition = barrel_dest.transform.position;
		Vector3 barrelPosition = barrel.transform.position;

		Vector3 directionOfTravel = targetPosition - barrelPosition;
		//now normalize the direction, since we only want the direction information
		directionOfTravel.Normalize();
		//scale the movement on each axis by the directionOfTravel vector components
		//barrel.transform.position = Vector3.Lerp(barrelPosition,targetPosition,Time.deltaTime);
		barrel.transform.Translate(
			(directionOfTravel.x * speed * (Time.deltaTime * GlobalVariables.gameSpeed)),
			(directionOfTravel.y * speed * (Time.deltaTime * GlobalVariables.gameSpeed)),
			(directionOfTravel.z * speed * (Time.deltaTime * GlobalVariables.gameSpeed)), Space.World);
	}


	//This function takes care of managing the hookshot status
	void ManageTether() {

		if ((Actions.Fire_Hook.WasPressed || aiHook) && !hookshotActive && !missedBarrel && !isHooked () ) {
			//IF the player presses the hookshot button and not hooked, then fire hookshot
			//Fix when game is about to start.
			stats.hookshotNum++;
            SoundManager.playSound(SoundClipEnum.Hookshot, SoundCategoryEnum.Generic, transform.position);
            if (hittingBarrel) {
				ship.GetComponent<playerInput> ().vibrate (.15f, .2f);
				hookshotActive = true;
			} else {
				stats.hookshotMisses ++;
				missedBarrel = true;
				tether.enabled = true;
				bool reverseTether = false;
				tether.SetPosition (0, hook.transform.position);
				missedLocation =  new Vector3(aim.transform.position.x,aim.transform.position.y,aim.transform.position.z);
				distanceCounter = 0f;
			}

		}

		if (missedBarrel) {
			
			Vector3 heading = missedLocation - hook.transform.position;
			tether.enabled = true;
			tether.SetPosition (0, hook.transform.position);
			float distance = heading.magnitude;
			Vector3 newpos = hook.transform.position;
			//distanceCounter will be incremented every frame to make the hookshot travel to the barrel slowly
			if (reverseTether) {
				if (stuckCounter <= 0f) {
					distanceCounter -= distance * missedTetherRollbackSpeed * (Time.deltaTime * GlobalVariables.gameSpeed);
					if (distanceCounter <= 0f) {
						tether.enabled = false;
						missedBarrel = false;
						reverseTether = false;
					} 

				} else {
					stuckCounter -= (Time.deltaTime * GlobalVariables.gameSpeed);
				}

			} else {
				distanceCounter += distance * missedTetherSpeed * (Time.deltaTime * GlobalVariables.gameSpeed);
				if (distanceCounter > (distance *extraDistance)) {
					reverseTether = true;
					splashParticle.transform.position = missedLocation;
					foreach(ParticleSystem system in splashParticle.GetComponentsInChildren<ParticleSystem> ()){
						system.Play ();
					}
					stuckCounter = stuckTime;
				}
			}

			newpos = hook.transform.position + (Vector3.Normalize (heading) * distanceCounter);
			tether.SetPosition (1, newpos); 

		}

		if (Actions.Fire_Hook.WasPressed && isHooked()) { 
			//unhook if already hooked
			UnHook ();
		}

		if (hookshotActive && !missedBarrel && hittingBarrel && (!isHooked())) {
			//So if the player has fired the hookshot and they are still within range of the barrel and they havent already hooked to the barrel

			aim.GetComponent<Renderer> ().material.color = Color.white;

			tether.enabled = true;
			tether.SetPosition (0, hook.transform.position);
			//Get the direction from the hook position to the barrel
			Vector3 heading = aim.transform.position - hook.transform.position;

			float distance = heading.magnitude;
			distanceCounter += distance * tetherSpeed * (Time.deltaTime * GlobalVariables.gameSpeed);
			Vector3 newpos;
			//distanceCounter will be incremented every frame to make the hookshot travel to the barrel slowly

			if (distanceCounter > distance) {
				//If the distancecounter is greater than the distance to the barrel, that means the hookshot must have reached the barrel. So time to hook the barrel
				tempReached = true;
				rb.isKinematic = true;
				newpos = hook.transform.position + Vector3.Normalize (heading) * distance;
                SoundManager.playSound(SoundClipEnum.Hookshothit, SoundCategoryEnum.Generic, transform.position);
                if (barrel.GetComponent<barrel> ().owner) {
					stats.numOfBarrelSteals += 1;
					barrel.GetComponent<barrel> ().owner.GetComponent<playerInput> ().gameStats.numOfBarrelsLost += 1;
					//If another player is already tethered to the barrel, unhook that player
					barrel.GetComponent<barrel> ().owner.transform.GetComponent<Hookshot> ().UnHook ();
				} /*else if (otherPlayer.isHooked () && otherPlayer.barrel==this.barrel) {
					otherPlayer.UnHook ();
				}*/

			} else {
				newpos = hook.transform.position + Vector3.Normalize (heading) * distanceCounter;
			}
			tether.SetPosition (1, newpos); 

		}

		if (!hittingBarrel  && hookshotActive && !tempReached && !isHooked()) {
			//IF the player somehow managed to move away from the barrel range before it hooked, then decativate hook
			tether.SetPosition (0, hook.transform.position); 
			reverseTether = true;
			stuckCounter = stuckTime;
			missedLocation = barrel.transform.position;
			missedBarrel = true;
			hookshotActive = false;
			//UnHook ();
		}

		if (isHooked()) {
			//If the player has already hooked to the barrel, simply reiterate (actually unhook if already hooked)
			tether.SetPosition (0, hook.transform.position); 
			tether.SetPosition (1, barrel.transform.position);
			if(!reachedDestination)
				PullInBarrel ();
		}
	}


	//checks if aim reticule's collider is touching the barrel. I could put this in a new script just for the reticule, but 
	//its good for now. 
	public bool isHooked(){
		if(barrel ==null){
			return false;
		}
		return (barrel.GetComponent<barrel> ().owner == this.gameObject || tempReached);

	}


	void OnTriggerStay(Collider other){ 
		if (other.gameObject.tag.Equals("barrel")) {
			if (barrel == null) {
				barrel = other.gameObject;
				rb = barrel.GetComponent<Rigidbody> ();
			}
			hittingBarrel = true;
			aim.GetComponent<Renderer> ().material.color = Color.yellow;
		}
	}


	void OnTriggerExit(Collider other){
		if (other.gameObject.tag.Equals("barrel")) {
			if (barrel == other.gameObject && !isHooked() && !hookshotActive) {
				barrel = null;
				rb = null;
			}
			hittingBarrel = false;
			aim.GetComponent<Renderer> ().material.color = Color.white;
		}
	} 


	//Basically reset hookshot variables to before the barrel was hooked
	public void UnHook() {
        uiManager.targetBarrel();
        if (barrel != null) {
			if (barrel.GetComponent<CharacterJoint> ()!=null && barrel.GetComponent<CharacterJoint> ().connectedBody) {
				var other = barrel.GetComponent<CharacterJoint> ().connectedBody.gameObject; 
				if (other == barrel_dest) {
					barrel.GetComponent<CharacterJoint> ().connectedBody = null;
					barrel.GetComponent<barrel> ().owner = null;
				}
                
                arrowController.TargetBarrel ();

			} else if (isHooked ()) {
               
                arrowController.TargetBarrel ();

			}
           
            rb.isKinematic = false;
			tether.enabled = false;
			hittingBarrel = false;
			reachedDestination = false;
			hookshotActive = false;
			distanceCounter = 0;
			tempReached = false;
			barrel = null;
			rb = null;
			missedBarrel = false;
		}

	}


}