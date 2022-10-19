using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace VMC.Ads
{
    public class AdsMaxMediation : AdsMediation
    {
        [Header("ID Real")]
#if UNITY_ANDROID
        [ReadOnly] public string maxAppID = "YOUR_OPENADS_ID_ADS_HERE";
        [ReadOnly] public string bannerId = "YOUR_BANNER_ID_ADS_HERE";
        [ReadOnly] public string interstitialId = "YOUR_INTERS_ID_ADS_HERE";
        [ReadOnly] public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#elif UNITY_IOS
        [ReadOnly] public string maxAppID = "YOUR_OPENADS_ID_ADS_HERE";
        [ReadOnly] public string bannerId="YOUR_BANNER_ID_ADS_HERE";
        [ReadOnly] public BannerAdsPosition bannerPosition;
        [ReadOnly] public string interstitialId="YOUR_INTERS_ID_ADS_HERE";
        [ReadOnly] public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#endif
        public override void Initialize()
        {
            base.Initialize();
#if VMC_ADS_MAX
            Settings.VMCSettingConfig config = Settings.VMCSettingConfig.LoadData();

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
            };

            MaxSdk.SetSdkKey(maxAppID);
            MaxSdk.InitializeSdk();
#endif
        }

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
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
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
#if VMC_ADS_MAX
            if (IsLoadedInterstitial)
            {
                MaxSdk.ShowInterstitial(interstitialId, placement);
            }
            else
            {
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
            this.OnInterstitialLoadFailed();
        }
        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            this.OnInterstitialDisplayFailed();
        }
        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            this.OnInterstitialDisplaySuccessed();
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
            this.OnRewardedLoadFailed();
        }
        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
            this.OnRewardedDisplayFailed();
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
    }
}