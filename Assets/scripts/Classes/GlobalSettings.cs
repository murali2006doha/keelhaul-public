using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public static class GlobalSettings {

    public static float soundMultiplier = PlayerPrefs.GetFloat ("Sound multiplier", 1f);
    public static float musicMultiplier = PlayerPrefs.GetFloat ("Music multiplier", 1f);
    public static bool waterRefraction = IntToBool(PlayerPrefs.GetInt ("Refraction", 1));
    public static bool waterReflection = IntToBool(PlayerPrefs.GetInt ("Reflection", 1));
    public static bool shadows = IntToBool(PlayerPrefs.GetInt ("Shadows", 1));


    public static void setSoundMultiplier(float x) {
		soundMultiplier = x;
        PlayerPrefs.SetFloat ("Sound multiplier", x);
        PlayerPrefs.Save ();
    }

    public static void setMusicMuliplier(float x) {
        musicMultiplier = x;
        PlayerPrefs.SetFloat ("Music multiplier", x);
        PlayerPrefs.Save ();
    }

	public static void setRefraction(Boolean state) {
		waterRefraction = state;
        PlayerPrefs.SetInt("Refraction", BoolToInt(state));
        PlayerPrefs.Save();

	}

	public static void setReflection(Boolean state) {
		waterReflection = state;
        PlayerPrefs.SetInt("Reflection", BoolToInt(state));
        PlayerPrefs.Save();
	}


	public static void setShadows(Boolean state) {
		shadows = state;

		if (state) {
			QualitySettings.shadows = ShadowQuality.All;
		} else {
			QualitySettings.shadows = ShadowQuality.Disable;
		}

        PlayerPrefs.SetInt("Shadows", BoolToInt(state));
        PlayerPrefs.Save();
	}

    static bool IntToBool(int i) {
        bool x = false;
        if (i == 1) {
            x = true;
        } else if (i == 0) {
            x = false;
        }

        return x;
    }


    static int BoolToInt(bool b) {
        int x = 0;
        if (b) {
            x = 1;
        } else if (!b) {
            x = 0;
        }

        return x;
    }

}
