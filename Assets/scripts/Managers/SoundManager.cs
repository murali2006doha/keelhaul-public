using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager {

    public static SoundLibrary library;

    public static Dictionary<SoundCategoryEnum, Dictionary<SoundClipEnum, SoundClip>> lookup;
    public static AudioSource source;

    public static void initLibrary()
    {
        lookup = new Dictionary<SoundCategoryEnum, Dictionary<SoundClipEnum, SoundClip>>();
        library = ((GameObject)Resources.Load("Prefabs/Sounds/SoundLibrary")).GetComponent<SoundLibrary>();
        foreach (SoundClip clip in library.sounds)
        {
            if (!lookup.ContainsKey(clip.category))
            {
                lookup.Add(clip.category,new Dictionary<SoundClipEnum, SoundClip>());
            }
            lookup[clip.category][clip.clipName] = clip;
        }
        source = GameObject.FindObjectOfType<AudioSource>();

    }

    public static void playSound(SoundClipEnum clipName, SoundCategoryEnum category = SoundCategoryEnum.Generic, Vector3 pos = default(Vector3))
    {
        SoundClip clip = lookup[category][clipName];
        if (clip != null && source != null)
        {
            source.PlayOneShot(clip.clip, clip.volume); 
        }
    }



}
