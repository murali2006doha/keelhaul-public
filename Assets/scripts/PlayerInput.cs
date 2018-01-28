using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using InControl;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour, StatsInterface
{

    //Refactor later into instantiator
    public ShipEnum type;
    public int shipNum;
    public GameObject scoreDestination;
    public Vector3 startingPoint;
    public Quaternion startingRotation;
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
    public ShipWorldCanvas worldCanvas;
    public GameObject pauseModal;

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
    public Animator invinciblity;
    public GameObject invincibilyPrefab;
    public GameObject invincibilityParticle;
    public bool gameStarted = false;
    public bool hasWon = false;
    public bool locked = false;
    public bool dying = false;
    public bool invincible = false;
    public bool startSinking = false;
    public bool aiFire;
    public FreeForAllStatistics gameStats = new FreeForAllStatistics();
    bool isPushed = false;
    public CharacterController cc;
    public bool touchingWind = false;


    public float rotationSpeed = 1f;
    private bool notInitalized = true;
    public ShipStatus status = ShipStatus.Waiting;

    internal double getHealthPercent()
    {
        if (stats.max_health == 0)
        {
            return 0f;
        }
        return health / stats.max_health;
    }

    public int playerId;
    public static UnityAction onHitRegister;    //extra actions for when player is hit

    void Start()
    {
        //Have to refactor this later
        manager = GameObject.FindObjectOfType<AbstractGameManager>();
        this.GetComponentInChildren<ShipInstantiator>().setupShipNames(this, type, this.GetId(), manager.getNumberOfTeams(), this.GetId());
        bool hasDevice = Actions == null || Actions.Device == null;
        motor.Initialize(
            cc,
            stats,
            transform,
            () => {
                vibrate(0.5f, 2f);
                uiManager.setBoostBar(0);
                uiManager.animManager.onBoost();
            },
            () => {

            },
            () => {
                uiManager.animManager.onBoostRecharged();
            },
            hasDevice);

        aimComponent.Initialize(transform, hasDevice,followCamera.camera);
        bombController.Initialize(
            stats,
            this,
            uiManager,
            gameStats,
            PathVariables.GetAssociatedBombForShip(type));
        InitializeHookshot();

        shipMeshComponent.Initialize(
            this,
            stats,
            uiManager,
            hookshotComponent,
            manager,
            bombController,
            hit
        );

        centralCannon.Initialize(
            this,
            this.transform,
            this.aimComponent.aim,
            stats,
            gameStats,
            motor,
            PathVariables.GetAssociatedCannonballForShip(type));
        altCannonComponent.Initialize(this, this.transform, this.aimComponent.aim, stats, uiManager);
        kraken = GameObject.FindObjectOfType<KrakenInput>();
        startingPoint = this.transform.position;
        startingRotation = this.transform.rotation;
        anim = GetComponentInChildren<ShipAnimator>();
        anim.category = CategoryHelper.convertType(type);
        followCamera.cullingMask = cullingMask;
        invinciblity = anim.GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
        print("ship:" + shipName);
        this.uiManager.gameObject.GetComponentInChildren<ProgressScript>().name = this.shipName;
        health = stats.max_health;
        oldEulerAngles = transform.rotation.eulerAngles;
        originalRotation = ship_model.transform.localRotation; // save the initial rotation
        InitializeShipInput();
        setStatus(ShipStatus.Waiting);
        this.GetComponent<PhotonView>().RPC("InstantiateWorldSpaceCanvas", PhotonTargets.OthersBuffered, this.GetId());
    }

    internal void InitializeForSabotage()
    {
        hookshotComponent.enabled = true;
    }

    internal void InitializeForDeathMatch()
    {
        aimComponent.aim.GetComponent<MeshRenderer>().enabled = false;
    }

    internal void SetUpScoreDestination(GameObject scoreDestination)
    {
        if (hookshotComponent)
        {
            hookshotComponent.destination = scoreDestination;
            shipMeshComponent.scoreDestination = scoreDestination;
        }
    }

    [PunRPC]
    public void InstantiateWorldSpaceCanvas(int shipNum) {
      worldCanvas = Instantiate(worldCanvas);
      this.worldCanvas.transform.SetParent(this.transform, false);
      this.worldCanvas.Initiialize(shipNum);
    }


    [PunRPC]
    public void UpdateWorldSpaceHealthBar(float sliderVal)
    {
        this.worldCanvas.UpdateHealthSlider(sliderVal);
    }

    public void SetLockedStatus(bool locked) {
        motor.locked = locked;
    }
    private void InitializeHookshot()
    {
        if (hookshotComponent)
        {
            var aimPhysics = aimComponent.GetComponent<AimPhysicsComponent>();
            if (aimPhysics)
            {
                hookshotComponent.Initialize(uiManager, gameStats, aimPhysics.isAimTouchingBarrel);
            }

        }
    }


    public void InitializeShipInput()
    {
        shipInput.actions = Actions;
        shipInput.onRotateChanged += motor.UpdateInput;
        shipInput.onRedButtonPress += bombController.handleBomb;
        shipInput.onLeftBumperDown += motor.Boost;
        shipInput.onRightRotateChanged += aimComponent.AimAt;
        shipInput.onRightTriggerDown += centralCannon.handleShoot;
        shipInput.onRightBumperDown += altCannonComponent.handleShoot;
        shipInput.onPauseButtonPress += this.instantiatePauseMenu;
        shipInput.onSelectButtonHoldDown += this.showStatsScreen;
        //shipInput.onSelectButtonRelease += null;
        shipInput.onSelectButtonRelease += this.hideStatsScreen;

        if (hookshotComponent)
        {
            shipInput.onLeftTriggerDown += hookshotComponent.HookBarrel;
            hookshotComponent.onHook += (x) => { if (x) { motor.setSpeedModifier(stats.barrelSlowDownFactor); } else { motor.setSpeedModifier(1); } };
        }
    }


    public void clearShipInput()
    {
        shipInput.onRotateChanged = null;
        shipInput.onRedButtonPress = null;
        shipInput.onLeftBumperDown = null;
        shipInput.onRightRotateChanged = null;
        shipInput.onRightTriggerDown = null;
        shipInput.onRightBumperDown = null;
        shipInput.onLeftTriggerDown = null;
        shipInput.onSelectButtonHoldDown = null;
        shipInput.onSelectButtonRelease = null;
    }


    void instantiatePauseMenu() {
        if (FindObjectOfType<CountDown>() == null && !FindObjectOfType<MenuModel>()) {
		
			clearShipInput();

            GameObject pause = Instantiate(pauseModal);
            pause.GetComponent<MenuModel>().pauseMenu.Initialize(this.Actions, this.manager, () => {
				InitializeShipInput();
				pause.GetComponent<MenuModel>().pauseMenu.ResumeGame();
                Destroy(pause.gameObject);
			});
        }
	}


    void showStatsScreen() {
        uiManager.InitializeStatsScreen(manager, this);
    }

    void hideStatsScreen() {
        uiManager.SetOffStatsScreen();
    }

    public void DisableUIForStats()
    {
        worldCanvas.gameObject.SetActive(false);
    }



    public HookshotComponent getHook() {
        return hookshotComponent;
    }

    public void activateInvincibility(bool showParticles = true)
    {
        if (GetComponent<PhotonView>().isMine)
        {
            invincible = true;
            if (!motor.sinking)
            {
                SetLockedStatus(false);
            }
            if (showParticles)
            {
                GetComponent<PhotonView>().RPC("ToggleInvincibilityParticle", PhotonTargets.AllBuffered, true);

            }
        }

    }

    public void deactivateInvincibility()
    {
        invincible = false;
        GetComponent<PhotonView>().RPC("ToggleInvincibilityParticle", PhotonTargets.AllBuffered,false);
    }

    [PunRPC]
    public void ToggleInvincibilityParticle(bool active)
    {

        invinciblity.SetBool("invincibility", active);

        if (invincibilityParticle != null)
        {
            invincibilityParticle.SetActive(active);

        }
    }


    public void reset()
    {
        anim.resetToIdle();
        setupRespawn();
        CancelInvoke();
    }

    void updateHealth()
    {
        var sliderVal = health / stats.max_health;
        uiManager.setHealthBar(sliderVal);
        this.GetComponent<PhotonView>().RPC("UpdateWorldSpaceHealthBar", PhotonTargets.Others, sliderVal);
    }

    void Update()
    {
        if (Actions != null)
        {
            updateHealth();
            if (manager.isGameOver())
            {
                if (Actions.Green)
                {
                    manager.ExitToCharacterSelect();
                }
            }
           
            uiManager.updateShipUI(this.transform.position, hookshotComponent.shouldShowTooltip());
            if (gameStarted)
            {
                uiManager.updateTutorialPrompts(followCamera.camera, Actions);
            }

        }
    }


    public void TurnOnAi() {
        this.GetComponent<ShipAIV2>().enabled = true;
        shipInput.enabled = !shipInput.enabled;
    }

    [PunRPC]
    public void ToggleDamageStates(float health)
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
            motor.StartSinking();
            Invoke("takeSinkDamage", 1f);
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

            gameStats.numOfTimesSubmergedByKraken += 1;
        if (hookshotComponent.enabled)
        {
            hookshotComponent.UnHook();

        }
        hit(health, kraken.id, true);
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
            if (hookshotComponent.enabled)
            {
                hookshotComponent.UnHook();

            }
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
        if (Actions !=null && Actions.Device != null)
        {
            shipInput.actions.Device.Vibrate(intensity);
            Invoke("stopVibrate", time);
        }
    }


    public void joinVibrate()
    {
        if (Actions.Device != null)
        {
            shipInput.actions.Device.Vibrate(1);
            Invoke("stopVibrate", .5f);
        }
    }


    void stopVibrate()
    {
        shipInput.actions.Device.StopVibration();
    }

    [PunRPC]
    public void PlayHitSound()
    {
        SoundManager.playSound(SoundClipEnum.ShipHit, SoundCategoryEnum.Generic, transform.position);
    }

    public void hit(float passedDamage, int id,bool isKraken = false)
    {
        if (onHitRegister != null) {
            onHitRegister ();
        }
        var player = manager.getPlayerWithId(id);
        if (!isKraken && (player == null || (manager.getNumberOfTeams()>1 &&  teamNo == player.teamNo)))
        {
            return;
        }

        if (!invincible && health > 0 && this.status == ShipStatus.Alive)
        {
            float actualDamage = (passedDamage > 0) ? passedDamage : damage;
            health -= actualDamage;

            gameStats.healthLost += actualDamage;
            followCamera.startShake();
            var photonView = GetComponent<PhotonView>();
            TurnRed();
            photonView.RPC("PlayHitSound", PhotonTargets.All, null);
            photonView.RPC("ToggleDamageStates", PhotonTargets.All, health);
            uiManager.animManager.onHit();
            if (!isKraken)
            {
                photonView.RPC("AddDamageStats", PhotonPlayer.Find(id), id, type.ToString(), actualDamage, true);
                gameStats.addTakenDamage(player.type.ToString(), actualDamage);
                if (PhotonNetwork.offlineMode)
                {
                    player.gameStats.addGivenDamage(type.ToString(), actualDamage);
                }
            }
            else
            {
                gameStats.addTakenDamage("kraken", actualDamage);
            }
            if (health <= 0)
            {
                setStatus(ShipStatus.Dead);
                vibrate(1f, 1f);
                if (hookshotComponent.enabled)
                {
                    hookshotComponent.UnHook();

                }
                checkColliders(false);
                foreach (DeathMatchGameManager manager1 in GameObject.FindObjectsOfType<DeathMatchGameManager>())
                {
                    manager1.GetComponent<PhotonView>().RPC("IncrementPoint", PhotonTargets.All, id);
                }
                if (isKraken)
                {
                    SabotageGameManager sabManager = (SabotageGameManager)manager;
                    sabManager.GetComponent<PhotonView>().RPC("IncrementPoint", PhotonTargets.All, id);
                }
                die(id);
                SendToKillFeed(id);
            }
            else
            {
                vibrate(.5f, .5f);
            }
        }

    }

    private void SendToKillFeed(int id)
    {
        foreach (PlayerInput player in FindObjectsOfType<PlayerInput>())
        {
            player.GetComponent<PhotonView>().RPC("AddToKillFeed", PhotonTargets.All, "P" + id, manager.getShipById(id), "P" + GetId(), type.ToString());
        }
        if (kraken)
        {
            kraken.uiManager.AddToKillFeed("P" + id, manager.getShipById(id), "P" + GetId(), type.ToString());
        }
    }



    public void SendBarrelScoreToKillFeed()
    {

        foreach (PlayerInput player in FindObjectsOfType<PlayerInput>())
        {
            player.GetComponent<PhotonView>().RPC("AddBarrelScoreToKillFeed", PhotonTargets.All, "P" + GetId(), type.ToString());
        }

        if (kraken)
        {
            kraken.uiManager.AddBarrelScoreToKillFeed("P" + GetId(), type.ToString());
        }
    }

    public void TurnRed()
    {
        anim.playDamageAnimation();
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

    [PunRPC]
    public void AddDamageStats(int id, string name,float damage, bool given)
    {
        if (PhotonNetwork.player.ID == id)
        {
            if (given)
            {
                gameStats.addGivenDamage(name, damage);
            }
            else
            {
                gameStats.addTakenDamage(name, damage);

            }
        }
    }

    [PunRPC]
    public void AddKillStats(int id)
    {
        if (PhotonNetwork.player.ID == id || PhotonNetwork.offlineMode)
        {
                gameStats.numOfKills++;
        }
    }

    public int GetId()
    {
        return PhotonNetwork.offlineMode ? shipNum : GetComponent<PhotonView>().ownerId;
    }

    public void die(int killerID)
    {
        if (hookshotComponent.enabled)
        {
            hookshotComponent.UnHook();
        }
        dying = true;
        uiManager.animManager.onDeath();
        SoundManager.playSound(SoundClipEnum.SinkExplosion, SoundCategoryEnum.Generic, transform.position);
        centralCannon.gameObject.SetActive(false);
        bombController.activateAllBombs();
        uiManager.showDeathAnimation(killerID, manager.getShipById(killerID));
        anim.triggerDeathAnimation();
        gameStats.numOfDeaths++;
        followCamera.zoomIn = true;
    }

    [PunRPC]
    public void AddToKillFeed(string killer, string killerShip,string victim, string victimShip)
    {
        if (GetComponent<PhotonView>().isMine)
        {
            uiManager.AddToKillFeed(killer,killerShip,victim,victimShip);
            if(killer == ("P" + GetId()))
            {
                uiManager.animManager.onKill();
            }
        }
    }

    [PunRPC]
    public void DQToKillFeed(int player)
    {
        if (GetComponent<PhotonView>().isMine)
        {
            uiManager.DQToKillFeed(player);
        }
    }

    [PunRPC]
    public void AddBarrelScoreToKillFeed(string player, string ship)
    {
        if (GetComponent<PhotonView>().isMine)
        {
            uiManager.AddBarrelScoreToKillFeed(player,ship);
            if (player == ("P" + GetId()))
            {
                uiManager.animManager.onKill();
            }
        }
    }


    public void setupRespawn()
    {
        if (!GetComponent<PhotonView>().isMine) {
            return;
        }
        vibrate(1f, 2f);
        velocity = 0f;
        isPushed = false;
        followCamera.zoomIn = false;
        uiManager.hideDeathAnimation();
        uiManager.animManager.OnRespawn();
        bombController.resetBombs();
        centralCannon.ResetShotRight();
        altCannonComponent.ResetShotAlt();
        stopPushForce();
        this.SetLockedStatus(false);
        //shipMesh.enabled = false;
        manager.respawnPlayer(this, startingPoint, startingRotation);
        //anim.triggerRespawnAnimation ();
        health = stats.max_health;
        followCamera.setRespawn();
    }


    public void rebirth()
    {
        centralCannon.gameObject.SetActive(true);
        dying = false;
        this.SetLockedStatus(false);
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


    public void AddToHealth(float extraHealth) {
    if (extraHealth > (stats.max_health - this.health)) {   //if greater than difference
        this.health = stats.max_health;
    } else if (this.health < stats.max_health) {
        this.health += extraHealth;
    }
    }

    public void setStatus(ShipStatus status)
    {

        if (!GetComponent<PhotonView>().isMine) {
            return;
        }

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

    [PunRPC]
    public void ChangeSkin(int altSkinCount)
    {
        shipMeshComponent.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>(PathVariables.GetAssociatedTextureSkinPath(type, altSkinCount));
    }
}
