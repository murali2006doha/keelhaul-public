using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DynamicAudioSource : MonoBehaviour {

	void Start () {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (PhotonNetwork.offlineMode)
        {
            audioSource.spatialBlend = 0f;
        }
        audioSource.enabled = true;
    }
    

}
