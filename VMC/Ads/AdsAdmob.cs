using System;
using UnityEngine;

namespace VMC.Ads
{
    public class AdsAdmob : MonoBehaviour, IAds
    {
        public void HideBanner()
        {
        }

        public void ShowBanner()
        {
        }

        public void ShowInterstitial(Action closeCallback)
        {
            closeCallback?.Invoke();
        }

        public void ShowRewardedVideo(Action<bool> rewaredCallback)
        {
            rewaredCallback?.Invoke(true);
        }
    }
}