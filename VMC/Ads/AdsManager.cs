using System;
using VMC.Ultilities;

namespace VMC.Ads
{
    public class AdsManager : Singleton<AdsManager>
    {
        public IAds ads;
        protected override void Awake()
        {
            base.Awake();
            ads = GetComponent<IAds>();
        }
        public void ShowBanner()
        {
            VMC.Debugger.Debug.Log("[ADS]", "Show banner");
            ads.ShowBanner();
        }
        public void HideBanner()
        {
            VMC.Debugger.Debug.Log("[ADS]", "Hide banner");
            ads.HideBanner();
        }

        public void ShowInterstitial(string placement, Action closeCallback)
        {
            VMC.Debugger.Debug.Log("[ADS]", "Show interstitial");
            ads.ShowInterstitial(placement, closeCallback);
        }

        public void ShowRewardedVideo(string placement, Action<bool> rewardedCallback)
        {
            VMC.Debugger.Debug.Log("[ADS]", "Show rewarded video");
#if UNITY_EDITOR
            rewardedCallback?.Invoke(true);
            return;
#endif
            ads.ShowRewardedVideo(placement, rewardedCallback);
        }
    }
}