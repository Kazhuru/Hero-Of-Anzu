using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEditor;

namespace RPG.AI
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseRange = 5f;
        [SerializeField] private float suspiciousTime = 2f;

        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private ActionScheduler scheduler;

        private Vector3 returnPosition;
        private Quaternion returnRotation;
        private float timeLastSawPlayer;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            RefreshReturnPosition();
            timeLastSawPlayer = Mathf.Infinity;
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
                GuardBehaviour();
            }
            timeLastSawPlayer += Time.deltaTime;
        }
        

        private void SuspicionBehaviour()
        {
            scheduler.CancelCurrentAction();
        }

        private void GuardBehaviour()
        {
            if (transform.position.z != returnPosition.z)
            {
                mover.StartMoveAction(returnPosition);
            }
            if (transform.position.z == returnPosition.z)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, returnRotation, Time.deltaTime);
            }
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player.gameObject);
        }

        private bool PlayerInRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseRange;
        }

        private void RefreshReturnPosition()
        {
            returnPosition = transform.position;
            returnRotation = transform.rotation;
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

