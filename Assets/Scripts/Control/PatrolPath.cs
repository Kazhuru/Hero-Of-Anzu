using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private List<Transform> waypoints;
        [SerializeField] Color gizmoColor = Color.white;

        private const float waypointSphereRaidus = 0.3f;

        private void Start()
        {
            if (waypoints == null || waypoints.Count == 0)
            {
                waypoints = new List<Transform>();
                foreach (Transform child in transform)
                {
                    waypoints.Add(child);
                }
            }
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawSphere(
                    waypoints[i].position, waypointSphereRaidus);
                Gizmos.DrawLine(
                    waypoints[i].position, waypoints[GetNextWaypointIndex(i)].position);
            }
        }

        public Vector3 GetWaypointPosition(int index)
        {
            return waypoints[index].position;
        }

        public int GetNextWaypointIndex(int index)
        {
            if (index + 1 < waypoints.Count)
            {
                return index + 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
