using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArrowController : MonoBehaviour {

	public Transform parent;
	public Transform barrel;
	public Transform home;
	public Sprite barrelArrow;
	public Sprite homeArrow;
	public float distance = 1.25f;
	private SpriteRenderer spriteRenderer;
	private Transform currTarget;

	public void Start() {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		barrel = GameObject.FindObjectOfType<Barrel> ().gameObject.transform;
		TargetBarrel ();
		UpdateArrow ();
	}

	public void Update() {
		UpdateArrow ();
	}

	private void UpdateArrow() {
		Vector3 direction = currTarget.position - parent.position;

		if (direction.magnitude < 2) {
			spriteRenderer.enabled = false;
		} else {
			transform.position = parent.position + direction.normalized * distance;
			transform.rotation = Quaternion.LookRotation (Vector3.up, direction);

			if (!spriteRenderer.enabled) {
				spriteRenderer.enabled = true;
			}
		}
	}

	public void TargetHome() {
		currTarget = home;
		spriteRenderer.sprite = homeArrow;
	}

	public void TargetBarrel() {
		currTarget = barrel;
		spriteRenderer.sprite = barrelArrow;
	}
}