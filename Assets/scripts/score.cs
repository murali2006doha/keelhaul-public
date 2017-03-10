using UnityEngine;
using System.Collections;

public class score : MonoBehaviour {

    [SerializeField] private float animationTime;
	// Use this for initialization
	void Start () {
        Invoke("KillSelf", animationTime);
	}
	
	// Update is called once per frame
	void KillSelf(){
		Destroy (this.gameObject);
	}
}
