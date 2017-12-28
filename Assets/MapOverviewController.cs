using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapOverviewController : MonoBehaviour {

    [SerializeField] GameObject[] MainMapObjects;
    [SerializeField] GameObject[] OutPostObjects;
    [SerializeField] GameObject LoadingPanel;


	// Use this for initialization
	void Start () {
        LoadingPanel.SetActive(false);
        ActivateOrDeactivateObjectSets(MainMapObjects, true);
        ActivateOrDeactivateObjectSets(OutPostObjects, false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Escape))
        {
            ActivateOrDeactivateObjectSets(MainMapObjects, true);
            ActivateOrDeactivateObjectSets(OutPostObjects, false);
        }
	}

    private void ActivateOrDeactivateObjectSets(GameObject [] objects, bool active)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(active);
        }
    }

    public void PickOutpost()
    {
        ActivateOrDeactivateObjectSets(MainMapObjects, false);
        ActivateOrDeactivateObjectSets(OutPostObjects, true);
    }

    public void PickGardens()
    {
        LoadingPanel.SetActive(true);
        SceneManager.LoadScene("Outpost");
    }
}
