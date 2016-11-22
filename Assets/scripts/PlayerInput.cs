using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using InControl;
using UnityEngine.SceneManagement;
using System;

public class PlayerInput : MonoBehaviour, StatsInterface
{

    //Refactor later into instantiator
    public ShipEnum type;
    public int shipNum;
    public GameObject scoreDestination;
    public Vector3 startingPoint;

    public PlayerActions Actions { get; set; }
    public ShipMotorComponent motor;
    public GameObject rammingSprite;
    public GameObject stunEffect;
    public UIManager uiManager;
    public Collider shipMesh;
    public GameObject krakenSpot;
    public GameObject dmgState1, dmgState2;
    public ShipStats stats;
    public string cullingMask;
    public cameraFollow followCamera;
    public GameObject victoryScreen;
    public string shipName;
    Vector3 oldEulerAngles;
    Quaternion originalRotation;
    Quaternion originalRotationValue;
    public GameObject ship_model;
    public GameObject spray;

    //Fixed vars
    AbstractGameManager manager;
    KrakenInput kraken;
    GameObject aiSign;
    public bool teamGame = false;
    public int teamNo = 0;
    public int placeInTeam = 1;


    //AIM stuff
    Hookshot hook_component;
    CannonController centralCannon;

    //BOMB stuff
    BombController bombCannon;

    //Current stats
    float pushMagnitude;
    Vector3 pushDirection;
    float velocity;
    float damage = 1;
    float health = 3;
    float boostTimer;
    public bool boosted = false;
    bool ai = false;
    public GameObject wake;
    public ShipAnimator anim;
    Animator invinciblity;
    public GameObject invincibilyPrefab;
    GameObject invincibilityParticle;
    public bool gameStarted = false;
    public bool hasWon = false;
    public bool locked = false;
    public bool dying = false;
    public bool invincible = false;
    public bool startSinking = false;
    public bool aiFire;
    public FreeForAllStatistics gameStats;
    bool isPushed = false;
    public CharacterController cc;
    public bool touchingWind = false;


    public float rotationSpeed = 1f;

    void Start()
    {
        motor.Initialize(cc, stats, transform);
        gameStats = new FreeForAllStatistics();
        manager = GameObject.FindObjectOfType<AbstractGameManager>();
        this.GetComponentInChildren<ShipInstantiator>().setupShipNames(this, type, shipNum, manager.getNumberOfTeams());
        hook_component = this.GetComponent<Hookshot>();
        hook_component.scoreDestination = scoreDestination.transform;
        hook_component.uiManager = uiManager;
        initCannons();
        kraken = GameObject.FindObjectOfType<KrakenInput>();
        startingPoint = this.transform.position;
        anim = GetComponentInChildren<ShipAnimator>();
        anim.category = CategoryHelper.convertType(type);
        this.hook_component.playerCam = followCamera.camera;
        this.hook_component.stats = gameStats;
        followCamera.cullingMask = cullingMask;
        invincibilityParticle = (GameObject)Instantiate(invincibilyPrefab, this.transform.position, invincibilyPrefab.transform.rotation);
        invincibilityParticle.SetActive(false);
        invincibilityParticle.transform.parent = transform;
        invinciblity = anim.GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
        print("ship:" + shipName);
        this.uiManager.gameObject.GetComponentInChildren<ProgressScript>().name = this.shipName;
        health = stats.max_health;
        oldEulerAngles = transform.rotation.eulerAngles;
        originalRotation = ship_model.transform.localRotation; // save the initial rotation
    }

    void initCannons()
    {
        centralCannon = this.GetComponentInChildren<CannonController>();
        centralCannon.aim = hook_component.aim;
        centralCannon.setDelays(stats.shootDelay, stats.alternateShootDelay);
        centralCannon.input = this;
        bombCannon = this.GetComponentInChildren<BombController>();
        bombCannon.input = this;
        bombCannon.bombComponent.parent = bombCannon;
        //centralCannon.cannonForce = this.cannonForce;
    }

    void updateWake()
    {
        wake.transform.rotation = this.transform.rotation;
    }

    public Hookshot getHook()
    {
        return hook_component;
    }

    public void activateInvincibility()
    {
        invincible = true;
        if (invinciblity)
        {
            invinciblity.SetBool("invincibility", true);
        }
        invincibilityParticle.SetActive(true);
    }

    public void deactivateInvincibility()
    {
        invincible = false;
        if (invinciblity)
        {
            invinciblity.SetBool("invincibility", false);
        }
        invincibilityParticle.SetActive(false);
    }

    public void reset()
    {
        anim.resetToIdle();
        setupRespawn();
        CancelInvoke();
    }

    void updateHealth()
    {
        uiManager.setHealthBar(health / stats.max_health);

    }

    void Update()
    {

        if (Actions != null)
        {

            updateHealth();

            if (hook_component.Actions == null)
                hook_component.Actions = Actions;

            if (locked && startSinking)
            {
                transform.Translate(transform.up * -1 * stats.sinkSpeed * (Time.deltaTime * GlobalVariables.gameSpeed));
            }
            if (health > 0 && !hasWon && !locked && !dying)
            {
                if (gameStarted)
                {
                    MoveBoat();
                    toggleDamageStates();
                    centralCannon.handleShoot(transform.forward * velocity * GlobalVariables.gameSpeed, velocity * GlobalVariables.gameSpeed);
                    bombCannon.handleBomb();
                    //tiltBoat ();


                }
                else
                {
                    rotateBoat();
                    //tiltBoat ();
                }
            }
            if (hasWon)
            {
                rotateBoat();
                if (Actions.Green)
                {
                    manager.exitToCharacterSelect();
                }
            }

            uiManager.updateShipUI(this.transform.position, hook_component.shouldShowTooltip());
            if (gameStarted)
            {
                uiManager.updateTutorialPrompts(followCamera.camera, Actions);
            }

        }
    }



    public void toggleDamageStates()
    {
        if (health / stats.max_health <= 0)
        {
            dmgState1.SetActive(false);
            dmgState2.SetActive(false);
        }
        else if (health / stats.max_health < .2)
        {
            dmgState1.SetActive(true);
            dmgState2.SetActive(true);
        }
        else if (health / stats.max_health < .5)
        {
            dmgState1.SetActive(true);
            dmgState2.SetActive(false);
        }
        else if (health / stats.max_health > .5)
        {
            dmgState1.SetActive(false);
            dmgState2.SetActive(false);
        }
    }

    public void sinkToYourDeath()
    { //not being used yet?
        if (!invincible)
        {
            manager.acknowledgeKill(kraken, this);
            locked = true;
            startSinking = true;

            Invoke("takeSinkDamage", stats.sinkTime);
        }
    }

    void OnParticleCollision(GameObject other)
    {

        ChineseJunkShotController particle = other.transform.root.GetComponent<ChineseJunkShotController>();
        if (particle)
        {
            if (particle.parent != this.transform.gameObject)
            {
                gameStats.damageTakenFromChinese += particle.damage;
                hit(particle.damage);
            }
        }

    }

    void takeSinkDamage()
    {
        if (!invincible)
        {
            gameStats.numOfTimesSubmergedByKraken += 1;
            hook_component.UnHook();
            hit(20);
            startSinking = false;
            locked = false;
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (gameStarted)
        {

            if (other.transform == scoreDestination.transform && hook_component.isHooked() && other.gameObject.tag.Equals("ScoringZone"))
            {

                hook_component.barrel.transform.position = Vector3.Lerp(hook_component.barrel.transform.position, scoreDestination.transform.position, Time.time);
                manager.acknowledgeBarrelScore(this, hook_component.barrel);
                //manager.incrementPoint (this, hook_component.barrel);
                uiManager.targetBarrel();

                LightPillar pillar = scoreDestination.transform.parent.GetComponentInChildren<LightPillar>();
                if (pillar != null)
                {
                    pillar.activatePillar();
                }
            }
            if (other.name.Contains("wind"))
            {
                touchingWind = true;
            }
        }
    }



    //checks collisions for kraken attachs, bombs, and barrel destination
    void OnTriggerEnter(Collider other)
    {
        if (gameStarted)
        {
            if (LayerMask.LayerToName(other.gameObject.layer).Equals("kraken_arm") && !invincible)
            {
                KrakenInput kraken = other.gameObject.transform.root.GetComponent<KrakenInput>();
                hit(stats.kraken_damage, kraken);
                other.gameObject.transform.root.GetComponent<KrakenInput>().vibrate(.5f, .5f);
            }
            else
            {
                bombCannon.handleTrigger(other); //collider is bomb
            }

            if (other.name == "nose")
            {
                PlayerInput otherPlayer = other.transform.root.GetComponent<PlayerInput>();
                if (otherPlayer.velocity > otherPlayer.stats.maxVelocity)
                {
                    Instantiate(rammingSprite, other.transform.position, Quaternion.identity);
                    addPushForce(otherPlayer.cc.velocity.normalized, Mathf.Max(otherPlayer.stats.weight - stats.weight, 0f));
                    otherPlayer.velocity = 0;
                }

            }

            if (other.name == "krakenNose")
            {
                KrakenInput kraken = other.transform.root.GetComponent<KrakenInput>();
                if (kraken.animator.isCurrentAnimName("headbash"))
                {
                    kraken.velocity = 0;
                    addPushForce(kraken.cc.velocity, kraken.getCurrentWeight() - stats.weight);
                }
            }

            if (other.name == "KrakenBubbles")
            {
                uiManager.triggerShipAlert();
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("wind"))
        {
            touchingWind = false;
        }
    }


    void rotateBoat()
    {
        Vector3 directionVector;
        directionVector = new Vector3(Actions.Rotate.X, 0f, Actions.Rotate.Y); //Get the direction the user is pushing the left analog stick in
        if (directionVector.magnitude > 0f)
        {
            Quaternion wanted_rotation = Quaternion.LookRotation(directionVector); // get the rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, wanted_rotation, stats.turnSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));
        }
    }


    //how to check clockwise or counter-clockwise rotation?
    void tiltBoat()
    {

        Vector3 directionVector;
        directionVector = new Vector3(Actions.Rotate.X, 0f, Actions.Rotate.Y); //Get the direction the user is pushing the left analog stick in
        Quaternion wanted_rotation = directionVector.Equals(Vector3.zero) ? Quaternion.identity : Quaternion.LookRotation(directionVector); // get the rotation

        if (oldEulerAngles == transform.rotation.eulerAngles)
        { //if not rotating, go back to original rotation

            ship_model.transform.localRotation = Quaternion.Lerp(ship_model.transform.localRotation, originalRotation, stats.tiltSpeed * (Time.deltaTime * GlobalVariables.gameSpeed));

        }
        else
        {

            float angle_difference = (wanted_rotation.eulerAngles.y - transform.rotation.eulerAngles.y);

            if (angle_difference < 0)
            {
                angle_difference = angle_difference + 360f;
            }


            if ((angle_difference < 180) && (angle_difference > 0))
            { //when tilting right

                Quaternion newRotation = Quaternion.Euler(stats.tiltAngle, 0f, 0f);
                ship_model.transform.localRotation = Quaternion.Lerp(ship_model.transform.localRotation, newRotation, stats.tiltSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));

            }
            else if ((angle_difference > 180) && (angle_difference < 360))
            {
                Quaternion newRotation = Quaternion.Euler(-stats.tiltAngle, 0f, 0f);
                ship_model.transform.localRotation = Quaternion.Lerp(ship_model.transform.localRotation, newRotation, stats.tiltSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));
            }

            oldEulerAngles = transform.rotation.eulerAngles; //set new oldEulerAngles

        }
    }



    void rotateSpray()
    {
        if (oldEulerAngles == transform.rotation.eulerAngles)
        {

            spray.transform.localRotation = Quaternion.Slerp(spray.transform.localRotation, originalRotationValue, 100f * (Time.deltaTime * GlobalVariables.gameSpeed));

        }
        else
        {
            oldEulerAngles = transform.rotation.eulerAngles;

            Vector3 directionVector;
            directionVector = new Vector3(Actions.Rotate.X, 0f, Actions.Rotate.Y);
            Quaternion wanted_rotation = Quaternion.LookRotation(directionVector);
            spray.transform.localRotation = Quaternion.RotateTowards(spray.transform.localRotation, wanted_rotation, (stats.turnSpeed * directionVector.magnitude) * (Time.deltaTime * GlobalVariables.gameSpeed));
        }
    }


    //This function takes care of moving the boat
    void MoveBoat()
    {
        motor.UpdateInput(new Vector3(Actions.Rotate.X, 0f, Actions.Rotate.Y));
        /*
        Vector3 directionVector;
        directionVector = new Vector3(Actions.Rotate.X, 0f, Actions.Rotate.Y); //Get the direction the user is pushing the left analog stick in
        if ((directionVector.magnitude == 0 && velocity != 0f) || (velocity > stats.maxVelocity) || isPushed)
        {
            velocity = Mathf.Max(0f, (velocity - (stats.deAccelerationSpeed * (Time.deltaTime * GlobalVariables.gameSpeed)))); //deaccelerate
        }
        
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z); //clamp y


        
        if (isPushed) {
           
			directionVector = pushDirection;
            transform.Rotate(Vector3.up, rotationSpeed * cc.velocity.magnitude, Space.World);
            cc.Move(pushDirection * velocity * (Time.deltaTime * GlobalVariables.gameSpeed));
            if (velocity <= 0 || cc.velocity.magnitude <=0.2f) {
                stopPushForce();
            }
            return;
		}

		if (boosted) {

			uiManager.setBoostBar ((Time.realtimeSinceStartup - boostTimer) / stats.boostResetTime);
			if ((Time.realtimeSinceStartup - boostTimer) > stats.boostResetTime)
			{
				boosted = false;
			}
		}

		if (Actions.Boost && !boosted){
            SoundManager.playSound(SoundClipEnum.Boost, SoundCategoryEnum.Generic, this.transform.position);
            velocity = stats.boostVelocity;
			gameStats.numOfBoosts++;
			vibrate (.5f, .5f);
			boosted = true;
			boostTimer = Time.realtimeSinceStartup;

		} 


		

		else if (directionVector.magnitude > 0f && velocity <= stats.maxVelocity) {
			velocity = Mathf.Min (stats.maxVelocity, velocity + (directionVector.magnitude * stats.moveSpeed * (Time.deltaTime * GlobalVariables.gameSpeed))); 
		}

		if (directionVector.magnitude >0f) { 
				Quaternion wanted_rotation = Quaternion.LookRotation (directionVector); // get the rotation
				transform.rotation = Quaternion.RotateTowards (transform.rotation, wanted_rotation, stats.turnSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));		
		}


		if (hook_component.isHooked ()) {	
		   cc.Move (transform.forward * velocity * stats.barrelSlowDownFactor * (Time.deltaTime * GlobalVariables.gameSpeed));
		} 

		else {
			if (touchingWind) {
				velocity = GlobalVariables.windFactor;
			}
		   cc.Move (transform.forward * velocity * (Time.deltaTime * GlobalVariables.gameSpeed));
    
		}

        if (this.transform.position.y !=0 && !dying)
        {
            transform.position = MathHelper.setY(transform.position,Mathf.Lerp(transform.position.y, 0, 1));
        }		       
		updateWake();
        */
    }



    public void addPushForce(Vector3 direction, float magnitude)
    {
        if (magnitude > 0 && !isPushed)
        {
            hook_component.UnHook();
            velocity = stats.maxVelocity * magnitude;
            followCamera.startShake();
            pushMagnitude = magnitude;
            isPushed = true;
            stunEffect.SetActive(true);
            pushDirection = direction;
        }

    }


    void stopPushForce()
    {
        isPushed = false;
        pushDirection = Vector3.zero;
        pushMagnitude = 0f;
        stunEffect.SetActive(false);

    }


    public void vibrate(float intensity, float time)
    {
        if (Actions.Device != null)
        {
            Actions.Device.Vibrate(intensity);
            Invoke("stopVibrate", time);
        }
    }


    public void joinVibrate()
    {
        if (Actions.Device != null)
        {
            Actions.Device.Vibrate(1);
            Invoke("stopVibrate", .5f);
        }
    }


    void stopVibrate()
    {
        Actions.Device.StopVibration();
    }

    public void hit(float passedDamage = 0f, StatsInterface attacker = null)
    {
        if (!invincible && health > 0)
        {
            if (this.teamGame && attacker is PlayerInput)
            {
                if (((PlayerInput)attacker).teamNo == this.teamNo)
                {
                    vibrate(.5f, .5f);
                    return;
                }
            }
            float actualDamage = (passedDamage > 0) ? passedDamage : damage;
            health -= actualDamage;
            SoundManager.playSound(SoundClipEnum.ShipHit, SoundCategoryEnum.Generic, transform.position);
            gameStats.healthLost += actualDamage;
            if (attacker is PlayerInput)
            {
                ((PlayerInput)attacker).gameStats.addGivenDamage(type.ToString(), actualDamage);
                this.gameStats.addTakenDamage(((PlayerInput)attacker).type.ToString(), actualDamage);
            }
            else if (attacker is KrakenInput)
            {
                ((KrakenInput)attacker).gameStats.addGivenDamage(type.ToString(), actualDamage);
                this.gameStats.addTakenDamage("kraken", actualDamage);
            }
            if (health <= 0)
            {
                vibrate(1f, 1f);
                hook_component.UnHook();
                checkColliders(false);
                manager.acknowledgeKill(attacker, this);
                die();
            }
            else
            {
                vibrate(.5f, .5f);
            }
        }
    }



    void checkColliders(bool check)
    {
        if (check)
        {
            GetComponent<CharacterController>().enabled = true;
        }
        else
        {
            GetComponent<CharacterController>().enabled = false;
        }

        shipMesh.enabled = check;
    }


    public void die()
    {
        hook_component.UnHook();
        dying = true;
        SoundManager.playSound(SoundClipEnum.SinkExplosion, SoundCategoryEnum.Generic, transform.position);
        centralCannon.gameObject.SetActive(false);
        bombCannon.activateAllBombs();
        anim.triggerDeathAnimation();
        gameStats.numOfDeaths++;
        followCamera.zoomIn = true;
    }


    public void setupRespawn()
    {
        bombCannon.bombCount = 3;
        velocity = 0f;
        isPushed = false;
        boosted = false;
        followCamera.zoomIn = false;
        bombCannon.resetBombs();
        centralCannon.ResetShots();
        stopPushForce();
        //shipMesh.enabled = false;
        manager.respawnPlayer(this, startingPoint);
        //anim.triggerRespawnAnimation ();
        health = stats.max_health;
        followCamera.setRespawn();
    }


    public void rebirth()
    {
        centralCannon.gameObject.SetActive(true);
        dying = false;

        activateInvincibility();
        Invoke("deactivateInvincibility", stats.invinciblityTime);

        checkColliders(true);
    }

    public Quaternion getCannonRotation()
    {
        return this.centralCannon.transform.rotation;
    }

    public Transform getCannonPosition()
    {
        return this.centralCannon.cannonBallPos;
    }


}