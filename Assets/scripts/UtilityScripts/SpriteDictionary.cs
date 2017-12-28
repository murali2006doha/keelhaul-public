
using UnityEngine;

[CreateAssetMenu(fileName = "Sprites", menuName = "UI/SpriteDictionary")]
public class SpriteDictionary : ScriptableObject {
  [SerializeField] private Sprite[] sprites;

  public Sprite Get(string name) {
    for (int i = 0; i < this.sprites.Length; i++) {
      if (this.sprites[i].name == name) {
        return this.sprites[i];
      }
    }

    return null;
  }
}