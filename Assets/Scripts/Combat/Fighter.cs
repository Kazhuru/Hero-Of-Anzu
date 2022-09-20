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

        private Health target;
        private Animator animator;
        private Mover mover;
        private ActionScheduler scheduler;

        private bool isOnAttackRoutine;
        private Coroutine attackRoutine;


        private void Awake()
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            isOnAttackRoutine = false;
        }

        private void Update()
        {
            if (target == null || target.GetIsDead()) { return; }
            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position);
            }
            else
            {
                mover.Cancel();
                StartAttackBehaviour();
            }
        }

        private void StartAttackBehaviour()
        {
            if (isOnAttackRoutine) { return; }
            isOnAttackRoutine = true;
            transform.LookAt(target.transform);
            attackRoutine = StartCoroutine(AttacksRoutine());
        }

        private IEnumerator AttacksRoutine()
        {
            while (isOnAttackRoutine)
            {
                if (target != null && !target.GetIsDead())
                {
                    animator.ResetTrigger("stopAttack");
                    animator.SetTrigger("attack");
                }
                yield return new WaitForSeconds(attackInterval);
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            scheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            isOnAttackRoutine = false;
            StopCoroutine(attackRoutine);
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");

        }

        //Animation Event
        public void Hit()
        {
            if (target != null) { target.TakeDamage(weaponDamage); }
            if (target == null || target.GetIsDead()) { Cancel(); }
        }
    }
}


