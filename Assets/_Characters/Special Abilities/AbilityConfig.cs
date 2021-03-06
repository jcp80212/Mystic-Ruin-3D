﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Character
{

    public abstract class AbilityConfig :  ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AudioClip [] audioClips = null;


        protected AbilityBehaviour behaviour;

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject gameObjectToAttachTo)
        {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(gameObjectToAttachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void Use(GameObject target)
        {
            behaviour.Use(target);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AudioClip GetRandomAudioClip()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}
