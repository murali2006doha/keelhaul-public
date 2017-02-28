using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

	public GameObject target;
	public Camera camera;
    public string cullingMask;
	public float x_diff, y_diff, z_diff = 0;
	public bool respawning = false;
	Vector3 offset;
	public float respawnFollowSpeed = 20f;
    public float followSpeed = 1f;
	public string [] cullingMasks;
	public bool ready;
    public float maxFollowSpeed = 3f;
    public float minFollowSpeed = .4f;
    bool targetSet = false;
    public bool zoomIn = false;
    public float zoomInValue, zoomInSpeed = 1f;
    public float normalZoomValue = 3.25f;
	public float duration = 2f;
	public float magnitude = .5f;

	float origDuration;
	float origMagnitude;
	float shakeTime;
	bool shaking;
    [SerializeField] UnityStandardAssets.ImageEffects.MotionBlur blurEffect;
    CharacterController character;
	// Use this for initialization
	void Start () {
		offset = new Vector3 (x_diff, y_diff, z_diff);
        getCharacterController();
		origDuration = duration;
		origMagnitude = magnitude;
    }

    void getCharacterController() {
        if (target)
            character = target.GetComponent<CharacterController>();
    }

    public void SetRectsOfCameras(Rect rect) {
        camera.rect = rect;
    }

	public void setCullingMasks()
	{
		foreach (string k in cullingMasks) {
			if (k.Equals (cullingMask) == false) {
				camera.cullingMask &=  ~(1 << LayerMask.NameToLayer(k));	

			}

		}


	}
	
	// Update is called once per frame
	void Update () {
		if (!targetSet) {
            if (target) {
                targetSet = true;
                transform.position = target.transform.position + offset;
            }
        }
		if (ready) {
            if (!character)
            {
                getCharacterController();
            }
            else {
                float velocity = Mathf.Clamp(character.velocity.magnitude, minFollowSpeed,maxFollowSpeed);
                offset = new Vector3(x_diff, y_diff, z_diff);
                if (!respawning)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position + offset, followSpeed * (Time.deltaTime * GlobalVariables.gameSpeed * velocity));
					if (shaking) {
						shake ();
					}
                }

                if (zoomIn && camera.orthographicSize > zoomInValue)
                {
                    camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomInValue, Time.deltaTime * zoomInSpeed);
                }

                else if (!zoomIn && camera.orthographicSize < normalZoomValue) {
                    camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, normalZoomValue, Time.deltaTime * zoomInSpeed);
                }
                if (respawning)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position + offset, respawnFollowSpeed * (Time.deltaTime * GlobalVariables.gameSpeed));
                    Vector3 distance = (target.transform.position + offset - transform.position);
                    if (distance.magnitude <= .01f)
                    {
                        respawning = false;
                        if (target.GetComponent<PlayerInput>())
                        {
                            target.GetComponent<PlayerInput>().anim.triggerRespawnAnimation();
                        }
                        else if (target.GetComponent<KrakenInput>())
                        {
                            target.GetComponent<KrakenInput>().animator.triggerRespawnAnimation();
                        }
                    }

                }
            }
			
		}

	}

	public void startShake(){
		if (!shaking) {
			shaking = true;
			shakeTime = Time.realtimeSinceStartup;
		}
        else
        {
            shakeTime = Time.realtimeSinceStartup;
        }
	}

	public void startShake(float randMag, float randDur){
		duration = randDur;
		magnitude = randMag;

		if (!shaking) {
			shaking = true;
			shakeTime = Time.realtimeSinceStartup;
		}
		else
		{
			shakeTime = Time.realtimeSinceStartup;
		}


	}


	void resetDurationMagnitude() {
		duration = origDuration;
		magnitude = origMagnitude;
	}



    public void ActivateMotionBlur() {
        blurEffect.enabled = true;
    }

    public void DeActivateMotionBlur() {
        blurEffect.enabled = false;
    }

	public void shake(){

		float percentComplete = (Time.realtimeSinceStartup - shakeTime) / duration;         
	    float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);    

	    float x = Random.value * 2.0f - 1.0f;
	    float y = Random.value * 2.0f - 1.0f;
	    x *= magnitude * damper;
	    y *= magnitude * damper;
	        
		transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y);
		if (percentComplete >= 1) {
			shaking = false;
			shakeTime = 0;
		}
		resetDurationMagnitude ();
	}



	public void setRespawn(){
		respawning = true;
	}
}
