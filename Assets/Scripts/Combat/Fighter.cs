using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        private Transform target;
        private Mover mover;

        [SerializeField] float weaponRange = 2f;

        private void Awake()
        {
            mover = FindObjectOfType<Mover>();
        }

        private void Update()
        {
            if (target != null)
            {
                mover.MoveTo(target.position);
                Debug.Log(Vector3.Distance(transform.position, target.position));
                bool isInRange = Vector3.Distance(transform.position, target.position) <= weaponRange;
                if(isInRange)
                    mover.Stop();
            }

        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }

        public void ClearTarget()
        {
            target = null;
        }
    }
}


