using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipAIV2 : MonoBehaviour {

    PlayerInput input;
    NavMeshAgent agent;
    ShipCannonComponent cannons;
    ShipMotorComponent motor;
    enum State { AGGRESIVE,OFFENSIVE,DEFENSIVE,TURTLE };
    DeathMatchGameManager manager;

    Dictionary<int, State> chanceOfState = new Dictionary<int, State>()
    {
        { 50,State.AGGRESIVE },
        {20,State.OFFENSIVE },
        {40,State.DEFENSIVE },
        {10,State.TURTLE }
    };

    State currentState = State.OFFENSIVE;

    public GameTypeEnum gameType = GameTypeEnum.DeathMatch;
    List<GameObject> bases = new List<GameObject>();
    List<GameObject> basesToSearch = new List<GameObject>();
    Vector3 targetSearchLocation;
    bool initialized;
    bool useAltFire;
    Vector3 targetPosition;
    bool started = false;
    List<System.Action<List<PlayerInput>, List<CannonBall>, List<BombComponent>>> actions;

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
        actions = new List<System.Action<List<PlayerInput>, List<CannonBall>, List<BombComponent>>>()
    {
        MoveTowardsEnemy, MoveAwayFromEnemy, DodgeThings, CheckAndFireAtTargets, DropBombsIfPossibleAtTargets, UseAltFire
    };
        initialized = true;
        if (!agent.isOnNavMesh)
        {
            var surface = FindObjectOfType<NavMeshSurface>();
            if (surface == null)
            {
                this.enabled = false;
            }
            else
            {
                surface.enabled = false;
                surface.enabled = true;
            }

        }

       

    }

    public void MakeDecision()
    {
        started = true;
        if (manager != null && manager.gameStarted)
        {
            var targetsNearBy = FindTargets();
            var cannonballsNearBy = FindProjectiles();
            var bombsNearBy = FindBombs();
            actions.Shuffle();
            foreach (System.Action<List<PlayerInput>, List<CannonBall>, List<BombComponent>> action in actions)
            {
                action(targetsNearBy, cannonballsNearBy, bombsNearBy);
            }

            Invoke("MakeDecision", 0.6f);
        }
       
    }

    private void UseAltFire(List<PlayerInput> targetsNearBy, List<CannonBall> cannonballsNearBy, List<BombComponent> bombsNearBy)
    {
        if (input.type == ShipEnum.AtlanteanShip)
        {
            if (input.getHealthPercent() < 0.3 && Random.Range(0,10) < 1)
            {
                input.altCannonComponent.handleShoot();
            } else if (input.getHealthPercent() < 0.3 && cannonballsNearBy.Count > 0  && Random.Range(0, 10) < 4)
            {
                input.altCannonComponent.handleShoot();
            }
            else if (bombsNearBy.Count > 0 && Random.Range(0, 10) < 7)
            {
                input.altCannonComponent.handleShoot();
            } else if (Random.Range(0, 20) < 1)
            {
                input.altCannonComponent.handleShoot();
            }
        }

        if (input.type == ShipEnum.ChineseJunkShip)
        {
            if (targetsNearBy.Count > 0 && Random.Range(0, 10) < 7)
            {
                useAltFire = true;
                CheckAndFireAtTargets(targetsNearBy,cannonballsNearBy,bombsNearBy);
            }
     
            else if (Random.Range(0, 10) < 1)
            {
                CheckAndFireAtTargets(targetsNearBy, cannonballsNearBy, bombsNearBy);
            }
        }

        /*if (input.type == ShipEnum.BlackbeardShip)
        {
            if (input.getHealthPercent() < 0.7 && Random.Range(0, 10) < 3)
            {
                input.altCannonComponent.handleShoot();
            }
            else if (targetsNearBy.Count > 0 && Random.Range(0, 10) < 6)
            {
                input.altCannonComponent.handleShoot();
            }
            else if (bombsNearBy.Count > 0 && Random.Range(0, 10) < 7)
            {
                input.altCannonComponent.handleShoot();
            }
            else if (Random.Range(0, 10) < 1)
            {
                input.altCannonComponent.handleShoot();
            }
        }
        */

        if (input.type == ShipEnum.VikingShip)
        {
            if (input.getHealthPercent() < 0.7 && Random.Range(0, 10) < 3)
            {
                input.altCannonComponent.handleShoot();
            }
            else if (targetsNearBy.Count > 0 && Random.Range(0, 10) < 6)
            {
                input.altCannonComponent.handleShoot();
            }
            else if (bombsNearBy.Count > 0 && Random.Range(0, 10) < 7)
            {
                input.altCannonComponent.handleShoot();
            }
            else if (Random.Range(0, 10) < 1)
            {
                input.altCannonComponent.handleShoot();
            }
        }

    }


    private void DropBombsIfPossibleAtTargets(List<PlayerInput> targetsNearBy, List<CannonBall> cannonballsNearBy, List<BombComponent> bombsNearBy)
    {
        if(targetsNearBy.Count > 0)
        {
            if(Random.Range(0,10) < 3)
            {
                input.bombController.handleBomb();
            }
        }
    }

    private void MoveAwayFromEnemy(List<PlayerInput> targetsNearBy, List<CannonBall> cannonballsNearBy, List<BombComponent> bombsNearBy)
    {
        if (targetsNearBy.Count > 0)
        {

            PlayerInput randomTarget = targetsNearBy[Random.Range(0, targetsNearBy.Count)];
            Vector3 move = (input.transform.position + randomTarget.transform.forward * 4.0f);
            targetPosition = MathHelper.addZ(MathHelper.addX(move, Random.Range(-3.0f, 3.0f)), Random.Range(-3.0f, 3.0f));
            Boost();

        }
    }

    private void DodgeThings(List<PlayerInput> targetsNearBy, List<CannonBall> cannonballsNearBy, List<BombComponent> bombsNearBy)
    {
        if (bombsNearBy.Count > 0)
        {
            foreach(BombComponent bomb in bombsNearBy)
            {
                float distance = Mathf.Abs(Vector3.Distance(input.transform.position, bomb.transform.position));
                if(distance < 0.2f)
                {
                    Boost();
                }
            }
        }
        if (cannonballsNearBy.Count > 0)
        {
            foreach (CannonBall ball in cannonballsNearBy)
            {
                float distance = Mathf.Abs(Vector3.Distance(input.transform.position, ball.transform.position));
                if (distance < 0.5f)
                {
                    var velocity = ball.GetVelocity();
                    var angle = Vector3.Angle(ball.transform.forward, -input.transform.forward);
                    if(Mathf.Abs(angle) < 60f)
                    {
                        targetPosition = (input.transform.position + (input.transform.right * 2.0f)* (angle>0?-1:1));
                    }
                }
            }
        }

    }

    private void Boost()
    {
        if (motor)
        {
            motor.Boost();
        }
        
    }

    private void MoveTowardsEnemy(List<PlayerInput> targetsNearBy, List<CannonBall> cannonballsNearBy, List<BombComponent> bombsNearBy)
    {
        if(targetsNearBy.Count > 0)
        {
            PlayerInput randomTarget = targetsNearBy[Random.Range(0, targetsNearBy.Count)];
            Vector3 move = (randomTarget.transform.position);
            targetPosition = MathHelper.addZ(MathHelper.addX(move, Random.Range(-3.0f, 3.0f)), Random.Range(-3.0f, 3.0f));
            Boost();
        }
        
    }

    private void ChangeStateMapBasedOnScenario(List<PlayerInput> targetsNearBy, List<CannonBall> cannonballsNearBy, List<BombComponent> bombsNearBy)
    {
        int confidence = 10;
        confidence -= input.getHealthPercent() < .3?5: (input.getHealthPercent() < .5?2:0);
        
    }

    void OnEnable()
    {
        if (!initialized)
        {
            Start();
        }
        if (motor)
            motor.aiControls = true;
        if (input)
            if (input.aimComponent)
                input.aimComponent.aiControls = true;
    }

    void OnDisable()
    {
        if (motor)
            motor.aiControls = false;
        if (input)
            if (input.aimComponent)
                input.aimComponent.aiControls = false;
    }

    void Update() {
        if (manager.gameStarted) {
            if(!started){
                MakeDecision();
            }
           agent.transform.localPosition = Vector3.zero;
           agent.transform.localRotation = Quaternion.identity;
           MoveToLocation(targetPosition);

        }


    }

    private void CheckAndFireAtTargets(List<PlayerInput> targets, List<CannonBall> cannonballsNearBy, List<BombComponent> bombsNearBy)
    {
        PlayerInput closest = null;
        List<PlayerInput> closeShips = new List<PlayerInput>();
        foreach(PlayerInput ship in targets)
        {
            if (ship == this.input || (manager.isTeam && ship.teamNo == input.teamNo) ) {
                continue;
            }

            var distance = Vector3.Distance(ship.transform.position, this.transform.position);
            if (distance < 6f)
            {
                closeShips.Add(ship);
            } 
        }
        if (closeShips.Count != 0)
        {
            closest = closeShips[Random.Range(0, closeShips.Count)];
            var aimPosition = MathHelper.addZ(MathHelper.addX(closest.transform.position, Random.Range(-.6f, 0.6f)), Random.Range(-.6f, 0.6f));
            input.aimComponent.AimAt((aimPosition - transform.position).normalized);
            if(input.status == ShipStatus.Alive)
            {
                if (useAltFire)
                {
                    input.altCannonComponent.handleShoot();
                }
                cannons.handleShoot();
            }
            
        }


    }

    private Vector3 FindGoodSpotsToSearch()
    {
        NavMeshHit hit = new NavMeshHit();
        if (targetSearchLocation == Vector3.zero || Vector3.Distance(targetSearchLocation, this.transform.position) < 2f)
        {
            if(basesToSearch.Count == 0)
            {
                basesToSearch.AddRange(bases);
            }

            var randInt = Random.Range(0,basesToSearch.Count);
            
            NavMesh.SamplePosition(basesToSearch[randInt].transform.position, out hit, 4f,NavMesh.AllAreas);
            basesToSearch.RemoveAt(randInt);
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
            if (ship == input || (manager.isTeam && ship.teamNo == input.teamNo))
            {
                continue;
            }
            if(Vector3.Distance(ship.transform.position, this.transform.position) < 8f)
            {
                targets.Add(ship);
            }
        }
        return targets;
    }

    List<CannonBall> FindProjectiles()
    {
        List<CannonBall> scaryCannonBalls = new List<CannonBall>();
        var cannonBalls = FindObjectsOfType<CannonBall>();
        foreach (CannonBall ball in cannonBalls)
        {
            if(ball.getOwner() != this.input && ball.getOwner().teamNo != input.teamNo)
            {
                if (Vector3.Distance(ball.transform.position, this.transform.position) < 8f)
                {
                    scaryCannonBalls.Add(ball);
                }
                
            }
            
        }
        return scaryCannonBalls;
    }

    List<BombComponent> FindBombs()
    {
        List<BombComponent> bombs = new List<BombComponent>();
        var bombFound = FindObjectsOfType<BombComponent>();
        foreach (BombComponent bomb in bombFound)
        {
            if (bomb.getPlayer()!= this.input && bomb.getPlayer().teamNo != input.teamNo)
            {
                if (Vector3.Distance(bomb.transform.position, this.transform.position) < 4f)
                {
                    bombs.Add(bomb);
                }

            }

        }
        return bombs;
    }

    internal void DisableAI()
    {
        GetComponentInChildren<NavMeshAgent>().enabled = false;
        this.enabled = false;
    }
}


