using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using RPG.Core;

namespace RPG.Character
{

    public abstract class SkillConfig : ScriptableObject
    {
        [Header("Skill General")]
        [SerializeField] float energyCost;
        [SerializeField] float coolDown;
        [SerializeField] float attackTime;
        [SerializeField] float attackDamage;
        [SerializeField] SkillDifficulty difficulty;
        enum SkillDifficulty { easy, normal, difficult }
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AudioClip[] audioClips = null;
        [SerializeField] AnimationClip animation;
        [SerializeField] WeaponType.weaponType type;
        [SerializeField] WeaponType.attackArea area;
        [SerializeField] WeaponType.skillType skillType;


        protected SkillStats skillStats;

        protected SkillBehaviour behaviour;

        public abstract SkillBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

        public float CoolDown { get { return coolDown; } }

        public float AttackTime { get { return attackTime; } }

        public WeaponType.skillType SkillType { get { return skillType; } }

        public WeaponType.attackArea AttackArea { get { return area; } }

        public WeaponType.weaponType WeaponType { get { return type; } }

        public float AttackDamage {  get { return attackDamage; } }

        public SkillStats SkillStats {  get { return skillStats; } set { skillStats = value; } }

        public bool IsInCoolDown()
        {
            if (!behaviour)
            {
                return false;
            }
            else
            {
                return behaviour.IsInCoolDown();
            }
        }

        public void AttachAbilityTo(GameObject gameObjectToAttachTo)
        {
            SkillBehaviour behaviourComponent = GetBehaviourComponent(gameObjectToAttachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void AddExp(float expToAdd)
        {
            skillStats.AddExp(expToAdd);
        }

        public void SetupSkillStats()
        {
            switch (difficulty)
            {
                case SkillDifficulty.easy:
                    //Debug.Log("Level:" + skillStats.Level);
                    skillStats = new SkillStats(0, 0, 1);
                    return;
                case SkillDifficulty.normal:
                    skillStats = new SkillStats(0, 0, 2.5f);
                    return;
                case SkillDifficulty.difficult:
                    skillStats = new SkillStats(0, 0, 5f);
                    return;
                default:
                    return;
            }
        }

        public void Hit(GameObject target)
        {
            behaviour.Hit(target);
        }

        public void Success(int rollGap, GameObject target)
        {
            behaviour.Success(rollGap, target);
        }

        public void Miss(GameObject target)
        {
            behaviour.Miss(target);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AnimationClip GetAnimationClip()
        {
            return animation;
        }

        public AudioClip GetRandomAudioClip()
        {
            if (audioClips.Length > 1)
            {
                return audioClips[Random.Range(0, audioClips.Length)];
            }
            else
            {
                return audioClips[0];
            }
        }

        public float GetRankBonus()
        {
            switch (difficulty)
            {
                case SkillDifficulty.easy:
                    return skillStats.Level * 2;
                default:
                    return 0;
            }
        }

        public void Save()
        {
            string path = Application.persistentDataPath + "/skill." + this.name + "." + skillStats._Creator + ".json";
            Debug.Log(path);
            var jsonString = JsonUtility.ToJson(skillStats);
            File.WriteAllText(path, jsonString);
        }

        public void Load()
        {
            string path = Application.persistentDataPath + "/skill." + this.name +"."+ skillStats._Creator +".json";
            Debug.Log(path);
            try
            {
                var json = File.ReadAllText(path);
                var stats = new SkillStats(0, 0, 0);
                JsonUtility.FromJsonOverwrite(json, stats);
                skillStats = stats;
                //Debug.Log("Current EXP for skill "+skillStats.CurrentExp);
            }
            catch (System.IO.IOException)
            {
                Debug.Log("Skill isn't saved, saving the skill now");
                Save();
            }
        }
        
    }
}
