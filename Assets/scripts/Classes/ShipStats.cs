using UnityEngine;
using System.Collections;

public class ShipStats : MonoBehaviour {
	public float maxVelocity = 2;
	public float max_health = 5;
	public int max_bombs = 3;

    public float weight = 1f;

	public float moveSpeed = 2f;
	public float turnSpeed = 120f;
	public float tiltSpeed = 2f;
	public int tiltAngle = 40;

	public float shootDelay=1.3f; 

	public float alternateShootDelay = 2f;
	public float boostVelocity = 5f;
	public float boostResetTime= 6f;
    public float inkSlowDownTime = 1f;
    public float barrelSlowDownFactor = 0.75f;

	public float sinkTime = 0.5f;
	public float sinkSpeed = 1f;
	public float deAccelerationSpeed= 2f;
	public float invinciblityTime = 5f;
    public float inkSlowDownFactor = .2f;
	public float kraken_damage = 1f;

}
