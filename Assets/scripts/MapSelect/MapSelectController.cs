using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
using System;

public class MapSelectController : MonoBehaviour {

	[SerializeField]
    public MapSelectView view;


    public void Initialize(PlayerSelectSettings settings, GameTypeEnum gameMode)
	{

        view.Initialize(gameMode);

	}

    public void OnSelectMap(Action action)
	{
        view.OnSelectMap(action);
	}
}
