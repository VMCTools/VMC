using System;

namespace VMC.Ads
{
    public interface IAds
    {
        void Initialize();
        void SetConsentAds(bool value);
        void SetVolume(float value);


        void InitializeBannerAds();
        void LoadBannerAds();
        void ShowBannerAds(BannerAdsPosition posi = BannerAdsPosition.BOTTOM);
        void HideBannerAds();



        void InitializeInterstitialAds();
        void LoadInterstitialAds();
        bool IsInterstitialAvailable();
        void ShowInterstitialAds(string placement, Action callback);


        void InitializeRewardedVideoAds();
        void LoadRewardedVideo();
        bool IsRewardVideoAvailable();
        void ShowRewardedVideo(string placement, Action<bool> OnSuccessed);
    }
}