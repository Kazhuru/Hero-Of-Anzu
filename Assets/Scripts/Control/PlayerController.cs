using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private Fighter fighter;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (CombatInteractions()) return;
            if (MovementInteractions()) return;

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
                        fighter.Attack(combatTarget);
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
                    mover.MoveTo(raycastHit.point);
                    fighter.ClearTarget();
                }
                validInteraction = true;
            }
            return validInteraction;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
