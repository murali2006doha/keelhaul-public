using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GlobalCanvas : MonoBehaviour {

    public GameObject countDownTimer;
    public Animator fadePanelAnimator;
    public GameObject finishText;
    public GameObject splitscreenImages;
    public GameOverStatsUI gameOverUI;
    public RawImage panel1;
    public RawImage panel2;


    public void setUpSplitScreen(int numOfPlayers)
    {

        for(int x = 0;x< splitscreenImages.transform.childCount; x++)
        {
            Transform child = splitscreenImages.transform.GetChild(x);
            if (int.Parse(child.name) > numOfPlayers)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
