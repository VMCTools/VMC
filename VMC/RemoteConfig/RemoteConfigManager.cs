#if VMC_REMOTE_FIREBASE
using Firebase.Extensions;
using Firebase.RemoteConfig;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VMC.Analystic;
using VMC.Ultilities;

namespace VMC.RemoteConfig
{
    public class RemoteConfigManager : Singleton<RemoteConfigManager>
    {
        public bool gotRemoteConfig;
        protected Dictionary<string, object> defaults;

        private void Start()
        {
            gotRemoteConfig = false;
            FirebaseAnalystic.OnFirebaseReady += FirebaseAnalystic_OnFirebaseReady;
        }
        protected override void OnDestroy()
        {
            FirebaseAnalystic.OnFirebaseReady -= FirebaseAnalystic_OnFirebaseReady;
            base.OnDestroy();
        }

        protected virtual void SetDefaultConfig()
        {
            defaults = new Dictionary<string, object>();
        }
#if VMC_REMOTE_FIREBASE
        protected virtual void HandleRemoteConfig(IDictionary<string, ConfigValue> allValues)
        {
            throw new NotImplementedException();
        }
#endif

        private void FirebaseAnalystic_OnFirebaseReady()
        {
#if VMC_REMOTE_FIREBASE
            InitRemoteconfig();
#endif
        }
#if VMC_REMOTE_FIREBASE
        public void InitRemoteconfig()
        {
            SetDefaultConfig();
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
              .ContinueWithOnMainThread(task =>
              {
                  // [END set_defaults]
                  Debug.Log("RemoteConfig configured and ready!");
                  gotRemoteConfig = true;
                  FetchDataAsync();
              });
        }
        public Task FetchDataAsync()
        {
            Debug.Log("Fetching data...");
            Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }
        private void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            // Fetch successful. Parameter values must be activated to use.
            remoteConfig.ActivateAsync()
              .ContinueWithOnMainThread(
                task => {
                    Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                    HandleRemoteConfig(remoteConfig.AllValues);
                });
        }
#endif
    }
}