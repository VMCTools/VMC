using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VMC.Settings
{
    //[CreateAssetMenu(fileName = "VMC Settings", menuName ="VMC/VMC Settings")]
    public class VMCSetting : EditorWindow
    {
        private VMCSettingConfig config;

        private void OnEnable()
        {
            config = VMCSettingConfig.LoadData();
        }
        // Add a menu item named "Do Something" to MyMenu in the menu bar.
        [MenuItem("VMC/Setting")]
        static void DoSomething()
        {
            // Get existing open window or if none, make a new one:
            VMCSetting window = (VMCSetting)EditorWindow.GetWindow(typeof(VMCSetting), true, "VMC Settings");
            window.Show();
        }
        void OnGUI()
        {
            GUILayout.Space(20);
            GUILayout.Label("Ads Config", EditorStyles.boldLabel);
            GUILayout.Space(5);
            config.enableAds = EditorGUILayout.BeginToggleGroup("Enable Ads", config.enableAds);
            config.adType = (Ads.AdsType)EditorGUILayout.EnumFlagsField("Ads Type", config.adType);
            config.adsLibrary = (AdsLibrary)EditorGUILayout.EnumFlagsField("Ads Library", config.adsLibrary);

            if (config.enableAds && !config.adsLibrary.Equals(AdsLibrary.None))
            {
                config.isTestMode = EditorGUILayout.Toggle("Test Mode", config.isTestMode);
                GUILayout.Space(10);
                if (config.adsLibrary.HasFlag(AdsLibrary.MaxMediation))
                {
                    config.maxAppId = EditorGUILayout.TextField("App Id Max", config.maxAppId);
                    GUILayout.Space(10);
                }

                if (config.adType.HasFlag(Ads.AdsType.OpenAds))
                {
                    config.openAdsId_Tier1 = EditorGUILayout.TextField("OpenAds Id Tier1", config.openAdsId_Tier1);
                    config.openAdsId_Tier2 = EditorGUILayout.TextField("OpenAds Id Tier2", config.openAdsId_Tier2);
                    config.openAdsId_Tier3 = EditorGUILayout.TextField("OpenAds Id Tier3", config.openAdsId_Tier3);
                    GUILayout.Space(10);
                }
                if (config.adType.HasFlag(Ads.AdsType.Banner))
                {
                    config.bannerId = EditorGUILayout.TextField("Banner Id Ads", config.bannerId);
                    config.bannerPosition = (Ads.BannerAdsPosition)EditorGUILayout.EnumPopup("Banner Position", config.bannerPosition);
                    GUILayout.Space(10);
                }
                if (config.adType.HasFlag(Ads.AdsType.Interstitial))
                {
                    config.interstitialId = EditorGUILayout.TextField("Interstitial Id Ads", config.interstitialId);
                    GUILayout.Space(10);
                }
                if (config.adType.HasFlag(Ads.AdsType.RewardedVideo))
                {
                    config.rewardedVideoId = EditorGUILayout.TextField("Rewarded Video Id Ads", config.rewardedVideoId);
                    GUILayout.Space(10);
                }
            }
            EditorGUILayout.EndToggleGroup();
            GUILayout.Space(20);




            GUILayout.Space(20);
            GUILayout.Label("Analystic Config", EditorStyles.boldLabel);
            GUILayout.Space(5);
            config.enableAnalyze = EditorGUILayout.BeginToggleGroup("Enable Analytics", config.enableAnalyze);
            config.analyzeLibrary = (AnalyzeLibrary)EditorGUILayout.EnumFlagsField("Analytics Library", config.analyzeLibrary);

            if (config.analyzeLibrary.HasFlag(AnalyzeLibrary.AppsFlyer))
            {
                config.AF_Dev_Key = EditorGUILayout.TextField("AppsFlyer Dev Key", config.AF_Dev_Key);
#if UNITY_IOS
                config.AF_App_Id = EditorGUILayout.TextField("AppsFlyer App ID", config.AF_App_Id);
#endif
            }

            EditorGUILayout.EndToggleGroup();
            GUILayout.Space(20);

            GUILayout.Space(20);
            GUILayout.Label("Debug Log", EditorStyles.boldLabel);
            GUILayout.Space(5);
            config.enableDebugLog = EditorGUILayout.BeginToggleGroup("Enable Debug Logs", config.enableDebugLog);
            config.debugLogLevel = (DebugLogLevel)EditorGUILayout.EnumFlagsField("Analytics Library", config.debugLogLevel);
            EditorGUILayout.EndToggleGroup();
            GUILayout.Space(20);



            GUILayout.Space(20);
            GUILayout.Label("Another Setting", EditorStyles.boldLabel);
            GUILayout.Space(5);
            config.isUsingDoTween = EditorGUILayout.Toggle("Enable DoTween", config.isUsingDoTween);
            GUILayout.Space(5);
            config.isUsingIAP = EditorGUILayout.Toggle("Enable IAP", config.isUsingIAP);
            GUILayout.Space(5);
            config.isUsingLocalNotification = EditorGUILayout.Toggle("Enable Local Notification", config.isUsingLocalNotification);
            GUILayout.Space(20);


            if (GUILayout.Button("Apply changes!"))
            {
                this.SaveChanges();
                this.Close();
            }
        }
        public override void SaveChanges()
        {
            base.SaveChanges();

            var target = EditorUserBuildSettings.activeBuildTarget;
            var group = BuildPipeline.GetBuildTargetGroup(target);


            // Get old defines
            string oldDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
            Debug.Log("[Define] Old: " + oldDefine);
            string[] oldDefines = oldDefine.Split(';');
            HashSet<string> defines = new HashSet<string>();
            for (int i = 0; i < oldDefines.Length; i++)
            {
                defines.Add(oldDefines[i]);
            }


            // Check defines
            config.Changes(ref defines);
            // End check defines


            // Apply new defines
            String[] newDefines = new String[defines.Count];
            defines.CopyTo(newDefines);
            string newDefine = string.Empty;
            for (int i = 0; i < newDefines.Length; i++)
            {
                newDefine += newDefines[i] + (i < defines.Count - 1 ? ";" : "");
            }
            Debug.Log("[Define] New: " + newDefine);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, newDefines);
        }

    }
}