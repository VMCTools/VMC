using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using VMC.Ultilities;
using Debug = VMC.Debugger.Debug;

namespace VMC.Ads
{
    public class AdsIronsourceMediation : AdsMediation
    {
#if VMC_ADS_IRONSOURCE
#if UNITY_ANDROID
        public string YOUR_APP_KEY_ANDROID = "YOUR_APP_KEY";
#else
        public string YOUR_APP_KEY_IOS = "YOUR_APP_KEY";
#endif
#endif
        public override void Initialize()
        {
            base.Initialize();
#if VMC_ADS_IRONSOURCE
#if VMC_ADS_TESTMODE
            Debug.Log("Ironsource chưa hỗ trợ test mode");
#else
#if UNITY_ANDROID
            YOUR_APP_KEY_ANDROID = config.ironsourceAppId;
#else
            YOUR_APP_KEY_IOS = config.ironsourceAppId;
#endif
#endif

            this.adsType = config.adType;

            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent; // init completed
            IronSourceEvents.onImpressionDataReadyEvent += IronSourceEvents_onImpressionDataReadyEvent; //event revenue

            IronSourceBannerEvents.onAdLoadedEvent += IronSourceBannerEvents_onAdLoadedEvent;
            IronSourceBannerEvents.onAdLoadFailedEvent += IronSourceBannerEvents_onAdLoadFailedEvent;
            IronSourceBannerEvents.onAdLeftApplicationEvent += IronSourceBannerEvents_onAdLeftApplicationEvent;

            IronSourceInterstitialEvents.onAdReadyEvent += IronSourceInterstitialEvents_onAdReadyEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += IronSourceInterstitialEvents_onAdLoadFailedEvent;
            IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;

            IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;

            IronSource.Agent.setConsent(true);
            IronSource.Agent.setMetaData("do_not_sell", "false");
            IronSource.Agent.setMetaData("is_child_directed", "false");
#if UNITY_ANDROID
            IronSource.Agent.init(YOUR_APP_KEY_ANDROID);
#else
            IronSource.Agent.init(YOUR_APP_KEY_IOS);
#endif
#endif
        }

#if VMC_ADS_IRONSOURCE
        private void SdkInitializationCompletedEvent()
        {
            IronSource.Agent.validateIntegration();
            if (adsType.HasFlag(AdsType.Banner))
                InitializeBannerAds();
            if (adsType.HasFlag(AdsType.Interstitial))
                InitializeInterstitialAds();
            if (adsType.HasFlag(AdsType.RewardedVideo))
                InitializeRewardedVideoAds();
        }
        private void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

        private void IronSourceEvents_onImpressionDataReadyEvent(IronSourceImpressionData impressionData)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Process_IronSourceEvents_onImpressionDataReadyEvent(impressionData);
            });

        }
        private void Process_IronSourceEvents_onImpressionDataReadyEvent(IronSourceImpressionData impressionData)
        {
            //Debug.Log("unity-script:  ImpressionSuccessEvent impressionData = " + impressionData);
            if (impressionData != null)
            {
                Firebase.Analytics.Parameter[] AdParameters = {
                    new Firebase.Analytics.Parameter("ad_platform", "ironSource"),
                    new Firebase.Analytics.Parameter("ad_source", impressionData.adNetwork),
                    new Firebase.Analytics.Parameter("ad_unit_name", impressionData.instanceName),
                    new Firebase.Analytics.Parameter("ad_format", impressionData.adUnit),
                    new Firebase.Analytics.Parameter("currency","USD"),
                    new Firebase.Analytics.Parameter("value",(double)impressionData.revenue)
                };
                VMC.Analystic.AnalysticManager.Instance.LogEvent("ad_impression", AdParameters);
            }
        }
#endif
        public override void SetVolume(float value)
        {
            base.SetVolume(value);
#if VMC_ADS_IRONSOURCE
            Debug.Log("Ironsource doesn't support SetVolume");
#endif
        }

        #region BANNER
#if VMC_ADS_IRONSOURCE
        private IronSourceBannerPosition GetAdPosition(BannerAdsPosition posi)
        {
            switch (posi)
            {
                case BannerAdsPosition.TOP: return IronSourceBannerPosition.TOP;
                case BannerAdsPosition.BOTTOM: return IronSourceBannerPosition.BOTTOM;
                default: return IronSourceBannerPosition.BOTTOM;
            }
        }
#endif
        public override void InitializeBannerAds()
        {
            base.InitializeBannerAds();
            LoadBannerAds();
        }
        public override void LoadBannerAds()
        {
            base.LoadBannerAds();
#if VMC_ADS_IRONSOURCE
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, GetAdPosition(bannerPosition));
#endif
        }

        public override float GetBannerHeight()
        {
#if VMC_ADS_IRONSOURCE
            return 50;
#else
            return 0;
#endif
        }

        public override void ShowBannerAds(BannerAdsPosition posi = BannerAdsPosition.BOTTOM)
        {
            base.ShowBannerAds(posi);
#if VMC_ADS_IRONSOURCE
            IronSource.Agent.displayBanner();
#endif
        }
        public override void HideBannerAds()
        {
            base.HideBannerAds();
#if VMC_ADS_IRONSOURCE
            IronSource.Agent.hideBanner();
#endif
        }
#if VMC_ADS_IRONSOURCE
        private void IronSourceBannerEvents_onAdLoadedEvent(IronSourceAdInfo obj)
        {
            Debug.Log("[IRONSOURCE-Banner]", "Loaded Ads!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnLoadBannerSuccessed();
            });
        }
        private void IronSourceBannerEvents_onAdLoadFailedEvent(IronSourceError obj)
        {
            Debug.Log("[IRONSOURCE-Banner]", "Load Failed Ads!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnLoadBannerFailed(obj.getErrorCode().ToString(), obj.getDescription());
            });
        }
        private void IronSourceBannerEvents_onAdLeftApplicationEvent(IronSourceAdInfo obj)
        {
            AdsManager.LeaveGameByPurpose = true;//ads
        }
#endif
        #endregion

        #region INTERSTITIAL
        public override void InitializeInterstitialAds()
        {
            base.InitializeInterstitialAds();
            LoadInterstitialAds();
        }
        public override void LoadInterstitialAds()
        {
            base.LoadInterstitialAds();
#if VMC_ADS_IRONSOURCE
            Debug.Log("[IRONSOURCE-Intersitial]", "Load Ads!");
            IronSource.Agent.loadInterstitial();
#endif
        }

        public override void ShowInterstitialAds(string placement, Action callback)
        {
            base.ShowInterstitialAds(placement, callback);
            if (!IsCanShowInterstitial) return;
#if VMC_ADS_IRONSOURCE
            if (IronSource.Agent.isInterstitialReady())
            {
                Debug.Log("[IRONSOURCE-Intersitial]", "Showing interstitial ad.");
                AdsManager.LeaveGameByPurpose = true;//ads
                IronSource.Agent.showInterstitial();
            }
            else
            {
                Debug.Log("[IRONSOURCE-Intersitial]", "Interstitial ad is not ready yet.");
                LoadInterstitialAds();
                callback?.Invoke();
            }
#endif
        }

#if VMC_ADS_IRONSOURCE

        private void IronSourceInterstitialEvents_onAdReadyEvent(IronSourceAdInfo obj)
        {
            Debug.Log("[IRONSOURCE-Intersitial]", "Loaded Ads!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnInterstititalLoadSuccessed();
            });
        }

        private void IronSourceInterstitialEvents_onAdLoadFailedEvent(IronSourceError obj)
        {
            Debug.Log("[IRONSOURCE-Intersitial]", "Load failed Ads!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnInterstitialLoadFailed(obj.getDescription());
            });
        }

        private void InterstitialOnAdClosedEvent(IronSourceAdInfo obj)
        {
            Debug.Log("[IRONSOURCE-Intersitial]", "Closed Ads!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnInterstitialDisplaySuccessed();
            });
        }
        private void InterstitialOnAdShowFailedEvent(IronSourceError arg1, IronSourceAdInfo arg2)
        {
            Debug.Log("[IRONSOURCE-Intersitial]", "Failed to show Ads!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnInterstitialDisplayFailed(arg1.getDescription());
            });
        }
        private void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo obj)
        {
            Debug.Log("[IRONSOURCE-Intersitial]", "Successed show Ads!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnInterstitialDisplaySuccessed();
            });
        }

#endif
        #endregion

        #region REWARDED VIDEO
        public override void InitializeRewardedVideoAds()
        {
            base.InitializeRewardedVideoAds();
            LoadRewardedVideo();
        }
        public override void LoadRewardedVideo()
        {
            base.LoadRewardedVideo();
#if VMC_ADS_IRONSOURCE
            Debug.Log("[IRONSOURCE-RewardedVideo]", "Load Rewarded");
            IronSource.Agent.loadRewardedVideo();
#endif
        }

        public override void ShowRewardedVideo(string placement, Action<bool> callback)
        {
            base.ShowRewardedVideo(placement, callback);
#if VMC_ADS_IRONSOURCE
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                Debug.Log("[IRONSOURCE-RewardedVideo]", "Show Rewarded");
                AdsManager.LeaveGameByPurpose = true;//ads
                IronSource.Agent.showRewardedVideo();
            }
            else
            {
                Debug.Log("[IRONSOURCE-RewardedVideo]", "Not loaded before -> return false");
                callback?.Invoke(false);
            }
#else
            callback?.Invoke(false);
#endif
        }

#if VMC_ADS_IRONSOURCE
        // Indicates that there’s an available ad.
        // The adInfo object includes information about the ad that was loaded successfully
        // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
        private void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
        {
            Debug.Log("[IRONSOURCE-RewardedVideo]", "On Load Successed Rewarded");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnRewardedLoadSuccessed();
            });
        }
        // Indicates that no ads are available to be displayed
        // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
        private void RewardedVideoOnAdUnavailable()
        {
            Debug.Log("[IRONSOURCE-RewardedVideo]", "On Load Failed Rewarded");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnRewardedLoadFailed(string.Empty);
            });
        }

        // The rewarded video ad was failed to show.
        private void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
        {
            Debug.Log("[IRONSOURCE-RewardedVideo]", "Failed to show RewardedVideo ads. " + error.getDescription());
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnRewardedDisplayFailed(error.getDescription());
            });
        }
        // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
        private void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
        {
            Debug.Log("[IRONSOURCE-RewardedVideo]", "Close Ads");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(this.OnRewardedDisplaySuccessed);
        }
        // The user completed to watch the video, and should be rewarded.
        // The placement parameter will include the reward data.
        // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
        private void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            Debug.Log("[IRONSOURCE-RewardedVideo]", "Impression Recorded!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(this.OnRewardedGotReward);
        }
#endif
        #endregion

    }
}