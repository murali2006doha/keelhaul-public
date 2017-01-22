using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;
//using System.Object;

public class NetworkedCharacterSelectView : MonoBehaviour {

	public PlayerActions Actions { get; set; }
    public NetworkedCharacterSelectButton [] buttons;
	private List<ShipEnum> characters = new List<ShipEnum> ();
	public ControllerSelect cc;
	GameObject csPanel;
	AbstractCharacterSelectController csc;
	public bool withKeyboard;

	//new CharacterSelection(player.selectedCharacter,player.Actions);

    // Use this for initialization
//   	public void Initialize(Action<ShipEnum> onClick) {
//        foreach (NetworkedCharacterSelectButton button in buttons) {
//            button.onClickAction = onClick;
//      }
//
//    }


	void Start() {
		cc = GameObject.FindObjectOfType<ControllerSelect> ();
		cc.withKeyboard = withKeyboard;
		cc.listening = false;
	}
		

	public void getSelectCharacter(AbstractCharacterSelectController csc) {
		this.csc = csc;
		this.csc.getShipType (0);
	}

	public void initializePanel(AbstractCharacterSelectController csc) {

		Object panelPrefab = Resources.Load(CharacterSelectModel.CSPanelPrefab, typeof(GameObject));
		csPanel = Instantiate(panelPrefab, GameObject.Find ("Container").transform.position, GameObject.Find ("Container").transform.rotation) as GameObject;
		Vector3 localscale = csPanel.gameObject.transform.localScale;
		csPanel.gameObject.GetComponent<CharacterSelectPanel> ().initializePanel (csc, csc.characters, csc.Actions);


		csPanel.gameObject.transform.SetParent(GameObject.Find ("Container").transform);
		csPanel.gameObject.transform.localScale = localscale;
	}
}
