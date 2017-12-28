using System;
using UnityEngine;

namespace RPG.Character
{
    public class SingleTargetAttackBehaviour: SkillBehaviour
    {

        // Use this for initialization
        void Start()
        {
            print("Single Target attack behaviour attached to " + gameObject.name);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Hit(GameObject target = null)
        {
            
        }

        public override void Success(int rollGap, GameObject target = null)
        {
            float spAmount = (float)(1 - (rollGap * .01) + 0.01f);
            print("SP amount = "+spAmount);
            config.AddExp(spAmount);
            PlayParticleEffect();
            PlayAbilitySound();
            StartCooldown();
            ConsumeFatigue();
        }

        public override void Miss(GameObject target = null)
        {
            ConsumeFatigue();
            StartCooldown();
        }

        private void ConsumeFatigue()
        {
            this.GetComponent<Skills>().ConsumeFatigue((config as SingleTargetAttackConfig).GetEnergyCost());
        }
    }
}