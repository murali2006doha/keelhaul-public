using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelHostHolder : MonoBehaviour {
  [SerializeField] private Image indicator;
  [SerializeField] private Text hostIndicator;
  [SerializeField] private ColorDictionary colors;


  public void Decorate(int playerIndex) {
    this.gameObject.SetActive(true);
    this.indicator.color = this.colors.Get(playerIndex);
    this.hostIndicator.color = this.colors.Get(playerIndex);
    this.hostIndicator.text = "P" + (playerIndex + 1);
  }


  public void Hide() {
    this.gameObject.SetActive(false);
  }
}
