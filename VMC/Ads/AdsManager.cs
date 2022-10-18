using System;
using UnityEngine;
using UnityEngine.UI.Extensions;
using VMC.Ultilities;

namespace VMC.Ads
{
    public class AdsManager : VMC.Ultilities.Singleton<AdsManager>
    {
        [SerializeField, ReadOnly] private bool enableAds;
        [SerializeField, ReadOnly] private AdsMediation ads;
        [SerializeField, ReadOnly] private AdsAdmobOpenAds admobOpenAds;

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;
            Settings.VMCSettingConfig config = Settings.VMCSettingConfig.LoadData();
            this.enableAds = config.enableAds;
            if (config.enableAds)
            {
                if (config.adsLibrary.HasFlag(Settings.AdsLibrary.MaxMediation))
                {
                    var max = (new GameObject("Max Mediation")).AddComponent<AdsMaxMediation>();
                    max.transform.SetParent(this.transform);
                    ads = max;
                }
                else if (config.adsLibrary.HasFlag(Settings.AdsLibrary.Admob))
                {
                    var admob = (new GameObject("Admob Mediation")).AddComponent<AdsAdmob>();
                    admob.transform.SetParent(this.transform);
                    ads = admob;
                }

                if (config.adType.HasFlag(AdsType.OpenAds))
                {
                    admobOpenAds = (new GameObject("Admob OpenAds")).AddComponent<AdsAdmobOpenAds>();
                    admobOpenAds.transform.SetParent(this.transform);
                }
            }
        }

        public void Initialize()
        {
            if (ads != null)
                ads.Initialize();
        }
        public void InitializeAOA()
        {
            if (admobOpenAds != null)
                admobOpenAds.Initialize();
        }

        private bool CheckValidate()
        {
            if (!enableAds)
            {
                VMC.Debugger.Debug.Log("[ADS]", "Not Enable Ads");
                return false;
            }
            if (ads == null)
            {
                VMC.Debugger.Debug.Log("[ADS]", "None of Ads are available!");
                return false;
            }
            return true;
        }
        public void ShowBanner()
        {
            if (!CheckValidate()) return;
            VMC.Debugger.Debug.Log("[ADS]", "Show banner");
            ads.ShowBannerAds();
        }
        public void HideBanner()
        {
            if (!CheckValidate()) return;
            VMC.Debugger.Debug.Log("[ADS]", "Hide banner");
            ads.HideBannerAds();
        }

        public void ShowInterstitial(string placement, Action closeCallback)
        {
            if (!CheckValidate())
            {
#if UNITY_EDITOR
                VMC.Debugger.Debug.Log("[ADS]", "UnityEditor Fake Show interstitial");
                closeCallback?.Invoke();
                return;
#else
                return;
#endif
            }
            VMC.Debugger.Debug.Log("[ADS]", "Show interstitial");
            ads.ShowInterstitialAds(placement, closeCallback);
        }

        public void ShowRewardedVideo(string placement, Action<bool> rewardedCallback)
        {
            if (!CheckValidate())
            {
#if UNITY_EDITOR
                VMC.Debugger.Debug.Log("[ADS]", "UnityEditor Fake Show rewardedVideo");
                rewardedCallback?.Invoke(true);
                return;
#else
                if(Debug.isDebugBuild)
                {
                    VMC.Debugger.Debug.Log("[ADS]", "UnityDebugBuild Fake Show rewardedVideo");
                    rewardedCallback?.Invoke(true);
                    return;
                }
                return;
#endif
            }
            VMC.Debugger.Debug.Log("[ADS]", "Show rewarded video");
            ads.ShowRewardedVideo(placement, rewardedCallback);
        }
    }
}