
#if VMC_ANALYZE_APPFLYER
using AppsFlyerSDK;
using System;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Debug = VMC.Debugger.Debug;

namespace VMC.Analystic
{
    public class AppsFlyerAnalystic : MonoBehaviour, IAnalystic

#if VMC_ANALYZE_APPFLYER
        , IAppsFlyerConversionData
#endif
    {
        [SerializeField, ReadOnly] private string AF_Dev_Key;
        [SerializeField, ReadOnly] private string AF_App_Id;


        public void Initialize()
        {
            Debug.Log("[Analystic]", "Init AppsFlyer!");
#if VMC_ANALYZE_APPFLYER
            VMC.Settings.VMCSettingConfig config = VMC.Settings.VMCSettingConfig.LoadData();
            AF_Dev_Key = config.AF_Dev_Key;
            AF_App_Id = config.AF_App_Id;

            AppsFlyer.initSDK(AF_Dev_Key, AF_App_Id, this);
            AppsFlyer.startSDK();
#endif
        }

        public void LogEvent(string nameEvent)
        {
        }

        public void onAppOpenAttribution(string attributionData)
        {
        }

        public void onAppOpenAttributionFailure(string error)
        {
        }

        public void onConversionDataFail(string error)
        {
        }

        public void onConversionDataSuccess(string conversionData)
        {
        }

        public void Log_LevelStart(int level)
        {
        }

        public void Log_LevelWin(int level, float time)
        {
        }

        public void Log_LevelLose(int level, float time)
        {
        }

        public void Log_CoinEarn(int value, string position)
        {
        }

        public void Log_CoinSpent(int value, string position, string item)
        {
        }

        public void Log_InappPurchase(string package, float amount)
        {
        }

        public void Log_UserInteract(string screen)
        {
        }

        public void Log_RewardedAdsShow(int level, string placement)
        {
#if VMC_ANALYZE_APPFLYER
            AppsFlyer.sendEvent("rewarded_video_show", null);
#endif
        }

        public void Log_RewardedAdsSuccessed(int level, string placement)
        {
        }

        public void Log_IntersitialAdsShow(int level, string placement)
        {
#if VMC_ANALYZE_APPFLYER
            AppsFlyer.sendEvent("Interstitial_show", null);
#endif
        }

        public void Log_IntersitialAdsSuccessed(int level, string placement)
        {
        }

        public void SetUserProperties_TotalSpend(int total)
        {
        }

        public void SetUserProperties_TotalEarn(int total)
        {
        }

        public void SetUserProperties_LevelReach(int level)
        {
        }

        public void SetUserProperties_DayDisplaying(int days)
        {
        }
    }
}