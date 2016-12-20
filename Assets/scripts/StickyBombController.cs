using UnityEngine;
using System.Collections;

public class StickyBombController : MonoBehaviour {

	public GameObject parent;
	public float lifeTime = 7f;
	public float timeToExplode = 5f;
	public float damage;
	PlayerInput ship;
	ShipCannonComponent cannoncontroller;
	Transform cannonballpos;

	public int cannonForce = 50; 
	public float speed = .10f;
	public Vector3 velocity;
	public float dampening = .1f;
	public GameObject normalHit;
	public GameObject explosion;
	public float magnitude;
	float timeToDestroyExplosion = 1f;

	// Use this for initialization
	void Start () {

		parent = GetComponent<SecondaryFire>().parent;
		ship = parent.GetComponent<PlayerInput>();

		cannonballpos = ship.getAltCannonPosition();

		gameObject.transform.position = cannonballpos.position + (velocity * dampening);
		gameObject.transform.rotation = cannonballpos.rotation;
		gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * cannonForce);

		Invoke ("KillSelf", lifeTime);

	}
	
	// Update is called once per frame
	void Update () {

	
	}
		

	public void OnTriggerEnter(Collider collider){

		if (collider.transform.root != parent.transform) {

			if (LayerMask.LayerToName (collider.gameObject.layer).Contains ("playerMesh")) { 

				Vector3 direction = GetComponent<Rigidbody> ().velocity.normalized;
				PlayerInput ship = collider.transform.root.GetComponent<PlayerInput> ();
				print (ship);
				ship.addPushForce (direction, magnitude);

				gameObject.AddComponent<FixedJoint> ();

				FixedJoint fixedjoint = GetComponent<FixedJoint> ();
				fixedjoint.connectedBody = collider.gameObject.GetComponent<Rigidbody> ();
				gameObject.GetComponent<Rigidbody> ().isKinematic = true;

				gameObject.transform.parent = collider.gameObject.transform;

				Explode (collider);


			} else if (LayerMask.LayerToName (collider.gameObject.layer).Equals ("kraken")) { 
				gameObject.AddComponent<FixedJoint> ();

				FixedJoint fixedjoint = GetComponent<FixedJoint> ();
				fixedjoint.connectedBody = collider.gameObject.GetComponent<Rigidbody> ();
				gameObject.GetComponent<Rigidbody> ().isKinematic = true;

				gameObject.transform.parent = collider.gameObject.transform;

				Explode (collider);
			}
			

			else {
				gameObject.AddComponent<FixedJoint>();

				FixedJoint fixedjoint = GetComponent<FixedJoint>();
				fixedjoint.connectedBody = collider.gameObject.GetComponent<Rigidbody>();

				gameObject.transform.parent = collider.gameObject.transform;

				gameObject.GetComponent<Rigidbody> ().isKinematic = true;

				Invoke ("Explode", timeToExplode);
			}

		}
	}


	public void Explode(Collider collision) {
	
		PlayerInput collidedShip = collision.transform.root.GetComponent<PlayerInput> ();
		collidedShip.hit (damage);
		Instantiate (explosion, transform.position, transform.rotation);
		Destroy(gameObject);
		Invoke ("destroyExplosion", timeToDestroyExplosion);

	}


	void destroyExplosion() {
		Destroy (explosion);
	}
		

	void KillSelf() {
		
		Instantiate (normalHit, transform.position, transform.rotation);
		Destroy(gameObject);
	}

}
