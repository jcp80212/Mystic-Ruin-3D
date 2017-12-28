using UnityEngine;

public class SkillStats {

    [SerializeField] string _creator;
    [SerializeField] string _id;
    [SerializeField] int level = 0;
    [SerializeField] float currentExp;
    [SerializeField] int needForNextLevel;
    [SerializeField]float difficultyMod;


    public string _Creator { get { return _creator; } set { _creator = value; } }
    public string _Id { set { _id = value; } }
    public float DifficultyMod {  set { difficultyMod = value;  } }
    public int Level {  get { return level; } set { level = value; } }
    public float CurrentExp { get { return currentExp; } set { currentExp = value; } }
    public int NeedForNextLevel { get { return needForNextLevel; } set { needForNextLevel = value; } }

    public SkillStats( int level, float currentExp, float difficutlyMod )
    {
        this.level = level;
        this.currentExp = currentExp;
        this.difficultyMod = difficutlyMod;
        calculateNeedForNextLevel();
    }

    void calculateNeedForNextLevel()
    {
        var mathfornextlevel = Mathf.Round(4 * (Mathf.Pow(level, 3)) / 5) * difficultyMod;
        needForNextLevel = (int)mathfornextlevel;
    }

    public void AddExp(float expToAdd)
    {
        currentExp += expToAdd;
        if (currentExp >= needForNextLevel)
        {
            level++;
            currentExp = currentExp - needForNextLevel;
            calculateNeedForNextLevel();
        }
    }
}
