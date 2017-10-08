using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class MapSelectView : MonoBehaviour {

	[SerializeField] private ActionButton startGame;
    [SerializeField] private ActionDropDown mapOptions;
    private Action onSelectMap;
    List<MapEnum> maps;
    public MapEnum selectedMap = MapEnum.TropicalMap;


    void Update() {
        selectedMap = maps[mapOptions.DropDownComponent.value];
    }


    public void Initialize(GameTypeEnum gameMode)
	{
        maps = new List<MapEnum>();
		maps.Add(MapTypeHelper.GetRandomMap());

		if (gameMode == GameTypeEnum.Sabotage)
        {
            maps = MapTypeHelper.GetSabotageOfflineMaps();
        }
        else if (gameMode == GameTypeEnum.DeathMatch)
        {
            maps = MapTypeHelper.GetDeathMatchOfflineMaps();
		}

		SetMaps(maps);
	}


	void SetMaps(List<MapEnum> maplist)
	{
		mapOptions.DropDownComponent.ClearOptions();

		for (int i = 0; i < maplist.Count; i++)
        {
            mapOptions.DropDownComponent.options.Add(new Dropdown.OptionData(maplist[i].ToString()));
			mapOptions.DropDownComponent.options[i].text = maplist[i].ToString();
		}
	}

    public void OnSelectMap(Action action)
	{
		this.startGame.SetAction(() => {
            action();
		});
	}

}
