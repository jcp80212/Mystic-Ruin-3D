using System.Collections;
using UnityEngine;

namespace RPG.Character
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float chaseRadius = 10f;
        [SerializeField] WaypointContrainer patrolPath;
        [SerializeField] float waypointTolerance = 2f;

        [SerializeField] SpawnPointContainer spawnPointContainer;

        enum State { idle, attacking, patrolling, chasing }
        State state = State.idle;

        float currentWeaponRange;
        float distanceToPlayer;
        Character character;
        Stats stats;
        WeaponSystem weaponSystem;
        
        int nextWaypointIndex;
        PlayerControl player = null;


        private void Start()
        {
            player = FindObjectOfType<PlayerControl>();
            character = GetComponent<Character>();
            stats = GetComponent<Stats>();
            weaponSystem = GetComponent<WeaponSystem>();
            GetComponent<HealthSystem>().SetHealthToMax();
            spawnPointContainer = GameObject.FindGameObjectWithTag("SpawnPointContainer").GetComponent<SpawnPointContainer>();
        }

        private PlayerControl updateClosestPlayer()
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("Player");
            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in gos)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
            if (closest != null)
                return closest.GetComponent<PlayerControl>();
            else
                return null;
        }

        private void Update()
        {
            player = updateClosestPlayer();
            if (player != null)
            {
                distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
                currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
                if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
                {
                    state = State.attacking;
                    StopAllCoroutines();
                    weaponSystem.AttackTarget(player.gameObject);
                    StartCoroutine(StanceChange());
                }
                if (distanceToPlayer > chaseRadius && state != State.patrolling)
                {
                    StopAllCoroutines();
                    StartCoroutine(Patrol());

                }
                if (distanceToPlayer > currentWeaponRange && state == State.attacking && state != State.chasing)
                {
                    //stop what we're doing
                    StopAllCoroutines();
                    //chase the player
                    StartCoroutine(ChasePlayer());
                }
                if (distanceToPlayer <= chaseRadius && state != State.chasing && state != State.attacking)
                {
                    //stop what we're doing
                    StopAllCoroutines();
                    //chase the player
                    StartCoroutine(ChasePlayer());
                }
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }

        }

        /*
        IEnumerator Patrol()
        {
            state = State.patrolling;

            while(patrolPath != null)
            {
                // work out where to go next
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                //tell character to go there
                character.SetDestination(nextWaypointPos);
                //cycle waypoints close
                CycleWaypointWhenClose(nextWaypointPos);
                //wait at a waypoint
                yield return new WaitForSeconds(.5f);
            }
        }*/
        IEnumerator StanceChange()
        {
            while(state == State.attacking)
            {
                bool changeStance = Random.Range(0f, 1f) <= .5f;
                if(changeStance)
                {
                    bool changeStanceUp = Random.Range(0f, 1f) <= .5f;
                    if (changeStanceUp)
                    {
                        weaponSystem.ChangeStanceUp();
                    }
                    else
                    {
                        weaponSystem.ChangeStanceDown();
                    }
                }
                yield return new WaitForSeconds(Random.Range(30, 200));
            }
        }

        IEnumerator Patrol()
        {
            state = State.patrolling;
            var spawnPoints = spawnPointContainer.spawnPoints();
            while(spawnPoints.Length != 0)
            {
                // work out where to go next
                Vector3 nextWaypointPos = spawnPoints[nextWaypointIndex].transform.position;
                //tell character to go there
                character.SetDestination(nextWaypointPos);
                //cycle waypoints close
                CycleWaypointWhenClose(nextWaypointPos);
                //wait at a waypoint
                yield return new WaitForSeconds(.5f);
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            var spawnPoints = spawnPointContainer.spawnPoints();
            if (Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                nextWaypointIndex = Random.Range(0, spawnPoints.Length);
            }
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while(distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDrawGizmos()
        {
            //draw attack sphere
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);


            //draw chase radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }
    }
}