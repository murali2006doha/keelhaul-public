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
  public string[] cullingMasks;
  public bool ready;
  public float maxFollowSpeed = 3f;
  public float minFollowSpeed = .4f;
  bool targetSet = false;
  public bool zoomIn = false;
  public float zoomInValue, zoomInSpeed = 1f;
  public float normalZoomValue = -1f;
  public float duration = 2f;
  public float magnitude = .5f;
  float shakeTime;
  bool shaking;

  [SerializeField]
  float groundDistance = 10f;
  [SerializeField]
  UnityStandardAssets.ImageEffects.MotionBlur blurEffect;
  CharacterController character;
  // Use this for initialization
  void Start() {
    offset = new Vector3(x_diff, y_diff, z_diff);
    getCharacterController();
  }

  void getCharacterController() {
    if (target)
      character = target.GetComponent<CharacterController>();
  }

  public void SetRectsOfCameras(Rect rect) {
    camera.rect = rect;
  }

  public void setCullingMasks() {
    foreach (string k in cullingMasks) {
      if (k.Equals(cullingMask) == false) {
        camera.cullingMask &= ~(1 << LayerMask.NameToLayer(k));

      }

    }


  }

  // Update is called once per frame
  void Update() {
    if (normalZoomValue == -1f) {
      normalZoomValue = camera.orthographicSize;
    }
    if (!targetSet) {
      if (target) {
        targetSet = true;
        transform.position = target.transform.position + offset;
      }
    }
    if (ready) {
      if (!character) {
        getCharacterController();
      } else {
        float velocity = Mathf.Clamp(character.velocity.magnitude, minFollowSpeed, maxFollowSpeed);
        offset = new Vector3(x_diff, y_diff, z_diff);
        if (!respawning) {
          this.ClampMovement(Vector3.MoveTowards(transform.position, target.transform.position + offset, followSpeed * (Time.deltaTime * GlobalVariables.gameSpeed * velocity)));

          if (shaking) {
            shake();
          }
        }

        if (zoomIn && camera.orthographicSize > zoomInValue) {
          camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomInValue, Time.deltaTime * zoomInSpeed);
        } else if (!zoomIn && camera.orthographicSize < normalZoomValue) {
          camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, normalZoomValue, Time.deltaTime * zoomInSpeed);
        }
        if (respawning) {
          this.transform.position = Vector3.MoveTowards(transform.position, target.transform.position + offset, respawnFollowSpeed * (Time.deltaTime * GlobalVariables.gameSpeed));
          Vector3 distance = (target.transform.position + offset - transform.position);
          if (distance.magnitude <= .01f) {
            respawning = false;
            if (target.GetComponent<PlayerInput>()) {
              target.GetComponent<PlayerInput>().anim.triggerRespawnAnimation();
            } else if (target.GetComponent<KrakenInput>()) {
              target.GetComponent<KrakenInput>().animator.triggerRespawnAnimation();
            }
          }

        }
      }

    }

  }

  public void startShake() {
    if (!shaking) {
      shaking = true;
      shakeTime = Time.realtimeSinceStartup;
    } else {
      shakeTime = Time.realtimeSinceStartup;
    }

  }

  public void ActivateMotionBlur() {
    blurEffect.enabled = true;
  }

  public void DeActivateMotionBlur() {
    blurEffect.enabled = false;
  }

  public void shake() {
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
  }

  public void setRespawn() {
    respawning = true;
  }

  private void ClampMovement(Vector3 intendedPosition) {
    var cachedPostion = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    var futureHorizontalPosition = new Vector3(intendedPosition.x, transform.position.y, transform.position.z);
    this.transform.position = futureHorizontalPosition;
    Debug.DrawLine(this.camera.transform.position, this.camera.transform.position + (this.camera.transform.forward * this.groundDistance));


    if (!Physics.Raycast(this.camera.transform.position, this.camera.transform.forward, this.groundDistance, LayerMask.GetMask("CameraCollision"))) {
      intendedPosition.Set(cachedPostion.x, intendedPosition.y, intendedPosition.z);
    }


    this.transform.position = cachedPostion;
    var futureVerticalPosition = new Vector3(transform.position.x, intendedPosition.y, intendedPosition.z);
    this.transform.position = futureVerticalPosition;
    Debug.DrawLine(this.camera.transform.position, this.camera.transform.position + (this.camera.transform.forward * this.groundDistance));

    if (!Physics.Raycast(this.camera.transform.position, this.camera.transform.forward, this.groundDistance, LayerMask.GetMask("CameraCollision"))) {
      intendedPosition.Set(intendedPosition.x, intendedPosition.y, cachedPostion.z);
    }

    this.transform.position = intendedPosition;
  }
}
