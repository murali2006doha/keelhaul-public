using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[System.Serializable]
public class Title{
	public string name;
	public string statsString;
	public string statVariable;
	public bool max;
	public float weight;
	public bool krakenOnly;
	public bool shipOnly;
}

public class Titles : MonoBehaviour {

	const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
	


	public Title[] titles;

	public void calculateTitles(List<FreeForAllStatistics> shipStats, List<FreeForAllStatistics> krakenStats){
		List<FreeForAllStatistics> stats = new List<FreeForAllStatistics> ();
		stats.AddRange (shipStats);
		stats.AddRange (krakenStats);
		foreach (Title title in titles) {
			if (title.krakenOnly) {
				calculateTitle (title, krakenStats);
			} else if (title.shipOnly) {
				calculateTitle (title, shipStats);
			} else {
				calculateTitle (title, stats);
			}
		}
	}

	public void calculateTitle(Title title,List<FreeForAllStatistics> stats){
		FreeForAllStatistics max=null;
		float maxVal = 0;
		string[] statVars = title.statVariable.Split ("|".ToCharArray());
		foreach (FreeForAllStatistics stat in stats) {
			float val = 0f;
			foreach (string var in statVars) {
				FieldInfo field = stat.GetType ().GetField (var);
				if (field != null) {
					object value = field.GetValue (stat);
					if(value!=null){
						val = Mathf.Max(float.Parse(value.ToString()) ,val);
					}
				}
			}
			if ((val >= maxVal && title.max) || (val<=maxVal && !title.max)) {
				maxVal = val;
				max = stat;
			}
		}
		title.statsString = title.statsString.Replace ("%", maxVal+"");
		if (max != null && maxVal!=0) {
			max.titles.Add (title);
		}
	}
}
