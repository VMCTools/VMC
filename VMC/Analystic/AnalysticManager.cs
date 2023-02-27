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
        public void LogEvent(string nameEvent)
        {
            foreach (var analytic in analytics)
            {
                analytic.LogEvent(nameEvent);
            }
        }

        public void LogEvent(string nameEvent, Dictionary<string, string> param)
        {
            foreach (var analytic in analytics)
            {
                analytic.LogEvent(nameEvent, param);
            }
        }
    }
}