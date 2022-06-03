using Newtonsoft.Json;
using UnityEngine;

namespace VMC.Ultilities
{
    public class PlayerPrefsHelper
    {
        public static bool Get(string key, bool defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return bool.Parse(PlayerPrefs.GetString(key));
            }
            else
            {
                PlayerPrefs.SetString(key, defaultValue.ToString());
                return defaultValue;
            }
        }
        public static void Set(string key, bool value)
        {
            PlayerPrefs.SetString(key, value.ToString());
        }

        public static int Get(string key, int defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            else
            {
                PlayerPrefs.SetInt(key, defaultValue);
                return defaultValue;
            }
        }
        public static void Set(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public static string Get(string key, string defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetString(key);
            }
            else
            {
                PlayerPrefs.SetString(key, defaultValue.ToString());
                return defaultValue;
            }
        }
        public static void Set(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
        public static T Get<T>(string key)
        {
            string data = Get(key, string.Empty);
            Debug.Log($"Get: {key} - {data}");
            return JsonConvert.DeserializeObject<T>(data);
        }
        public static void Set<T>(string key, T obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            Debug.Log($"Save: {key} - {data}");
            Set(key, data);
        }
    }
}
