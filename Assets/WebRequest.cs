using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System;
using LitJson;


public class WebRequest : MonoBehaviour
{
    [SerializeField] InputField email;
    [SerializeField] InputField password;

    public static WebRequest webRequest;


    void Awake()
    {
        if (webRequest == null)
        {
            DontDestroyOnLoad(gameObject);
            webRequest = this;

        }
        else if (webRequest != this)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        
    }

    public void btnLogin()
    {
        StartCoroutine(Login());
    }

    public void BtnRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Login()
    {
        User user = new User(email.text, password.text);
        string json = JsonUtility.ToJson(user);

        var ts = TimeSpan.FromTicks(user.LastFoodTick);
        var minutes = ts.TotalMinutes;

        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest www = UnityWebRequest.Put("http://localhost:3001/users/login", myData);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.Send();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error + " The Code is " + www.responseCode.ToString());

        }
        else
        {
            user = JsonUtility.FromJson<User>(www.downloadHandler.text);

            GameManager.manager.XAuth = www.GetResponseHeader("x-auth");
            StartCoroutine(GetCharacters());
            GameManager.manager._ID = user._ID;
            GameManager.manager.Food = user.Food;
            GameManager.manager.Gold = user.Gold;
            GameManager.manager.LastFoodTick = user.LastFoodTick;
        }
    }

    IEnumerator Register()
    {
        User user = new User(email.text, password.text);
        string json = JsonUtility.ToJson(user);

        var ts = TimeSpan.FromTicks(user.LastFoodTick);
        var minutes = ts.TotalMinutes;

        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest www = UnityWebRequest.Put("http://localhost:3001/users", myData);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.Send();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error + " The Code is " + www.responseCode.ToString());

        }
        else
        {
            user = JsonUtility.FromJson<User>(www.downloadHandler.text);

            GameManager.manager.XAuth = www.GetResponseHeader("x-auth");
            GameManager.manager._ID = user._ID;
            Debug.Log(user._ID);
        }
    }

    IEnumerator GetCharacters()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3001/Characters");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("x-auth", GameManager.manager.XAuth);
        yield return www.Send();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error + " The Code is " + www.responseCode.ToString()+" for getting character data");

        }
        else
        {
            string jsonString = www.downloadHandler.text;
            JsonData characterData = JsonMapper.ToObject(jsonString);
            SaveCharacter[] characters = new SaveCharacter[characterData["characters"].Count];
            for (int i = 0; i < characterData["characters"].Count; i++)
            {
                var chardata = characterData["characters"][i].ToJson();
                SaveCharacter tmpCharacter = JsonUtility.FromJson<SaveCharacter>(chardata.ToString());
                characters[i] = tmpCharacter;

            }
            GameManager.manager.Characters = characters;
            //StartCoroutine(UpdateCharacter(GameManager.manager.Characters[0]));
            GameManager.manager.CharacterDataLoaded = true;
           // Debug.Log(GameManager.manager.Characters[0].name);
        }
    }

    IEnumerator GetCharacterWithID(string id)
    {
        SaveCharacter character = new SaveCharacter("Testing", 1, 1);
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3001/Characters/"+id);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("x-auth", GameManager.manager.XAuth);
        yield return www.Send();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error + " The Code is " + www.responseCode.ToString());

        }
        else
        {
            character = JsonUtility.FromJson<SaveCharacter>(www.downloadHandler.text);
            //Debug.Log(character.name);
        }
    }

    public void UpdateCharacterStats(SaveCharacter characterIn)
    {
        StartCoroutine(UpdateCharacter(characterIn));
    }

    IEnumerator UpdateCharacter(SaveCharacter characterIn)
    {
        string json = JsonUtility.ToJson(characterIn);

        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put("http://localhost:3001/Characters/"+characterIn._id, myData);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("x-auth", GameManager.manager.XAuth);
        yield return www.Send();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error + " The Code is " + www.responseCode.ToString());

        }
        else
        {

            //Debug.Log(www.downloadHandler.text.ToString());
        }
    }

    IEnumerator CreateCharacter()
    {

        SaveCharacter character = new SaveCharacter("Testing", 1, 1);
        string json = JsonUtility.ToJson(character);

        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put("http://localhost:3001/CreateCharacter", myData);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("x-auth", GameManager.manager.XAuth);
        yield return www.Send();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error + " The Code is " + www.responseCode.ToString());

        }
        else
        {

            Debug.Log(www.downloadHandler.text.ToString());
        }
    }

    IEnumerator CreateSkill(SkillStats stats)
    {

        string json = JsonUtility.ToJson(stats);

        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = UnityWebRequest.Put("http://localhost:3001/CreateSkill", myData);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("x-auth", GameManager.manager.XAuth);
        yield return www.Send();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error + " The Code is " + www.responseCode.ToString());

        }
        else
        {

            Debug.Log(www.downloadHandler.text.ToString());
        }
    }
}

public class SaveCharacter
{
    [SerializeField] public string name = "Testing";
    [SerializeField] public int level = 1;
    [SerializeField] public float exp = 1;
    [SerializeField] public string _id;
    [SerializeField] public int endurance;
    [SerializeField] public int willpower;
    [SerializeField] public int strength;
    [SerializeField] public int dexterity;
    [SerializeField] public int wisdom;
    [SerializeField] public int intelligence;
    [SerializeField] public int speed;
    [SerializeField] public int agility;
    [SerializeField] public int constitution;
    [SerializeField] public int statPoints;
    [SerializeField] public int needForNextLevel;
    [SerializeField] public float skillPoints;
    [SerializeField] public float maxHealthPoints;
    [SerializeField] public float currentHealthPoints;
    [SerializeField] public float maxFatiguePoints;
    [SerializeField] public float currentFatiguePoints;


    public SaveCharacter(string name, int level, float exp)
    {
        this.name = name;
        this.level = level;
        this.exp = exp;
    }

    public SaveCharacter(string _id, string name, int level, float exp)
    {
        this._id = _id;
        this.name = name;
        this.level = level;
        this.exp = exp;
    }
}

public class User
{ 
    [SerializeField] string email = "test1@gmail.com";
    [SerializeField] string password = "abc123!";
    [SerializeField] int gold = 0;
    [SerializeField] int food = 0;
    [SerializeField] long lastFoodTick = System.DateTime.Now.Ticks;
    [SerializeField] string _id;
    
    public long LastFoodTick {  get { return lastFoodTick; } }

    public string _ID { get { return _id; } }
    public int Food {  get { return food; } }
    public int Gold {  get { return gold; } }

    public User(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}

public class JsonHelper
{
    //Usage:
    //YouObject[] objects = JsonHelper.getJsonArray<YouObject> (jsonString);
    public static T[] getJsonArray<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.array;
    }

    //Usage:
    //string jsonString = JsonHelper.arrayToJson<YouObject>(objects);
    public static string arrayToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.array = array;
        return JsonUtility.ToJson(wrapper);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
