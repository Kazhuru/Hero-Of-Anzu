using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEditor;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseRange = 5f;
        [SerializeField] private float suspiciousTime = 2f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float runningSpeed = 4f;
        [SerializeField] private float patrolDwellingTime = 2f;

        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private ActionScheduler scheduler;
        private NavMeshAgent navMeshAgent;

        private Vector3 returnPosition;
        private Quaternion returnRotation;
        private float timeLastSawPlayer;
        private int currentWaypointIndex;
        private float timeDwelling = 0f;

        private const float waypointCloseTolerance = 0.5f;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            RefreshReturnPosition();
            timeLastSawPlayer = Mathf.Infinity;
            currentWaypointIndex = 0;
        }

        private void Update()
        {
            if (health.IsDead()) { return; }

            if (PlayerInRange())
            {
                timeLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if (timeLastSawPlayer < suspiciousTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            timeLastSawPlayer += Time.deltaTime;
        }
        

        private void SuspicionBehaviour()
        {
            scheduler.CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            navMeshAgent.speed = patrolSpeed;
            Vector3 nextPosition = returnPosition;
            if(patrolPath != null)
            {
                if (IsPositionAtWaypoint())
                {
                    ReachingWaypointActions();
                }
                nextPosition = GetCurrentWaypoint();
            }
            else
            {
                RotateToReturnPosition(nextPosition);
            }
            mover.StartMoveAction(nextPosition);  
        }

        private void RotateToReturnPosition(Vector3 nextPosition)
        {
            if (CloseEnoughPositions(transform.position, nextPosition, waypointCloseTolerance))
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, returnRotation, Time.deltaTime);
            }
        }

        private void ReachingWaypointActions()
        {
            if (timeDwelling > patrolDwellingTime)
            {
                CycleCurrentWaypoint();
                timeDwelling = 0f;
            }
            timeDwelling += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            navMeshAgent.speed = runningSpeed;
            fighter.Attack(player.gameObject);
        }

        private bool PlayerInRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseRange;
        }

        private bool IsPositionAtWaypoint()
        {
            Vector3 patrolPos = patrolPath.GetWaypointPosition(currentWaypointIndex);
            return CloseEnoughPositions(patrolPos, transform.position, waypointCloseTolerance);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypointPosition(currentWaypointIndex);
        }

        private void CycleCurrentWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextWaypointIndex(currentWaypointIndex);
        }

        private void RefreshReturnPosition()
        {
            returnPosition = transform.position;
            returnRotation = transform.rotation;
        }

        private static bool CloseEnoughPositions(Vector3 position1, Vector3 position2, float tolerance)
        {
            return (Math.Abs(position1.x - position2.x) <= tolerance) 
                && (Math.Abs(position1.y - position2.y) <= tolerance) 
                && (Math.Abs(position1.z - position2.z) <= tolerance);
        }

        #region Gizmos
#if UNITY_EDITOR
        //Gizmo's functions called on Unity Editor

        private void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying)
            {
                if (health.IsDead()) { return; }
                if (PlayerInRange())
                {
                    Gizmos.DrawLine(transform.position, player.transform.position);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
#endif
        #endregion
    }
}

