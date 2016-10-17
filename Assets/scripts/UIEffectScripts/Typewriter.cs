using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Typewriter : MonoBehaviour {
    public string word;
    public float textSpeed = 0.5f;
    Text text;
    int index = 0;
	void Start () {
        text = this.GetComponent<Text>();
        for(int x = 0; x< word.Length; x++)
        {
            Invoke("AddLetter", x * textSpeed * GlobalVariables.gameSpeed);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void AddLetter()
    {
        index++;
        text.text = word.Substring(0, index);
    }
}
