using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RPG.Character
{
    public class Skills : MonoBehaviour
    {
        [SerializeField] SkillConfig[] skills;
        [SerializeField] SkillConfig[] offensiveSkills;
        [SerializeField] SkillConfig[] defensiveSkills;
        [SerializeField] Image fatigueBar;
        [SerializeField] float maxFatiguePoints = 100;
        [SerializeField] float RegenPointsPerSecond = 1;
        [SerializeField] AudioClip outOfEnergySound;

        [SerializeField]float currentFatiguePoints;
        [SerializeField] float skillPoints;

        List<SkillConfig> activeSkills; 

        float energyAsPercent { get { return currentFatiguePoints / maxFatiguePoints; } }
        public float CurrentFatiguePoints { get { return currentFatiguePoints; } }
        public float MaxFatiguePoints { get { return maxFatiguePoints; } }

        public int GetNumberOfSkills()
        {
            return skills.Length;
        }

        public SkillConfig[] GetSkills {  get { return skills; } set { skills = value; } }

        

        // Use this for initialization
        void Start()
        {
            currentFatiguePoints = maxFatiguePoints;
            AttachInitialSkills();
            //UpdateOffensiveSkills(GetComponent<WeaponSystem>().GetCurrentWeapon().WeaponType);
            UpdateFatigueBar();
            StartCoroutine(AddFatiguePoints());
        }

        void LoadSkillStats()
        {
            for (int i = 0; i < skills.Length; i++)
            {
                skills[i].Load();
            }
        }

        private void Update()
        {
            if (currentFatiguePoints < maxFatiguePoints)
            {
                AddFatiguePoints();
            }
        }

        IEnumerator AddFatiguePoints()
        {
            while (true)
            {
                maxFatiguePoints = GetComponent<Stats>().MaxFatigueAmount();
                if (currentFatiguePoints >= Mathf.Epsilon)
                {
                    var pointsToAdd = GetComponent<Stats>().FatigueRegenAmount();
                    currentFatiguePoints = Mathf.Clamp(currentFatiguePoints + pointsToAdd, 0, maxFatiguePoints);
                }
                yield return new WaitForSeconds(30f);
            }
        }

        public void ConsumeFatigue(float amount)
        {
            float newEnergyPoints = currentFatiguePoints - amount;
            currentFatiguePoints = Mathf.Clamp(newEnergyPoints, 0.0f, maxFatiguePoints);
            UpdateFatigueBar();
        }

        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentFatiguePoints;
        }

        private void UpdateFatigueBar()
        {
            if (fatigueBar != null)
            {
                float xValue = energyAsPercent;
                fatigueBar.fillAmount = xValue;
            }
        }

        public void AttachInitialSkills()
        {
            for (int i = 0; i < skills.Length; i++)
            {
                skills[i].AttachAbilityTo(gameObject);
                skills[i].SetupSkillStats();
                if(this.tag == "Player")
                {
                    Debug.Log(this.GetComponent<Stats>()._Id);
                    skills[i].SkillStats._Creator = this.GetComponent<Stats>()._Id;
                    skills[i].Load();
                }
            }
        }

        public float GetDefensiveSkillRankBonus(WeaponType.weaponType weaponType, WeaponType.attackArea attackArea)
        {
            float rankBonus = 0 ;
            for (int i = 0; i < skills.Length; i++)
            {
                if(skills[i].WeaponType == weaponType && skills[i].SkillType == WeaponType.skillType.defensive && skills[i].SkillStats.Level != 0)
                {
                    rankBonus += skills[i].GetRankBonus();
                }
            }
            return rankBonus;
        }

        public SkillConfig GetRandomSkill(WeaponType.weaponType weaponType)
        {
            UpdateOffensiveSkills(weaponType);
            if (activeSkills.Count != 0)
            {
                SkillConfig SkillToUse = activeSkills[UnityEngine.Random.Range(0, activeSkills.Count)];
                return SkillToUse;
            }
            else
            {
                return null;
            }
        }

        void UpdateOffensiveSkills(WeaponType.weaponType weaponType)
        {
            activeSkills = new List<SkillConfig>();
            for (int i = 0; i < skills.Length; i++)
            {
                if (!skills[i].IsInCoolDown() && skills[i].SkillType == WeaponType.skillType.offensive && weaponType == skills[i].WeaponType)
                {
                    activeSkills.Add(skills[i]);
                }
            }
        }
        /*
        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            var energyComponent = GetComponent<SpecialAbilities>();
            float energyCost = skills[0].GetEnergyCost();

            if (energyCost <= currentFatiguePoints)
            {
                energyComponent.ConsumeEnergy(energyCost);
                print("Using skill" + abilityIndex);
                // var abilitParams = new AbilityUseParams(enemy, baseDamage);
                skills[abilityIndex].Use(target);
            }
            else
            {
                audioSource.PlayOneShot(outOfEnergySound);
            }
        }*/

        public void AddSkillPoints(float skillpointToAdd)
        {
            skillPoints += skillpointToAdd;
        }

        public float Skillpoints()
        {
            return skillPoints;
        }
    }
}

