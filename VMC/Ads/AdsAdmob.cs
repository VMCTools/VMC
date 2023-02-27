
#if VMC_ADS_ADMOB
using GoogleMobileAds.Api;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using VMC.Analystic;
using Debug = VMC.Debugger.Debug;

namespace VMC.Ads
{
    public class AdsAdmob : AdsMediation
    {
#if VMC_ADS_ADMOB
#if UNITY_ANDROID
        [Header("ID Real")]
        [ReadOnly] public string openAdsId = "YOUR_OPENADS_ID_ADS_HERE";
        [ReadOnly] public string bannerId = "YOUR_BANNER_ID_ADS_HERE";
        [ReadOnly] public string interstitialId = "YOUR_INTERS_ID_ADS_HERE";
        [ReadOnly] public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#elif UNITY_IOS
        [Header("ID Real")]
        [ReadOnly] public string openAdsId = "YOUR_OPENADS_ID_ADS_HERE";
        [ReadOnly] public string bannerId="YOUR_BANNER_ID_ADS_HERE";
        [ReadOnly] public string interstitialId="YOUR_INTERS_ID_ADS_HERE";
        [ReadOnly] public string rewardedVideoId = "YOUR_REWARDED_ID_ADS_HERE";
#endif

#if UNITY_ANDROID
        [Header("ID TEST")]
        [ReadOnly] private string openAdsIdTest = "ca-app-pub-3940256099942544/3419835294";
        [ReadOnly] private string bannerIdTest = "ca-app-pub-3940256099942544/6300978111";
        [ReadOnly] private string interstitialIdTest = "ca-app-pub-3940256099942544/1033173712";
        [ReadOnly] private string rewardedVideoIdTest = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
        [Header("ID TEST")]
        [ReadOnly] private string openAdsIdTest = "ca-app-pub-3940256099942544/5662855259";
        [ReadOnly] private string bannerIdTest = "ca-app-pub-3940256099942544/2934735716";
        [ReadOnly] private string interstitialIdTest = "ca-app-pub-3940256099942544/4411468910";
        [ReadOnly] private string rewardedVideoIdTest = "ca-app-pub-3940256099942544/1712485313";
#endif
        private BannerView bannerView;
        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;
#endif
        public override void Initialize()
        {
            base.Initialize();
#if VMC_ADS_ADMOB
#if VMC_ADS_TESTMODE
            this.openAdsId = this.openAdsIdTest;
            this.bannerId = this.bannerIdTest;
            this.interstitialId = this.interstitialIdTest;
            this.rewardedVideoId = this.rewardedVideoIdTest;
#else
            this.bannerId = config.bannerId;
            this.interstitialId = config.interstitialId;
            this.rewardedVideoId = config.rewardedVideoId;
#endif
            this.adsType = config.adType;

            var deviceTest = new List<string> { "377807EAA67F63DF0FFE8F146CF568C7" };
            RequestConfiguration requestConfiguration = new RequestConfiguration.Builder().SetTestDeviceIds(deviceTest).build();
            MobileAds.SetRequestConfiguration(requestConfiguration);

            MobileAds.Initialize(initStatus =>
            {
                if (adsType.HasFlag(AdsType.Banner))
                    InitializeBannerAds();
                if (adsType.HasFlag(AdsType.Interstitial))
                    InitializeInterstitialAds();
                if (adsType.HasFlag(AdsType.RewardedVideo))
                    InitializeRewardedVideoAds();
            });
#endif
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
            this.bannerView.OnBannerAdLoaded += BannerView_OnBannerAdLoaded; ;
            this.bannerView.OnBannerAdLoadFailed += BannerView_OnBannerAdLoadFailed;

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the banner with the request.
            this.bannerView.LoadAd(request);
#endif
        }

        public override float GetBannerHeight()
        {
#if VMC_ADS_ADMOB
            if (bannerView == null)
                return 0;
            return bannerView.GetHeightInPixels();
#else
            return 0;
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
        private void BannerView_OnBannerAdLoaded()
        {
            this.OnLoadBannerSuccessed();
        }
        private void BannerView_OnBannerAdLoadFailed(LoadAdError obj)
        {
            this.OnLoadBannerFailed(obj.GetCode().ToString(), obj.GetMessage());
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
            // Clean up the old ad before loading a new one.
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }
            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder().Build();

            // send the request to load the ad.
            InterstitialAd.Load(interstitialId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.Log("[ADMOB-Intersitial]", "Failed to load intersitital ads. " + error.GetMessage());
                        Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            this.OnInterstitialLoadFailed(error.GetMessage());
                        });
                        return;
                    }
                    else
                    {
                        Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(this.OnInterstititalLoadSuccessed);
                    }
                    interstitialAd = ad;

                    interstitialAd.OnAdFullScreenContentFailed += Ad_OnAdFullScreenContentFailed;
                    interstitialAd.OnAdFullScreenContentClosed += Ad_OnAdFullScreenContentClosed;
                    interstitialAd.OnAdImpressionRecorded += Ad_OnAdImpressionRecorded;
                });
#endif
        }

        public override void ShowInterstitialAds(string placement, Action callback)
        {
            base.ShowInterstitialAds(placement, callback);
            if (!IsCanShowInterstitial) return;
#if VMC_ADS_ADMOB
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                interstitialAd.Show();
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
                LoadInterstitialAds();
                callback?.Invoke();
            }
#endif
        }

#if VMC_ADS_ADMOB
        private void Ad_OnAdFullScreenContentClosed()
        {
            Debug.Log("[ADMOB-Intersitial]", "Closed Ads!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnInterstitialDisplaySuccessed();
            });
        }
        private void Ad_OnAdFullScreenContentFailed(AdError obj)
        {
            Debug.Log("[ADMOB-Intersitial]", "Failed to show Ads!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnInterstitialDisplayFailed(obj.GetMessage());
            });
        }

        private void Ad_OnAdImpressionRecorded()
        {
            Debug.Log("[ADMOB-Intersitial]", "Cliked!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnInterstitialClicked();
            });
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
            // Clean up the old ad before loading a new one.
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder().Build();

            // send the request to load the ad.
            RewardedAd.Load(rewardedVideoId, adRequest, (RewardedAd ad, LoadAdError error) =>
                 {
                     // if error is not null, the load request failed.
                     if (error != null || ad == null)
                     {
                         Debug.Log("[ADMOB-RewardedVideo]", "Failed to load RewardedVideo ads. " + error.GetMessage());
                         Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
                         {
                             this.OnRewardedLoadFailed(error.GetMessage());
                         });
                         return;
                     }
                     else
                     {
                         Debug.Log("[ADMOB-RewardedVideo]", "Loaded!");
                         Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(this.OnRewardedLoadSuccessed);
                     }

                     rewardedAd = ad;
                     rewardedAd.OnAdFullScreenContentFailed += RewardedAd_OnAdFullScreenContentFailed;
                     rewardedAd.OnAdFullScreenContentClosed += RewardedAd_OnAdFullScreenContentClosed;
                     rewardedAd.OnAdPaid += RewardedAd_OnAdPaid;
                     rewardedAd.OnAdImpressionRecorded += RewardedAd_OnAdImpressionRecorded;
                 });
#endif
        }

        public override void ShowRewardedVideo(string placement, Action<bool> callback)
        {
            base.ShowRewardedVideo(placement, callback);
#if VMC_ADS_ADMOB
            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                rewardedAd.Show((Reward reward) =>
                {
                    // TODO: Reward the user.
                    // nothing handle this, because it call in callback function
                });
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
        private void RewardedAd_OnAdFullScreenContentFailed(AdError obj)
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Failed to show RewardedVideo ads. " + obj.GetMessage());
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                this.OnRewardedDisplayFailed(obj.GetMessage());
            });
        }
        private void RewardedAd_OnAdFullScreenContentClosed()
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Close Ads");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(this.OnRewardedDisplaySuccessed);
        }
        private void RewardedAd_OnAdPaid(AdValue obj)
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Earn reward!");
            this.OnRewardedGotReward();
        }
        private void RewardedAd_OnAdImpressionRecorded()
        {
            Debug.Log("[ADMOB-RewardedVideo]", "Impression Recorded!");
            Ultilities.UnityMainThreadDispatcher.Instance().Enqueue(this.OnRewardedClicked);
        }
#endif
        #endregion

    }
}