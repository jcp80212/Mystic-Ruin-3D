using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour {


    //Fields to be set by inspector
    [SerializeField] string _id;
    [SerializeField] int Endurance;
    [SerializeField] int Willpower;
    [SerializeField] int Strength;
    [SerializeField] int Dexterity;
    [SerializeField] int Wisdom;
    [SerializeField] int Intelligence;
    [SerializeField] int Speed;
    [SerializeField] int Agility;
    [SerializeField] int Constitution;
    [SerializeField] int StatPoints;
    [SerializeField] int level;
    [SerializeField] float exp;
    [SerializeField] int needForNextLevel;
    [SerializeField] Image ExpGraphic;
    [SerializeField] string title;

    public string Name { get { return title; } set { title = value; } }
    public int NeedForNextLevel { get { return needForNextLevel; } set { needForNextLevel = value; } }
    public string _Id { get { return _id; } set { _id = value; } }


    private void Update()
    {/*
        if (this.tag == "Player")
        {
            if (ExpGraphic != null)
            {
                ExpGraphic.fillAmount = expAsPercent();
            }
            else
            {
                ExpGraphic.fillAmount = .5f;
            }
        }*/
    }

    private void Start()
    {
        updateNeedForNextLevel();
    }

    public void updateNeedForNextLevel()
    {
        needForNextLevel =(int)Mathf.Round(4*(Mathf.Pow(level, 3))/5);
    }

    public void addExp(float expToGive)
    {
        exp += expToGive;
        while(exp >= needForNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        exp = exp - needForNextLevel;
        level++;
        StatPoints = StatPoints + 10;
        updateNeedForNextLevel();
        print("You have leveled up to level " + level);
    }

    public int OffenseSuccess()
    {
        int success = 0;
        success += Dexterity;
        return success;
    }

    public int DefenseSuccess()
    {
        int success = 0;
        success += Agility;
        return success;

    }

    public float RegenAmount()
    {
        float regenAmount = 0.0f;
        regenAmount = (float)Willpower * .5f;
        return regenAmount;
    }

    public int MaxHpAmount()
    {
        return (int)(Constitution * 1.5f + 100);
    }

    public float FatigueRegenAmount()
    {
        float regenAmount = 0.0f;
        regenAmount = (float)Constitution * .5f;
        return regenAmount;
    }

    public int MaxFatigueAmount()
    {
        return (int)(Endurance * 1.5f + 50);
    }

    private float expAsPercent()
    {
        return exp / needForNextLevel;
    }

    public float GetDamage()
    {
        return Strength;
    }

    public float GetDamageResistance()
    {
        return Willpower;
    }

    public float Exp
    {
        get { return exp; }
        set { exp = value; }
    }

    public int Level
    {
        get { return level;  }
        set { level = value; }
    }
}
