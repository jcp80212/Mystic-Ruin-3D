using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    [SerializeField] Text goldText;
    [SerializeField] Text foodText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        goldText.text = "Gold: " + GameManager.manager.Gold;
        foodText.text = "Food: " + GameManager.manager.Food;
	}

    public void BackToMainMap()
    {
        SceneManager.LoadScene("MapOverview");
    }
}
