
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

        public void ATTShow()
        {

        }
        public void ATTSuccess()
        {

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

#if VMC_GROUP_1 
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
#elif VMC_GROUP_2
        public void Log_CheckPoint(int id) { }
        public void Log_LevelStart(int level, int current_gold) { }
        public void Log_LevelComplete(int level, float timeplayed) { }
        public void Log_LevelFail(int level, int failcount) { }
        public void Log_EarnVirtualCurrency(string virtual_currency_name, long value, string source) { }
        public void Log_SpendVirtualCurrency(string virtual_currency_name, long value, string item_name) { }
        public void Log_AdsRewardOffer(string placment)
        {
#if VMC_ANALYZE_APPFLYER
            AppsFlyer.sendEvent("af_rewarded_ad_eligible", null);
#endif 
        }
        public void Log_AdsRewardClick(string placment) { }
        public void Log_AdsRewardShow(string placment)
        {
#if VMC_ANALYZE_APPFLYER
            AppsFlyer.sendEvent("af_rewarded_displayed", null);
#endif 
        }
        public void Log_AdsRewardFail(string placment, string errormsg) { }
        public void Log_AdsRewardComplete(string placment)
        {
#if VMC_ANALYZE_APPFLYER
            AppsFlyer.sendEvent("af_rewarded_ad_completed", null);
#endif 
        }

        public void Log_AdsInterOffer()
        {
#if VMC_ANALYZE_APPFLYER
            AppsFlyer.sendEvent("af_inters_ad_eligible", null);
#endif 
        }
        public void Log_AdsInterFail(string errormsg) { }
        public void Log_AdsInterLoad()
        {
#if VMC_ANALYZE_APPFLYER
            AppsFlyer.sendEvent("af_inters_api_called", null);
#endif 
        }
        public void Log_AdsInterShow()
        {
#if VMC_ANALYZE_APPFLYER
            AppsFlyer.sendEvent("af_inters_displayed", null);
#endif 
        }
        public void Log_AdsInterClick() { }


        public void UserProperty_RetentType(int retent_type) { }
        public void UserProperty_DayPlayed(int days_played) { }
        public void UserProperty_PayingType(int paying_type) { }
        public void UserProperty_Level(int level) { }


        public void Log_TutorialCompletion(bool isSuccess, string tutorialId)
        {
#if VMC_ANALYZE_APPFLYER
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"af_success", isSuccess.ToString() },
                {"af_tutorial_id",tutorialId},
            };
            AppsFlyer.sendEvent("af_tutorial_completion", parameters);
#endif
        }
        public void Log_LevelAchieved(int level, int score)
        {
#if VMC_ANALYZE_APPFLYER
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"af_level", level.ToString() },
                {"af_score",score.ToString() },
            };
            AppsFlyer.sendEvent("af_level_achieved", parameters);
#endif
        }
        public void Log_AchievementUnlocked(int contentId, int level)
        {
#if VMC_ANALYZE_APPFLYER
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"af_content_id", contentId.ToString() },
                {"af_level",level.ToString() },
            };
            AppsFlyer.sendEvent("af_achievement_unlocked", parameters);
#endif 
        }
        public void Log_Purchase(float revenue, string currency, int quantity, int contentId)
        {
#if VMC_ANALYZE_APPFLYER
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                {"af_revenue", revenue.ToString() },
                {"af_currency",currency.ToString() },
                {"af_quantity",quantity.ToString() },
                {"af_content_id",contentId.ToString() },
            };
            AppsFlyer.sendEvent("af_purchase", parameters);
#endif 
        }
#endif

    }
}