
#if VMC_ANALYZE_APPFLYER
using AppsFlyerSDK;
using System;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Debug = VMC.Debugger.Debug;

namespace VMC.Analystic
{
    public class AppsFlyerAnalystic : MonoBehaviour, IAnalystic

#if VMC_ANALYZE_APPFLYER
        , IAppsFlyerConversionData
#endif
    {
        [SerializeField, ReadOnly] private string AF_Dev_Key;
        [SerializeField, ReadOnly] private string AF_App_Id;


        public void Initialize()
        {
            Debug.Log("[Analystic]", "Init AppsFlyer!");
#if VMC_ANALYZE_APPFLYER
            VMC.Settings.VMCSettingConfig config = VMC.Settings.VMCSettingConfig.LoadData();
            AF_Dev_Key = config.AF_Dev_Key;
            AF_App_Id = config.AF_App_Id;

            AppsFlyer.initSDK(AF_Dev_Key, AF_App_Id, this);
            AppsFlyer.startSDK();
#endif
        }
        public void LogEvent(string nameEvent)
        {
            // Not show event
        }
        public void LogEvent(string nameEvent, Dictionary<string, string> param)
        {
            // Not show event
        }

#if VMC_ANALYZE_APPFLYER
        public void onAppOpenAttribution(string attributionData)
        {
        }

        public void onAppOpenAttributionFailure(string error)
        {
        }

        public void onConversionDataFail(string error)
        {
        }

        public void onConversionDataSuccess(string conversionData)
        {
        }
#endif
    }
}