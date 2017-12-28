using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character {
    public class SkillContainer : MonoBehaviour {

        [SerializeField] List<GameObject> skillSets;

        // Use this for initialization
        void Start() {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).tag == "Skill Set")
                {
                    skillSets.Add(transform.GetChild(i).gameObject);
                }
            }
        }

        public Skill GetSkill(WeaponType.weaponType weaponType)
        {
            Skill skillToReturn = null;
            for (int i = 0; i < skillSets.Count; i++)
            {
                if (skillSets[i].GetComponent<SkillSet>().Type() == weaponType)
                {
                    skillToReturn = skillSets[i].GetComponent<SkillSet>().getRandomAttack(weaponType);
                }
            }
            return skillToReturn;
        }

        public float GetDefensiveRankBonus(WeaponType.weaponType weaponType, WeaponType.attackArea area)
        {
            float defBonus = 0.0f;
            for (int i = 0; i < skillSets.Count; i++)
            {
                if (skillSets[i].GetComponent<SkillSet>().Type() == weaponType)
                {
                    defBonus = skillSets[i].GetComponent<SkillSet>().GetDefensiveRankBonus(area);
                }
            }
            return defBonus;
        } 
    }
}
