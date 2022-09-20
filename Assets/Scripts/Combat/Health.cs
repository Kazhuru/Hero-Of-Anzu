using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        private Animator animator;
        private CapsuleCollider capsuleCollider;
        private NavMeshAgent navMeshAgent;
        private bool isDead;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            isDead = false;
        }

        public void TakeDamage(float damage)
        {
            if (!isDead)
            {
                health = Mathf.Max(health - damage, 0);
                if (health == 0)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            animator.SetTrigger("dead");
            isDead = true;
            capsuleCollider.enabled = false;
            navMeshAgent.enabled = false;
        }

        public bool GetIsDead() { return isDead; }
    }
}