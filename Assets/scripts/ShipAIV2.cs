
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipAIV2 : MonoBehaviour {

    PlayerInput input;
    NavMeshAgent agent;
    ShipCannonComponent cannons;
    ShipMotorComponent motor;
    enum State { ATTACKING,ESCAPING };
    DeathMatchGameManager manager;

    State currentState = State.ATTACKING;

    public GameTypeEnum gameType = GameTypeEnum.DeathMatch;
    List<GameObject> bases = new List<GameObject>();
    Vector3 targetSearchLocation;
    

    void Start () {
        input = GetComponent<PlayerInput>();
        motor = input.motor;
        motor.aiControls = true;
        input.aimComponent.aiControls = true;
        cannons = input.centralCannon;
        agent = GetComponentInChildren<NavMeshAgent>();
        manager = FindObjectOfType<DeathMatchGameManager>();
        foreach (GameObject island in FindObjectOfType<MapObjects>().shipStartingLocations)
        {
            bases.Add(island);
        }



    }

    private void OnEnable()
    {
        if (motor)
            motor.aiControls = !motor.aiControls;
        if (input)
            if (input.aimComponent)
                input.aimComponent.aiControls = !input.aimComponent.aiControls;
    }

    void Update() {
       
       agent.transform.localPosition = Vector3.zero;
       agent.transform.localRotation = Quaternion.identity;
       var targets = FindTargets();
       if (currentState == State.ATTACKING)
        {
            if (targets.Count > 0)
            {
                PlayerInput closest = GetClosestTarget(targets);
                Vector3 move = (closest.transform.position);
                MoveToLocation(move);
                CheckAndFireAtTargets(targets);
            }
            else
            {
                Vector3 move =  FindGoodSpotsToSearch();
                MoveToLocation(move);
            }
        }


    }

    private void CheckAndFireAtTargets(List<PlayerInput> targets)
    {
        PlayerInput closest = null;
        var closest_dist = 99f;
        foreach(PlayerInput ship in targets)
        {
            var distance = Vector3.Distance(ship.transform.position, this.transform.position);
            if (distance < 4f)
            {
                if (distance < closest_dist)
                {
                    closest_dist = distance;
                    closest = ship;
                }
            } 
        }
        if (closest != null)
        {
            input.aimComponent.AimAt(closest.transform.position);
            cannons.handleShoot();
            
        }


    }

    private Vector3 FindGoodSpotsToSearch()
    {
        NavMeshHit hit = new NavMeshHit();
        if (targetSearchLocation == Vector3.zero || Vector3.Distance(targetSearchLocation, this.transform.position) < 2f)
        {
            var randInt = Random.Range(0,3);
            
            NavMesh.SamplePosition(bases[randInt].transform.position, out hit, 4f,NavMesh.AllAreas);
            if (hit.hit)
            {
                targetSearchLocation = hit.position;
                return hit.position;
            }
        }
        else
        {
            return targetSearchLocation;
        }
        NavMesh.SamplePosition(transform.position,out hit, 4f, NavMesh.AllAreas);
        targetSearchLocation = hit.position;
        return targetSearchLocation;         
    }

    private void MoveToLocation(Vector3 move)
    {
        agent.SetDestination(move);
        if (agent.desiredVelocity != Vector3.zero)
        {
            Vector3 move_dir = transform.InverseTransformDirection(agent.desiredVelocity);
            if (input.shipInput.onRotateChanged != null)
            {
                motor.UpdateInput(new Vector3(Mathf.Clamp(agent.desiredVelocity.x, -1f, 1f), 0f, Mathf.Clamp(agent.desiredVelocity.z, -1f, 1f)));
            }
        }
    }

    private PlayerInput GetClosestTarget(List<PlayerInput> targets)
    {
        return targets[0];
    }

    List<PlayerInput> FindTargets()
    {
        List<PlayerInput> targets = new List<PlayerInput>();
        foreach(PlayerInput ship in manager.getPlayers())
        {
            if (ship == input)
            {
                continue;
            }
            if(Vector3.Distance(ship.transform.position, this.transform.position) < 5f)
            {
                targets.Add(ship);
            }
        }
        return targets;
    }
}
