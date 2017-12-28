using UnityEngine.EventSystems;
using UnityEngine;

public class UICharacterPanelClick : EventTrigger {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnPointerClick(PointerEventData data)
    {
        GetComponent<UICharacterPanel>().PlayerSpawn();
        //GameObject.Destroy(this.gameObject);
    }
}
