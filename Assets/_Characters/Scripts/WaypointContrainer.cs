using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class WaypointContrainer : MonoBehaviour
    {


        private void OnDrawGizmos()
        {
            Vector3 firstPosition = transform.GetChild(0).position;
            Vector3 previousPosition = firstPosition;
            foreach(Transform waypoint in transform)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(waypoint.position, .2f);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, firstPosition);
        }
    }
}
