using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] string title;
        [SerializeField] int Level;
        [SerializeField] float exp;
        int needForNextLevel;
        [SerializeField] AudioClip HitSound;
        [SerializeField] AudioClip MissSound;
        [SerializeField] AnimationClip anim;
        [SerializeField] float energyCost;
        [SerializeField] float coolDown;
        [SerializeField] float attackTime;
        [SerializeField] float damage;
        [SerializeField] SkillDifficulty difficulty;
        enum SkillDifficulty { easy, normal, difficult }
        [SerializeField] WeaponType.attackArea area;
        [SerializeField] WeaponType.skillType skillType;
        float difficultyMod;
        bool isInCoolDown;


        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public float Exp
        {
            get { return exp; }
            set { exp = value; }
        }
        public int level
        {
            get { return Level; }
            set { Level = value; }
        }

        void calculateNeedForNextLevel()
        {
            float mathfornextlevel = Level * difficultyMod * 10;
            needForNextLevel = (int)mathfornextlevel;
        }

        public void AddExp(float expToAdd)
        {
            exp += expToAdd;
            if (exp >= needForNextLevel)
            {
                Level++;
                exp = exp - needForNextLevel;
                calculateNeedForNextLevel();
            }
        }

        public float Damage()
        {
            return damage;
        }

        public WeaponType.attackArea AttackArea()
        {
            return area;
        }

        public AnimationClip GetAnimationClip()
        {
            return anim;
        }

        public float AttackTime()
        {
            return attackTime;
        }

        public bool IsInCoolDown()
        {
            return isInCoolDown;
        }

        public WeaponType.skillType SkillType()
        {
            return skillType;
        }

        // Use this for initialization
        void Start()
        {
            switch(difficulty)
            {
                case SkillDifficulty.easy:
                    difficultyMod = 1;
                    break;
                case SkillDifficulty.normal:
                    difficultyMod = 1;
                    break;
                case SkillDifficulty.difficult:
                    difficultyMod = 1;
                    break;
                default:
                    difficultyMod = 1;
                    break;
            }
            isInCoolDown = false;
            calculateNeedForNextLevel();
        }

        IEnumerator CoolDown()
        {
            while (isInCoolDown == true)
            {
                yield return new WaitForSeconds(coolDown);
                isInCoolDown = false;
            }
        }

        private void PlayAbilitySound(AudioClip audio)
        {
            var abilitySound = audio;
            var audioSource = GetComponent<AudioSource>();

            audioSource.PlayOneShot(abilitySound);
        }

        public void Hit(GameObject target, float spAmount)
        {
            spAmount = spAmount / 100f;
            print(spAmount);
            //PlayAbilitySound(HitSound);
            AddExp(spAmount);
            isInCoolDown = true;
            StartCoroutine(CoolDown());
        }
        
        public void Miss(GameObject target)
        {
           // PlayAbilitySound(MissSound);
            isInCoolDown = true;
            StartCoroutine(CoolDown());
        }

        public float GetRankBonus()
        {
            switch (difficulty)
            {
                case SkillDifficulty.easy:
                    return Level * 2;
                case SkillDifficulty.normal:
                    return Level * 2;
                case SkillDifficulty.difficult:
                    return Level * 2;
                default:
                    return 0;
            }
        }
    }
}
