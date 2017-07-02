using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MatchMakingView : MonoBehaviour {

    [SerializeField] private ActionButton findMatch;
    [SerializeField] private List<ActionToggle> matchOptions;
    [SerializeField] Transform SelectMode;
    private int minPlayers;
    // Use this for initialization
    Dictionary<int, bool> matchOptionsDict;
    
    public void Initialize(Action<Dictionary<int,bool>> findMatchClick) {
        matchOptionsDict = new Dictionary<int, bool>();
      
        for (int i = 0; i < matchOptions.Count; i++) {
            var storedIndex = i;
            matchOptions[i].SetAction(toggled =>
            {
                matchOptionsDict[storedIndex] = toggled;
            });
        }

        SetFindMatchAction(findMatchClick);
        //this.findMatch.SetAction(() => findMatchClick(this.matchOptionsDict));
    }

    public List<ActionToggle> GetMatchOptions() {
        return matchOptions;
    }


    public ActionButton GetFindMatch() {
        return findMatch;
    }


    void SetFindMatchAction(Action<Dictionary<int,bool>> findMatchClickAction) {

        this.findMatch.SetAction(() => {
            List<bool> bools = new List<bool>(matchOptionsDict.Values);
            if (bools.TrueForAll(b => b == false)) {
                SelectMode.gameObject.SetActive(true);
                Invoke("TurnOffText", 1f);
            } else {
                findMatchClickAction(this.matchOptionsDict);
            }
        });
    }


    void TurnOffText() {
        SelectMode.gameObject.SetActive(false);
    }


}
