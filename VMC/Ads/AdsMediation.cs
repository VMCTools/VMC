using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VMC.Ads;
using VMC.Ultilities;
using VMC.Analystic;
using VMC.Settings;
using UnityEngine.UI.Extensions;
#if VMC_DOTWEEN
using DG.Tweening;
#endif
using Debug = VMC.Debugger.Debug;

namespace VMC.Ads
{
    public class AdsMediation : MonoBehaviour, IAds
    {

        public bool TestMode;

        public AdsType adsType;

        private const string KEY_ENABLE_ADS = "User_Setting_Enable_Ads";
        private const string KEY_CONSENT_ADS = "User_Setting_Consent_Ads";

        private bool? isEnableAds;
        public bool IsEnableAds
        {
            get
            {
                if (isEnableAds == null)
                {
                    isEnableAds = bool.Parse(PlayerPrefs.GetString(KEY_ENABLE_ADS, "true"));
                }
                return (bool)isEnableAds;
            }
            set
            {
                isEnableAds = value;
                PlayerPrefs.SetString(KEY_ENABLE_ADS, isEnableAds.ToString());
            }
        }

        private bool? isConsentAds;
        public bool IsConsentAds
        {
            get
            {
                if (isConsentAds == null)
                {
                    isConsentAds = bool.Parse(PlayerPrefs.GetString(KEY_CONSENT_ADS, "false"));
                }
                return (bool)isConsentAds;
            }
            set
            {
                isConsentAds = value;
                PlayerPrefs.SetString(KEY_CONSENT_ADS, isEnableAds.ToString());
            }
        }

        [SerializeField, ReadOnly] private float intervalTimeInterstitial = 10f;
        private DateTime nextTimeReadyInter;
        protected bool IsCanShowInterstitial => DateTime.Now >= nextTimeReadyInter;

        protected bool isShowingBanner;
        protected BannerAdsPosition bannerPosition;

        private int retryInterAttempt;
        protected bool IsLoadedInterstitial;
        protected string interstitialPlacement;
        protected Action interstitialCallback;

        private int retryRewardedAttempt;
        protected bool IsLoadedRewardedVideo;
        protected string rewardedPlacement;
        protected bool gotReward;
        protected Action<bool> rewardedCallback;

        public static bool ResumeFromAds;

        protected VMCSettingConfig config;
        public virtual void Initialize()
        {
            this.config = Settings.VMCSettingConfig.LoadData();
            this.intervalTimeInterstitial = config.intervalTimeInterstitial;

            nextTimeReadyInter = DateTime.Now;
            SetConsentAds(IsConsentAds);
        }
        public virtual void SetConsentAds(bool value)
        {
        }
        public virtual void SetVolume(float value)
        {
        }
        protected void SetWatchingAds(bool isWatching)
        {
            ResumeFromAds = isWatching;
        }

        #region BANNER
        public virtual void InitializeBannerAds() { }
        public virtual void LoadBannerAds() { }
        public virtual void ShowBannerAds(BannerAdsPosition position = BannerAdsPosition.BOTTOM)
        {
            this.bannerPosition = position;
            this.isShowingBanner = true;
        }
        public virtual void HideBannerAds()
        {
            this.isShowingBanner = false;
        }
        protected void OnLoadBannerSuccessed()
        {
            if (isShowingBanner) ShowBannerAds(this.bannerPosition);
            else HideBannerAds();
        }
        #endregion

        #region INTERSTITIAL
        public virtual void InitializeInterstitialAds() { }
        public virtual void LoadInterstitialAds()
        {
            IsLoadedInterstitial = false;
#if VMC_GROUP_2
            AnalysticManager.Instance.Log_AdsInterLoad();
#endif
        }
        public bool IsInterstitialAvailable()
        {
            return IsLoadedInterstitial;
        }
        public virtual void ShowInterstitialAds(string placement, Action callback = null)
        {
            if (!IsCanShowInterstitial)
            {
                VMC.Debugger.Debug.Log("[ADS]", "Cant Show interstitial because interval time");
                callback?.Invoke();
                return;
            }
            if (IsLoadedInterstitial)
            {
                this.interstitialPlacement = placement;
                this.interstitialCallback = callback;
                SetWatchingAds(true);
#if VMC_GROUP_2
                AnalysticManager.Instance.Log_AdsInterShow();
#endif
                VMC.Debugger.Debug.Log("[ADS]", "Real show");
            }
            else
            {
                VMC.Debugger.Debug.Log("[ADS]", "Cant Show interstitial because not loaded");
            }
        }
        protected void SetIntervalInterstitial()
        {
            nextTimeReadyInter = DateTime.Now.AddSeconds(intervalTimeInterstitial);
        }

        protected void OnInterstititalLoadSuccessed()
        {
            IsLoadedInterstitial = true;
            retryInterAttempt = 0;
        }
        protected void OnInterstitialLoadFailed(string errorMsg)
        {
            retryInterAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, retryInterAttempt));
            Invoke(nameof(LoadInterstitialAds), (float)retryDelay);
#if VMC_GROUP_2
            AnalysticManager.Instance.Log_AdsInterFail(errorMsg);
#endif
        }
        protected void OnInterstitialDisplayFailed(string errorMsg)
        {
            LoadInterstitialAds();
            SetWatchingAds(false);
            interstitialCallback?.Invoke();

#if VMC_GROUP_2
            AnalysticManager.Instance.Log_AdsInterFail(errorMsg);
#endif
        }
        protected void OnInterstitialDisplaySuccessed()
        {
            LoadInterstitialAds();
            SetWatchingAds(false);
            interstitialCallback?.Invoke();
            SetIntervalInterstitial(); // reset interval Inter after view Inter
        }
        protected void OnInterstitialClicked()
        {
#if VMC_GROUP_2
            AnalysticManager.Instance.Log_AdsInterClick();
#endif
        }
        #endregion

        #region REWARDED VIDEO
        public virtual void InitializeRewardedVideoAds() { }
        public virtual void LoadRewardedVideo()
        {
            IsLoadedRewardedVideo = false;
        }
        public bool IsRewardVideoAvailable()
        {
            return IsLoadedRewardedVideo;
        }
        public virtual void ShowRewardedVideo(string placement, Action<bool> callback)
        {
            if (IsLoadedRewardedVideo)
            {
                this.rewardedPlacement = placement;
                this.rewardedCallback = callback;
                this.gotReward = false;
                SetWatchingAds(true);

#if VMC_GROUP_2
                AnalysticManager.Instance.Log_AdsRewardShow(this.rewardedPlacement);
#endif
            }
        }
        protected void OnRewardedLoadSuccessed()
        {
            IsLoadedRewardedVideo = true;
            retryRewardedAttempt = 0;
        }
        protected void OnRewardedLoadFailed(string errorMsg)
        {
            retryRewardedAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, retryRewardedAttempt));
            Invoke(nameof(LoadRewardedVideo), (float)retryDelay);
            //#if VMC_GROUP_2
            //            AnalysticManager.Instance.Log_AdsRewardFail(this.rewardedPlacement, errorMsg);
            //#endif
        }
        protected void OnRewardedDisplayFailed(string errorMsg)
        {
            LoadRewardedVideo();
            SetWatchingAds(false);
            rewardedCallback?.Invoke(gotReward);
#if VMC_GROUP_2
            AnalysticManager.Instance.Log_AdsRewardFail(this.rewardedPlacement, errorMsg);
#endif
        }
        protected void OnRewardedClicked()
        {
#if VMC_GROUP_2
            AnalysticManager.Instance.Log_AdsRewardClick(this.rewardedPlacement);
#endif
        }

        protected void OnRewardedDisplaySuccessed()
        {
            LoadRewardedVideo();
            SetWatchingAds(false);
            rewardedCallback?.Invoke(gotReward);
            SetIntervalInterstitial(); // reset interval Inter after view Rewarded
#if VMC_GROUP_2
            AnalysticManager.Instance.Log_AdsRewardComplete(this.rewardedPlacement);
#endif
        }
        protected void OnRewardedGotReward()
        {
            this.gotReward = true;
        }
        #endregion
    }
}