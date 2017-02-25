using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMakingController : MonoBehaviour {

    [SerializeField] MatchMakingView view;
	// Use this for initialization

    Action<Dictionary<int, bool>> onMatchButtonClick;
    public void Initiailze(Action<Dictionary<int, bool>> matchOptions)
    {
       
        view.Initialize(matchOptions);
    }
	
}
