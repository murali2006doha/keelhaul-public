using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRounds : MonoBehaviour {
    public GameObject startingPos;
    public List<GameObject> targetRounds;
    public int round = -1;
	public void EnableNextRound()
    {
        if(round >= 0)
        {
            targetRounds[round].SetActive(false);
        }
        round++;
        if(round < targetRounds.Count)
        {
            targetRounds[round].SetActive(true);
        }
    }

    internal int GetBrokenCount()
    {
        int count = 0;
        if(round >= 0 && round < targetRounds.Count)
        {
            var targets = targetRounds[round].GetComponentsInChildren<TargetScript>();
            foreach(TargetScript target in targets)
            {
                if (target.activated)
                {
                    count++;
                }
            }
        }
        return count;
    }

    internal int GetTotalCount()
    {
        int count = 0;
        if (round >= 0 && round < targetRounds.Count)
        {
            var targets = targetRounds[round].GetComponentsInChildren<TargetScript>();
            return targets.Length;
        }
        return count;
    }
}
