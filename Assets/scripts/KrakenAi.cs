using UnityEngine;
using System.Collections;

public class KrakenAi : MonoBehaviour {
	public KrakenInput input;
	public NavMeshAgent agent;
    public KrakenStats stats;
	public barrel barrel;
	Camera cam;

	Vector3 enemyLastPos = Vector3.zero;

	enum State  {SEARCHING,WAITING,ATTACKING,ESCAPING,DYING};
	bool isSubmerged = false;
	State currentState;
	// Use this for initialization
	void Start () {
		barrel = GameObject.FindObjectOfType<barrel> ();
		input = this.GetComponent<KrakenInput> ();
		agent = this.GetComponent<NavMeshAgent> ();
		cam = input.followCamera.camera;
		print ("KrakenAI: " + State.SEARCHING);
		currentState = State.SEARCHING;
		//agent.velocity = Math.min (input.maxVelocity, agent.velocity);
		agent.speed = stats.maxVelocity;
		agent.SetDestination (barrel.transform.position);
		agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		agent.autoBraking = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (!isSubmerged) {
			agent.speed = stats.maxVelocity;
		} else {
			agent.speed = stats.submergedMaxVelocity;
		}
		if (currentState == State.SEARCHING) {
			agent.SetDestination (barrel.transform.position);
			if (agent.remainingDistance > 10f && !isSubmerged) {
				if (!input.dying && !input.isSubmerging) {
					input.Actions.Blue.SetValue (1, 1);
				} else if (input.isSubmerging) {
					input.Actions.Blue.SetValue (0, 1);
					isSubmerged = true;
				}

			}
			if(agent.remainingDistance <0.5f){
				currentState = State.WAITING;
				agent.Stop ();
				print ("KrakenAI: " + State.WAITING);
			}

		}
		if (currentState == State.WAITING) {
			print (barrel.owner + " " + input.isSubmerging);
			if (barrel.owner != null && isSubmerged && !input.dying && !input.attacking && input.submerged) {
				
			} else if (!input.submerged) {
				
				isSubmerged = false;
			} 

		}
	}
}
