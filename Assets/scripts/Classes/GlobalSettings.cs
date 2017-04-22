using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public static class GlobalSettings {

    public static float soundMultiplier = PlayerPrefs.GetFloat ("Music multiplier");
    public static float musicMultiplier = PlayerPrefs.GetFloat ("Sound multiplier");
    public static bool waterRefraction = true;
    public static bool waterReflection = true;

    public static event Action OnSoundChange;
    public static event Action OnMusicChange;

    public static void setSoundMultiplier(float x) {
        soundMultiplier = x;
        PlayerPrefs.SetFloat ("Sound multiplier", x);
        PlayerPrefs.Save ();
        OnSoundChange.Invoke ();
    }

    public static void setMusicMuliplier(float x) {
        musicMultiplier = x;
        PlayerPrefs.SetFloat ("Music multiplier", x);
        PlayerPrefs.Save ();
        OnMusicChange.Invoke ();
    }
}

