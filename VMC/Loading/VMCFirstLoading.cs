using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;
using Debug = VMC.Debugger.Debug;

namespace VMC
{
    public class VMCFirstLoading : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        [SerializeField] private float fakeLoadingTime = 7f;
        [SerializeField, ReadOnly] private float countTimeFakeLoading;
        [SerializeField, ReadOnly] private bool isCounting;
        // Start is called before the first frame update
        public event Action<float> OnProgressLoading;
        private static bool isInited = false;

        [SerializeField, ReadOnly] private bool isLoading;
        AsyncOperation asyncLoad;
        void Start()
        {
            if (isInited)
            {
                isCounting = false;
                StartCoroutine(LoadYourAsyncScene());
                return;
            }
            else
            {
                Debug.Log("[Fake Loading]", "Init all libraries!");
                isCounting = true;
                countTimeFakeLoading = 0f;
                StartCoroutine(LoadYourAsyncScene());
                InitStep1();
                Invoke(nameof(InitStep2), fakeLoadingTime * .2f);
                Invoke(nameof(InitStep3), fakeLoadingTime * .8f);
            }

        }
        IEnumerator LoadYourAsyncScene()
        {
            isLoading = true;
            asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                if (!isCounting && asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
            isLoading = asyncLoad.isDone;
        }
        // Update is called once per frame
        void LateUpdate()
        {
            if (isCounting)
            {
                countTimeFakeLoading += Time.deltaTime;
                if (countTimeFakeLoading >= fakeLoadingTime)
                {
                    isCounting = false;
                    isInited = true;
                    OnProgressLoading?.Invoke(1);
                }
                else
                {
                    OnProgressLoading?.Invoke(countTimeFakeLoading / fakeLoadingTime);
                }
            }
        }

        private void InitStep1()
        {
            // init Ads
            VMC.Ads.AdsManager.Instance.Initialize();
            // init Appflyer
            VMC.Analystic.AnalysticManager.Instance.InitializeAppflyer();
        }
        private void InitStep2()
        {
            // init Firebase
            VMC.Ads.AdsManager.Instance.InitializeAOA();
        }
        private void InitStep3()
        {
            // init AOA
            VMC.Analystic.AnalysticManager.Instance.InitializeFirebase();
            VMC.Ads.AdsManager.Instance.ShowAppOpenAds();
        }
    }
}