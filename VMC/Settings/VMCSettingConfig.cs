using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VMC.Settings
{
    [CreateAssetMenu(fileName = "VMC Settings", menuName = "VMC/Setting")]
    public class VMCSettingConfig : ScriptableObject
    {
        [Header("Ads Config")]
        public bool enableAds = false;
        public Ads.AdsType adType;
        public AdsLibrary adsLibrary;


#if UNITY_ANDROID
        public bool isTestMode = false;
        public string maxAppId = "YOUR_APP_ID_HERE";
        public string openAdsId_Tier1 = "YOUR_OPENADS_ID_ADS_HERE";
        public string openAdsId_Tier2 = "YOUR_OPENADS_ID_ADS_HERE";
        public string openAdsId_Tier3 = "YOUR_OPENADS_ID_ADS_HERE";

        public string bannerId = "YOUR_BANNER_ID_ADS_HERE";
        public Ads.BannerAdsPosition bannerPosition = Ads.BannerAdsPosition.BOTTOM;
        public string interstitialId = "YOUR_INTERS_ID_ADS_HERE";
        public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#elif UNITY_IOS
        public bool isTestMode = false;
        public string maxAppId = "YOUR_APP_ID_HERE";
        public string openAdsId_Tier1 = "YOUR_OPENADS_ID_ADS_HERE";
        public string openAdsId_Tier2 = "YOUR_OPENADS_ID_ADS_HERE";
        public string openAdsId_Tier3 = "YOUR_OPENADS_ID_ADS_HERE";
        public string bannerId="YOUR_BANNER_ID_ADS_HERE";
        public string interstitialId="YOUR_INTERS_ID_ADS_HERE";
        public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#endif
        [Header("Analytics Config")]
        public bool enableAnalyze = false;
        public AnalyzeLibrary analyzeLibrary;
        [Header("AppsFlyer Config")]
#if UNITY_ANDROID
        public string AF_Dev_Key = "AF_Dev_Key";
        public string AF_App_Id = string.Empty; // Android not Use
#elif UNITY_IOS
        public string AF_Dev_Key = "AF_Dev_Key";
        public string AF_App_Id = "AF_App_Id";
#else
        public string AF_Dev_Key = "AF_Dev_Key";
        public string AF_App_Id = "AF_App_Id";
#endif


        [Header("Debug Config")]
        public bool enableDebugLog = false;
        public DebugLogLevel debugLogLevel;

        [Header("Another Settings")]
        public bool isUsingDoTween = true;
        public bool isUsingIAP = false;
        public bool isUsingLocalNotification = false;

        public static VMCSettingConfig LoadData()
        {
            //string path = Path.Combine(Application.persistentDataPath, pathFile);
            //if (File.Exists(path))
            //{
            //    string json = File.ReadAllText(path);
            //    return JsonUtility.FromJson<VMCSettingConfig>(json);
            //}
            //return new VMCSettingConfig();
          return Resources.Load<VMCSettingConfig>("VMC Settings");
        }
        public void Save()
        {
            //string json = JsonUtility.ToJson(this);
            //File.WriteAllText(Path.Combine(Application.persistentDataPath, pathFile), json); 
        }
        public void Changes(ref HashSet<string> defines)
        {
            #region Ads check
            if (enableAds && isTestMode)
            {
                defines.Add(Define.VMC_ADS_TESTMODE.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ADS_TESTMODE.ToString());
            }

            if (enableAds && adsLibrary.HasFlag(AdsLibrary.Admob))
            {
                defines.Add(Define.VMC_ADS_ADMOB.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ADS_ADMOB.ToString());
            }
            if (enableAds && adsLibrary.HasFlag(AdsLibrary.MaxMediation))
            {
                defines.Add(Define.VMC_ADS_MAX.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ADS_MAX.ToString());
            }
            #endregion
            #region Analytics check
            if (enableAnalyze && analyzeLibrary.HasFlag(AnalyzeLibrary.Firebase))
            {
                defines.Add(Define.VMC_ANALYZE_FIREBASE.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ANALYZE_FIREBASE.ToString());
            }
            if (enableAnalyze && analyzeLibrary.HasFlag(AnalyzeLibrary.AppsFlyer))
            {
                defines.Add(Define.VMC_ANALYZE_APPFLYER.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ANALYZE_APPFLYER.ToString());
            }
            #endregion
            #region Debug log check
            if (enableDebugLog && debugLogLevel.HasFlag(DebugLogLevel.Debug))
            {
                defines.Add(Define.VMC_DEBUG_NORMAL.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_DEBUG_NORMAL.ToString());
            }
            if (enableDebugLog && debugLogLevel.HasFlag(DebugLogLevel.Warning))
            {
                defines.Add(Define.VMC_DEBUG_WARNING.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_DEBUG_WARNING.ToString());
            }
            if (enableDebugLog && debugLogLevel.HasFlag(DebugLogLevel.Error))
            {
                defines.Add(Define.VMC_DEBUG_ERROR.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_DEBUG_ERROR.ToString());
            }
            if (enableDebugLog && debugLogLevel.HasFlag(DebugLogLevel.Assert))
            {
                defines.Add(Define.VMC_DEBUG_ASSERT.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_DEBUG_ASSERT.ToString());
            }

            #endregion

            if (isUsingDoTween)
            {
                defines.Add(Define.VMC_DOTWEEN.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_DOTWEEN.ToString());
            }
            if (isUsingIAP)
            {
                defines.Add(Define.VMC_IAP.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_IAP.ToString());
            }
            if (isUsingLocalNotification)
            {
                defines.Add(Define.VMC_NOTIFICATION.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_NOTIFICATION.ToString());
            }





            Save();
        }
       
    }

    [Flags]
    [Serializable]
    public enum AdsLibrary
    {
        None = 0,
        Admob = 1,
        MaxMediation = 2
    }

    [Flags]
    [Serializable]
    public enum AnalyzeLibrary
    {
        None = 0,
        Firebase = 1,
        AppsFlyer = 2
    }

    [Flags]
    [Serializable]
    public enum DebugLogLevel
    {
        None = 0,
        Debug = 1,
        Warning = 2,
        Error = 4,
        Assert = 8
    }
}