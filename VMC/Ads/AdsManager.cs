using System;
using VMC.Ultilities;

namespace VMC.Ads
{
    public class AdsManager : Singleton<AdsManager>
    {
        private IAds ads;
        public void ShowBanner()
        {
            ads.ShowBanner();
        }
        public void HideBanner()
        {
            ads.HideBanner();
        }

        public void ShowInterstitial(Action closeCallback)
        {
            ads.ShowInterstitial(closeCallback);
        }

        public void ShowRewardedVideo(Action<bool> rewardedCallback)
        {
#if UNITY_EDITOR
            rewardedCallback?.Invoke(true);
            return;
#endif
            ads.ShowRewardedVideo(rewardedCallback);
        }
    }
}