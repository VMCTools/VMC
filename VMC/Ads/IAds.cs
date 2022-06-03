using System;

namespace VMC.Ads
{
    public interface IAds
    {
        void ShowBanner();
        void HideBanner();

        void ShowInterstitial(Action closeCallback);

        void ShowRewardedVideo(Action<bool> rewaredCallback);
    }
}