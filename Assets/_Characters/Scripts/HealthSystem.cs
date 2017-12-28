using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RPG.Character
{

    public class HealthSystem : MonoBehaviour
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] DamageSounds;
        [SerializeField] AudioClip[] DeathSounds;
        [SerializeField] float deathVanishSeconds = 5f;

        const string DEATH_TRIGGER = "Death";
        [SerializeField]float currentHealthPoints = 100f;
        Animator animator;

        AudioSource audioSource;

        Character characterMovement;

        public float CurrentHealthPoints {  get { return currentHealthPoints; } }
        public float MaxHealthPoints { get { return maxHealthPoints; } }

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();
            StartCoroutine(AddHealthPoints());
        }

        // Update is called once per frame
        void Update()
        {
            UpdateHealthBar();
        }

        IEnumerator AddHealthPoints()
        {
            while(true)
            {
                maxHealthPoints = GetComponent<Stats>().MaxHpAmount();
                if (currentHealthPoints >= Mathf.Epsilon)
                {
                    var pointsToAdd = GetComponent<Stats>().RegenAmount();
                    currentHealthPoints = Mathf.Clamp(currentHealthPoints + pointsToAdd, 0, maxHealthPoints);
                }
                yield return new WaitForSeconds(30f);
            }
        }

        void UpdateHealthBar()
        {
            if (healthBar)
            {
                healthBar.fillAmount = healthAsPercentage;
            }

        }

        public float healthAsPercentage
        {
            get
            {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        public void TakeDamage(float damage)
        {
            bool characterDies = (currentHealthPoints - damage <= 0);
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            //var clip = DamageSounds[Random.Range(0, DamageSounds.Length)];
            //audioSource.PlayOneShot(clip);
            if (characterDies)
            {
                //Kill player
                StartCoroutine(KillCharacter());
            }

        }

        public void SetHealthToMax()
        {
            currentHealthPoints = maxHealthPoints;
        }

        public void Heal(float points)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
        }

        IEnumerator KillCharacter()
        {
            StopAllCoroutines();
            characterMovement.Kill();
            //trigger death animation
            animator.SetTrigger(DEATH_TRIGGER);

            var playerComponent = GetComponent<PlayerControl>();
            if (playerComponent && playerComponent.isActiveAndEnabled)
            {
                AudioClip audioClip = DeathSounds[Random.Range(0, DeathSounds.Length)];
                audioSource.PlayOneShot(audioClip);
                SceneManager.LoadScene(0);
                yield return new WaitForSeconds(5.0f);
                //reload the scene
                
            }
            else 
            {
                var stats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
                var skills = GameObject.FindGameObjectWithTag("Player").GetComponent<Skills>();
                stats.addExp(this.GetComponent<Stats>().Exp);
                skills.AddSkillPoints(this.GetComponent<Skills>().Skillpoints());
                DestroyObject(gameObject, deathVanishSeconds);
            }

            //player death sound
            audioSource.clip = DeathSounds[UnityEngine.Random.Range(0, DeathSounds.Length)];
            audioSource.Play();


        }
    }
}
