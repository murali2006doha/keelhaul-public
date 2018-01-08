
using UnityEngine;

[CreateAssetMenu(fileName = "Colors", menuName = "UI/ColorDictionary")]
public class ColorDictionary : ScriptableObject {
  [SerializeField] private Color[] colors;

  public Color Get(int index) {
    return this.colors[index];
  }
}