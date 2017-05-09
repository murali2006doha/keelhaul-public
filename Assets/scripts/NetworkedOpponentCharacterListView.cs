using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedOpponentCharacterListView : MonoBehaviour {

    [SerializeField]
    NetworkedSelectedCharacterView view;
    // Use this for initialization
    Dictionary<string, NetworkedSelectedCharacterView> playerMap = new Dictionary<string, NetworkedSelectedCharacterView>();



    public void AddCharacterView(string playerId) {
        var instantiatedView = Instantiate(view, this.transform, false);
        instantiatedView.Initialize(playerId);
        playerMap.Add(playerId, instantiatedView);
    }

    public void RefreshCharacterView(string selectedShip, string playerId)
    {
        if (playerMap.ContainsKey(playerId)) {
            playerMap[playerId].SetCharacterPortrait(selectedShip);
        }
    }

    public void DestroyCharacterView(string playerId) {
        Destroy(playerMap[playerId].gameObject);
    }
}
