using UnityEngine;
using System.Collections;
using InControl;
using System;

public class HookshotComponent : MonoBehaviour
{

    //Ship instantiate


    //Already Present in Prefab and variables to tweak

    [Header("Scene Variables")]
    public Transform hook;
    public Transform aim;
    public GameObject barrel_dest;

    [Header("Value Variables")]
    public float tetherSpeed = 10f;
    public float missedTetherSpeed = 4f;
    public float missedTetherRollbackSpeed = 3f;
    public float extraDistance = 1.2f;
    public float stuckTime = 0.5f;

    [Header("Other/AutoSet Variables")]
    public GameObject barrelGameObj = null;
    public Barrel barrel = null;
    public GameObject destination = null;
    public bool hookshotActive = false;
    public bool aiHook = false;
    public FreeForAllStatistics stats;
    public UIManager uiManager;
    public Func<bool> isAimOnBarrel;
    public Action<float> changeSpeed;
    public GameObject splashParticle;

    Transform ship;
    Rigidbody rb;
    LineRenderer tether;

    Vector3 missedLocation;
    Vector3 barrel_anchor;

    float distanceCounter = 0;
    bool reverseTether = false;
    private bool hooking = false;
    private bool hooked = false;
    private bool stuck = false;
    internal Action<bool> onHook;
    public bool autoHook;

    void Start()
    {
        this.ship = this.gameObject.transform;
        tether = GetComponent<LineRenderer>();
        barrel = FindObjectOfType<Barrel>();
        barrelGameObj = barrel!=null?barrel.gameObject:null;
        rb = barrelGameObj!=null?barrelGameObj.GetComponent<Rigidbody>():null;

    }

    void Update()
    {
        if (hooked)
        {
            tether.SetPosition(0, hook.transform.position);
            tether.SetPosition(1, barrel_dest.transform.position);
            barrelGameObj.transform.position = barrel_dest.transform.position;
        }
    }


    IEnumerator handleHit()
    {
        while (hookshotActive)
        {
            tether.SetPosition(0, hook.transform.position);
            //Get the direction from the hook position to the barrel
            Vector3 heading = barrelGameObj.transform.position - hook.transform.position;
            float distance = heading.magnitude;
            distanceCounter += distance * tetherSpeed * (Time.deltaTime * GlobalVariables.gameSpeed);
            Vector3 newpos;
            //distanceCounter will be incremented every frame to make the hookshot travel to the barrel slowly
            if (distanceCounter > distance)
            {
                //If the distancecounter is greater than the distance to the barrel, that means the hookshot must have reached the barrel. So time to hook the barrel
                barrelGameObj.GetComponent<Rigidbody>().isKinematic = true;
                newpos = hook.transform.position + Vector3.Normalize(heading) * distance;
                SoundManager.playSound(SoundClipEnum.Hookshothit, SoundCategoryEnum.Generic, transform.position);
                if (barrelGameObj.GetComponent<Barrel>().owner && barrelGameObj.GetComponent<Barrel>().owner.GetComponent<PlayerInput>())
                {
                    stats.numOfBarrelSteals += 1;
                    barrelGameObj.GetComponent<Barrel>().owner.GetComponent<PlayerInput>().gameStats.numOfBarrelsLost += 1;
                    //If another player is already tethered to the barrel, unhook that player
                    barrelGameObj.GetComponent<Barrel>().owner.transform.GetComponent<HookshotComponent>().UnHook();
                }
                hookshotActive = false;
                hooking = true;
                onHook(true);


            }
            else
            {
                newpos = hook.transform.position + Vector3.Normalize(heading) * distanceCounter;
            }
            tether.SetPosition(1, newpos);
            yield return null;
        }
        StartCoroutine("PullInBarrel");

    }

    internal void Initialize(UIManager uiManager, FreeForAllStatistics gameStats, Func<bool> aimCheckFunction)
    {
        stats = gameStats;
        isAimOnBarrel = aimCheckFunction;
        this.uiManager = uiManager;
        barrel = FindObjectOfType<Barrel>();
        barrelGameObj = barrel != null ? barrel.gameObject : null;
        rb = barrelGameObj != null ? barrelGameObj.GetComponent<Rigidbody>() : null;
        this.autoHook = barrel!=null?barrel.autoHook:false;
    }

    public void AutoHookBarrel()
    {
        if (!barrel.isScoring && barrelGameObj.GetComponent<Barrel>().owner == null)
        {
            onHook(true);
            tether.enabled = true;
            tether.SetPosition(0, hook.transform.position);
            tether.SetPosition(1, barrelGameObj.transform.position);
            barrelGameObj.transform.position = barrel_dest.transform.position;
            CompleteHookingBarrel();
        }

    }

    //Missed Coroutine
    IEnumerator animateMissed()
    {
        while (hookshotActive)
        {
            if (stuck)
            {
                tether.SetPosition(0, hook.transform.position);
                yield return null;
            }
            tether.SetPosition(0, hook.transform.position);
            Vector3 heading = missedLocation - hook.transform.position;
            float distance = heading.magnitude;
            Vector3 newpos = hook.transform.position;
            //In Reverse Animation state
            if (reverseTether)
            {
                distanceCounter -= distance * missedTetherRollbackSpeed * 2 * (Time.deltaTime * GlobalVariables.gameSpeed);
                if (distanceCounter <= 0f)
                {
                    tether.enabled = false;
                    reverseTether = false;
                    stuck = false;
                    hookshotActive = false;
                }
            }
            else
            {
                distanceCounter += distance * missedTetherSpeed * (Time.deltaTime * GlobalVariables.gameSpeed);
                if (distanceCounter > (distance * extraDistance))
                {
                    reverseTether = true;
                    splashParticle.transform.position = missedLocation;
                    foreach (ParticleSystem system in splashParticle.GetComponentsInChildren<ParticleSystem>())
                    {
                        system.Play();
                    }
                    stuck = true;
                    Invoke("ResetStuck", stuckTime);
                }
            }
            newpos = hook.transform.position + (Vector3.Normalize(heading) * distanceCounter);
            tether.SetPosition(1, newpos);
            yield return null;
        }


    }

    public void ResetStuck()
    {
        stuck = false;
    }

    public void HookBarrel()
    {
        if (autoHook)
        {
            return;
        }
        if (!hookshotActive && !hooked)
        {
            stats.hookshotNum++;
            hookshotActive = true;
            SoundManager.playSound(SoundClipEnum.Hookshot, SoundCategoryEnum.Generic, transform.position);
            if (isAimOnBarrel())
            {
                ship.GetComponent<PlayerInput>().vibrate(.15f, .2f);
                ShowHookShot();
            }
            else
            {
                stats.hookshotMisses++;
                ShowHookshotMiss(new Vector3(aim.position.x, 0f, aim.position.z));
            }

        }
        else if (hooked)
        {
            UnHook();
        }
    }


	public Vector3 getBarrelPosition() {
		return barrelGameObj.transform.position;
	}

	public void setBarrelPosition(Vector3 transform) {
		barrelGameObj.transform.position = transform;
        barrel.isScoring = true;
	}

    private void ShowHookShot()
    {
        tether.enabled = true;
        tether.SetPosition(0, hook.transform.position);
        StartCoroutine("handleHit");
    }

    private void ShowHookshotMiss(Vector3 pos)
    {
        tether.enabled = true;
        tether.SetPosition(0, hook.transform.position);
        missedLocation = pos;
        StartCoroutine("animateMissed");
    }

    IEnumerator PullInBarrel()
    {
        while (hooking)
        {
            if (barrelGameObj != null && rb != null)
            {
                tether.SetPosition(0, hook.transform.position);
                tether.SetPosition(1, barrelGameObj.transform.position);
                Vector3 targetPosition = barrel_dest.transform.position;
                Vector3 barrelPosition = barrelGameObj.transform.position;
                Physics.IgnoreCollision(rb.GetComponent<Collider>(), GetComponent<Collider>());


                if (Vector3.Distance(barrelPosition, targetPosition) > .1f)
                {
                    var barrelJoint = barrelGameObj.GetComponent<CharacterJoint>();
                    if (barrelJoint != null)
                    {
                        barrel_anchor = barrelJoint.anchor;
                        Destroy(barrelJoint);
                    }

                    var relativePoint = transform.InverseTransformPoint(barrelGameObj.transform.position);
                    //checks which side the barrel is on
                    MoveTowardsTarget();

                }
                else
                {
                    CompleteHookingBarrel();

                }
            }
            yield return null;
        }
        // rotates barrel around the ship and pulls it in behind the ship at the same time

    }

    private void CompleteHookingBarrel()
    {
        rb.isKinematic = false;
        barrelGameObj.GetComponent<Barrel>().owner = this.gameObject;
        if (!autoHook)
        {
            barrelGameObj.AddComponent<CharacterJoint>();
            barrelGameObj.GetComponent<CharacterJoint>().anchor = barrel_anchor;
            barrelGameObj.GetComponent<CharacterJoint>().connectedBody = barrel_dest.GetComponent<Rigidbody>();
        }
        hooking = false;
        hooked = true;
        uiManager.setTarget(destination);
    }

    internal bool shouldShowTooltip()
    {
        return !autoHook && !hooked && isAimOnBarrel();
    }

    private void MoveTowardsTarget()
    {

        // pulls in the barrel towards the empty game object, which sits directly behind the ship 

        //the speed, in units per second, we want to move towards the target
        float speed = 5;

        Vector3 targetPosition = barrel_dest.transform.position;
        Vector3 barrelPosition = barrelGameObj.transform.position;

        Vector3 directionOfTravel = targetPosition - barrelPosition;
        //now normalize the direction, since we only want the direction information
        directionOfTravel.Normalize();
        //scale the movement on each axis by the directionOfTravel vector components
        //barrel.transform.position = Vector3.Lerp(barrelPosition,targetPosition,Time.deltaTime);
        barrelGameObj.transform.Translate(
            (directionOfTravel.x * speed * (Time.deltaTime * GlobalVariables.gameSpeed)),
            (directionOfTravel.y * speed * (Time.deltaTime * GlobalVariables.gameSpeed)),
            (directionOfTravel.z * speed * (Time.deltaTime * GlobalVariables.gameSpeed)), Space.World);
    }



    //checks if aim reticule's collider is touching the barrel. I could put this in a new script just for the reticule, but 
    //its good for now. 
    public bool isHooked()
    {
        if (barrelGameObj == null)
        {
            return false;
        }
        return (barrelGameObj.GetComponent<Barrel>().owner == this.gameObject || hooked);

    }



    //Basically reset hookshot variables to before the barrel was hooked
    public void UnHook()
    {
        uiManager.targetBarrel();
        if (barrelGameObj != null)
        {
            if (!autoHook) { 
                if (barrelGameObj.GetComponent<CharacterJoint>() != null && barrelGameObj.GetComponent<CharacterJoint>().connectedBody)
                {
                    var other = barrelGameObj.GetComponent<CharacterJoint>().connectedBody.gameObject;
                    if (other == barrel_dest)
                    {
                        barrelGameObj.GetComponent<CharacterJoint>().connectedBody = null;
                        barrelGameObj.GetComponent<Barrel>().owner = null;
                    }

                }
            }
            barrelGameObj.GetComponent<Barrel>().owner = null;
            rb.isKinematic = false;
            tether.enabled = false;
            hookshotActive = false;
            stuck = false;
            hooked = false;
            hooking = false;
            distanceCounter = 0;
            onHook(false);
            StopAllCoroutines();
        }

    }


}