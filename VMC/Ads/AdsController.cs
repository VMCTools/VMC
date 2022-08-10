using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VMC.Ads;
using VMC.Ultilities;
#if VMC_DOTWEEN
using DG.Tweening;
#endif
using Debug = VMC.Debugger.Debug;

namespace VMC.Ads
{
    public class AdsController : Singleton<AdsController>, IAds
    {

        public bool TestMode;
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



        private float MIN_TIME_SHOWITERSTIRIAL = 90;
        private float countDownTimeInter = 0;
        private bool isCanShowInterstitial = true;

        protected bool isShowingBanner;
        protected bool IsLoadedInterstitial;
        protected bool IsLoadedRewardedVideo;

        protected override void Awake()
        {
            base.Awake();
            SetIntervalTimeForInterstitial();
            SetConsentAds(IsConsentAds);
        }

#if !VMC_DOTWEEN
        private void LateUpdate()
        {
            if (!isCanShowInterstitial)
            {
                countDownTimeInter -= Time.deltaTime;
                if (countDownTimeInter < 0)
                {
                    isCanShowInterstitial = true;
                    countDownTimeInter = MIN_TIME_SHOWITERSTIRIAL;
                }
            }
        }
#endif

        public void SetConsentAds(bool value)
        {
            I_SetConsentAds(value);
        }

        protected virtual void I_SetConsentAds(bool value)
        {
        }


        public void RequestBanner()
        {
            if (IsEnableAds)
                I_RequestBanner();
        }
        protected virtual void I_RequestBanner()
        {

        }

        public void ShowBanner(BannerAdsPosition posi = BannerAdsPosition.BOTTOM)
        {
            if (IsEnableAds)
            {
                isShowingBanner = true;
                I_ShowBanner();
            }
        }
        protected virtual void I_ShowBanner(BannerAdsPosition posi = BannerAdsPosition.BOTTOM)
        {

        }

        public void HideBanner()
        {
            isShowingBanner = false;
            I_HideBanner();
        }
        protected virtual void I_HideBanner()
        {

        }

        public void RequestInterstitial()
        {
            if (IsEnableAds)
                I_RequestInterstitial();
        }
        protected virtual void I_RequestInterstitial()
        {

        }
        public bool IsInterstitialAvailable()
        {
            if (IsEnableAds && isCanShowInterstitial)
                return IsLoadedInterstitial;
            else return false;
        }
        public void ShowInterstitial(Action callback)
        {
            if (IsEnableAds && isCanShowInterstitial && IsLoadedInterstitial)
            {
                I_ShowInterstitial(callback);
                SetIntervalTimeForInterstitial();
            }
            else
            {
                callback?.Invoke();
            }
        }
        protected virtual void I_ShowInterstitial(Action callback)
        {

        }
        private void SetIntervalTimeForInterstitial()
        {
            isCanShowInterstitial = false;
#if VMC_DOTWEEN
            DOVirtual.DelayedCall(MIN_TIME_SHOWITERSTIRIAL, () =>
            {
                isCanShowInterstitial = true;
            });
#else
                countDownTimeInter = MIN_TIME_SHOWITERSTIRIAL;
#endif
        }


        public bool IsRewardVideoAvailable()
        {
            return IsLoadedRewardedVideo;
        }
        public void ShowRewardedVideo(Action<bool> OnSuccessed)
        {
            Debug.Log("VMC Show Rewarded");
            if (IsLoadedRewardedVideo)
                I_ShowRewardedVideo(OnSuccessed);
            else
            {
                I_RequestRewardedVideo();
                OnSuccessed?.Invoke(false);
            }
        }
        protected virtual void I_ShowRewardedVideo(Action<bool> OnSuccessed)
        {

        }
        protected virtual void I_RequestRewardedVideo()
        {

        }

        public void SetVolume(float value)
        {
            I_SetVolume(value);
        }
        protected virtual void I_SetVolume(float value)
        {

        }

    }
}