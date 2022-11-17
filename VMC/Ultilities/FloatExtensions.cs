using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ultilities
{
    public static class FloatExtensions
    {
        //public static string ShowTimeDHMS(this float time)
        //{
        //    return $"{timeSpan.Days.ToString("00")}:{timeSpan.Hours.ToString("00")}:{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";
        //}
        //public static string ShowTimeHMS(this float time)
        //{
        //    return $"{timeSpan.Hours.ToString("00")}:{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";
        //}
        public static string ShowTimeMS(this float time)
        {
            return $"{((int)time / 60).ToString("00")}:{((int)time % 60).ToString("00")}";
        }
    }
}