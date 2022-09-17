using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float attackInterval;
        [SerializeField] private float weaponDamage = 20;

        private Transform target;
        private Animator playerAnim;
        private Mover mover;
        private ActionScheduler scheduler;

        private bool isOnAttackRoutine;
        private Coroutine attackRoutine;


        private void Awake()
        {
            playerAnim = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            isOnAttackRoutine = false;
        }

        private void Update()
        {
            if (target == null) return;

            if (!GetIsInRange())
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }

        }

        private void AttackBehaviour()
        {
            if (isOnAttackRoutine) return;
            isOnAttackRoutine = true;
            attackRoutine = StartCoroutine(AttacksIntervalRoutine());
        }

        private IEnumerator AttacksIntervalRoutine()
        {
            while (isOnAttackRoutine)
            {
                playerAnim.SetTrigger("attack");
                yield return new WaitForSeconds(attackInterval);
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            scheduler.StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
            isOnAttackRoutine = false;
            StopCoroutine(attackRoutine);
        }

        //Animation Event
        public void Hit()
        {
            if (target != null)
            {
                Health targetHealth = target.GetComponent<Health>();
                if (targetHealth != null) targetHealth.TakeDamage(weaponDamage);
            }
        }
    }
}


