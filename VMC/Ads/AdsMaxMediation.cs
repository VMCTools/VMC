using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ads
{
    public class AdsMaxMediation : AdsController
    {
        [Header("Applovin Config")]
#if UNITY_ANDROID
        public string Max_SDK_Id = "YOUR_SDK_KEY_HERE";
        public string bannerAdUnitId = "YOUR_BANNER_AD_UNIT_ID";
        public MaxSdkBase.BannerPosition bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;
        public string interstitialAdUnitId = "YOUR_INTER_AD_UNIT_ID";
        public string rewardedAdUnitId = "YOUR_REWARDED_AD_UNIT_ID";
#elif UNITY_IOS
        public string Max_SDK_Id = "YOUR_SDK_KEY_HERE";
        public string bannerAdUnitId = "YOUR_BANNER_AD_UNIT_ID";
        public MaxSdkBase.BannerPosition bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;
        public string interstitialUnitId = "YOUR_INTER_AD_UNIT_ID";
        public string rewardedAdUnitId = "YOUR_REWARDED_AD_UNIT_ID";
#else
        public string Max_SDK_Id = "YOUR_SDK_KEY_HERE";
        public string bannerAdUnitId = "YOUR_BANNER_AD_UNIT_ID";
        public MaxSdkBase.BannerPosition bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;
        public string interstitialUnitId = "YOUR_INTER_AD_UNIT_ID";
        public string rewardedAdUnitId = "YOUR_REWARDED_AD_UNIT_ID";
#endif
        private int retryInterAttempt;
        private Action interCallback;


        private int retryRewardedAttempt;
        private bool gotReward = false;
        private Action<bool> rewardedCallback;

        protected override void I_Init()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                // AppLovin SDK is initialized, start loading ads
                if (adsType.HasFlag(AdsType.Banner))
                    InitializeBannerAds();
                if (adsType.HasFlag(AdsType.Interstitial))
                    InitializeInterstitialAds();
                if (adsType.HasFlag(AdsType.RewardedVideo))
                    InitializeRewardedAds();
            };

            MaxSdk.SetSdkKey(Max_SDK_Id);
            MaxSdk.InitializeSdk();
        }
        private void InitializeBannerAds()
        {
            // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
            // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
            MaxSdk.CreateBanner(bannerAdUnitId, bannerPosition);

            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
        }
        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

        private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        protected override void I_ShowBanner()
        {
            MaxSdk.ShowBanner(bannerAdUnitId);
        }
        protected override void I_HideBanner()
        {
            MaxSdk.HideBanner(bannerAdUnitId);
        }
        public void InitializeInterstitialAds()
        {
            // Attach callback
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

            // Load the first interstitial
            I_RequestInterstitial();
        }

        protected override void I_RequestInterstitial()
        {
            MaxSdk.LoadInterstitial(interstitialAdUnitId);
        }
        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'
            // Reset retry attempt
            retryInterAttempt = 0;
        }

        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to load 
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

            retryInterAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, retryInterAttempt));

            Invoke(nameof(I_RequestInterstitial), (float)retryDelay);
        }

        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
            I_RequestInterstitial();
            AdsController.ResumeFromAds = false;
            interCallback?.Invoke();
        }

        private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad.
            I_RequestInterstitial();
            AdsController.ResumeFromAds = false;
            interCallback?.Invoke();
        }
        protected override void I_ShowInterstitial(string placement, System.Action callback)
        {
            if (MaxSdk.IsInterstitialReady(interstitialAdUnitId))
            {
                this.interCallback = callback;
                AdsController.ResumeFromAds = true;
                MaxSdk.ShowInterstitial(interstitialAdUnitId, placement);
            }
            else
            {
                callback?.Invoke();
            }
        }
        public void InitializeRewardedAds()
        {
            // Attach callback
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            // Load the first rewarded ad
            I_RequestRewardedVideo();
        }

        protected override void I_RequestRewardedVideo()
        {
            MaxSdk.LoadRewardedAd(rewardedAdUnitId);
        }
        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

            // Reset retry attempt
            retryRewardedAttempt = 0;
        }

        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Rewarded ad failed to load 
            // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).
            retryRewardedAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, retryRewardedAttempt));
            Invoke(nameof(I_RequestRewardedVideo), (float)retryDelay);
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
            I_RequestRewardedVideo();
            AdsController.ResumeFromAds = false;
            rewardedCallback?.Invoke(gotReward);
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is hidden. Pre-load the next ad
            I_RequestRewardedVideo();
            AdsController.ResumeFromAds = false;
            rewardedCallback?.Invoke(gotReward);
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            // The rewarded ad displayed and the user should receive the reward.
            this.gotReward = true;
        }

        private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Ad revenue paid. Use this callback to track user revenue.

        }
        protected override void I_ShowRewardedVideo(string placement, Action<bool> callback)
        {
            if (MaxSdk.IsRewardedAdReady(rewardedAdUnitId))
            {
                this.gotReward = false;
                this.rewardedCallback = callback;
                AdsController.ResumeFromAds = true;
                MaxSdk.ShowRewardedAd(rewardedAdUnitId, placement);
            }
            else
            {
                callback?.Invoke(false);
            }
        }
    }
}