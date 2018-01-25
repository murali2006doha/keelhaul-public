using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class MapSelectView : MonoBehaviour {

	[SerializeField] 
    private ActionButton startGame;

    [SerializeField]
    private ActionButton upButton;

    [SerializeField]
    private ActionButton downButton;

    [SerializeField]
    private Image mapImage;

    [SerializeField]
    private SpriteDictionary mapImages;

    [SerializeField]
    private Transform loadingScreen;

    [SerializeField]
    private float loadingTime;

    [SerializeField]
    private Animator mapViewAnimator;

    private Action<MapEnum> onMapSelect;
    List<MapEnum> maps = new List<MapEnum>();
    public MapEnum selectedMap = MapEnum.TropicalMap;

    private int selectedMapIndex = 0;
    private bool mapSelected;

    public bool skipMapSelect = false;

    public void Start()
    {
        
        
            if (skipMapSelect)
            {
                Hide();
                onMapSelect(selectedMap);
            }
        
    }

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

        if(gameMode == GameTypeEnum.Targets)
        {
            maps = MapTypeHelper.GetDeathMatchOfflineMaps();
            skipMapSelect = true;
        }

        this.selectedMap = this.maps[this.selectedMapIndex];
        this.mapImage.sprite = this.mapImages.Get(selectedMap.ToString());

    }

    public void ChangeMap(int direction)
    {
        AnimateArrows(direction);

        this.selectedMapIndex += direction;
        if (this.selectedMapIndex >= this.maps.Count) {
            this.selectedMapIndex = 0;
        } else if (this.selectedMapIndex < 0)
        {
            this.selectedMapIndex = this.maps.Count - 1;
        }

        this.selectedMap = this.maps[this.selectedMapIndex];
        this.mapImage.sprite = this.mapImages.Get(selectedMap.ToString());

    }

    private void AnimateArrows(int direction)
    {
        if (direction == -1)
        {
            mapViewAnimator.SetTrigger("down");
        }
        else if (direction == 1)
        {
            mapViewAnimator.SetTrigger("up");
        }
    }

    public void StartGame() {
        this.onMapSelect(this.selectedMap);
        StartCoroutine(LoadNewScene());
    }

    public void OnSelectMap(Action action)
	{
		this.startGame.SetAction(() => {
            action();
		});
	}


    IEnumerator LoadNewScene()
    {
        loadingScreen.gameObject.SetActive(true);

        yield return new WaitForSeconds(loadingTime);

        AsyncOperation async = SceneManager.LoadSceneAsync("Game");

        while (!async.isDone)
        {
            yield return null;
        }

    }

}
