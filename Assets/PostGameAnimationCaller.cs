using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostGameAnimationCaller : MonoBehaviour {

    public Image ranking;
    public Image characterName;
    public Text playerName;

	public void CallManager()
    {
        FindObjectOfType<DeathMatchGameManager>().inStatScreen = true;
    }
}
