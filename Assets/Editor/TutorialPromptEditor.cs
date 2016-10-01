using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TutorialPrompt))]
[CanEditMultipleObjects]
public class TutorialPromptEditor : Editor
{

    public override void OnInspectorGUI()
    {
        TutorialPrompt myTarget = (TutorialPrompt)target;
        
        myTarget.promptNumber = EditorGUILayout.IntField("Prompt Number", myTarget.promptNumber);
        myTarget.timeOut = EditorGUILayout.FloatField("Time Out duration in seconds", myTarget.timeOut);
        myTarget.input = (InputEnum) EditorGUILayout.EnumPopup("Input to complete prompt",myTarget.input);
        myTarget.isOnScreenCheck = EditorGUILayout.ToggleLeft("Show when Object On Screen or Show after certain prompt",myTarget.isOnScreenCheck);
        if (!myTarget.isOnScreenCheck)
        {
            myTarget.previousPromptNumber = EditorGUILayout.IntField("Previous Prompt", myTarget.previousPromptNumber);
            myTarget.gameObj = null;
            myTarget.gameObjectName = "";
        }
        else
        {
            myTarget.isFindByName = EditorGUILayout.ToggleLeft("Find object by name instead of reference", myTarget.isFindByName);
            if (!myTarget.isFindByName)
            {
                myTarget.gameObj = (GameObject)EditorGUILayout.ObjectField("Object on screen", myTarget.gameObj, typeof(GameObject), true);
                myTarget.gameObjectName = "";
            }
            else
            {
                myTarget.gameObjectName = EditorGUILayout.TextField("name", myTarget.gameObjectName);
                myTarget.gameObj = null;
            }
            myTarget.previousPromptNumber = -1;
        }
    }
}
