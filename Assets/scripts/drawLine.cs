using UnityEngine;
using System.Collections;

public class drawLine : MonoBehaviour {

	private LineRenderer lineRenderer;

	public Transform origin;
	public Transform destination;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.SetPosition (0, origin.position);
		lineRenderer.SetPosition (1, destination.position);
		lineRenderer.SetWidth (.07f, .07f);

	
	}

	// Update is called once per frame
	void Update () {
		lineRenderer.SetPosition (0, origin.position);
		lineRenderer.SetPosition (1, destination.position);
	}
}
