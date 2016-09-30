using UnityEngine;
using System.Collections.Generic;
using System;

public class TutorialUIManager : MonoBehaviour {

    public List<TutorialPrompt> prompts = new List<TutorialPrompt>();
    public List<TutorialPrompt> promptQueue = new List<TutorialPrompt>();
    TutorialPrompt currentPrompt = null;
    public int lastPromptNum = 0;
	// Use this for initialization
	void Start () {
	    TutorialPrompt[] initial  = this.GetComponentsInChildren<TutorialPrompt>();
        foreach(TutorialPrompt prompt in initial){
            prompt.enabled = false;
            prompts.Add(prompt);
            prompt.manager = this;
            prompt.canvasGroup = prompt.GetComponent<CanvasGroup>();
        }
	}

    public void updateTutorial(Camera cam, PlayerActions input)
    {
        if (currentPrompt == null && promptQueue.Count > 0)
        {
            currentPrompt = promptQueue[0];
            promptQueue.Remove(currentPrompt);
            lastPromptNum = currentPrompt.promptNumber;
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
                if (lastPromptNum == prompt.previousPromptNumber)
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
