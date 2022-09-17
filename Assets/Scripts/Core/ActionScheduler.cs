using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction currentAction;

        public void StartAction(IAction action)
        {
            if (action == currentAction) return;
            if (currentAction != null)
            {
                Debug.Log("Scheduler: action canceled " + currentAction);
                currentAction.Cancel();
            }
            currentAction = action;
        }
    }
}


