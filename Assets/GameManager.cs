using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] string xAuth;
    [SerializeField] string _id;
    [SerializeField] int gold;
    [SerializeField] int food;
    [SerializeField] long lastFoodTick;
    [SerializeField] SaveCharacter[] characters;
    [SerializeField] GameObject selectedCharacter;

    private bool GameLoaded = false;
    private bool characterDataLoaded = false;

    public GameObject SelectedCharacter { get { return selectedCharacter; } set { selectedCharacter = value; } }
    public bool CharacterDataLoaded { get { return characterDataLoaded; } set { characterDataLoaded = value; } }
    public string XAuth {  get { return xAuth; } set { xAuth = value; } }
    public string _ID { get { return _id; } set { _id = value; } }
    public int Gold { get { return gold; } set { gold = value; } }
    public int Food {  get { return food; } set { food = value; } }
    public long LastFoodTick {  get { return lastFoodTick; } set { lastFoodTick = value; } }
    public SaveCharacter[] Characters { get { return characters; } set { characters = value; } }

    public static GameManager manager;


    void Awake()
    {
        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;

        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        if(xAuth != "" && GameLoaded == false && characterDataLoaded == true)
        {
            SceneManager.LoadScene("MapOverview", LoadSceneMode.Single);
            GameLoaded = true;
        }
    }
}
