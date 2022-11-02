#if VMC_REMOTE_FIREBASE
using Firebase.Extensions;
using Task = System.Threading.Tasks.Task;
#endif
using UnityEngine;
using VMC.Analystic;
using VMC.Ultilities;
using Debug = VMC.Debugger.Debug;
using System;

namespace VMC.Settings
{
    public class VMCManager : Singleton<VMCManager>
    {
        public bool EnableRemote = false;
        private void Start()
        {
            Application.targetFrameRate = 60;
            if (Application.platform == RuntimePlatform.IPhonePlayer && PlayerPrefs.GetInt("ATTShowed", 0) == 0 && UnityATTPlugin.Instance.IsIOS14AndAbove())
            {
                AnalysticManager.Instance.ATTShow();
                UnityATTPlugin.Instance.ShowATTRequest((action) =>
                {
                    if (action == ATTStatus.Authorized)
                        AnalysticManager.Instance.ATTSuccess();
                });
                PlayerPrefs.SetInt("ATTShowed", 1);
            }
            FirebaseAnalystic.OnFirebaseReady += FirebaseAnalystic_OnFirebaseReady;
        }

        private void FirebaseAnalystic_OnFirebaseReady()
        {
#if VMC_FIREBASE_MESSAGING
            Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
            Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
#endif
#if VMC_REMOTE_FIREBASE
            InitRemoteconfig();
#endif
        }
#if VMC_FIREBASE_MESSAGING
        public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
        {
            Debug.Log("[Firebase Message]", "Received Registration Token: " + token.Token);
        }

        public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
        {
            Debug.Log("[Firebase Message]", "Received a new message from: " + e.Message.From);
        }
#endif


#if VMC_REMOTE_FIREBASE
        public void InitRemoteconfig()
        {
            // [START set_defaults]
            System.Collections.Generic.Dictionary<string, object> defaults = new System.Collections.Generic.Dictionary<string, object>();

            // These are the values that are used if we haven't fetched data from the
            // server

            Settings.VMCSettingConfig config = Settings.VMCSettingConfig.LoadData();
            // yet, or if we ask for values that the server doesn't have:
            defaults.Add("config_aoa_id", config.openAdsId_Tier1);

            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
              .ContinueWithOnMainThread(task =>
              {
                  // [END set_defaults]
                  Debug.Log("RemoteConfig configured and ready!");
                  FetchDataAsync();
                  //isFirebaseInitialized = true;
              });
        }
        public Task FetchDataAsync()
        {
            Debug.Log("Fetching data...");
            Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }
        void FetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
            {
                Debug.Log("Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                Debug.Log("Fetch encountered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("Fetch completed successfully!");
            }
        }
#endif
    }
}