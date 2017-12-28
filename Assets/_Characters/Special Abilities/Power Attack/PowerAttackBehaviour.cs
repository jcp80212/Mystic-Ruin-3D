using UnityEngine;

namespace RPG.Character {
    public class PowerAttackBehaviour : AbilityBehaviour {

        // Use this for initialization
        void Start() {
            print("Power attack behaviour attached to " + gameObject.name);
        }

        // Update is called once per frame
        void Update() {

        }
        public override void Use(GameObject target)
        {
            AttackDamage(target);
            PlayParticleEffect();
            PlayAbilitySound();
        }

        private void AttackDamage(GameObject target)
        {

            print("Power attack used, by: " + gameObject.name);
            float damageToDeal =(config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(damageToDeal);
        }
    }
}
