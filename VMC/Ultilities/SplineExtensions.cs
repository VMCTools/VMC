using UnityEngine;
using Debug = VMC.Debugger.Debug;

namespace VMC.Ultilities
{
    public static class SplineExtensions
    {
        public static Vector3 GetPoint(this Vector3[] arrs, float t)
        {
            if (arrs == null)
            {
                Debug.LogWarning("Array is null!!!");
                return Vector3.zero;
            }
            int length = arrs.Length;
            switch (length)
            {
                case 0: return Vector3.zero;
                case 1: return arrs[0];
                case 2: return (1 - t) * arrs[0] + t * arrs[1];
                case 3: return Mathf.Pow(1 - t, 2) * arrs[0] + 2 * (1 - t) * t * arrs[1] + t * t * arrs[2];
                case 4: return Mathf.Pow(1 - t, 3) * arrs[0] + 3 * Mathf.Pow(1 - t, 2) * t * arrs[1] + 3 * (1 - t) * Mathf.Pow(t, 2) * arrs[2] + Mathf.Pow(t, 3) * arrs[3];
            }
            Debug.LogWarning("Array is too Large!!!");
            return Vector3.zero;
        }
    }
}