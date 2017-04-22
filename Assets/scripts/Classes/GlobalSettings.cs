using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public static class GlobalSettings {

	public static float soundMultiplier = 1f;
	public static float musicMultiplier = 1f;
	public static bool waterRefraction = true;
	public static bool waterReflection = true;

	public static event Action OnSoundChange;
	public static event Action OnMusicChange;

	public static void setSoundMultiplier(float x) {
		soundMultiplier = x;
		OnSoundChange.Invoke ();
	}

	public static void setMusicMuliplier(float x) {
		musicMultiplier = x;
		OnMusicChange.Invoke ();
	}
}

