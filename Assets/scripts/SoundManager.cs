using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

    [System.Serializable]
    public class SoundClip
    {
        public string name;
        public AudioClip clip;
        public float volume = 0.5f;
    }

    public List<SoundClip> sounds = new List<SoundClip>();
    AudioSource audioSource;

    public void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void playSound(string name)
    {
        SoundClip clip = findClipByName(name);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip.clip, clip.volume);
        }
    }

    SoundClip findClipByName(string name)
    {
        foreach(SoundClip clip in sounds)
        {
            if(clip.name == name)
            {
                return clip;
            }
        }
        return null;
    }
	
}
