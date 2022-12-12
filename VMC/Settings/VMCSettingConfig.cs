using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VMC.Ultilities;

namespace VMC.Settings
{
    [CreateAssetMenu(fileName = "VMC Settings", menuName = "VMC/Setting")]
    public class VMCSettingConfig : ScriptableObject
    {
        [Header("Group Config")]
        public SKDGroup groupSDK;

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
        [Tooltip("Thời gian tối thiểu giữa 2 lần show Admob Open Ads")]
        public float intervalTimeAOA = 20f;

        public string bannerId = "YOUR_BANNER_ID_ADS_HERE";
        public Ads.BannerAdsPosition bannerPosition = Ads.BannerAdsPosition.BOTTOM;
        public string interstitialId = "YOUR_INTERS_ID_ADS_HERE";
        public float intervalTimeInterstitial = 15f;
        public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#elif UNITY_IOS
        public bool isTestMode = false;
        public string maxAppId = "YOUR_APP_ID_HERE";
        public string openAdsId_Tier1 = "YOUR_OPENADS_ID_ADS_HERE";
        public string openAdsId_Tier2 = "YOUR_OPENADS_ID_ADS_HERE";
        public string openAdsId_Tier3 = "YOUR_OPENADS_ID_ADS_HERE";
        [Tooltip("Thời gian tối thiểu giữa 2 lần show Admob Open Ads")]
        public float intervalTimeAOA = 20f;

        public string bannerId = "YOUR_BANNER_ID_ADS_HERE";
        public Ads.BannerAdsPosition bannerPosition = Ads.BannerAdsPosition.BOTTOM;
        public string interstitialId = "YOUR_INTERS_ID_ADS_HERE";
        public float intervalTimeInterstitial = 15f;
        public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#else
        public bool isTestMode = false;
        public string maxAppId = "YOUR_APP_ID_HERE";
        public string openAdsId_Tier1 = "YOUR_OPENADS_ID_ADS_HERE";
        public string openAdsId_Tier2 = "YOUR_OPENADS_ID_ADS_HERE";
        public string openAdsId_Tier3 = "YOUR_OPENADS_ID_ADS_HERE";
        [Tooltip("Thời gian tối thiểu giữa 2 lần show Admob Open Ads")]
        public float intervalTimeAOA = 20f;

        public string bannerId = "YOUR_BANNER_ID_ADS_HERE";
        public Ads.BannerAdsPosition bannerPosition = Ads.BannerAdsPosition.BOTTOM;
        public string interstitialId = "YOUR_INTERS_ID_ADS_HERE";
        public float intervalTimeInterstitial = 15f;
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

        [Header("Remote Config")]
        public bool enableRemoteConfig = false;

        [Header("Debug Config")]
        public bool enableDebugLog = false;
        public DebugLogLevel debugLogLevel;

        [Header("Facebook")]
        public bool isUsingFacebook = false;

        [Header("DOTween")]
        public bool isUsingDoTween = true;

        [Header("IAP")]
        public bool isUsingIAP = false;

        [Header("Addressable")]
        public bool isUsingAddressable = false;

        [Header("Notification")]
        public NotificationType notificationType;

        [Header("App Review Rating")]
        public bool isUsingAppReview = false;

        public static VMCSettingConfig LoadData()
        {
            var setting = Resources.Load<VMCSettingConfig>("VMC Settings");
            if (setting != null) return setting;
            else
            {
                ScriptableObjectUtility.CreateAsset<VMCSettingConfig>("Assets/Resources/VMC Settings.asset");
                return Resources.Load<VMCSettingConfig>("VMC Settings");
            }
        }
    }

    [Serializable]
    public enum SKDGroup
    {
        Group1 = 1,
        Group2 = 2
    }

    [Flags]
    [Serializable]
    public enum AdsLibrary
    {
        None = 0,
        Admob = 1,
        MaxMediation = 2,
        IronsourceMediation = 4
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
    [Flags]
    [Serializable]
    public enum NotificationType
    {
        None = 0,
        LocalNotification = 1,
        FirebaseMessaging = 2
    }
}