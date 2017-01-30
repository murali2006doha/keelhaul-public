using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public abstract class AbstractGameManager : MonoBehaviour {

    // Use this for initialization
    [HideInInspector]
    public GameObject screenSplitter;

    void Start () {
	
	}

    protected void RunStartUpActions() {
        Physics.gravity = new Vector3(0f, -0.1f, 0f);
        Application.targetFrameRate = -1; //Unlocks the framerate at start
        Resources.UnloadUnusedAssets();
    }

    public virtual void acknowledgeBarrelScore(PlayerInput player, GameObject barrel)
    {

    }
    public virtual void acknowledgeKill(StatsInterface attacker, StatsInterface victim)
    {

    }

    public abstract void exitToCharacterSelect();
    
    public abstract void respawnPlayer(PlayerInput player, Vector3 startingPoint);
    virtual public void respawnKraken(KrakenInput player, Vector3 startingPoint)
    {

    }

    protected void demoScript(cameraFollow[] cams)
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            screenSplitter.SetActive(false);
            cams[0].camera.rect = new Rect(0, 0, 1, 1);
            cams[1].camera.rect = new Rect(0, 0, 0, 0);
            cams[2].camera.rect = new Rect(0, 0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            screenSplitter.SetActive(false);
            cams[1].camera.rect = new Rect(0, 0, 1, 1);
            cams[0].camera.rect = new Rect(0, 0, 0, 0);
            cams[2].camera.rect = new Rect(0, 0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            screenSplitter.SetActive(false);
            cams[2].camera.rect = new Rect(0, 0, 1, 1);
            cams[0].camera.rect = new Rect(0, 0, 0, 0);
            cams[1].camera.rect = new Rect(0, 0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SceneManager.LoadScene(1);
        }
    }

    public abstract bool isGameOver();
    internal abstract int getNumberOfTeams();
    public abstract List<PlayerInput> getPlayers();
}
