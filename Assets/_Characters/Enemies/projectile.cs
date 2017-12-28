using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Character
{
    public class projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed = .1f;
        [SerializeField] GameObject shooter;
        float damageCaused = 10;

        const float DESTROY_DELAY = 0.01f;


        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public float GetDefaultLanuchSpeed()
        {
            return projectileSpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer != shooter.layer)
            {
                DamageIfDamageable(collision);
            }


        }

        private void DamageIfDamageable(Collision collision)
        {
            Component damageableComponent = collision.gameObject.GetComponent(typeof(HealthSystem));
            if (damageableComponent)
            {
                (damageableComponent as HealthSystem).TakeDamage(damageCaused);
            }
            Destroy(gameObject, DESTROY_DELAY);
        }
    }
}
