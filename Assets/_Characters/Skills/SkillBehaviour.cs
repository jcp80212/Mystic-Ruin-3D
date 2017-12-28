using System.Collections;
using UnityEngine;


namespace RPG.Character
{
    public abstract class SkillBehaviour : MonoBehaviour
    {
        protected SkillConfig config;

        bool isInCooldown = false;

        const float PARTICLE_CLEAN_UP_DELAY = 0.5f;

        public abstract void Success(int rollGap, GameObject target = null);
        
        public abstract void Hit(GameObject target = null);

        public abstract void Miss(GameObject target = null);

        public bool IsInCoolDown()
        {
            return isInCooldown;
        }

        public void SetConfig(SkillConfig configToSet)
        {
            config = configToSet;
        }

        protected void PlayParticleEffect()
        {
            //Instantiate particle system thats attached to the player
            var particlePrefab = config.GetParticlePrefab();
            var particleObject = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);

            particleObject.transform.parent = transform;
            //Get the particle system component
            ParticleSystem myParticleSystem = particleObject.GetComponent<ParticleSystem>();

            //play the particle system
            myParticleSystem.Play();

            StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }

        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            Destroy(particlePrefab);
        }

        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAudioClip();
            var audioSource = GetComponent<AudioSource>();

            audioSource.PlayOneShot(abilitySound);
        }

        protected void StartCooldown()
        {
            isInCooldown = true;
            StartCoroutine(CoolDown());
        }

        IEnumerator CoolDown()
        {
            while (isInCooldown == true)
            {
                yield return new WaitForSeconds(config.CoolDown);
                isInCooldown = false;
            }
        }
    }
}

