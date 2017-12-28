using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterSelectionPanel : MonoBehaviour {

    [SerializeField]GameObject characterPanel;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < GameManager.manager.Characters.Length; i++)
        {
            var tmpCharacter = GameManager.manager.Characters[i];
            var panel = Instantiate(characterPanel, this.transform, false);
            panel.GetComponent<UICharacterPanel>().characterText.text = "Name: " + tmpCharacter.name;
            panel.GetComponent<UICharacterPanel>().PanelCharacter = GameManager.manager.Characters[i];
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
