using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VMC.Ultilities
{
    public static class ArrayExtensions
    {
        public static string ToString<T>(this T[] arr)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            for (int i = 0; i < arr.Length; i++)
            {
                builder.Append(arr[i].ToString());
                if (i != arr.Length - 1) builder.Append(" , ");
            }
            builder.Append("]");
            return builder.ToString();
        }
        public static float TotalLength(this Vector3[] path)
        {
            float pathDistance = 0;
            for (int i = 0; i < path.Length - 1; i++)
            {
                pathDistance += Vector3.Distance(path[i], path[i + 1]);
            }
            return pathDistance;
        }

    }
}