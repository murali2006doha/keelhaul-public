using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using InControl;
using System;

public class KrakenInput : MonoBehaviour, StatsInterface {

    //This the variable that defines what player actions player can input.
    //To define more player actions check the playerActions script
    public PlayerActions Actions { get; set; }

    [Header("Drag and drop variables")] //these variables should only ever be set by dragging and dropping from within the prefab
    public GameObject victoryScreen;
    public GameObject krakenhit;
    public SpriteRenderer submergeSprite;
    public KrakenAnimator animator;
    public KrakenStats stats;

    [Header("Status Variables")] // used for debugging and communication between scripts
    public bool hasWon = false;
    public bool submerged = false;
    public bool smash = false;
    public float health;
    public bool isSubmerging = false;
    public bool dying = false;
    public bool gameStarted = false;
    public bool hittingPlayer = false;
    public bool attacking = false;
    public int currentState;
    public int currentStage;

    [Header("Scene Variables")]
    public cameraFollow followCamera;
    public AbstractGameManager manager;
    public UIManager uiManager;
    public GameObject bubbles;
    public float velocity;
    public GameObject wake;
    public GameObject spray;
    public spitBallCannon spitter;

    //properties | variables like speed and health that change over time inside this script only

    GameObject previousShip;
    Transform krakenArmPosition;
    Vector3 startingPoint;
    bool ai = false;
    GameObject hittingShip;
    string cullingMask = "kraken_ui";
    bool hasHitShip = false;
    float headbashChargeTime = 0f;
    public CharacterController cc;
    GameObject aiSign;
    


    public FreeForAllStatistics gameStats;



    void Start()
    {

        initializeVariables();

    }

    void initializeVariables()
    {

        health = stats.stages[currentStage].max_health;
        startingPoint = transform.position;
        currentStage = 0;
        cc = GetComponent<CharacterController>();
        krakenArmPosition = transform.Find("KrakenArm");
        animator.splashParticles = ArrayHelper.filterTag(this.GetComponentsInChildren<ParticleSystem>(), "Submerge");
        animator.emergeSplashParticles = ArrayHelper.filterTag(this.GetComponentsInChildren<ParticleSystem>(), "Emerge");
        manager = GameObject.FindObjectOfType<AbstractGameManager>();
        gameStats = new FreeForAllStatistics();

    }


    public void incrementPoint()
    {
        manager.incrementPoint(this);
        currentStage = Mathf.Min(currentStage, currentStage + 1);
    }


	void updateHealth() {
		uiManager.setHealthBar(health / stats.stages[currentStage].max_health);

	}

    // Update is called once per frame
    void Update()
    {
        if (gameStarted) {
		updateHealth ();
        }

        if (Actions != null && !animator.isCurrentAnimName("death") && gameStarted)
        {
            followCamera.cullingMask = cullingMask;
            updateKraken();

        }

        if (gameStarted && Input.GetKeyUp(KeyCode.Z))
        {
            if (!ai)
            {
                ai = true;
                activateAI();
            }
            else
            {
                ai = false;
                this.GetComponent<KrakenAi>().enabled = false;
                aiSign.SetActive(false);
                this.GetComponent<NavMeshAgent>().enabled = false;
            }
        }
        else if (!gameStarted && manager.isGameOver())
        {
            rotateKraken();
        }

        if (gameStarted)
        {
            uiManager.updateTutorialPrompts(followCamera.camera, Actions);
        }

        if (hasWon)
        {
            if (Actions.Green)
            {
                manager.exitToCharacterSelect();

            }
        }
    }


    void activateAI()
    {

        aiSign.SetActive(true);
        //.GetComponent<CharacterController> ().enabled = false;
        this.GetComponent<NavMeshAgent>().enabled = true;
        this.GetComponent<KrakenAi>().enabled = true;

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

    void OnTriggerEnter(Collider other)
    {

        if (LayerMask.LayerToName(other.gameObject.layer).Equals("player"))
        {
            hittingPlayer = true;
            hittingShip = other.gameObject;
        }

        if ((LayerMask.LayerToName(other.gameObject.layer).Equals("bomb") || LayerMask.LayerToName(other.gameObject.layer).Equals("team bomb")))
        {//to activate a bomb
            if (submerged == false)
            { //only if not submerged
                gameStats.numOfBombsDetonated++;
                Bomb b = other.gameObject.GetComponent<Bomb>();
                StartCoroutine(b.ActivateBomb(other.gameObject));
            }
        }

        if ((LayerMask.LayerToName(other.gameObject.layer).Equals("explosion") || LayerMask.LayerToName(other.gameObject.layer).Equals("team explosion")))
        {//check if kraken is in range when a bomb is exploding

            if (submerged == false)
            { //only if not submerged

                Instantiate(krakenhit, transform.position, transform.rotation);
                hit();
            }
        }
    }


    void rotateKraken()
    {
        Vector3 directionVector;
        directionVector = new Vector3(Actions.Rotate.X, 0f, Actions.Rotate.Y); //Get the direction the user is pushing the left analog stick in
        Quaternion wanted_rotation = Quaternion.LookRotation(directionVector); // get the rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, wanted_rotation, stats.stages[currentStage].submergedTurnSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));
    }


    void OnTriggerExit(Collider other)
    {

        if (LayerMask.LayerToName(other.gameObject.layer).Equals("player"))
        {
            hittingPlayer = false;
            hittingShip = null;
        }

    }


    //returns if the string state is the current animation state
    bool isCurrentState(string state)
    {

        if (Animator.StringToHash(state) == currentState)
        {
            return true;
        }

        return false;
    }


    void updateKraken()
    {
        currentState = animator.getState();
        moveKraken();
        uiManager.updateCompass(this.transform.position);
        if (animator.isCurrentAnimName("idle"))
        {
            if (Actions.Fire.WasPressed && stats.canPerformAction(Actions.Fire.Name, currentStage)) {
                //animator.startFire(); re-enable for spitball
				animator.executeSmash();
				Invoke("resetSmash", 0.1f);
            }

            if (Actions.Blue.WasPressed && stats.canPerformAction(Actions.Blue.Name, currentStage))
            { //Submerge
                SoundManager.playSound(SoundClipEnum.KrakenSubmerge,SoundCategoryEnum.KrakenStageOne, transform.position);
                velocity = 0;
                animator.submergeKraken();
                Invoke("disableSpray", stats.stages[currentStage].emergeTime);

            }

            if (Actions.Alt_Fire.WasPressed && stats.canPerformAction(Actions.Alt_Fire.Name, currentStage))
            {
				// re-enable after evolution
                //animator.chargeHeadbash();
                //headbashChargeTime = Time.realtimeSinceStartup;
            }
        }

        if (animator.isCurrentAnimName("spitCharge")) {
            if (!Actions.Fire.IsPressed)
            {
                animator.stopCharging();
            }
        }

        if (animator.isCurrentAnimName("headbashCharge"))
        {
            float headbashChargedMagnitude =  Time.realtimeSinceStartup - headbashChargeTime;
            constantVibrate(headbashChargedMagnitude);
            if (!Actions.Alt_Fire.IsPressed) {
                velocity = Mathf.Min(stats.stages[currentStage].maxHeadbashChargeVelocity, stats.stages[currentStage].headbashChargeVelocity * headbashChargedMagnitude);
                animator.launchHeadbash();
            }
        }
        if (animator.isCurrentAnimName("submerged"))
        {
            if (hittingShip)
            {
                gameStats.timeSpentUnderShips += Time.deltaTime;
            }

            if (Actions.Blue.WasPressed && stats.canPerformAction(Actions.Blue.Name, currentStage))
            {  //rise up 
                SoundManager.playSound(SoundClipEnum.KrakenBubble, SoundCategoryEnum.Generic,transform.position);
                velocity = 0;
                isSubmerging = true;
                submergeSprite.enabled = false;
                bubbles.SetActive(true);
                attacking = false;
                Invoke("emergeKraken", stats.stages[currentStage].emergeTime);
            }
        }
    }

    internal float getCurrentWeight()
    {
        return stats.stages[currentStage].weight;
    }

    void disableSpray()
    {

        //wake.SetActive (false);
        spray.SetActive(false);
    }

    void enableSpray()
    {

        //wake.SetActive (true);
        spray.SetActive(true);
    }

    void moveKraken()
    {

        if (animator.isCurrentAnimName("death") || animator.isCurrentAnimName("emergeAttack"))
        {
            return;
        }

        float maxVelocity = stats.stages[currentStage].maxVelocity;
        float moveSpeed = stats.stages[currentStage].moveSpeed;
        float turnSpeed = stats.stages[currentStage].turnSpeed;
        animator.setSpeedParameter(velocity);

        if (animator.isCurrentAnimName("idle"))
        {
            maxVelocity = stats.stages[currentStage].maxVelocity;
            moveSpeed = stats.stages[currentStage].moveSpeed;
            turnSpeed = stats.stages[currentStage].turnSpeed;
        }
        else if (animator.isCurrentAnimName("submerged"))
        {
            gameStats.timeSpentSubmerged += Time.deltaTime;
            maxVelocity = stats.stages[currentStage].submergedMaxVelocity;
            moveSpeed = stats.stages[currentStage].submergedMoveSpeed;
            turnSpeed = stats.stages[currentStage].submergedTurnSpeed;

			if (bubbles.activeSelf) {
				maxVelocity = stats.stages[currentStage].emergingMaxVelocity;
				moveSpeed = stats.stages[currentStage].emergingMoveSpeed;
				turnSpeed = stats.stages[currentStage].emergingTurnSpeed;
			}
        }
        else  if (animator.isCurrentAnimName("spitCharge") || animator.isCurrentAnimName("headbashCharge")) {
            moveSpeed = 0;
        }

        Vector3 directionVector;
        directionVector = new Vector3(Actions.Rotate.X, 0f, Actions.Rotate.Y); //Get the direction the user is pushing the left analog stick in

        velocity = Mathf.Max(0f, (velocity - (stats.stages[currentStage].deaccelaration * (Time.deltaTime * GlobalVariables.gameSpeed)))); //Deaccelerate
		velocity = Mathf.Min(maxVelocity, velocity + (directionVector.magnitude * moveSpeed * (Time.deltaTime * GlobalVariables.gameSpeed))); ////Accelerate

        if (animator.isCurrentAnimName("headbash") && velocity <= 0f)
        {
            animator.finishHeadbash();
            stopVibrate();
        }
            if (directionVector != Vector3.zero)
        {
            Quaternion wanted_rotation = Quaternion.LookRotation(directionVector); // get the rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, wanted_rotation, stats.stages[currentStage].turnSpeed * directionVector.magnitude * (Time.deltaTime * GlobalVariables.gameSpeed));
        }

        cc.Move(transform.forward * velocity * (Time.deltaTime * GlobalVariables.gameSpeed));

        if (transform.position.y != startingPoint.y)
        {
            transform.position = new Vector3(transform.position.x, startingPoint.y, transform.position.z);
        }
    }


    public void die()
    {

        dying = true;
        gameStats.numOfDeaths++;

        if (previousShip != null)
        {
            previousShip.GetComponent<playerInput>().locked = false;
        }
        followCamera.zoomIn = true;
        animator.triggerDeathAnimation();
		spitter.clearSpitball ();
        disableSpray();
    }


    public void setupRespawn()
    {

        //shipMesh.enabled = false;
        followCamera.zoomIn = false;
        manager.respawnKraken(this, startingPoint);
        //anim.triggerRespawnAnimation ();
        health = stats.stages[currentStage].max_health;
        followCamera.setRespawn();
        isSubmerging = false;
        submerged = false;
        smash = false;
        attacking = false;
        submergeSprite.enabled = false;
     //   enableSpray();

    }


    public void rebirth()
    {

        dying = false;
        enableCollisions();
        isSubmerging = false;
    }


    public void hit(float dmg = 0, StatsInterface attacker = null)
    {

        if (dmg == 0)
        {
            dmg = stats.stages[currentStage].damage;
        }

        if (!animator.isCurrentAnimName("death"))
        {

            if (attacker != null)
            {
                ((playerInput)attacker).gameStats.addGivenDamage("kraken", dmg);
                gameStats.addTakenDamage(((playerInput)attacker).type.ToString(), dmg);
            }

            health -= dmg;
            gameStats.healthLost += dmg;

            if (health <= 0)
            {
                vibrate(1f, 1f);
                die();
            }
            else
            {
                vibrate(.5f, .5f);
            }
				
        }

    }


    public void joinVibrate()
    {
        Actions.Device.Vibrate(1);
        Invoke("stopVibrate", .5f);
    }


    public void vibrate(float intensity, float time)
    {

        if (Actions.Device != null)
        {
            Actions.Device.Vibrate(intensity);
            Invoke("stopVibrate", time);
        }
    }

    public void constantVibrate(float intensity) {

        if (Actions.Device != null)
        {
            Actions.Device.Vibrate(intensity);
        }
    }

    void stopVibrate()
    {
        Actions.Device.StopVibration();
    }


    public void emergeKraken()
    {

        bubbles.SetActive(false);
        hasHitShip = false;

        if (hittingShip && !hittingShip.GetComponent<playerInput>().invincible)
        {
            hasHitShip = true;
        }

        if (hittingPlayer && hittingShip)
        {

            transform.position = new Vector3(transform.position.x, startingPoint.y, transform.position.z);
            Vector3 shipPos = hittingShip.GetComponent<playerInput>().transform.position;
            float distance = Vector3.Distance(krakenArmPosition.position, shipPos);

            Vector3 direction = shipPos - krakenArmPosition.position;
            direction.Normalize();
            transform.position = transform.position + (direction * Mathf.Abs(distance));
            transform.position = new Vector3(transform.position.x, startingPoint.y, transform.position.z);

            if (!hittingShip.GetComponent<playerInput>().invincible)
            {
                hittingShip.GetComponent<playerInput>().locked = true;
            }


            animator.emergeKrakenAttack(hasHitShip ? hittingShip : null);
            previousShip = hittingShip;
            hittingPlayer = false;
            hittingShip = null;

        }
        else
        {
            animator.emergeKraken();
            isSubmerging = false;
        }

        gameObject.layer = LayerMask.NameToLayer("kraken");
        submerged = false;
     //   enableSpray();

    }


    public void resetEmerge()
    {
        hasHitShip = false;
        isSubmerging = false;
    }


    public void disableCollisions()
    {
        cc.enabled = false;
    }


    public void enableCollisions()
    {
        cc.enabled = true;
    }


    public void resetSmash()
    {
        animator.setSmash(false);
        smash = false;
    }


    public void resetSubmerging()
    {
        isSubmerging = false;
        animator.sinkableShip = null;
        gameObject.layer = LayerMask.NameToLayer("kraken_submerged");
        submergeSprite.enabled = true;
    }


    void updateWake()
    {
        //wake.transform.rotation = this.transform.rotation;
    }

    public void reset()
    {
        animator.resetToIdle();
        CancelInvoke();
    }

}

