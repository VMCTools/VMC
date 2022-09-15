using System;

namespace VMC.Ads
{
    public interface IAds
    {
        void SetConsentAds(bool value);
        void SetVolume(float value);
        void RequestBanner();
        void ShowBanner(BannerAdsPosition posi = BannerAdsPosition.BOTTOM);
        void HideBanner();

        void RequestInterstitial();
        bool IsInterstitialAvailable();
        void ShowInterstitial(string placement, Action callback);

        bool IsRewardVideoAvailable();
        void ShowRewardedVideo(string placement, Action<bool> OnSuccessed);
    }
}