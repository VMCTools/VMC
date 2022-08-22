using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VMC.Ultilities
{
    public static class ListExtensions
    {
        public static string ToString<T>(this List<T> list)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            for (int i = 0; i < list.Count; i++)
            {
                builder.Append(list[i].ToString());
                if (i != list.Count - 1) builder.Append(" , ");
            }
            builder.Append("}");
            return builder.ToString();
        }
        public static List<T> Clone<T>(this List<T> original) where T : Component
        {
            List<T> temp = new List<T>();
            foreach (var line in original)
            {
                if (line.gameObject.activeSelf)
                {
                    T item = UnityEngine.Object.Instantiate<T>(line);
                    temp.Add(item);
                }
            }
            return temp;
        }
        public static float TotalLength(this List<Vector3> list)
        {
            float pathDistance = 0;
            for (int i = 0; i < list.Count - 1; i++)
            {
                pathDistance += Vector3.Distance(list[i], list[i + 1]);
            }
            return pathDistance;
        }
    }
}