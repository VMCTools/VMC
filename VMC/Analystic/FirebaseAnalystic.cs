
#if VMC_ANALYZE_FIREBASE
using Firebase.Analytics;
#endif
using UnityEngine;
using Debug = VMC.Debugger.Debug;
namespace VMC.Analystic
{
    public class FirebaseAnalystic : MonoBehaviour, IAnalystic
    {
#if VMC_ANALYZE_FIREBASE
        private bool isInitedFirebase = false;
#endif
        public void Initialize()
        {
#if VMC_ANALYZE_FIREBASE
            Debug.Log("Firebase", "Init!");
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

        public void LogEvent(string eventName)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
            Debug.Log("Firebase", "Log message: " + eventName);
#endif
        }
        public void Log_LevelStart(int level)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                Parameter[] parameters = { new Parameter("level", level) };
                FirebaseAnalytics.LogEvent("level_start", parameters);
            }
#endif
        }
        public void Log_LevelWin(int level, float time)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                Parameter[] parameters = { new Parameter("level", level), new Parameter("time", time) };
                FirebaseAnalytics.LogEvent("level_win", parameters);
            }
#endif
        }
        public void Log_LevelLose(int level, float time)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                Parameter[] parameters = { new Parameter("level", level), new Parameter("time", time) };
                FirebaseAnalytics.LogEvent("level_lose", parameters);
            }
#endif
        }
        public void Log_CoinEarn(int value, string position)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                Parameter[] parameters = { new Parameter("value", value), new Parameter("position", position) };
                FirebaseAnalytics.LogEvent("coin_earn", parameters);
            }
#endif
        }
        public void Log_CoinSpent(int value, string position, string item)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                Parameter[] parameters = { new Parameter("value", value), new Parameter("position", position), new Parameter("item", item) };
                FirebaseAnalytics.LogEvent("coin_spent", parameters);
            }
#endif
        }
        public void Log_InappPurchase(string package, float amount)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                Parameter[] parameters = { new Parameter("package", package), new Parameter("amount", amount) };
                FirebaseAnalytics.LogEvent("in_app_purchase", parameters);
            }
#endif
        }
        public void Log_UserInteract(string screen)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                Parameter[] parameters = { new Parameter("screen", screen) };
                FirebaseAnalytics.LogEvent("user_interact", parameters);
            }
#endif
        }
        public void Log_RewardedAdsShow(int level, string placement)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                Parameter[] parameters = { new Parameter("level", level), new Parameter("placement", placement) };
                FirebaseAnalytics.LogEvent("rewarded_video_show", parameters);
            }
#endif
        }
        public void Log_RewardedAdsSuccessed(int level, string placement)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                Parameter[] parameters = { new Parameter("level", level), new Parameter("placement", placement) };
                FirebaseAnalytics.LogEvent("watch_ad_rewarded", parameters);
            }
#endif
        }
        public void Log_IntersitialAdsShow(int level, string placement)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                Parameter[] parameters = { new Parameter("level", level), new Parameter("placement", placement) };
                FirebaseAnalytics.LogEvent("Interstitial_show", parameters);
            }
#endif
        }
        public void Log_IntersitialAdsSuccessed(int level, string placement)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                Parameter[] parameters = { new Parameter("level", level), new Parameter("placement", placement) };
                FirebaseAnalytics.LogEvent("watch_ad_inter", parameters);
            }
#endif
        }

        public void SetUserProperties_TotalSpend(int total)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                FirebaseAnalytics.SetUserProperty("total_spent", total.ToString());
            }
#endif
        }
        public void SetUserProperties_TotalEarn(int total)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                FirebaseAnalytics.SetUserProperty("total_earn", total.ToString());
            }
#endif
        }
        public void SetUserProperties_LevelReach(int level)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                FirebaseAnalytics.SetUserProperty("level_reach", level.ToString());
            }
#endif
        }
        public void SetUserProperties_DayDisplaying(int days)
        {
#if VMC_ANALYZE_FIREBASE
            if (isInitedFirebase)
            {
                FirebaseAnalytics.SetUserProperty("days_playing", days.ToString());
            }
#endif
        }

    }

}