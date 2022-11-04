using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = VMC.Debugger.Debug;
namespace VMC.Analystic
{
    public class AnalysticManager : VMC.Ultilities.Singleton<AnalysticManager>, IAnalystic
    {
        private List<IAnalystic> analytics = new List<IAnalystic>();
        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;

#if VMC_ANALYZE_APPFLYER
            var appflyer = (new GameObject("AppsFlyer Analytic")).AddComponent<AppsFlyerAnalystic>();
            appflyer.transform.SetParent(this.transform);
            analytics.Add(appflyer);
#endif
#if VMC_ANALYZE_FIREBASE
            var firebase = (new GameObject("Firebase Analytic")).AddComponent<FirebaseAnalystic>();
            firebase.transform.SetParent(this.transform);
            analytics.Add(firebase);
#endif
        }
        public void Initialize()
        {
            Debug.Log("[Analystic]", "Init All analytics!");
            foreach (var analytic in analytics)
            {
                analytic.Initialize();
            }
        }

        public void InitializeFirebase()
        {
            foreach (var analytic in analytics)
            {
                if (analytic is FirebaseAnalystic)
                    analytic.Initialize();
            }
        }
        public void InitializeAppflyer()
        {
            foreach (var analytic in analytics)
            {
                if (analytic is AppsFlyerAnalystic)
                {
                    analytic.Initialize();
                }
            }
        }

        public void ATTShow()
        {
            foreach (var analytic in analytics)
            {
                analytic.ATTShow();
            }
        }
        public void ATTSuccess()
        {
            foreach (var analytic in analytics)
            {
                analytic.ATTSuccess();
            }
        }

        public void LogEvent(string nameEvent)
        {
            foreach (var analytic in analytics)
            {
                analytic.LogEvent(nameEvent);
            }
        }

#if VMC_GROUP_1
        public void Log_LevelStart(int level)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_LevelStart(level);
            }
        }

        public void Log_LevelWin(int level, float time)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_LevelWin(level, time);
            }
        }

        public void Log_LevelLose(int level, float time)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_LevelLose(level, time);
            }
        }

        public void Log_CoinEarn(int value, string position)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_CoinEarn(value, position);
            }
        }

        public void Log_CoinSpent(int value, string position, string item)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_CoinSpent(value, position, item);
            }
        }

        public void Log_InappPurchase(string package, float amount)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_InappPurchase(package, amount);
            }
        }

        public void Log_UserInteract(string screen)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_UserInteract(screen);
            }
        }

        public void Log_RewardedAdsShow(int level, string placement)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_RewardedAdsShow(level, placement);
            }
        }

        public void Log_RewardedAdsSuccessed(int level, string placement)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_RewardedAdsSuccessed(level, placement);
            }
        }

        public void Log_IntersitialAdsShow(int level, string placement)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_IntersitialAdsShow(level, placement);
            }
        }

        public void Log_IntersitialAdsSuccessed(int level, string placement)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_IntersitialAdsSuccessed(level, placement);
            }
        }

        public void SetUserProperties_TotalSpend(int total)
        {
            foreach (var analytic in analytics)
            {
                analytic.SetUserProperties_TotalSpend(total);
            }
        }

        public void SetUserProperties_TotalEarn(int total)
        {
            foreach (var analytic in analytics)
            {
                analytic.SetUserProperties_TotalEarn(total);
            }
        }

        public void SetUserProperties_LevelReach(int level)
        {
            foreach (var analytic in analytics)
            {
                analytic.SetUserProperties_LevelReach(level);
            }
        }

        public void SetUserProperties_DayDisplaying(int days)
        {
            foreach (var analytic in analytics)
            {
                analytic.SetUserProperties_DayDisplaying(days);
            }
        }
#elif VMC_GROUP_2
        public void Log_CheckPoint(int id)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_CheckPoint(id);
            }
        }
        public void Log_LevelStart(int level, int current_gold)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_LevelStart(level, current_gold);
            }
        }
        public void Log_LevelComplete(int level, float timeplayed)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_LevelComplete(level, timeplayed);
            }
        }
        public void Log_LevelFail(int level, int failcount)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_LevelFail(level, failcount);
            }
        }
        public void Log_EarnVirtualCurrency(string virtual_currency_name, long value, string source)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_EarnVirtualCurrency(virtual_currency_name, value, source);
            }
        }
        public void Log_SpendVirtualCurrency(string virtual_currency_name, long value, string item_name)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_SpendVirtualCurrency(virtual_currency_name, value, item_name);
            }
        }
        public void Log_AdsRewardOffer(string placment)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_AdsRewardOffer(placment);
            }
        }
        public void Log_AdsRewardClick(string placment)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_AdsRewardClick(placment);
            }
        }
        public void Log_AdsRewardShow(string placment)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_AdsRewardShow(placment);
            }
        }
        public void Log_AdsRewardFail(string placment, string errormsg)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_AdsRewardFail(placment, errormsg);
            }
        }
        public void Log_AdsRewardComplete(string placment)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_AdsRewardComplete(placment);
            }
        }

        public void Log_AdsInterOffer()
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_AdsInterOffer();
            }
        }
        public void Log_AdsInterFail(string errormsg)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_AdsInterFail(errormsg);
            }
        }

        public void Log_AdsInterLoad()
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_AdsInterLoad();
            }
        }

        public void Log_AdsInterShow()
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_AdsInterShow();
            }
        }
        public void Log_AdsInterClick()
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_AdsInterClick();
            }
        }

        public void UserProperty_RetentType(int retent_type)
        {
            foreach (var analytic in analytics)
            {
                analytic.UserProperty_RetentType(retent_type);
            }
        }
        public void UserProperty_DayPlayed(int days_played)
        {
            foreach (var analytic in analytics)
            {
                analytic.UserProperty_DayPlayed(days_played);
            }
        }
        public void UserProperty_PayingType(int paying_type)
        {
            foreach (var analytic in analytics)
            {
                analytic.UserProperty_PayingType(paying_type);
            }
        }
        public void UserProperty_Level(int level)
        {
            foreach (var analytic in analytics)
            {
                analytic.UserProperty_Level(level);
            }
        }


        public void Log_TutorialCompletion(bool isSuccess, string tutorialId)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_TutorialCompletion(isSuccess, tutorialId);
            }
        }
        public void Log_LevelAchieved(int level, int score)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_LevelAchieved(level, score);
            }
        }
        public void Log_AchievementUnlocked(int contentId, int level)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_AchievementUnlocked(contentId, level);
            }
        }
        public void Log_Purchase(float revenue, string currency, int quantity, int contentId)
        {
            foreach (var analytic in analytics)
            {
                analytic.Log_Purchase(revenue, currency, quantity, contentId);
            }
        }
#endif
    }
}