using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject
    {

        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;
        [SerializeField] WeaponType.weaponType weaponType;

        public WeaponType.weaponType WeaponType { get { return weaponType; } }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public float GetMinTimeBetweenHits()
        {
            //TODO take animation time into account
            return minTimeBetweenHits;
        }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        //so that asset packs cannot cause crashes
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0]; 
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

    }
}
