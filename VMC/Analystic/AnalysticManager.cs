using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VMC.Ultilities;

namespace VMC.Analystic
{
    public class AnalysticManager : Singleton<AnalysticManager>, IAnalystic
    {
        private List<IAnalystic> analytics = new List<IAnalystic>();
        private void Start()
        {

#if VMC_ANALYZE_APPFLYER
            var appflyer = (new GameObject("AppsFlyer Analytics")).AddComponent<AppsFlyerAnalystic>();
            appflyer.transform.SetParent(this.transform);
            analytics.Add(appflyer);
#endif
#if VMC_ANALYZE_FIREBASE
            var firebase = (new GameObject("Firebase Analytics")).AddComponent<FirebaseAnalystic>();
            firebase.transform.SetParent(this.transform);
            analytics.Add(firebase);
#endif
            Initialize();

        }
        public void Initialize()
        {
            foreach (var analytic in analytics)
            {
                analytic.Initialize();
            }
        }

        public void LogEvent(string nameEvent)
        {
            foreach (var analytic in analytics)
            {
                analytic.LogEvent(nameEvent);
            }
        }

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
    }
}