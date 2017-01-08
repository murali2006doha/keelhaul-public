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

    [Header("Component Variables")]
    public ShipMotorComponent motor;
    public BombControllerComponent bombController;
    public AimComponent aimComponent;
    public HookshotComponent hookshotComponent;
    public ShipCannonComponent centralCannon;
    public AbstractAltCannonComponent altCannonComponent;
	public ShipMeshPhysicsComponent shipMeshComponent;

    [Header("Other Scene Variables")]
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


    //Current stats
    float pushMagnitude;
    float lastInkHitTime;
    Vector3 pushDirection;
    public float velocity;
    float damage = 1;
    float health = 3;
    bool ai = false;
    public GameObject wake;
    public AbstractInputManager shipInput;
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
    private bool notInitalized = true;
    public ShipStatus status = ShipStatus.Waiting;
    public int playerId;


    void Start()
    {
        //Have to refactor this later
        manager = GameObject.FindObjectOfType<AbstractGameManager>();
        this.GetComponentInChildren<ShipInstantiator>().setupShipNames(this, type, shipNum, manager.getNumberOfTeams(), playerId);

        motor.Initialize(
            cc, 
            stats, 
            transform, 
            () => {
                uiManager.setBoostBar(0);
                followCamera.ActivateMotionBlur();
            },
            () => {
                followCamera.DeActivateMotionBlur();
            }
        );

        aimComponent.Initialize(transform);
        bombController.Initialize(stats, this, uiManager, gameStats);
        InitializeHookshot();
        
		shipMeshComponent.Initialize(
            this, 
            stats, 
            uiManager, 
            scoreDestination, 
            hookshotComponent, 
            manager, 
            bombController,
            hit
        );

        centralCannon.Initialize(this, this.transform, this.aimComponent.aim, stats, gameStats, motor);
        altCannonComponent.Initialize(this, this.transform, this.aimComponent.aim, stats, uiManager);


        gameStats = new FreeForAllStatistics();
        kraken = GameObject.FindObjectOfType<KrakenInput>();
        startingPoint = this.transform.position;
        anim = GetComponentInChildren<ShipAnimator>();
        anim.category = CategoryHelper.convertType(type);
        followCamera.cullingMask = cullingMask;
        invincibilityParticle = Instantiate(invincibilyPrefab, this.transform.position, invincibilyPrefab.transform.rotation);
        invincibilityParticle.SetActive(false);
        invincibilityParticle.transform.parent = transform;
        invinciblity = anim.GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
        print("ship:" + shipName);
        this.uiManager.gameObject.GetComponentInChildren<ProgressScript>().name = this.shipName;
        health = stats.max_health;
        oldEulerAngles = transform.rotation.eulerAngles;
        originalRotation = ship_model.transform.localRotation; // save the initial rotation
        InitializeShipInput();
        setStatus(ShipStatus.Waiting);
    }


    private void InitializeHookshot()
    {
        if (hookshotComponent)
        {
            var aimPhysics = aimComponent.GetComponent<AimPhysicsComponent>();
            if (aimPhysics)
            {
                hookshotComponent.Initialize(uiManager, gameStats, aimPhysics.isAimTouchingBarrel,scoreDestination);
            }

        }
    }


    void InitializeShipInput()
    {
        shipInput.actions = Actions;
        shipInput.onRotateChanged += motor.UpdateInput;
        shipInput.onRedButtonPress += bombController.handleBomb;
        shipInput.onLeftBumperDown += motor.Boost;
        shipInput.onRightRotateChanged += aimComponent.AimAt;
        shipInput.onRightTriggerDown += centralCannon.handleShoot;
        shipInput.onRightBumperDown += altCannonComponent.handleShoot;

        if (hookshotComponent)
        {
            shipInput.onLeftTriggerDown += hookshotComponent.HookBarrel;
            hookshotComponent.onHook += (x) => { if (x) { motor.setSpeedModifier(stats.barrelSlowDownFactor); } else { motor.setSpeedModifier(1); } };
        }
    }

    public HookshotComponent getHook()
    {
        return hookshotComponent;
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

            if (locked && startSinking)
            {
                transform.Translate(transform.up * -2 * stats.sinkSpeed * (Time.deltaTime * GlobalVariables.gameSpeed));
            }
            if (hasWon)
            {
                if (Actions.Green)
                {
                    manager.exitToCharacterSelect();
                }
            }

            uiManager.updateShipUI(this.transform.position, hookshotComponent.shouldShowTooltip());
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
            print("hyes");
            manager.acknowledgeKill(kraken, this);
            locked = true;
            startSinking = true;
            setStatus(ShipStatus.Dead);

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
           //     hit(particle.damage);
            }
        }

    }

    void takeSinkDamage()
    {
        if (!invincible)
        {
            gameStats.numOfTimesSubmergedByKraken += 1;
            hookshotComponent.UnHook();
       //     hit(20);
            startSinking = false;
            locked = false;
        }
    }




    private void resetInkSpeed()
    {
        if (!hookshotComponent.isHooked() && (Time.realtimeSinceStartup - lastInkHitTime) > stats.inkSlowDownTime)
        {
            motor.setSpeedModifier(1);
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


    public void addPushForce(Vector3 direction, float magnitude)
    {
        if (magnitude > 0 && !isPushed)
        {
            hookshotComponent.UnHook();
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



    public void hit(float passedDamage, int id)
    {
        if (!invincible && health > 0)
        {

            float actualDamage = (passedDamage > 0) ? passedDamage : damage;
            health -= actualDamage;
            SoundManager.playSound(SoundClipEnum.ShipHit, SoundCategoryEnum.Generic, transform.position);
            gameStats.healthLost += actualDamage;
            if (health <= 0)
            {
                vibrate(1f, 1f);
                hookshotComponent.UnHook();
                checkColliders(false);
                foreach (DeathMatchGameManager manager1 in GameObject.FindObjectsOfType<DeathMatchGameManager>())
                {
                    manager1.GetComponent<PhotonView>().RPC("IncrementPoint", PhotonTargets.All, id);
                }
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
        hookshotComponent.UnHook();
        dying = true;
        SoundManager.playSound(SoundClipEnum.SinkExplosion, SoundCategoryEnum.Generic, transform.position);
        centralCannon.gameObject.SetActive(false);
        bombController.activateAllBombs();
        anim.triggerDeathAnimation();
        gameStats.numOfDeaths++;
        followCamera.zoomIn = true;
    }


    public void setupRespawn()
    {
        velocity = 0f;
        isPushed = false;
        followCamera.zoomIn = false;
        bombController.resetBombs();
        centralCannon.ResetShotRight();
        altCannonComponent.ResetShotAlt();
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
        setStatus(ShipStatus.Alive);
        activateInvincibility();
        Invoke("deactivateInvincibility", stats.invinciblityTime);

        checkColliders(true);
    }

    public Quaternion getAltCannonRotation()
    {
        return this.altCannonComponent.transform.rotation;
    }

    public Transform getAltCannonPosition()
    {
        return this.altCannonComponent.cannonBallPos;
    }

    public void setStatus(ShipStatus status)
    {
        this.status = status;
        motor.reset();
        centralCannon.ResetShotRight();
        altCannonComponent.ResetShotAlt();
        shipInput.onRotateChanged -= motor.UpdateInput;
        shipInput.onRedButtonPress -= bombController.handleBomb;
        shipInput.onLeftBumperDown -= motor.Boost;
        shipInput.onRightRotateChanged -= aimComponent.AimAt;
        shipInput.onRightTriggerDown -= centralCannon.handleShoot;
        shipInput.onRightBumperDown -= altCannonComponent.handleShoot;

        if (hookshotComponent)
        {
            shipInput.onLeftTriggerDown -= hookshotComponent.HookBarrel;
        }
        if (status == ShipStatus.Dead)
        {
            motor.enabled = false;
        }
        else
        {
            motor.enabled = true;
        }

        if (status == ShipStatus.Waiting)
        {
            shipInput.onRightRotateChanged += aimComponent.AimAt;
        }
        if (status == ShipStatus.Alive)
        {
            shipInput.onRotateChanged += motor.UpdateInput;
            shipInput.onRedButtonPress += bombController.handleBomb;
            shipInput.onLeftBumperDown += motor.Boost;
            shipInput.onRightRotateChanged += aimComponent.AimAt;
            shipInput.onRightTriggerDown += centralCannon.handleShoot;
            shipInput.onRightBumperDown += altCannonComponent.handleShoot;

            if (hookshotComponent)
            {
                shipInput.onLeftTriggerDown += hookshotComponent.HookBarrel;
            }
        }
    }


}
