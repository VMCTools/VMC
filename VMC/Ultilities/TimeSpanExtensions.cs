using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ultilities
{
    public static class TimeSpanExtensions
    {
        public static string ShowTimeDHMS(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Days.ToString("00")}:{timeSpan.Hours.ToString("00")}:{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";
        }
        public static string ShowTimeHMS(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours.ToString("00")}:{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";
        }
        public static string ShowTimeMS(this TimeSpan timeSpan)
        {
            return $"{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";
        }
    }
}