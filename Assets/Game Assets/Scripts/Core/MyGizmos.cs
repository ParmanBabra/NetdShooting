using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NetdShooting.Core
{
    public static class MyGizmos
    {
        public static void DrawFOV(Vector3 start, Vector3 direction, float fov, float range)
        {
            float arrowHeadLength = 0.25f;
            float arrowHeadAngle = 20.0f;

            Gizmos.DrawRay(start, direction.normalized * range);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(start + direction.normalized * range, right * arrowHeadLength);
            Gizmos.DrawRay(start + direction.normalized * range, left * arrowHeadLength);

            var leftRayRotation = Quaternion.AngleAxis(-fov / 2, Vector3.up);
            var rightRayRotation = Quaternion.AngleAxis(fov / 2, Vector3.up);

            Vector3 leftRayDirection = leftRayRotation * direction;
            Vector3 rightRayDirection = rightRayRotation * direction;
            Gizmos.DrawRay(start, leftRayDirection * range);
            Gizmos.DrawRay(start, rightRayDirection * range);
        }
    }
}
