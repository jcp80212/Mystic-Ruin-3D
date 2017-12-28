using System.Collections;
using UnityEngine;
using RPG.CameraUI; // TODO consider re-wiring
using System.IO;
using System.Collections.Generic;

namespace RPG.Character
{
    public class PlayerControl : MonoBehaviour
    {
       // SpecialAbilities abilities;
        WeaponSystem weaponSystem;
        Character character;

        private void Start()
        {
            RegisterForMouseEvents();
            character = GetComponent<Character>();
           // abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
            //Load();
            StartCoroutine(SaveRoutine());
        }

        private void Update()
        {
             // ScanForAbilityKeyDown();
        }
        /*
        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex ++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }
        */
        IEnumerator SaveRoutine()
        {
            yield return new WaitForSeconds(60);
            while (true)
            {
                UpdateCharacter();
                print("Saving PlayerController");
                yield return new WaitForSeconds(60);
            }
        }
        
        private void RegisterForMouseEvents()
        {
            var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0) && GameManager.manager.SelectedCharacter == this.gameObject)
            {
                character.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject) && GameManager.manager.SelectedCharacter == this.gameObject)
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(0) && !IsTargetInRange(enemy.gameObject) && GameManager.manager.SelectedCharacter == this.gameObject)
            {
                StartCoroutine(MoveAndAttack(enemy));
            }
        }

        IEnumerator MoveToTarget(GameObject target)
        {
            character.SetDestination(target.transform.position);
            while(!IsTargetInRange(target.gameObject))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            weaponSystem.AttackTarget(enemy.gameObject);
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }

        public void UpdateCharacter()
        {
            var stats = GetComponent<Stats>();
            var healthsystem = GetComponent<HealthSystem>();
            var skillsystem = GetComponent<Skills>();
            SaveCharacter tmpCharacter = new SaveCharacter(stats._Id, stats.Name, stats.Level, stats.Exp);
            tmpCharacter.needForNextLevel = stats.NeedForNextLevel;
            tmpCharacter.currentFatiguePoints = skillsystem.CurrentFatiguePoints;
            tmpCharacter.maxFatiguePoints = skillsystem.MaxFatiguePoints;
            tmpCharacter.currentHealthPoints = healthsystem.CurrentHealthPoints;
            tmpCharacter.maxHealthPoints = healthsystem.MaxHealthPoints;
            WebRequest.webRequest.UpdateCharacterStats(tmpCharacter);
            
        }
    }

    public class SaveSkill
    {
        [SerializeField]string name;
        [SerializeField]float exp;
        [SerializeField]int level;

        public SaveSkill (string name, float exp, int level)
        {
            this.name = name;
            this.exp = exp;
            this.level = level;
        }
    }
}
