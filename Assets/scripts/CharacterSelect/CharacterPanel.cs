using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour {

  [SerializeField] private bool isPlayer;
  [SerializeField] private Text characterName;
  [SerializeField] private Text status;

  public void Initialize() {
    this.SignOut();
  }

  public void SignIn(bool isPlayer, int playerIndex) {
    this.isPlayer = isPlayer;
    this.DecorateSignedIn(playerIndex);
  }

  public void SignOut() {
    this.characterName.text = string.Empty;
    this.status.text = "A to sign in!";
  }


  private void DecorateSignedIn(int playerIndex) {
    this.status.text = isPlayer? ("Player " + playerIndex) : "Bot";
  }
}
