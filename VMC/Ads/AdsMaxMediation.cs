using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using VMC.Analystic;
using Debug = VMC.Debugger.Debug;

namespace VMC.Ads
{
    public class AdsMaxMediation : AdsMediation
    {
#if UNITY_ANDROID
        [Header("ID Real")]
        [ReadOnly] public string maxAppID = "YOUR_OPENADS_ID_ADS_HERE";
        [ReadOnly] public string bannerId = "YOUR_BANNER_ID_ADS_HERE";
        [ReadOnly] public string interstitialId = "YOUR_INTERS_ID_ADS_HERE";
        [ReadOnly] public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#elif UNITY_IOS
        [Header("ID Real")]
        [ReadOnly] public string maxAppID = "YOUR_OPENADS_ID_ADS_HERE";
        [ReadOnly] public string bannerId="YOUR_BANNER_ID_ADS_HERE";
        [ReadOnly] public BannerAdsPosition bannerPosition;
        [ReadOnly] public string interstitialId="YOUR_INTERS_ID_ADS_HERE";
        [ReadOnly] public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#endif
        #region OpenAds

        private string AppOpenAdUnitId = "YOUR_AD_UNIT_ID";
        private DateTime nextTimeToShow;
        private float intervalTimeShowAds;
        private bool showFirstOpen = false;

        public static bool ConfigOpenApp = true;
        public static bool ConfigResumeApp = true;
        #endregion


        public override void Initialize()
        {
            base.Initialize();
            Debug.Log("[Ads]", "Init Max SDK!");
#if VMC_ADS_MAX
            this.TestMode = config.isTestMode;
            this.adsType = config.adType;
            if (config.isTestMode)
            {
                //this.openAdsId = this.openAdsIdTest;
                //this.bannerId = this.bannerIdTest;
                //this.interstitialId = this.interstitialIdTest;
                //this.rewardedVideoId = this.rewardedVideoIdTest;
            }
            else
            {
                this.maxAppID = config.maxAppId;
                this.bannerId = config.bannerId;
                this.bannerPosition = config.bannerPosition;
                this.interstitialId = config.interstitialId;
                this.rewardedVideoId = config.rewardedVideoId;

                this.AppOpenAdUnitId = config.openAdsId_Tier1;
                intervalTimeShowAds = config.intervalTimeAOA;

            }
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                // AppLovin SDK is initialized, start loading ads
                if (adsType.HasFlag(AdsType.Banner))
                    InitializeBannerAds();
                if (adsType.HasFlag(AdsType.Interstitial))
                    InitializeInterstitialAds();
                if (adsType.HasFlag(AdsType.RewardedVideo))
                    InitializeRewardedVideoAds();

                MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
                if (adsType.HasFlag(AdsType.OpenAds) && !showFirstOpen && ConfigOpenApp)
                {
                    ShowAdIfReady();
                    showFirstOpen = true;
                }
            };

            MaxSdk.SetSdkKey(maxAppID);
            MaxSdk.InitializeSdk();
#endif
        }


        #region OPEN ADS
        private void OnApplicationPause(bool pauseStatus)
        {
            if (adsType.HasFlag(AdsType.OpenAds) && !pauseStatus && ConfigResumeApp && !AdsMediation.ResumeFromAds)
            {
                if (DateTime.Now.CompareTo(nextTimeToShow) > 0)
                    ShowAdIfReady();
            }
        }
        public void ShowAdIfReady()
        {
            Debug.Log("[Ads MAX]", $"Show OpenAds if ready {AppOpenAdUnitId}!");
#if VMC_ADS_MAX
            if (MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId))
            {
                Debug.Log("[Ads MAX]", $"OpenAds Show {AppOpenAdUnitId}!");
                MaxSdk.ShowAppOpenAd(AppOpenAdUnitId);
                nextTimeToShow = DateTime.Now.AddSeconds(intervalTimeShowAds);

                SetIntervalInterstitial(); // reset interval Inter after view OpenAds
            }
            else
            {
                Debug.Log("[Ads MAX]", $"OpenAds not ready {AppOpenAdUnitId}!");
                MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
            }
#endif
        }
#if VMC_ADS_MAX
        public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
        }
#endif
        #endregion

        #region BANNER
#if VMC_ADS_MAX
        private MaxSdkBase.BannerPosition ConvertPosition(BannerAdsPosition position = BannerAdsPosition.BOTTOM)
        {
            switch (position)
            {
                case BannerAdsPosition.BOTTOM:
                    return MaxSdkBase.BannerPosition.BottomCenter;
                case BannerAdsPosition.TOP:
                    return MaxSdkBase.BannerPosition.TopCenter;
                default:
                    return MaxSdkBase.BannerPosition.BottomCenter;
            }
        }
#endif
        public override void InitializeBannerAds()
        {
            base.InitializeBannerAds();
#if VMC_ADS_MAX
            MaxSdk.CreateBanner(this.bannerId, ConvertPosition(this.bannerPosition));
#if VMC_GROUP_2
            MaxSdk.SetBannerExtraParameter(this.bannerId, "adaptive_banner", "false");
#endif

            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
#endif
        }
        public override void LoadBannerAds()
        {
            base.LoadBannerAds();
            // do nothing
        }
        public override void ShowBannerAds(BannerAdsPosition position = BannerAdsPosition.BOTTOM)
        {
            base.ShowBannerAds(position);
#if VMC_ADS_MAX 
            MaxSdk.ShowBanner(bannerId);
#endif
        }
        public override void HideBannerAds()
        {
            base.HideBannerAds();
#if VMC_ADS_MAX
            MaxSdk.HideBanner(bannerId);
#endif
        }
#if VMC_ADS_MAX
        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            this.OnLoadBannerSuccessed();
        }
#endif
        #endregion

        #region INTERSTITIAL
        public override void InitializeInterstitialAds()
        {
            base.InitializeInterstitialAds();

#if VMC_ADS_MAX
            // Attach callback
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += Interstitial_OnAdClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            // Load the first interstitial
            LoadInterstitialAds();
#endif
        }


        public override void LoadInterstitialAds()
        {
            base.LoadInterstitialAds();
#if VMC_ADS_MAX
            MaxSdk.LoadInterstitial(interstitialId);
#endif
        }
        public override void ShowInterstitialAds(string placement, Action callback = null)
        {
            base.ShowInterstitialAds(placement, callback);
            if (!IsCanShowInterstitial)
            {
                VMC.Debugger.Debug.Log("[ADS] Max", "Cant show because isCanShowInterstitial=false");
                return;
            }
#if VMC_ADS_MAX
            if (IsLoadedInterstitial)
            {
                VMC.Debugger.Debug.Log("[ADS] Max", "Call show");
                MaxSdk.ShowInterstitial(interstitialId, placement);
            }
            else
            {
                VMC.Debugger.Debug.Log("[ADS] Max", "Cant show because not loaded");
                callback?.Invoke();
            }
#endif
        }

#if VMC_ADS_MAX
        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            this.OnInterstititalLoadSuccessed();
        }
        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            this.OnInterstitialLoadFailed(errorInfo.Message);
        }
        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            this.OnInterstitialDisplayFailed(errorInfo.Message);
        }
        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            this.OnInterstitialDisplaySuccessed();
        }
        private void Interstitial_OnAdClickedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            this.OnInterstitialClicked();
        }
#endif
        #endregion

        #region REWARDED
        public override void InitializeRewardedVideoAds()
        {
            base.InitializeRewardedVideoAds();
#if VMC_ADS_MAX
            // Attach callback
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += Rewarded_OnAdClickedEvent;
            // Load the first rewarded ad
            LoadRewardedVideo();
#endif
        }

        public override void LoadRewardedVideo()
        {
            base.LoadRewardedVideo();
#if VMC_ADS_MAX
            MaxSdk.LoadRewardedAd(rewardedVideoId);
#endif
        }
        public override void ShowRewardedVideo(string placement, Action<bool> callback)
        {
            base.ShowRewardedVideo(placement, callback);
#if VMC_ADS_MAX
            if (MaxSdk.IsRewardedAdReady(rewardedVideoId))
            {
                MaxSdk.ShowRewardedAd(rewardedVideoId, placement);
            }
            else
            {
                callback?.Invoke(false);
            }
#endif
        }

#if VMC_ADS_MAX
        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

            // Reset retry attempt
            this.OnRewardedLoadSuccessed();
        }
        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Rewarded ad failed to load 
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).
            this.OnRewardedLoadFailed(errorInfo.Message);
        }
        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
            this.OnRewardedDisplayFailed(errorInfo.Message);
        }

        private void Rewarded_OnAdClickedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            this.OnRewardedClicked();
        }
        private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is hidden. Pre-load the next ad
            this.OnRewardedDisplaySuccessed();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            // The rewarded ad displayed and the user should receive the reward.
            this.OnRewardedGotReward();
        }
#endif

        #endregion

#if VMC_ADS_MAX
        private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
        {
#if VMC_ANALYZE_FIREBASE
            double revenue = impressionData.Revenue;
            var impressionParameters = new[] {
new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
new Firebase.Analytics.Parameter("ad_format", impressionData.Placement), // Please check this - as we couldn't find format refereced in your unity docs https://dash.applovin.com/documentation/mediation/unity/getting-started/advanced-settings#impression-level-user-revenue - api
new Firebase.Analytics.Parameter("value", revenue),
new Firebase.Analytics.Parameter("currency", "USD"), // All Applovin revenue is sent in USD
};
            Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
#if VMC_GROUP_2
            Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_abi", impressionParameters);
#endif
#endif
        }
#endif
    }
}