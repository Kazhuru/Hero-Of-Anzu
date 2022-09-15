using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent navmesh;
        private Animator playerAnim;

        void Start()
        {
            navmesh = GetComponent<NavMeshAgent>();
            playerAnim = GetComponent<Animator>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        public void MoveTo(Vector3 position)
        {
            navmesh.destination = position;
            navmesh.isStopped = false;
        }

        public void Stop()
        {
            navmesh.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(navmesh.velocity);
            float speed = Math.Abs(localVelocity.z);
            playerAnim.SetFloat("fowardSpeed", speed);
        }
    }
}
