using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameCheats : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            DestroyAll();
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        if (Input.GetKey(KeyCode.Alpha8) && Input.GetKey(KeyCode.Alpha7))
        {
            Application.Quit();
        }
    }



    private void DestroyAll()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            if (o != this.gameObject) {
                Destroy(o);
            }
        }
    }
}
