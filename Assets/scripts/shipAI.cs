using UnityEngine;
using System.Collections;

public class shipAI : MonoBehaviour {

	public PlayerInput input;
	public UnityEngine.AI.NavMeshAgent agent;
	public Barrel barrel;
	Camera cam;
	GameObject otherShip;
	GameObject kraken;
	bool dying = false;
	HookshotComponent hook_component;

	enum State  {SEARCHING,ATTACHING,SCORING,ESCAPING,DYING};
	bool isSubmerged = false;
	State currentState;

	// Use this for initialization
	void Start () {
		barrel = GameObject.FindObjectOfType<Barrel> ();
		input = this.GetComponent<PlayerInput> ();
		hook_component = input.getHook ();
		agent = this.GetComponent<UnityEngine.AI.NavMeshAgent> ();
		cam = input.followCamera.camera;
		foreach (PlayerInput player in GameObject.FindObjectsOfType<PlayerInput> ()) {
			if (player.gameObject != this.gameObject) {
				otherShip = player.gameObject;
				print("otherShip:" + player.shipName + "  ." + this.input.shipName + "");
			}
		}
		kraken = GameObject.FindObjectOfType<KrakenInput> ().gameObject;

		print ("Ship" + input.shipName + " AI: " + State.SEARCHING);
		currentState = State.SEARCHING;
		//agent.velocity = Math.min (input.maxVelocity, agent.velocity);
		agent.speed = input.stats.maxVelocity * GlobalVariables.gameSpeed;
		agent.SetDestination (barrel.transform.position);
		//agent.updatePosition = false;
		agent.angularSpeed = 120;
		agent.autoBraking = false;
		//agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

	}
	
	// Update is called once per frame
	void Update () {
		
		/*if (input.gameStarted) { //Works but jittery movement;
			agent.speed = input.maxVelocity * GlobalVariables.gameSpeed;
			input.GetComponent<CharacterController> ().Move (transform.forward * input.maxVelocity * (Time.deltaTime * GlobalVariables.gameSpeed));
			agent.Warp (gameObject.transform.position);
		}*/
		if (currentState == State.SEARCHING) {
			agent.Resume ();
			hook_component.aiHook = false;
			agent.SetDestination (barrel.transform.position);
			if(agent.remainingDistance <2f){
				currentState = State.ATTACHING;
				print ("Ship" + input.shipName + " AI: " + State.ATTACHING);

			}
		}
		if (currentState == State.ATTACHING) {
			Vector3 move = barrel.transform.position;
			if (Vector3.Distance (this.transform.position, move) > 4f) {
				print ("Ship" + input.shipName + " AI: " + State.SEARCHING);
				currentState = State.SEARCHING;
			} else if(agent.remainingDistance < 0.25f ){
				if (barrel.owner == null) {
					agent.Stop ();
				}
				if (this.hook_component.aim.GetComponent<Renderer> ().material.color == Color.yellow && !hook_component.hookshotActive) {
					hook_component.aiHook = true;
				} else {
					hook_component.aiHook = false;
				}
			}


			if (hook_component.isHooked ()) {
				hook_component.aiHook = false;
				print ("Ship" + input.shipName + " AI: " + State.SCORING);
				currentState = State.SCORING;
			}
		}
		if (currentState == State.SCORING) {
			hook_component.aiHook = false;
			agent.speed = input.stats.maxVelocity * input.stats.barrelSlowDownFactor *  GlobalVariables.gameSpeed;
			agent.Resume ();
			agent.SetDestination (input.scoreDestination.transform.FindChild ("winstate").position);
			if (!hook_component.isHooked ()) {
				print ("Ship" + input.shipName + " AI: " + State.SEARCHING);
				currentState = State.SEARCHING;
			}
		} else {
			agent.speed = input.stats.maxVelocity *  GlobalVariables.gameSpeed;
		}
			

		if (Vector3.Distance (otherShip.transform.position, this.transform.position) < 5f) { //SHOOT
			Vector3 move = (this.transform.position - otherShip.transform.position).normalized;
			if (currentState != State.ATTACHING) {

				input.aiFire = true;
			}
		} else {
			input.aiFire = false;
		}

		if (!input.gameStarted || input.dying) {
			agent.Stop ();
			agent.updatePosition = false;

			dying = true;
		} else if(input.gameStarted) {
			if (dying) {
				dying = false;
				agent.updatePosition = true;
				agent.Warp (input.scoreDestination.transform.position);
				agent.Resume ();
			}
		}
	}
}
