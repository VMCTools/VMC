using System;

namespace VMC.Ads
{
    [Flags]
    [Serializable]
    public enum AdsType 
    {
        None = 0,
        OpenAds = 1,
        Banner = 2,
        Interstitial = 4,
        RewardedVideo = 8
    }
}