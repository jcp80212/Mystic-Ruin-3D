using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Character
{
    public class SpecialAbilities : MonoBehaviour
    {

        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPoints = 100;
        [SerializeField] float RegenPointsPerSecond = 1;
        [SerializeField] AudioClip outOfEnergySound;

        float currentEnergyPoints;
        AudioSource audioSource;
        
        float energyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
            audioSource = GetComponent<AudioSource>();
            AttachInitialAbilities();
            UpdateEnergyBar();
        }
        private void Update()
        {
            if(currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }

        private void AddEnergyPoints()
        {
            var pointsToAdd = RegenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0.0f, maxEnergyPoints);
            UpdateEnergyBar();
        }

        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
        }

        private void UpdateEnergyBar()
        {
            float xValue = energyAsPercent;
            energyBar.fillAmount = xValue;
        }

        private void AttachInitialAbilities()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                abilities[i].AttachAbilityTo(gameObject);
            }
        }


        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            var energyComponent = GetComponent<SpecialAbilities>();
            float energyCost = abilities[0].GetEnergyCost();

            if (energyCost <= currentEnergyPoints)
            {
                energyComponent.ConsumeEnergy(energyCost);
                print("Using special ability" + abilityIndex);
               // var abilitParams = new AbilityUseParams(enemy, baseDamage);
               abilities[abilityIndex].Use(target);
            }
            else
            {
                audioSource.PlayOneShot(outOfEnergySound);
            }
        }
    }
}
