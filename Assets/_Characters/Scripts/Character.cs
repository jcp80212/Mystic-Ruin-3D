using System;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using RPG.CameraUI;
using System.Collections;

namespace RPG.Character
{
    [RequireComponent(typeof(Stats))]
    public class Character : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] float audioSourceSpatialBlend = 0.5f;

        [Header("Animator")]
        [SerializeField]RuntimeAnimatorController animatorController;
        [SerializeField]AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        [Header("Movement")]
        [SerializeField] float moveSpeedMultiplier = .7f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;

        [Header("Capsule Collider")]
        [SerializeField]float capsuleRadius = 0.2f;
        [SerializeField] Vector3 capsuleCenter = new Vector3(0, 1.03f, 0);
        [SerializeField] float capsuleHeight = 2.03f;

        [Header("NavMeshAgent")]
        [SerializeField] float navMeshAgentSteeringSpeed = 1.0f;
        [SerializeField] float navMeshAgentStoppingDistance = 2f;
        [SerializeField] float navMeshObsticleAvoidanceRadius = 1.0f;

        NavMeshAgent navMeshAgent;
        Animator animator;
        Rigidbody myRigidbody;
        [Header("Required Components")]
        [SerializeField] HealthSystem myHealthSystem;
        [SerializeField] Stats myStats;
        [SerializeField] Skills mySkills;
        [SerializeField] WeaponSystem myWeaponSystem;

        float turnAround;
        float forwardAmount;
        bool isAlive = true;


        public bool IsAlive()
        {
            return isAlive;
        }

        void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;

            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = capsuleCenter;
            capsuleCollider.radius = capsuleRadius;
            capsuleCollider.height = capsuleHeight;

            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;


            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = navMeshAgentSteeringSpeed;
            navMeshAgent.stoppingDistance = navMeshAgentStoppingDistance;
            navMeshAgent.radius = navMeshObsticleAvoidanceRadius;
            navMeshAgent.autoBraking = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updatePosition = true;
        }

        private void Update()
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        public void Kill()
        {
            //to allow death signiling
            isAlive = false;
        }

        private void Start()
        {
            myHealthSystem = GetComponent<HealthSystem>();
            mySkills = GetComponent<Skills>();
            myStats = GetComponent<Stats>();
            myWeaponSystem = GetComponent<WeaponSystem>();
            if(this.tag == "Player")
            {
                //Load("Player");
               // StartCoroutine(PlayerSave());
            }
            
        }

        IEnumerator PlayerSave()
        {
            while(true)
            {
                Save("Player");
                print("Saving Player");
                yield return new WaitForSeconds(60);
            }
        }

        public float GetAnimSpeedMultiplier()
        {
            return animator.speed;
        }

        public void Save(string type)
        {
            string path = Application.dataPath + "/Resources/"+type+"/HealthSystem.json";
            string json = JsonUtility.ToJson(myHealthSystem);
            File.WriteAllText(path, json);
            path = Application.dataPath + "/Resources/" + type + "/Skills.json";
            json = JsonUtility.ToJson(mySkills);
            File.WriteAllText(path, json);
            path = Application.dataPath + "/Resources/" + type + "/Stats.json";
            json = JsonUtility.ToJson(myStats);
            File.WriteAllText(path, json);
            path = Application.dataPath + "/Resources/" + type + "/WeaponSystem.json";
            json = JsonUtility.ToJson(myWeaponSystem);
            File.WriteAllText(path, json);

        }

        public void Load(string type)
        {
            string path = Application.dataPath + "/Resources/"+type+"/HealthSystem.json";
            var json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, myHealthSystem);
            path = Application.dataPath + "/Resources/" + type + "/Skills.json";
            json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, mySkills);
            path = Application.dataPath + "/Resources/" + type + "/Stats.json";
            json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, myStats);
            path = Application.dataPath + "/Resources/" + type + "/WeaponSystem.json";
            json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, myWeaponSystem);
            myWeaponSystem.PutWeaponInHand(myWeaponSystem.GetCurrentWeapon());
        }

        private void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                velocity.y = myRigidbody.velocity.y;
                myRigidbody.velocity = velocity;
            }
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

        public void SetDestination(Vector3 worldPos)
        {
            navMeshAgent.destination = worldPos;
        }

        void Move(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            SetForwardAndTurn(movement);

            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        private void SetForwardAndTurn(Vector3 movement)
        {
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }
            var localmove = transform.InverseTransformDirection(movement);
            turnAround = Mathf.Atan2(localmove.x, localmove.z);
            forwardAmount = localmove.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAround, 0.1f, Time.deltaTime);

            animator.speed = animationSpeedMultiplier;


        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAround * turnSpeed * Time.deltaTime, 0);
        }
    }
}