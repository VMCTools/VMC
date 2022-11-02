
#if VMC_APP_REVIEW
using Google.Play.Review;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VMC.Ultilities;
using Debug = VMC.Debugger.Debug;


namespace VMC.AppReview
{
    public class AppReviewRating : SingletonAdvance<AppReviewRating>
    {
#if VMC_APP_REVIEW
        // Create instance of ReviewManager
        private ReviewManager _reviewManager;
        private PlayReviewInfo _playReviewInfo;
#endif

        public void StartRequestReview(
#if UNITY_IPHONE || UNITY_IOS
            string STORE_APP_ID
#endif
            )
        {

#if UNITY_ANDROID
#if VMC_APP_REVIEW
            StartCoroutine(RequestReview());
#else
            Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
#endif
#elif UNITY_IPHONE || UNITY_IOS
            if (!UnityEngine.iOS.Device.RequestStoreReview())
            {
                Application.OpenURL("https://apps.apple.com/app/id" + STORE_APP_ID);
            }
#endif
        }


        public IEnumerator RequestReview()
        {
#if VMC_APP_REVIEW && UNITY_ANDROID
            if (_reviewManager == null)
            {
                _reviewManager = new();
            }
            var requestFlowOperation = _reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                Debug.Log("[App Review Rate] Request", requestFlowOperation.Error.ToString());
                yield break;
            }
            _playReviewInfo = requestFlowOperation.GetResult();
            var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
            yield return launchFlowOperation;
            _playReviewInfo = null; // Reset the object
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                Debug.Log("[App Review Rate] Show", launchFlowOperation.Error.ToString());
                yield break;
            }
            // The flow has finished. The API does not indicate whether the user
            // reviewed or not, or even whether the review dialog was shown. Thus, no
            // matter the result, we continue our app flow.
#else
            yield return null;
#endif

        }
    }
}