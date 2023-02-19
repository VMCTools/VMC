using System;

namespace VMC.Ultilities
{
    public static class FloatExtensions
    {
        public static string ShowTimeDHMS(this float time)
        {
            return TimeSpan.FromSeconds(time).ToString(@"dd\.hh\:mm\:ss");
        }
        public static string ShowTimeHMS(this float time)
        {
            return TimeSpan.FromSeconds(time).ToString(@"\ hh\:mm\:ss");
        }
        public static string ShowTimeMS(this float time)
        {
            return TimeSpan.FromSeconds(time).ToString(@"mm\:ss");
        }
    }
}