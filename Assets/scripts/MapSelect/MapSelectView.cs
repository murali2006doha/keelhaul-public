using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class MapSelectView : MonoBehaviour {

	[SerializeField] private ActionButton startGame;
    [SerializeField]
    private ActionButton upButton;
    [SerializeField]
    private ActionButton downButton;
    [SerializeField]
    private Text mapText;

    private Action<MapEnum> onMapSelect;
    List<MapEnum> maps = new List<MapEnum>();
    public MapEnum selectedMap = MapEnum.TropicalMap;

    private int selectedMapIndex = 0;
    private bool mapSelected;

    public void Show() {
        this.gameObject.SetActive(true);
        this.ChangeMap(0);
    }

    public void Hide() {
        this.mapSelected = false;
        this.selectedMapIndex = 0;
        this.gameObject.SetActive(false);
    }


    public void Initialize(GameTypeEnum gameMode, Action<MapEnum> onMapSelect)
	{
        this.onMapSelect = onMapSelect;

        this.upButton.SetAction(() => this.ChangeMap(1));
        this.downButton.SetAction(() => this.ChangeMap(-1));
        this.startGame.SetAction(this.StartGame);

        if (gameMode == GameTypeEnum.Sabotage)
        {
            maps = MapTypeHelper.GetSabotageOfflineMaps();
        }
        else if (gameMode == GameTypeEnum.DeathMatch)
        {
            maps = MapTypeHelper.GetDeathMatchOfflineMaps();
		}
        
	}

    public void ChangeMap(int direction)
    {
        this.selectedMapIndex += direction;
        if (this.selectedMapIndex >= this.maps.Count) {
            this.selectedMapIndex = 0;
        } else if (this.selectedMapIndex < 0)
        {
            this.selectedMapIndex = this.maps.Count - 1;
        }

        this.selectedMap = this.maps[this.selectedMapIndex];

        this.mapText.text = this.maps[this.selectedMapIndex].ToString();
    }

    public void StartGame() {
        this.onMapSelect(this.selectedMap);
    }

    public void OnSelectMap(Action action)
	{
		this.startGame.SetAction(() => {
            action();
		});
	}

}
