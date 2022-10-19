using System;
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
        void Start()
        {
            if (isInited)
            {
                isCounting = false;
                SceneManager.LoadScene(sceneName);
                return;
            }
            else
            {
                Debug.Log("[Fake Loading]", "Init all libraries!");
                isCounting = true;
                countTimeFakeLoading = 0f;
                InitStep1();
                Invoke(nameof(InitStep2), fakeLoadingTime / 7);
            }
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
                    InitStep3();
                    isInited = true;
                    SceneManager.LoadScene(sceneName);
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
            // init AOA
            VMC.Ads.AdsManager.Instance.InitializeAOA();
        }
        private void InitStep3()
        {
            // init Firebase
            VMC.Analystic.AnalysticManager.Instance.InitializeFirebase();
        }
    }
}