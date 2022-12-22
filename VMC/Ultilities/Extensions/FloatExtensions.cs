namespace VMC.Ultilities.Extension
{
    public static class FloatExtensions
    {
        public static string ToStringSS(this float t)
        {
            return t.ToString("00");
        }
        public static string ToStringMMSS(this float t)
        {
            int minute = (int)(t / 60);
            int second = (int)(t - minute * 60);
            return $"{minute.ToString("00")}:{second.ToString("00")}";
        }

        public static string ToStringHHMMSS(this float t)
        {
            int hour = (int)(t / 3600);
            float remain = t - hour * 3600;
            int minute = (int)(remain / 60);
            int second = (int)(remain - minute * 60);
            return $"{hour.ToString("00")}:{minute.ToString("00")}:{second.ToString("00")}";
        }
    }
}