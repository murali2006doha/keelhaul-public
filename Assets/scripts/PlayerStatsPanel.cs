using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class PlayerStatsPanel : MonoBehaviour {


    public Text positionText;
    public Text playerNumberText;
    public Image portrait;
    public Text scoreText;
    public Slider scoreSlider;
    int size;

    public void Initialize(float pointsPercent, string scoreText, string positionText, String playerNumberText, Sprite portrait, int size) {
        this.positionText.text = positionText;
        this.playerNumberText.text = playerNumberText;
        this.portrait.sprite = portrait;
        this.scoreSlider.value = pointsPercent;
        this.scoreText.text = scoreText;
    }
}
