using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private Fighter fighter;
        private Health health;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead()) { return; }
            if (CombatInteractions()) { return; }
            if (MovementInteractions()) { return; }

            Debug.Log("Nothing to do..");
        }

        private bool CombatInteractions()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            bool validInteraction = false;
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.TryGetComponent<CombatTarget>(out var combatTarget))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        fighter.Attack(combatTarget.gameObject);
                    }
                    validInteraction = true;
                    break;
                }
            }
            return validInteraction;
        }

        private bool MovementInteractions()
        {
            bool validInteraction = false;
            if (Physics.Raycast(GetMouseRay(), out RaycastHit raycastHit))
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(raycastHit.point);
                }
                validInteraction = true;
            }
            return validInteraction;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public Fighter GetPlayerFighter() { return fighter; }
    }
}
