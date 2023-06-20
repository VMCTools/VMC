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

        private int retryBannerAttempt;
        private bool IsLoadedBanner;

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


        public static event Action<BannerState, string, string> OnBannerStateChange;

        public enum BannerState
        {
            Load,
            LoadSuccessed,
            LoadFailed
        }
        public static event Action<IntersState, string, string> OnInterStateChange;
        public enum IntersState
        {
            Load,
            LoadSuccessed,
            LoadFailed,
            Show,
            ShowSuccessed,
            ShowFailed
        }
        public static event Action<RewardedState, string, string> OnRewardedStateChange;
        public enum RewardedState
        {
            Load,
            LoadSuccessed,
            LoadFailed,
            Show,
            ShowSuccessed,
            ShowFailed
        }

        #region Config
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
        #endregion

        #region Open Ads
        public virtual void ShowAppOpenAds()
        {
            Debug.Log("Show App Open Ads");
        }
        #endregion

        #region BANNER
        public virtual void InitializeBannerAds() { }
        public virtual void LoadBannerAds()
        {
            IsLoadedBanner = false;
            OnBannerStateChange?.Invoke(BannerState.Load, string.Empty, string.Empty);
        }
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
            retryBannerAttempt = 0;
            IsLoadedBanner = true;
            if (isShowingBanner) ShowBannerAds(this.bannerPosition);
            else HideBannerAds();
            OnBannerStateChange?.Invoke(BannerState.LoadSuccessed, string.Empty, string.Empty);
        }
        protected void OnLoadBannerFailed(string code, string message)
        {
            IsLoadedBanner = false;
            retryBannerAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, retryBannerAttempt));
            Invoke(nameof(LoadBannerAds), (float)retryDelay);
            OnBannerStateChange?.Invoke(BannerState.LoadFailed, code, message);
        }

        public virtual float GetBannerHeight()
        {
            return 0;
        }


        #endregion

        #region INTERSTITIAL
        public virtual void InitializeInterstitialAds() { }
        public virtual void LoadInterstitialAds()
        {
            IsLoadedInterstitial = false;
            OnInterStateChange?.Invoke(IntersState.Load, string.Empty, string.Empty);
        }
        public bool IsInterstitialAvailable()
        {
            return IsLoadedInterstitial;
        }
        public virtual void ShowInterstitialAds(string placement, Action callback = null)
        {
            if (!IsEnableAds)
            {
                VMC.Debugger.Debug.Log("[ADS]", "Cant Show interstitial because !IsEnableAds");
                callback?.Invoke();
                return;
            }
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
                VMC.Debugger.Debug.Log("[ADS]", "Real show");
                OnInterStateChange?.Invoke(IntersState.Show, placement, string.Empty);
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
            OnInterStateChange?.Invoke(IntersState.LoadSuccessed, string.Empty, string.Empty);
        }
        protected void OnInterstitialLoadFailed(string errorMsg)
        {
            retryInterAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, retryInterAttempt));
            Invoke(nameof(LoadInterstitialAds), (float)retryDelay);
            OnInterStateChange?.Invoke(IntersState.LoadFailed, string.Empty, errorMsg);
        }
        protected void OnInterstitialDisplayFailed(string errorMsg)
        {
            LoadInterstitialAds();
            SetWatchingAds(false);
            interstitialCallback?.Invoke();
            OnInterStateChange?.Invoke(IntersState.ShowFailed, interstitialPlacement, string.Empty);
        }
        protected void OnInterstitialDisplaySuccessed()
        {
            LoadInterstitialAds();
            SetWatchingAds(false);
            interstitialCallback?.Invoke();
            SetIntervalInterstitial(); // reset interval Inter after view Inter
            OnInterStateChange?.Invoke(IntersState.ShowSuccessed, interstitialPlacement, string.Empty);
        }
        protected void OnInterstitialClicked()
        {
        }

        #endregion

        #region REWARDED VIDEO
        public virtual void InitializeRewardedVideoAds() { }
        public virtual void LoadRewardedVideo()
        {
            IsLoadedRewardedVideo = false;
            OnRewardedStateChange?.Invoke(RewardedState.Load, string.Empty, string.Empty);
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
                OnRewardedStateChange?.Invoke(RewardedState.Show, placement, string.Empty);
            }
        }
        protected void OnRewardedLoadSuccessed()
        {
            IsLoadedRewardedVideo = true;
            retryRewardedAttempt = 0;
            OnRewardedStateChange?.Invoke(RewardedState.LoadSuccessed, string.Empty, string.Empty);
        }
        protected void OnRewardedLoadFailed(string errorMsg)
        {
            retryRewardedAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, retryRewardedAttempt));
            Invoke(nameof(LoadRewardedVideo), (float)retryDelay);
            OnRewardedStateChange?.Invoke(RewardedState.LoadFailed, string.Empty, string.Empty);
        }
        protected void OnRewardedDisplayFailed(string errorMsg)
        {
            LoadRewardedVideo();
            SetWatchingAds(false);
            rewardedCallback?.Invoke(gotReward);
            OnRewardedStateChange?.Invoke(RewardedState.ShowFailed, rewardedPlacement, string.Empty);
        }
        protected void OnRewardedClicked()
        {
        }

        protected void OnRewardedDisplaySuccessed()
        {
            LoadRewardedVideo();
            SetWatchingAds(false);
            rewardedCallback?.Invoke(gotReward);
            SetIntervalInterstitial(); // reset interval Inter after view Rewarded
            OnRewardedStateChange?.Invoke(RewardedState.ShowSuccessed, rewardedPlacement, string.Empty);
        }
        protected void OnRewardedGotReward()
        {
            this.gotReward = true;
        }
        #endregion
    }
}