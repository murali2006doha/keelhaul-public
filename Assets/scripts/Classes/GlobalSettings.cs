using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

	public static class GlobalSettings {

    public static float soundMultiplier = PlayerPrefs.GetFloat ("Sound multiplier", 1f);
    public static float musicMultiplier = PlayerPrefs.GetFloat ("Music multiplier", 1f);
    public static bool waterRefraction = true;
    public static bool waterReflection = true;
	public static bool shadows = true;

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

	public static void setRefraction(Boolean state) {
		waterRefraction = state;
	}

	public static void setReflection(Boolean state) {
		waterReflection = state;
	}


	public static void setShadows(Boolean state) {
		shadows = state;

		if (state) {
			QualitySettings.shadows = ShadowQuality.All;
		} else {
			QualitySettings.shadows = ShadowQuality.Disable;
		}
	}

}
