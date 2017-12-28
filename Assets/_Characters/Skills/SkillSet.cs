using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class SkillSet : MonoBehaviour
    {

        [SerializeField] WeaponType.weaponType type;
        [SerializeField] int Level;
        [SerializeField] float exp;
        [SerializeField] int needForNextLevel;
        [SerializeField] List<GameObject> OffensiveSkills;
        [SerializeField] List<GameObject> DefensiveSkills;
        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Skill>().SkillType() == WeaponType.skillType.offensive)
                {
                    OffensiveSkills.Add(transform.GetChild(i).gameObject);
                }
                if (transform.GetChild(i).GetComponent<Skill>().SkillType() == WeaponType.skillType.defensive)
                {
                    DefensiveSkills.Add(transform.GetChild(i).gameObject);
                }
            }
        }

        public float GetDefensiveRankBonus(WeaponType.attackArea area)
        {
            float defBonus = 0.0f;
            for (int i = 0; i < DefensiveSkills.Count; i++)
            {
                if(DefensiveSkills[i].GetComponent<Skill>().AttackArea() == area)
                {
                    defBonus += DefensiveSkills[i].GetComponent<Skill>().GetRankBonus();
                }
            }
            return defBonus;
        }
        

        public Skill getRandomAttack(WeaponType.weaponType weaponType)
        {
            Skill skillToReturn = null;
            List<Skill> tmpList = new List<Skill>();
            for (int i = 0; i < OffensiveSkills.Count; i++)
            {
               if(!OffensiveSkills[i].GetComponent<Skill>().IsInCoolDown())
               {
                    tmpList.Add(OffensiveSkills[i].GetComponent<Skill>());
               }
            }
            if(tmpList.Count > 0)
            {
                skillToReturn = tmpList[Random.Range(0, tmpList.Count)];
            }

            return skillToReturn;
        }

        public WeaponType.weaponType Type()
        {
            return type;
        }
    }
}
