using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


namespace RPG.Character
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig;

        GameObject target;
        GameObject weaponObject;
        Animator animator;
        Character character;
        Skills skills;
        int rollGap = 0;

        Stats stats;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        enum State { Attacking, Idling }
        State state = State.Idling;
        enum Stance { Defensive, Wary, Protective, Balanced, Threatening, Aggressive, Berserk }
        Stance stance = Stance.Balanced;
        float stanceBonus = 0;

        // Use this for initialization
        void Start()
        {
            character = GetComponent<Character>();
            animator = GetComponent<Animator>();
            skills = GetComponent<Skills>();
            stats = GetComponent<Stats>();
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
        }

        // Update is called once per frame
        void Update()
        {
            // todo check continuously if we should still be attacking
            bool targetIsDead;
            bool targetIsOutOfRange;
            
            if (target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {
                // test if target is dead
                var targethealth = target.GetComponent<HealthSystem>().healthAsPercentage;
                targetIsDead = targethealth <= Mathf.Epsilon;

                // test if target is out of range
                var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                targetIsOutOfRange = distanceToTarget > currentWeaponConfig.GetMaxAttackRange();
             }
            
            float characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = (characterHealth <= Mathf.Epsilon);
            
            if (characterIsDead || targetIsOutOfRange || targetIsDead)
            {
                state = State.Idling;
                StopAllCoroutines();
            }

        }

        public float GetStanceBonus()
        {
            return stanceBonus;
        }

        public void ChangeStanceUp()
        {
            switch (stance)
            {
                case Stance.Berserk:
                    print("Can't shift stance up any higher.");
                    break;
                case Stance.Aggressive:
                    print("You are now in a Berserk stance.");
                    stanceBonus = .75f;
                    stance = Stance.Berserk;
                    break;
                case Stance.Threatening:
                    print("You are now in a Aggressive stance.");
                    stanceBonus = .50f;
                    stance = Stance.Aggressive;
                    break;
                case Stance.Balanced:
                    print("You are now in a Threatening stance.");
                    stanceBonus = .25f;
                    stance = Stance.Threatening;
                    break;
                case Stance.Protective:
                    print("You are now in a Balanced stance.");
                    stanceBonus = 0f;
                    stance = Stance.Balanced;
                    break;
                case Stance.Wary:
                    print("You are now in a Protective stance.");
                    stanceBonus = -.25f;
                    stance = Stance.Protective;
                    break;
                case Stance.Defensive:
                    print("You are now in a Wary stance.");
                    stanceBonus = -.50f;
                    stance = Stance.Wary;
                    break;
            }
        }
        public void ChangeStanceDown()
        {
            switch (stance)
            {
                case Stance.Berserk:
                    print("You are now in a Aggressive stance.");
                    stanceBonus = .50f;
                    stance = Stance.Aggressive;
                    break;
                case Stance.Aggressive:
                    print("You are now in a Threatening stance.");
                    stanceBonus = .25f;
                    stance = Stance.Threatening;
                    break;
                case Stance.Threatening:
                    print("You are now in a Balanced stance.");
                    stanceBonus = 0.0f;
                    stance = Stance.Balanced;
                    break;
                case Stance.Balanced:
                    print("You are now in a Protective stance.");
                    stanceBonus = -.25f;
                    stance = Stance.Protective;
                    break;
                case Stance.Protective:
                    print("You are now in a Wary stance.");
                    stanceBonus = -.50f;
                    stance = Stance.Wary;
                    break;
                case Stance.Wary:
                    print("You are now in a Defensive stance.");
                    stanceBonus = -.75f;
                    stance = Stance.Defensive;
                    break;
                case Stance.Defensive:
                    print("You can't change your stance any lower.");
                    break;
            }
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            if (state != State.Attacking)
            {
                StartCoroutine(AttackTargetRepeatedly());
            }
        }

        IEnumerator AttackTargetRepeatedly()
        {
            //determine if alive( attacker and defender)
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;

            while(attackerStillAlive && targetStillAlive)
            {
                state = State.Attacking;
                transform.LookAt(target.transform);
                SkillConfig skill = skills.GetRandomSkill(currentWeaponConfig.WeaponType);
                if (skill != null)
                {
                    Animator animator = GetComponent<Animator>();
                    SetAttackAnimation(skill.GetAnimationClip());
                    animator.SetTrigger(ATTACK_TRIGGER);
                    if (CanAttack(target, skill))
                    {
                        float gapSp = rollGap / 100;
                        skill.Success(rollGap, target);
                        print(CalculateDamage(target, skill));
                        target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage(target, skill));
                    }
                    else
                    {
                        skill.Miss(target);
                    }
                    if (this.tag == "Player")
                    {
                        skill.Save();
                    }
                    yield return new WaitForSeconds(currentWeaponConfig.GetMinTimeBetweenHits() + skill.AttackTime);
                }
                else
                {
                    yield return new WaitForSeconds(.2f);
                }
            }
            
        }

        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            currentWeaponConfig = weaponToUse;

            var weaponPrefab = weaponToUse.GetWeaponPrefab();

            GameObject dominantHand = RequestDominantHand();

            Destroy(weaponObject); //empties hands
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);

            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        public bool CanAttack(GameObject target, SkillConfig skill)
        {
            bool canAttack = false;
            int success = 0;
            Stats targetStats = target.GetComponent<Stats>();
            int targetSuccess = targetStats.DefenseSuccess() + (int)target.GetComponent<Skills>().GetDefensiveSkillRankBonus(target.GetComponent<WeaponSystem>().currentWeaponConfig.WeaponType, skill.AttackArea);
            targetSuccess += (int) ( - target.GetComponent<WeaponSystem>().GetStanceBonus() * (targetSuccess));
            success += targetSuccess;
            success += 50;
            int mySuccess = stats.OffenseSuccess() + (int)(skill.GetRankBonus());
            mySuccess += (int)(GetStanceBonus() * mySuccess);
            success -= mySuccess;
            success = (int) Mathf.Clamp(success, 5f, 95f);
            int roll = Random.Range(0, 100);
            rollGap = roll - success;
            Debug.Log("[Roll:"+roll + "/ Success:" + success+"]");
            if (roll > success)
            {
                canAttack = true;
            }

            return canAttack;
        }

        private float CalculateDamage(GameObject target, SkillConfig skill)
        {
            int startDamage =(int) (currentWeaponConfig.GetAdditionalDamage() + skill.AttackDamage + stats.GetDamage() - target.GetComponent<Stats>().GetDamageResistance());
            float gapDamage = rollGap/100;
            gapDamage = startDamage * gapDamage;
            startDamage = startDamage + (int)gapDamage;
            startDamage = Mathf.Clamp(startDamage, 1, startDamage);
            return startDamage;
            /*
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
            float damageBeforeCritical = baseDamage + currentWeaponConfig.GetAdditionalDamage();
            if (isCriticalHit)
            {
                criticalHitParticle.Play();
                return damageBeforeCritical * criticalHitMultiplyer;
            }
            else
            {
                return damageBeforeCritical;
            }
            */
        }
        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand Found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand Scripts on Player, please remove one");
            return dominantHands[0].gameObject;

        }

        private void SetAttackAnimation(AnimationClip clip)
        {
            if(!character.GetOverrideController())
            {
                Debug.Break();
                Debug.LogAssertion("Please provide" + gameObject + " with an animator override controller");
            }
            var animatorOverrideController = character.GetOverrideController();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = clip;
        }

        private void SetAttackAnimation()
        {
            Animator animator = GetComponent<Animator>();
            var animatorOverrideController = character.GetOverrideController();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();


        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= currentWeaponConfig.GetMaxAttackRange();
        }
    }
}
