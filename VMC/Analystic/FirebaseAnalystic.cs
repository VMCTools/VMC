
#if VMC_ANALYZE_FIREBASE
using Firebase.Analytics;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using VMC.Settings;
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
        public void LogEvent(string eventName)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
            Debug.Log("Firebase", "Log message: " + eventName);
#endif
        }
#if VMC_ANALYZE_FIREBASE
        public void LogEvent(string eventName, Firebase.Analytics.Parameter[] param)
        {
            if (!isInitedFirebase) return;
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, param);
            Debug.Log("Firebase", "Log message: " + eventName);
        }
#endif
        public void LogEvent(string eventName, AnalyzeLibrary specialPlatform)
        {
            if (!specialPlatform.HasFlag(AnalyzeLibrary.Firebase))
            {
                return; // không chỉ định firebase log event
            }
            LogEvent(eventName);
        }
        public void LogEvent(string eventName, Dictionary<string, string> param)
        {
#if VMC_ANALYZE_FIREBASE
            if (!isInitedFirebase) return;

            Parameter[] parameters = new Parameter[param.Count];
            byte count = 0;
            foreach (var item in param)
            {
                if (long.TryParse(item.Value, out long longValue))
                {
                    parameters[count] = new Parameter(item.Key, longValue);
                }
                else if (double.TryParse(item.Value, out double doubleValue))
                {
                    parameters[count] = new Parameter(item.Key, doubleValue);
                }
                else
                {
                    parameters[count] = new Parameter(item.Key, item.Value);
                }
                count++;
            }
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parameters);
            Debug.Log("Firebase", "Log message: " + eventName);
#endif
        }
    }

}