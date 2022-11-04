using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VMC.Ads;
using VMC.Analystic;

namespace VMC.Examples
{
    public class TestSDK : MonoBehaviour
    {
        [SerializeField] private Button[] btns;
        [SerializeField] private Text txtMess;
        private void Start()
        {
            if (btns.Length > 0)
            {
                btns[0].GetComponentInChildren<Text>().text = "Show Banner";
                btns[0].onClick.AddListener(() =>
                {
                    txtMess.text = "Show Banner";
                    AdsManager.Instance.ShowBanner();
                });
            }
            if (btns.Length > 1)
            {
                btns[1].GetComponentInChildren<Text>().text = "Hide Banner";
                btns[1].onClick.AddListener(() =>
                {
                    txtMess.text = "Hide Banner";
                    AdsManager.Instance.HideBanner();
                });
            }
            if (btns.Length > 2)
            {
                btns[2].GetComponentInChildren<Text>().text = "Show Inters";
                btns[2].onClick.AddListener(() =>
                {
                    txtMess.text = "Show Inter";
                    AdsManager.Instance.ShowInterstitial("Test", () =>
                    {
                        txtMess.text += "\nShow Inter Complete";
                        Debug.Log("Close inters");
                    });
                });
            }
            if (btns.Length > 3)
            {
                btns[3].GetComponentInChildren<Text>().text = "Show Rewarded";
                btns[3].onClick.AddListener(() =>
                {
                    txtMess.text = "Show Rewarded";
                    AdsManager.Instance.ShowRewardedVideo("Test", (result) =>
                    {
                        txtMess.text += "\nShow Rewarded callback: " + result;
                        Debug.Log("Close rewarded and got reward? " + result);
                    });
                });
            }
            if (btns.Length > 4)
            {
                btns[4].GetComponentInChildren<Text>().text = "Logevent Normal";
                btns[4].onClick.AddListener(() =>
                {
                    AnalysticManager.Instance.LogEvent("test");
                    Debug.Log("Log event test!");
                });
            }
            if (btns.Length > 5)
            {
                btns[5].GetComponentInChildren<Text>().text = "Logevent Level Start";
                btns[5].onClick.AddListener(() =>
                {

#if VMC_GROUP_1
                    AnalysticManager.Instance.Log_LevelStart(1);
#elif VMC_GROUP_2
                    AnalysticManager.Instance.Log_LevelStart(1, 0);
#endif
                    Debug.Log("Log event level start!");
                });
            }
        }
    }
}