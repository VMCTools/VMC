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

#if VMC_ADS_IRONSOURCE && UNITY_IOS
                    // Set the flag as true 
                    AudienceNetwork.AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(action == ATTStatus.Authorized);
#endif
                });
                PlayerPrefs.SetInt("ATTShowed", 1);
            }
            else
            {
#if VMC_ADS_IRONSOURCE && UNITY_IOS
                // Set the flag as true 
                AudienceNetwork.AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(UnityATTPlugin.Instance.GetATTStatus() == ATTStatus.Authorized);
#endif
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