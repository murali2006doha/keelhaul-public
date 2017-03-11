using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipWorldCanvas : MonoBehaviour {
    [SerializeField]
    private TMPro.TextMeshPro textMesh;

    [SerializeField]
    private restrainPositionAndRotation restrainer;

    string playerNum;

    [SerializeField]
    Slider healthBar;


    public void Initiialize(int playerNum) {
        textMesh.text = "P" + playerNum.ToString();
        restrainer.enabled = true;
    }
    public void UpdateHealthSlider(float health) {
        float step = GlobalVariables.uiSliderSpeed * Time.deltaTime;
        healthBar.value = Mathf.MoveTowards(healthBar.value, health, step);
    }
}
