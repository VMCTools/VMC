using System;
using UnityEngine;
using UnityEngine.UI.Extensions;
using VMC.Ultilities;

namespace VMC.Ads
{
    public class AdsManager : Singleton<AdsManager>
    {
        [SerializeField, ReadOnly] private bool enableAds;
        [SerializeField, ReadOnly] private AdsMediation ads;
        protected override void Awake()
        {
            base.Awake();

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
                    var admobOpenAds = (new GameObject("Admob OpenAds")).AddComponent<AdsAdmobOpenAds>();
                    admobOpenAds.transform.SetParent(this.transform);
                }
            }
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
            if (!CheckValidate()) return;
            VMC.Debugger.Debug.Log("[ADS]", "Show interstitial");
            ads.ShowInterstitialAds(placement, closeCallback);
        }

        public void ShowRewardedVideo(string placement, Action<bool> rewardedCallback)
        {
            if (!CheckValidate()) return;
            VMC.Debugger.Debug.Log("[ADS]", "Show rewarded video");
            ads.ShowRewardedVideo(placement, rewardedCallback);
        }
    }
}