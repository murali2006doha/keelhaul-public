using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MatchMakingView : MonoBehaviour {

    [SerializeField] private ActionButton findMatch;
    [SerializeField]  private List<ActionToggle> matchOptions;
    private int minPlayers;
    // Use this for initialization
    Dictionary<int, bool> matchOptionsDict;
    public void Initialize(Action<Dictionary<int,bool>> findMatchClick) {
        matchOptionsDict = new Dictionary<int, bool>();
      
        for (int i = 0; i < matchOptions.Count; i++) {
            Debug.Log(i);
            var storedIndex = i;
            matchOptions[i].SetAction(toggled =>
            {
                matchOptionsDict[storedIndex] = toggled;
            });
        }

        this.findMatch.SetAction(() => findMatchClick(this.matchOptionsDict));
    }

}
