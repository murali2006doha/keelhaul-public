using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SoundClip
{
    public SoundClipEnum clipName;
    public SoundCategoryEnum category;
    public AudioClip clip;
    public float volume = 0.5f;
}
public class SoundLibrary : MonoBehaviour {

    public List<SoundClip> sounds = new List<SoundClip>();

}
