using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(menuName = ("RPG/Skill/Combat Manuever"))]
    public class CombatManueversConfig : SkillConfig
    {
        [Header("Combat Manuevers Specific")]
        [SerializeField]
        float extraDamage = 10f;

        public override SkillBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<CombatManueversBehaviour>();
        }

        public float GetExtraDamage()
        {
            return extraDamage;
        }
    }
}