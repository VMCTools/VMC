
#if VMC_ADS_ADMOB
using GoogleMobileAds.Api;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Debug = VMC.Debugger.Debug;

namespace VMC.Ads
{
    public class AdsAdmob : AdsMediation
    {
        [Header("ID Real")]
#if UNITY_ANDROID
        [ReadOnly] public string openAdsId = "YOUR_OPENADS_ID_ADS_HERE";
        [ReadOnly] public string bannerId = "YOUR_BANNER_ID_ADS_HERE";
        [ReadOnly] public string interstitialId = "YOUR_INTERS_ID_ADS_HERE";
        [ReadOnly] public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#elif UNITY_IOS
        [ReadOnly] public string openAdsId = "YOUR_OPENADS_ID_ADS_HERE";
        [ReadOnly] public string bannerId="YOUR_BANNER_ID_ADS_HERE";
        [ReadOnly] public string interstitialId="YOUR_INTERS_ID_ADS_HERE";
        [ReadOnly] public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#endif
#if VMC_ADS_ADMOB

        [Header("ID TEST")]
#if UNITY_ANDROID
        [ReadOnly] private string openAdsIdTest = "ca-app-pub-3940256099942544/3419835294";
        [ReadOnly] private string bannerIdTest = "ca-app-pub-3940256099942544/6300978111";
        [ReadOnly] private string interstitialIdTest = "ca-app-pub-3940256099942544/1033173712";
        [ReadOnly] private string rewardedVideoIdTest = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
        [ReadOnly] private string openAdsIdTest = "ca-app-pub-3940256099942544/5662855259";
        [ReadOnly] private string bannerIdTest = "ca-app-pub-3940256099942544/2934735716";
        [ReadOnly] private string interstitialIdTest = "ca-app-pub-3940256099942544/4411468910";
        [ReadOnly] private string rewardedVideoIdTest = "ca-app-pub-3940256099942544/1712485313";
#endif
        private AdsType adType;

        private BannerView bannerView;
        private InterstitialAd interstitial;
        private RewardedAd rewardedAd;

#endif
        public override void Initialize()
        {
#if VMC_ADS_ADMOB
            Settings.VMCSettingConfig config = Settings.VMCSettingConfig.LoadData();
            if (config.isTestMode)
            {
                this.openAdsId = this.openAdsIdTest;
                this.bannerId = this.bannerIdTest;
                this.interstitialId = this.interstitialIdTest;
                this.rewardedVideoId = this.rewardedVideoIdTest;
            }
            else
            {
                this.bannerId = config.bannerId;
                this.interstitialId = config.interstitialId;
                this.rewardedVideoId = config.rewardedVideoId;
            }
            this.adType = config.adType;

            var deviceTest = new List<string> { "377807EAA67F63DF0FFE8F146CF568C7" };
            RequestConfiguration requestConfiguration = new RequestConfiguration.Builder().SetTestDeviceIds(deviceTest).build();
            MobileAds.SetRequestConfiguration(requestConfiguration);

            MobileAds.Initialize(initStatus =>
            {
                if (adType.HasFlag(AdsType.Banner))
                    InitializeBannerAds();
                if (adType.HasFlag(AdsType.Interstitial))
                    InitializeInterstitialAds();
                if (adType.HasFlag(AdsType.RewardedVideo))
                    InitializeRewardedVideoAds();
            });
#endif
            base.Initialize();
        }

        public override void SetVolume(float value)
        {
            base.SetVolume(value);
#if VMC_ADS_ADMOB
            MobileAds.SetApplicationVolume(value);
#endif
        }

        #region BANNER
#if VMC_ADS_ADMOB
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
        public override void InitializeBannerAds()
        {
            base.InitializeBannerAds();
            //LoadBannerAds();
        }
        public override void LoadBannerAds()
        {
            base.LoadBannerAds();
#if VMC_ADS_ADMOB

            if (this.bannerView != null)
                this.bannerView.Destroy();
            // Create a smart banner at the top of the screen.
            this.bannerView = new BannerView(bannerId, AdSize.SmartBanner, GetAdPosition(bannerPosition));

            // Called when an ad request has successfully loaded.
            this.bannerView.OnAdLoaded += this.bannerView_OnAdLoaded;

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the banner with the request.
            this.bannerView.LoadAd(request);
            Debug.LogError("load");
#endif
        }
        public override void ShowBannerAds(BannerAdsPosition posi = BannerAdsPosition.BOTTOM)
        {
            base.ShowBannerAds(posi);
#if VMC_ADS_ADMOB
            if (bannerView != null)
            {
                bannerView.Show();
            }
            else
            {
                Debug.LogError("reload");
                LoadBannerAds();
            }
#endif
        }
        public override void HideBannerAds()
        {
            base.HideBannerAds();
#if VMC_ADS_ADMOB
            if (bannerView != null)
                bannerView.Hide();
#endif
        }
#if VMC_ADS_ADMOB
        private void bannerView_OnAdLoaded(object sender, EventArgs args)
        {
            this.OnLoadBannerSuccessed();
        }
#endif
        #endregion



        #region INTERSTITIAL
        public override void InitializeInterstitialAds()
        {
            base.InitializeInterstitialAds();
            LoadInterstitialAds();
        }
        public override void LoadInterstitialAds()
        {
            base.LoadInterstitialAds();
#if VMC_ADS_ADMOB
            // Initialize an InterstitialAd.
            this.interstitial = new InterstitialAd(interstitialId);

            // Called when an ad request has successfully loaded.
            this.interstitial.OnAdLoaded += intersitial_OnAdLoaded;
            // Called when an ad request failed to load.
            this.interstitial.OnAdFailedToLoad += interstitial_OnAdFailedToLoad;
            // Called when the ad is closed.
            this.interstitial.OnAdClosed += interstitial_OnAdClosed;
            // Called when the ad is failed to show.
            this.interstitial.OnAdFailedToShow += Interstitial_OnAdFailedToShow;

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            this.interstitial.LoadAd(request);
#endif
        }
        public override void ShowInterstitialAds(string placement, Action callback)
        {
            base.ShowInterstitialAds(placement, callback);
#if VMC_ADS_ADMOB
            if (this.interstitial == null)
            {
                LoadInterstitialAds();
                callback?.Invoke();
            }
            else if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
            else
            {
                callback?.Invoke();
            }
#endif
        }

#if VMC_ADS_ADMOB
        private void intersitial_OnAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("[ADMOB-Intersitial]", "Loaded!");
            this.OnInterstititalLoadSuccessed();
        }
        private void interstitial_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log("[ADMOB-Intersitial]", "Failed to load intersitital ads. " + args.LoadAdError.GetMessage());
            this.OnInterstitialLoadFailed();
        }
        private void interstitial_OnAdClosed(object sender, EventArgs args)
        {
            Debug.Log("[ADMOB-Intersitial]", "Closed Ads!");
            this.OnInterstitialDisplaySuccessed();
        }
        private void Interstitial_OnAdFailedToShow(object sender, AdErrorEventArgs e)
        {
            Debug.Log("[ADMOB-Intersitial]", "Failed to show Ads!");
            this.OnInterstitialDisplayFailed();
        }
#endif
        #endregion
        #region REWARDED VIDEO
        public override void InitializeRewardedVideoAds()
        {
            base.InitializeRewardedVideoAds();
            LoadRewardedVideo();
        }
        public override void LoadRewardedVideo()
        {
            base.LoadRewardedVideo();
#if VMC_ADS_ADMOB
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
        public override void ShowRewardedVideo(string placement, Action<bool> callback)
        {
            base.ShowRewardedVideo(placement, callback);
#if VMC_ADS_ADMOB
            if (this.rewardedAd.IsLoaded())
            {
                this.rewardedAd.Show();
            }
            else
            {
                callback?.Invoke(false);
            }
#else
            callback?.Invoke(false);
#endif
        }

#if VMC_ADS_ADMOB
        private void RewardedAd_OnAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Loaded!");
            this.OnRewardedLoadSuccessed();
        }
        private void RewardedAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Failed to load RewardedVideo ads. " + e.LoadAdError.GetMessage());
            this.OnRewardedLoadFailed();
        }
        private void RewardedAd_OnAdFailedToShow(object sender, AdErrorEventArgs args)
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Failed to show RewardedVideo ads. " + args.AdError.GetMessage());
            this.OnRewardedDisplayFailed();
        }
        private void RewardedAd_OnAdClosed(object sender, EventArgs args)
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Close Ads");
            this.OnRewardedDisplaySuccessed();
        }
        private void RewardedAd_OnUserEarnedReward(object sender, Reward args)
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Earn reward!");
            this.OnRewardedGotReward();
        }
#endif
        #endregion

    }
}