using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraPriority : MonoBehaviour {

    CinemachineVirtualCamera cvc;
    GameObject player;

	// Use this for initialization
	void Start () {
        cvc = GetComponent<CinemachineVirtualCamera>();
        cvc.Follow = GameObject.FindGameObjectWithTag("Player Spawn Point").transform;
        cvc.LookAt = GameObject.FindGameObjectWithTag("Player Spawn Point").transform;

    }
	
	// Update is called once per frame
	void Update () {
        if(GameManager.manager.SelectedCharacter != null)
        {
            cvc.Follow = GameManager.manager.SelectedCharacter.transform;
            cvc.LookAt = GameManager.manager.SelectedCharacter.transform;
        }
        else
        {
            cvc.Follow = GameObject.FindGameObjectWithTag("Player Spawn Point").transform;
            cvc.LookAt = GameObject.FindGameObjectWithTag("Player Spawn Point").transform;
        }
    }
}
