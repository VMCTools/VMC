using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ultilities
{
    public static class Vector3Extensions
    {
        public static float DistancePath(this Vector3[] path)
        {
            float pathDistance = 0;
            for (int i = 0; i < path.Length - 1; i++)
            {
                pathDistance += Vector3.Distance(path[i], path[i + 1]);
            }
            return pathDistance;
        }

        public static Vector3 SetZ(this Vector3 vector3, float newZ)
        {
            vector3.z = newZ;
            return vector3;
        }
    }
}