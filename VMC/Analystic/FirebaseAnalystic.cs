
#if VMC_ANALYZE_FIREBASE
using Firebase.Analytics;
#endif
using System;
using UnityEngine;
using Debug = VMC.Debugger.Debug;
namespace VMC.Analystic
{
    public class FirebaseAnalystic : MonoBehaviour, IAnalystic
    {
#if VMC_ANALYZE_FIREBASE
        private bool isInitedFirebase = false;
#endif
        public static event Action OnFirebaseReady;
        public void Initialize()
        {
            Debug.Log("[Analystic]", "Init firebase!");
#if VMC_ANALYZE_FIREBASE
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    //app = Firebase.FirebaseApp.DefaultInstance;
                    isInitedFirebase = true;
                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                    OnFirebaseReady?.Invoke();
                }
                else
                {
                    Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
#else
            Debug.LogWarning("Firebase-Analystic", "Not enable Firebase in VMC/Setting!");
#endif
        }


        public void ATTShow()
        {

        }
        public void ATTSuccess()
        {

        }
        public void LogEvent(string eventName)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
            Debug.Log("Firebase", "Log message: " + eventName);
#endif
        }
#if VMC_GROUP_1

        public void Log_LevelStart(int level)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter("level", level) };
            FirebaseAnalytics.LogEvent("level_start", parameters);
#endif
        }
        public void Log_LevelWin(int level, float time)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter("level", level), new Parameter("time", time) };
            FirebaseAnalytics.LogEvent("level_win", parameters);
#endif
        }
        public void Log_LevelLose(int level, float time)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter("level", level), new Parameter("time", time) };
            FirebaseAnalytics.LogEvent("level_lose", parameters);
#endif
        }
        public void Log_CoinEarn(int value, string position)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter("value", value), new Parameter("position", position) };
            FirebaseAnalytics.LogEvent("coin_earn", parameters);
#endif
        }
        public void Log_CoinSpent(int value, string position, string item)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter("value", value), new Parameter("position", position), new Parameter("item", item) };
            FirebaseAnalytics.LogEvent("coin_spent", parameters);
#endif
        }
        public void Log_InappPurchase(string package, float amount)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter("package", package), new Parameter("amount", amount) };
            FirebaseAnalytics.LogEvent("in_app_purchase", parameters);
#endif
        }
        public void Log_UserInteract(string screen)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter("screen", screen) };
            FirebaseAnalytics.LogEvent("user_interact", parameters);
#endif
        }
        public void Log_RewardedAdsShow(int level, string placement)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter("level", level), new Parameter("placement", placement) };
            FirebaseAnalytics.LogEvent("rewarded_video_show", parameters);
#endif
        }
        public void Log_RewardedAdsSuccessed(int level, string placement)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter("level", level), new Parameter("placement", placement) };
            FirebaseAnalytics.LogEvent("watch_ad_rewarded", parameters);
#endif
        }
        public void Log_IntersitialAdsShow(int level, string placement)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter("level", level), new Parameter("placement", placement) };
            FirebaseAnalytics.LogEvent("Interstitial_show", parameters);
#endif
        }
        public void Log_IntersitialAdsSuccessed(int level, string placement)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter("level", level), new Parameter("placement", placement) };
            FirebaseAnalytics.LogEvent("watch_ad_inter", parameters);
#endif
        }

        public void SetUserProperties_TotalSpend(int total)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.SetUserProperty("total_spent", total.ToString());
#endif
        }
        public void SetUserProperties_TotalEarn(int total)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.SetUserProperty("total_earn", total.ToString());
#endif
        }
        public void SetUserProperties_LevelReach(int level)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.SetUserProperty("level_reach", level.ToString());
#endif
        }
        public void SetUserProperties_DayDisplaying(int days)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.SetUserProperty("days_playing", days.ToString());
#endif
        }
#elif VMC_GROUP_2

        public void Log_CheckPoint(int id)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.LogEvent($"checkpoint_{id.ToString("d2")}");
#endif
        }
        public void Log_LevelStart(int level, int current_gold)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter(FirebaseAnalytics.ParameterLevel, level), new Parameter("current_gold", current_gold) };
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, parameters);
#endif
        }
        public void Log_LevelComplete(int level, float timeplayed)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter(FirebaseAnalytics.ParameterLevel, level), new Parameter("timeplayed", timeplayed) };
            FirebaseAnalytics.LogEvent("level_complete", parameters);
#endif
        }
        public void Log_LevelFail(int level, int failcount)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = { new Parameter(FirebaseAnalytics.ParameterLevel, level), new Parameter("failcount", failcount) };
            FirebaseAnalytics.LogEvent("level_fail", parameters);
#endif
        }
        public void Log_EarnVirtualCurrency(string virtual_currency_name, long value, string source)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = {
                new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, virtual_currency_name),
                new Parameter(FirebaseAnalytics.ParameterValue, value) ,
                new Parameter(FirebaseAnalytics.ParameterSource, source) ,
            };
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency, parameters);
#endif
        }
        public void Log_SpendVirtualCurrency(string virtual_currency_name, long value, string item_name)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = {
                new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, virtual_currency_name),
                new Parameter(FirebaseAnalytics.ParameterValue, value) ,
                new Parameter(FirebaseAnalytics.ParameterItemName, item_name) ,
            };
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSpendVirtualCurrency, parameters);
#endif
        }
        public void Log_AdsRewardOffer(string placment)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = {
                new Parameter("placement",placment),
            };
            FirebaseAnalytics.LogEvent("ads_reward_offer", parameters);
#endif
        }
        public void Log_AdsRewardClick(string placment)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = {
                new Parameter("placement",placment),
            };
            FirebaseAnalytics.LogEvent("ads_reward_click", parameters);
#endif
        }
        public void Log_AdsRewardShow(string placment)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = {
                new Parameter("placement",placment),
            };
            FirebaseAnalytics.LogEvent("ads_reward_show", parameters);
#endif
        }
        public void Log_AdsRewardFail(string placment, string errormsg)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = {
                new Parameter("placement",placment),
                new Parameter("errormsg",errormsg),
            };
            FirebaseAnalytics.LogEvent("ads_reward_fail", parameters);
#endif
        }
        public void Log_AdsRewardComplete(string placment)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = {
                new Parameter("placement",placment),
            };
            FirebaseAnalytics.LogEvent("ads_reward_complete", parameters);
#endif
        }

        public void Log_AdsInterOffer()
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.LogEvent("ad_inter_offer");
#endif
        }
        public void Log_AdsInterFail(string errormsg)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Parameter[] parameters = {
                new Parameter("errormsg",errormsg),
            };
            FirebaseAnalytics.LogEvent("ad_inter_fail", parameters);
#endif
        }
        public void Log_AdsInterLoad()
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.LogEvent("ad_inter_load");
#endif
        }
        public void Log_AdsInterShow()
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.LogEvent("ad_inter_show");
#endif
        }
        public void Log_AdsInterClick()
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.LogEvent("ad_inter_click");
#endif
        }


        public void UserProperty_RetentType(int retent_type)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.SetUserProperty("retent_type",retent_type.ToString());
#endif
        }
        public void UserProperty_DayPlayed(int days_played)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.SetUserProperty("days_played", days_played.ToString());
#endif
        }
        public void UserProperty_PayingType(int paying_type)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.SetUserProperty("paying_type", paying_type.ToString());
#endif
        }
        public void UserProperty_Level(int level)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            FirebaseAnalytics.SetUserProperty("level", level.ToString());
#endif
        }


        public void Log_TutorialCompletion(bool isSuccess, string tutorialId)
        {
        }
        public void Log_LevelAchieved(int level, int score)
        {
        }
        public void Log_AchievementUnlocked(int contentId, int level)
        {
        }
        public void Log_Purchase(float revenue, string currency, int quantity, int contentId)
        {
        }
#endif

    }

}