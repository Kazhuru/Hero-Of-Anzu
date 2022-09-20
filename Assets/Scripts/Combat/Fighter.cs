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
            if (!TargetIsValid()) { return; }
            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position);
                StopAttackRoutine();
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
            attackRoutine = StartCoroutine(AttacksRoutine());
        }

        private IEnumerator AttacksRoutine()
        {
            while (isOnAttackRoutine)
            {
                if (TargetIsValid())
                {
                    transform.LookAt(target.transform);
                    StartAttackAnimation();
                }
                yield return new WaitForSeconds(attackInterval);
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            scheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            StopAttackRoutine();
        }

        private void StopAttackRoutine()
        {
            StopAttackAnimation();
            if (attackRoutine != null) { StopCoroutine(attackRoutine); }
            isOnAttackRoutine = false;
        }

        private void StopAttackAnimation()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        private void StartAttackAnimation()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }

        public bool TargetIsValid()
        {
            return target != null && !target.IsDead();
        }

        //Animation Event
        public void Hit()
        {
            if (TargetIsValid()) { target.TakeDamage(weaponDamage); }
            if (target != null && target.IsDead()) { Cancel(); }
        }
    }
}
