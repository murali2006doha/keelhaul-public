using UnityEngine;
using System.Collections;

public class ChineseJunkShotController : MonoBehaviour {

    public GameObject parent;
    public float lifeTime;
    HookshotComponent hook;
    PlayerInput ship;
    public float damage = 0.1f;
    public Vector3 offset;
    public ParticleSystem firework;
    float originalSpeed;


	// Use this for initialization
    void Start () {
        parent = GetComponent<SecondaryFire>().parent;
        ship = parent.GetComponent<PlayerInput>();
	transform.rotation = ship.getAltCannonRotation();
        transform.position = parent.transform.position + offset;
        float force = GetComponent<SecondaryFire>().force;
        GetComponent<CannonBall>().setOwner(parent.transform);
	GetComponent<CannonBall> ().reflectForce = force;
        GetComponent<Rigidbody>().AddForce(transform.forward * force);       
    }

    // Update is called once per frame
    void Update()
    {
       	// firework.startSpeed = Mathf.Clamp(hook.moveVector.magnitude, .5f, 1f);
    	//  firework.collision.collidesWith &= ~(1 << LayerMask.NameToLayer("SomeLayer"));
    }

    void KillSelf() {
        Destroy(this.gameObject);
    }
}
