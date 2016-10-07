using UnityEngine;
using System.Collections;

public class TutorialPrompt : MonoBehaviour {

    public float timeOut;
    public int previousPromptNumber = -1;
    public int promptNumber;
    public bool isOnScreenCheck;
    public bool isFindByName;
    public GameObject gameObj;
    public string gameObjectName;
    public TutorialUIManager manager;
    public InputEnum input;
    public CanvasGroup canvasGroup;
    bool fadeOut = false;
    public float fadeSpeed = 0.2f;
    public float timeToFadeAfterInput = 0f;

    public void UpdateWithInput(PlayerActions actions)
    {
        if(!fadeOut && canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += fadeSpeed * Time.deltaTime * GlobalVariables.gameSpeed;
        } else if (fadeOut && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime * GlobalVariables.gameSpeed;
        } else if (fadeOut)
        {
            destroyObject();
        }

        if (InputEnumConverter.wasPressed(input, actions))
        {
            CancelInvoke();
            Invoke("FadeOut", timeToFadeAfterInput);
        }
    }

    void FadeOut()
    {
        fadeOut = true;
    }

    void destroyObject()
    {
        manager.clearPrompt();
        Destroy(this.gameObject);
    }
    
    public void fadeIn()
    {
        Invoke("FadeOut", timeOut);
    }

}
