using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GlobalVariables {

    public static float gameSpeed = 1.25f;
	public static Dictionary<string,string> shipToPrefabLocation = new Dictionary<string, string> () {
		{"blackbeard","Ship/Blackbeard Ship"},
		{"atlantean","Ship/Atlantean Ship"},
		{"chinese","Ship/Chinese Junk Ship"}

	};

	public static float windFactor = 4f;
    public static float minCcVelocity = 0.2f;
}
