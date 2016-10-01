using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
public class TutorialUIManager : MonoBehaviour {

    public List<TutorialPrompt> prompts = new List<TutorialPrompt>();
    public List<TutorialPrompt> promptQueue = new List<TutorialPrompt>();
    TutorialPrompt currentPrompt = null;
    HashSet<int> completedPrompts = new HashSet<int>();
	// Use this for initialization
	void Start () {
	    TutorialPrompt[] initial  = this.GetComponentsInChildren<TutorialPrompt>();
        foreach(TutorialPrompt prompt in initial){
            prompt.enabled = false;
            prompts.Add(prompt);
            prompt.manager = this;
            prompt.canvasGroup = prompt.GetComponent<CanvasGroup>();
            if (prompt.isFindByName)
            {
                prompt.gameObj = GameObject.Find(prompt.gameObjectName);
                prompt.isFindByName = false;
            }
        }
        completedPrompts.Add(0);

    }

    public void updateTutorial(Camera cam, PlayerActions input)
    {
        if (currentPrompt == null && promptQueue.Count > 0)
        {
            currentPrompt = promptQueue[0];
            promptQueue.Remove(currentPrompt);
            completedPrompts.Add(currentPrompt.promptNumber);
            currentPrompt.enabled = true;
            currentPrompt.fadeIn();
        }

        if(currentPrompt !=null)
        {
            currentPrompt.UpdateWithInput(input);
        }
        foreach (TutorialPrompt prompt in prompts)
        {
            if (!prompt.isOnScreenCheck)
            {
                if (completedPrompts.Contains(prompt.previousPromptNumber))
                {
                    promptQueue.Add(prompt);
                }
            } else if (prompt.gameObj !=null)
            {
                Vector3 screenPoint = cam.WorldToViewportPoint(prompt.gameObj.transform.position);
                bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
                if (onScreen)
                {
                    promptQueue.Add(prompt);
                }
            }
        }
        prompts.RemoveAll(prompt => promptQueue.Contains(prompt));
        
    }

    public void clearPrompt()
    {
        currentPrompt = null;
    }

    public bool isEmpty()
    {
        return currentPrompt == null && prompts.Count == 0 && promptQueue.Count == 0;
    }
}
