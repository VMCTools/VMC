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
        public static void ReSetX(this Vector3 vector3)
        {
            vector3.x = 0;
        }
        public static Vector3 SetX(this Vector3 vector3, float newX)
        {
            vector3.x = newX;
            return vector3;
        }
        public static void ReSetY(this Vector3 vector3)
        {
            vector3.y = 0;
        }
        public static Vector3 SetY(this Vector3 vector3, float newY)
        {
            vector3.y = newY;
            return vector3;
        }
        public static void ReSetZ(this Vector3 vector3)
        {
            vector3.z = 0;
        }
        public static Vector3 SetZ(this Vector3 vector3, float newZ)
        {
            vector3.z = newZ;
            return vector3;
        }

        public static void ReSetXY(this Vector3 vector3)
        {
            vector3.x = 0;
            vector3.y = 0;
        }
        public static void ReSetXZ(this Vector3 vector3)
        {
            vector3.x = 0;
            vector3.z = 0;
        }
        public static void ReSetYZ(this Vector3 vector3)
        {
            vector3.y = 0;
            vector3.z = 0;
        }
        /// <summary>
        /// Reset vector3 to Vector3.zero
        /// </summary>
        public static void ReSet(this Vector3 vector3)
        {
            vector3.x = 0;
            vector3.y = 0;
            vector3.z = 0;
        }
    }
}