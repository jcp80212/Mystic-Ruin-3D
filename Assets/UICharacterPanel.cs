using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.Character;

public class UICharacterPanel : MonoBehaviour {

    public Text characterText;
    [SerializeField] SaveCharacter panelCharacter;
    [SerializeField] GameObject Player;
    [SerializeField] Image healthImage;
    [SerializeField] Image fatigueImage;
    [SerializeField] Image experienceImage;
    [SerializeField] Text txtLevel;
    [SerializeField] Text txtLevelPercent;
    [SerializeField] Text txtHealth;
    [SerializeField] Text txtFatigue;
    private Transform playerSpawnPoint;
    private bool spawned = false;
    private GameObject SavedPlayer;

    public SaveCharacter PanelCharacter { get { return panelCharacter; } set { panelCharacter = value; } }

	// Use this for initialization
	void Start () {
        playerSpawnPoint = GameObject.FindGameObjectWithTag("Player Spawn Point").transform;
    }
	
	// Update is called once per frame
	void Update () {
        updateGraphics();

	}

    public void PlayerSpawn()
    {
        if (spawned == false)
        {
            SavedPlayer = Instantiate(Player, playerSpawnPoint, true);
            SavedPlayer.GetComponent<Stats>().Level = panelCharacter.level;
            SavedPlayer.GetComponent<Stats>().Name = panelCharacter.name;
            SavedPlayer.GetComponent<Stats>().Exp = panelCharacter.exp;
            SavedPlayer.GetComponent<Stats>()._Id = panelCharacter._id;
            SavedPlayer.GetComponent<Stats>().NeedForNextLevel = panelCharacter.needForNextLevel;
            GameManager.manager.SelectedCharacter = SavedPlayer;
            Debug.Log("Instantiating: " + panelCharacter.name);
            spawned = true;
        }
        else
        {
            GameManager.manager.SelectedCharacter = SavedPlayer;
        }
    }

    float healthPercent()
    {
        if (spawned == false)
        {
            return panelCharacter.currentHealthPoints / panelCharacter.maxHealthPoints;
        }
        else
        {
            return SavedPlayer.GetComponent<HealthSystem>().CurrentHealthPoints/SavedPlayer.GetComponent<HealthSystem>().MaxHealthPoints;
        }
    }
    float fatiguePercent()
    {
        if (spawned == false)
        {
            return panelCharacter.currentFatiguePoints / panelCharacter.maxFatiguePoints;
        }
        else
        {
            return SavedPlayer.GetComponent<Skills>().CurrentFatiguePoints / SavedPlayer.GetComponent<Skills>().MaxFatiguePoints;
        }
    }
    float experiencePercent()
    {
        if (spawned == false)
        {
            return panelCharacter.exp / panelCharacter.needForNextLevel;
        }
        else
        {
            return SavedPlayer.GetComponent<Stats>().Exp / SavedPlayer.GetComponent<Stats>().NeedForNextLevel;
        }
    }

    string healthText()
    {
        if(spawned == false)
        {
            return panelCharacter.currentHealthPoints.ToString() + "/" + panelCharacter.maxHealthPoints.ToString();
        }
        else
        {
            return SavedPlayer.GetComponent<HealthSystem>().CurrentHealthPoints.ToString()+"/"+ SavedPlayer.GetComponent<HealthSystem>().MaxHealthPoints.ToString();
        }
    }

    string fatigueText()
    {
        if (spawned == false)
        {
            return panelCharacter.currentFatiguePoints.ToString() + "/" + panelCharacter.maxFatiguePoints.ToString();
        }
        else
        {
            return SavedPlayer.GetComponent<Skills>().CurrentFatiguePoints.ToString() + "/" + SavedPlayer.GetComponent<Skills>().MaxFatiguePoints.ToString();
        }
    }

    private void updateGraphics()
    {
        healthImage.fillAmount = healthPercent();
        fatigueImage.fillAmount = fatiguePercent();
        experienceImage.fillAmount = experiencePercent();
        txtHealth.text = healthText();
        txtFatigue.text = fatigueText();
        var levelPercent = experiencePercent() * 100;
        txtLevelPercent.text = levelPercent.ToString() + "%";
        if (spawned == false)
        {
            txtLevel.text = "Level "+ panelCharacter.level.ToString();
        }
        else
        {
            txtLevel.text = "Level " + SavedPlayer.GetComponent<Stats>().Level.ToString();
        }
        

    }
}
