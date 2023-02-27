#if VMC_REMOTE_FIREBASE
using Firebase.Extensions;
using Task = System.Threading.Tasks.Task;
#endif
using UnityEngine;
using VMC.Analystic;
using VMC.Ultilities;
using Debug = VMC.Debugger.Debug;
using System;

#if VMC_FACEBOOK
using Facebook.Unity;
#endif

namespace VMC.Settings
{
    public class VMCManager : Singleton<VMCManager>
    {
        public bool EnableRemote = false;
        public bool EnableAOA = false;
        private void Start()
        {
            Application.targetFrameRate = 60;
            if (Application.platform == RuntimePlatform.IPhonePlayer && PlayerPrefs.GetInt("ATTShowed", 0) == 0 && UnityATTPlugin.Instance.IsIOS14AndAbove())
            {
                //AnalysticManager.Instance.ATTShow();
                UnityATTPlugin.Instance.ShowATTRequest((action) =>
                {
                    if (action == ATTStatus.Authorized)
                    {
                        //AnalysticManager.Instance.ATTSuccess();
                    }
                });
                PlayerPrefs.SetInt("ATTShowed", 1);
            }
            FirebaseAnalystic.OnFirebaseReady += FirebaseAnalystic_OnFirebaseReady;


            VMC.Notifications.LocalNotification.Instance.RegisterNotificationChannel();
            VMC.Notifications.LocalNotification.Instance.ClearNotifications();
            VMC.Notifications.LocalNotification.Instance.SendNotification(1, 3 * 24 * 60 * 60 * 1000, "Monsters came back!", "Let's kill them all!", new Color32());

#if VMC_FACEBOOK
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
#endif
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
            defaults.Add("enable_aoa", false);
            EnableAOA = false;
            //defaults.Add("config_aoa_id", config.openAdsId_Tier1);


            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
              .ContinueWithOnMainThread(task =>
              {
                  // [END set_defaults]
                  Debug.Log("RemoteConfig configured and ready!");
                  FetchDataAsync();
                  //isFirebaseInitialized = true;
                  EnableRemote = true;
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

            if (Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("enable_aoa").BooleanValue)
            {
                EnableAOA = true;
            }
        }
#endif


#if VMC_FACEBOOK
        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }
#endif
    }
}