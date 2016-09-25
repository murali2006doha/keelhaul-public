using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager {

    public static SoundLibrary library;

    public static Dictionary<SoundCategoryEnum, Dictionary<string, SoundClip>> lookup;
    public static AudioSource source;

    public static void initLibrary()
    {
        lookup = new Dictionary<SoundCategoryEnum, Dictionary<string, SoundClip>>();
        library = ((GameObject)Resources.Load("Prefabs/Sounds/SoundLibrary")).GetComponent<SoundLibrary>();
        foreach (SoundClip clip in library.sounds)
        {
            if (!lookup.ContainsKey(clip.category))
            {
                lookup.Add(clip.category,new Dictionary<string, SoundClip>());
            }
            lookup[clip.category][clip.name] = clip;
        }
        source = GameObject.FindObjectOfType<AudioSource>();

    }

    public static void playSound(string name, Vector3 pos)
    {
        playSound(name, SoundCategoryEnum.Generic, pos);
    }

    public static void playSound(string name, SoundCategoryEnum category, Vector3 pos)
    {
        SoundClip clip = lookup[category][name];
        if (clip != null && source != null)
        {
            source.PlayOneShot(clip.clip, clip.volume); // For now we will ignore position...
        }
    }

    public static void playSound(string name)
    {
        playSound(name, SoundCategoryEnum.Generic);
    }

    public static void playSound(string name, SoundCategoryEnum category)
    {
        SoundClip clip = lookup[category][name];
        if (clip != null && source != null)
        {
            source.PlayOneShot(clip.clip, clip.volume);
        }
    }

}
