using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSelectDescriptionView : MonoBehaviour {
    [SerializeField]
    Text characterSelectDescription;

    [SerializeField]
    TMPro.TextMeshProUGUI altFireText;

    [SerializeField]
    TMPro.TextMeshProUGUI altFireIcon;

    public void OnCharacterSelectChange(ShipEnum ship) {
        altFireIcon.text = "<sprite=\"ability-spritesheet\" name=\"" + ship.ToString() + "\">";
        if (ship == ShipEnum.BlackbeardShip)
        {
            characterSelectDescription.text = "Blackbeard is a pirate who wears a hat to cover his baldness that he's been suffering from as a child. Despite his luxurious black beard, his original moniker was actually Baldbeard. However anyone who dares to call him that often end up disappearing.";
            altFireText.text = "Rage Mode";

        }
        else if (ship == ShipEnum.ChineseJunkShip)
        {
            characterSelectDescription.text = "As clever as she is dangerous, Madame Gung quickly rose to the top of black market mafia dealing in sea monster parts that no one else dared to hunt. In her spare time she likes to play Xiangqi";
            altFireText.text = "Firework Shot";
        }
        else if (ship == ShipEnum.AtlanteanShip) {
            characterSelectDescription.text = "A reluctant pirate with a singular goal: Kill the kraken";
            altFireText.text = "Proton Shield";
        }
    }
}
