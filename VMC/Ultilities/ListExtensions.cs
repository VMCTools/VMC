using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ultilities
{
    public static class ListExtensions
    {
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
    }
}