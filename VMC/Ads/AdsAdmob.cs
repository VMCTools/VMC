
#if VMC_ADMOB
using GoogleMobileAds.Api;
#endif
using System;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Debug = VMC.Debugger.Debug;

namespace VMC.Ads
{
    public class AdsAdmob : AdsController
    {
        [Header("ADMOB")]
        public string bannerId;
        public string interstitialId;
        public string rewardedVideoId;


        [ReadOnly] private string bannerIdTest = "ca-app-pub-3940256099942544/6300978111";
        [ReadOnly] private string interstitialIdTest = "ca-app-pub-3940256099942544/1033173712";
        [ReadOnly] private string rewardedVideoIdTest = "ca-app-pub-3940256099942544/5224354917";

#if VMC_ADMOB

        private BannerAdsPosition bannerPosition;
        private BannerView bannerView;
        private InterstitialAd interstitial;
        private Action onCloseInterstitial;
        private RewardedAd rewardedAd;

        private Action<bool> onCloseRewardedVideo;
        private bool gotReward;
        private bool isRewardWaiting;

#endif

        private void Start()
        {
#if VMC_ADMOB
            MobileAds.Initialize(initStatus =>
            {
                I_RequestRewardedVideo();
            });

            if (TestMode)
            {
                this.bannerId = bannerIdTest;
                this.interstitialId = interstitialIdTest;
                this.rewardedVideoId = rewardedVideoIdTest;
            }

#endif
        }
        protected override void I_SetConsentAds(bool value)
        {

        }
        protected override void I_SetVolume(float value)
        {
#if VMC_ADMOB
            MobileAds.SetApplicationVolume(value);
#endif
        }

        #region BANNER
#if VMC_ADMOB
        private AdPosition GetAdPosition(BannerAdsPosition posi)
        {
            switch (posi)
            {
                case BannerAdsPosition.TOP: return AdPosition.Top;
                case BannerAdsPosition.BOTTOM: return AdPosition.Bottom;
                default: return AdPosition.Bottom;
            }
        }
#endif
        protected override void I_RequestBanner()
        {
#if VMC_ADMOB
            // Create a smart banner at the top of the screen.
            this.bannerView = new BannerView(bannerId, AdSize.SmartBanner, GetAdPosition(bannerPosition));

            // Called when an ad request has successfully loaded.
            this.bannerView.OnAdLoaded += this.bannerView_OnAdLoaded;


            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the banner with the request.
            this.bannerView.LoadAd(request);
#endif
        }

#if VMC_ADMOB
        private void bannerView_OnAdLoaded(object sender, EventArgs args)
        {
            if (!isShowingBanner) bannerView.Hide();
        }
#endif

        protected override void I_ShowBanner(BannerAdsPosition posi = BannerAdsPosition.BOTTOM)
        {
#if VMC_ADMOB
            this.bannerPosition = posi;
#endif
            I_RequestBanner();
        }
        protected override void I_HideBanner()
        {
#if VMC_ADMOB
            bannerView.Hide();
#endif
        }
        #endregion
        #region INTERSTITIAL
        protected override void I_RequestInterstitial()
        {
#if VMC_ADMOB
            // Initialize an InterstitialAd.
            this.interstitial = new InterstitialAd(interstitialId);

            // Called when an ad request has successfully loaded.
            this.interstitial.OnAdLoaded += intersitial_OnAdLoaded;
            // Called when an ad request failed to load.
            this.interstitial.OnAdFailedToLoad += interstitial_OnAdFailedToLoad;
            // Called when the ad is closed.
            this.interstitial.OnAdClosed += interstitial_OnAdClosed;

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            this.interstitial.LoadAd(request);
#endif
        }

#if VMC_ADMOB
        private void intersitial_OnAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("[ADMOB-Intersitial]", "Loaded!");
            IsLoadedInterstitial = true;
        }
        private void interstitial_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log("[ADMOB-Intersitial]", "Failed to load intersitital ads. " + args.LoadAdError.GetMessage());
            IsLoadedInterstitial = false;
        }
        private void interstitial_OnAdClosed(object sender, EventArgs args)
        {
            Debug.Log("[ADMOB-Intersitial]", "Closed Ads!");
            onCloseInterstitial?.Invoke();
            I_RequestInterstitial();
        }
#endif

        protected override void I_ShowInterstitial(Action callback)
        {
#if VMC_ADMOB
            if (this.interstitial.IsLoaded())
            {
                onCloseInterstitial = callback;
                this.interstitial.Show();
            }
            else
            {
                callback?.Invoke();
            }
#endif
        }
        #endregion
        #region REWARDED VIDEO
        protected override void I_RequestRewardedVideo()
        {
#if VMC_ADMOB
            this.rewardedAd = new RewardedAd(rewardedVideoId);

            // Called when an ad request has successfully loaded.
            this.rewardedAd.OnAdLoaded += RewardedAd_OnAdLoaded;
            // Called when an ad request failed to load.
            this.rewardedAd.OnAdFailedToLoad += RewardedAd_OnAdFailedToLoad;
            // Called when an ad request failed to show.
            this.rewardedAd.OnAdFailedToShow += RewardedAd_OnAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            this.rewardedAd.OnUserEarnedReward += RewardedAd_OnUserEarnedReward;
            // Called when the ad is closed.
            this.rewardedAd.OnAdClosed += RewardedAd_OnAdClosed;


            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded ad with the request.
            this.rewardedAd.LoadAd(request);
#endif
        }
        protected override void I_ShowRewardedVideo(Action<bool> OnSuccessed)
        {
#if VMC_ADMOB
            if (this.rewardedAd.IsLoaded())
            {
                this.onCloseRewardedVideo = OnSuccessed;
                this.gotReward = false;
                this.isRewardWaiting = true;
                this.rewardedAd.Show();
            }
            else
            {
                OnSuccessed?.Invoke(false);
            }
#else
                OnSuccessed?.Invoke(false);
#endif
        }

#if VMC_ADMOB
        private void RewardedAd_OnAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Loaded!");
            IsLoadedRewardedVideo = true;
        }
        private void RewardedAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Failed to load RewardedVideo ads. " + e.LoadAdError.GetMessage());
            IsLoadedInterstitial = false;
        }
        private void RewardedAd_OnAdFailedToShow(object sender, AdErrorEventArgs args)
        {

            Debug.Log("[ADMOB-RewardedVideo]", "Failed to show RewardedVideo ads. " + args.AdError.GetMessage());
            gotReward = false;
            onCloseRewardedVideo?.Invoke(gotReward);
        }
        private void RewardedAd_OnAdClosed(object sender, EventArgs args)
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Close Ads");
            //onCloseRewardedVideo?.Invoke(gotReward);
            I_RequestRewardedVideo();
        }
        private void RewardedAd_OnUserEarnedReward(object sender, Reward args)
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Earn reward!");
            gotReward = true;
            //onCloseRewardedVideo?.Invoke(gotReward);
        }
        private void OnApplicationPause(bool isPaused)
        {
            if (!isPaused)
            {
                if (isRewardWaiting)
                {
                    onCloseRewardedVideo?.Invoke(gotReward);
                    isRewardWaiting = false;
                }
            }
        }
#endif
        #endregion

    }
}