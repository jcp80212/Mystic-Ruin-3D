using System;
using UnityEngine;

namespace RPG.Character
{
    public class CombatManueversBehaviour : SkillBehaviour
    {

        // Use this for initialization
        void Start()
        {
            print("Combat Manuevers behaviour attached to " + gameObject.name);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Success(int rollGap, GameObject target = null)
        {
            
        }

        public override void Hit(GameObject target = null)
        {
            config.AddExp(.1f);
            PlayParticleEffect();
            PlayAbilitySound();
            StartCooldown();
        }

        public override void Miss(GameObject target = null)
        {
            StartCooldown();
        }
    }
}