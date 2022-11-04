
#if VMC_ADS_ADMOB
using GoogleMobileAds.Api;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = VMC.Debugger.Debug;

namespace VMC.Ads
{
    public class AdsAdmobOpenAds : VMC.Ultilities.Singleton<AdsAdmobOpenAds>
    {
#if VMC_ADS_ADMOB
        private string ID_TIER_1 = "TIER_1_HERE";
        private string ID_TIER_2 = "TIER_2_HERE";
        private string ID_TIER_3 = "TIER_3_HERE";
        private AppOpenAd ad;
        private bool IsAdAvailable => ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;
        private bool isShowingAd = false;
        private bool showFirstOpen = false;
        private DateTime loadTime;
        private int tierIndex = 1;

        private float intervalTimeShowAds;
#endif
        private DateTime nextTimeToShow;

        public static bool ConfigOpenApp = true;
        public static bool ConfigResumeApp = true;

        public void Initialize()
        {
            Debug.Log("[Ads]", "Init AOA!");
#if VMC_ADS_ADMOB
            Settings.VMCSettingConfig config = Settings.VMCSettingConfig.LoadData();
            ID_TIER_1 = config.openAdsId_Tier1;
            ID_TIER_2 = config.openAdsId_Tier2;
            ID_TIER_3 = config.openAdsId_Tier3;
            intervalTimeShowAds = config.intervalTimeAOA;
            if (config.isTestMode)
            {
#if UNITY_ANDROID
                ID_TIER_1 = "ca-app-pub-3940256099942544/3419835294"; // ID test
#elif UNITY_IOS
                ID_TIER_1 = "ca-app-pub-3940256099942544/5662855259"; // ID test
#endif
                ID_TIER_2 = ID_TIER_1;
                ID_TIER_3 = ID_TIER_2;
            }


            var deviceTest = new List<string> { "377807EAA67F63DF0FFE8F146CF568C7" };
            RequestConfiguration requestConfiguration = new RequestConfiguration.Builder().SetTestDeviceIds(deviceTest).build();
            MobileAds.SetRequestConfiguration(requestConfiguration);

            MobileAds.Initialize(status =>
            {
                LoadAd(); // load ad in init
            });
#endif
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause && ConfigResumeApp && !AdsMediation.ResumeFromAds)
            {
                if (DateTime.Now.CompareTo(nextTimeToShow) > 0)
                    ShowAdIfAvailable();
            }
        }

        private void LoadAd()
        {
            if (!VMC.Settings.VMCManager.Instance.EnableAOA)
                return;
#if VMC_ADS_ADMOB
            string id = ID_TIER_1;
            if (tierIndex == 2)
                id = ID_TIER_2;
            else if (tierIndex == 3)
                id = ID_TIER_3;

            Debug.Log("Start request Open App Ads Tier " + tierIndex);

            //#if VMC_REMOTE_FIREBASE
            //            if (Settings.VMCManager.Instance.EnableRemote)
            //            {
            //                id = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("config_aoa_id").StringValue;
            //            }
            //#endif


            //var config = RequestConfiguration.Builder().setTestDeviceIds(Arrays.asList("377807EAA67F63DF0FFE8F146CF568C7"));
            AdRequest request = new AdRequest.Builder().Build();

            AppOpenAd.LoadAd(id, ScreenOrientation.Portrait, request, ((appOpenAd, error) =>
            {
                if (error != null)
                {
                    // Handle the error.
                    Debug.LogFormat("Failed to load the ad. (reason: {0}), tier {1}", error.LoadAdError.GetMessage(), tierIndex);
                    tierIndex++;
                    if (tierIndex <= 3)
                        LoadAd(); // load add
                    else
                        tierIndex = 1;
                    return;
                }

                // App open ad is loaded.
                ad = appOpenAd;
                tierIndex = 1;
                loadTime = DateTime.UtcNow;
                if (!showFirstOpen && ConfigOpenApp)
                {
                    ShowAdIfAvailable();
                    showFirstOpen = true;
                }
            }));
#endif
        }


        public void ShowAdIfAvailable()
        {
            if (!VMC.Settings.VMCManager.Instance.EnableAOA)
                return;

#if VMC_ADS_ADMOB
            if(ad==null)
            {
                LoadAd(); // when ads is null -> reload ads
                return;
            }

            if (!IsAdAvailable || isShowingAd)
            {
                return;
            }

            ad.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
            ad.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
            ad.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
            ad.OnAdDidRecordImpression += HandleAdDidRecordImpression;
            ad.OnPaidEvent += HandlePaidEvent;

            nextTimeToShow = DateTime.Now.AddSeconds(intervalTimeShowAds);
            ad.Show();
#endif
        }


#if VMC_ADS_ADMOB
        private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
        {
            Debug.Log("Closed app open ad");
            // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
            ad = null;
            isShowingAd = false;
            LoadAd(); //load ads when close prev ads
        }

        private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
        {
            Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
            // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
            ad = null;
            LoadAd(); // load ads when failed show prev ads
        }

        private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
        {
            Debug.Log("Displayed app open ad");
            isShowingAd = true;
        }

        private void HandleAdDidRecordImpression(object sender, EventArgs args)
        {
            Debug.Log("Recorded ad impression");
        }

        private void HandlePaidEvent(object sender, AdValueEventArgs args)
        {
            Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
                args.AdValue.CurrencyCode, args.AdValue.Value);
        }
#endif
    }
}