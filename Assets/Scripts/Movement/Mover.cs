using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent navmesh;
        private Animator animator;
        private ActionScheduler scheduler;

        private void Awake()
        {
            navmesh = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            scheduler = GetComponent<ActionScheduler>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 position)
        {
            scheduler.StartAction(this);
            MoveTo(position);
        }

        public void MoveTo(Vector3 position)
        {
            navmesh.destination = position;
            navmesh.isStopped = false;
        }

        public void Cancel()
        {
            if (navmesh.enabled) { navmesh.isStopped = true; }
        }

        private void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(navmesh.velocity);
            float speed = Math.Abs(localVelocity.z);
            animator.SetFloat("fowardSpeed", speed);
        }
    }
}
