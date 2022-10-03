using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VMC.Ads;

namespace VMC.Settings
{
    [CustomEditor(typeof(VMCSettingConfig))]
    public class VMCSettingConfigEditor : Editor
    {
        [Header("Ads Config")]
        SerializedProperty enableAds;
        SerializedProperty adType;
        SerializedProperty adsLibrary;

        SerializedProperty isTestMode;
        SerializedProperty maxAppId;
        SerializedProperty openAdsId_Tier1;
        SerializedProperty openAdsId_Tier2;
        SerializedProperty openAdsId_Tier3;
        SerializedProperty bannerId;
        SerializedProperty bannerPosition;
        SerializedProperty interstitialId;
        SerializedProperty rewardedVideoId;



        [Header("Analytics Config")]
        SerializedProperty enableAnalyze;
        SerializedProperty analyzeLibrary;
        SerializedProperty AF_Dev_Key;
        SerializedProperty AF_App_Id;

        [Header("Debug Config")]
        SerializedProperty enableDebugLog;
        SerializedProperty debugLogLevel;

        [Header("Another Settings")]
        SerializedProperty isUsingDoTween;
        SerializedProperty isUsingIAP;
        SerializedProperty isUsingLocalNotification;
        SerializedProperty isUsingAddressable;

        void OnEnable()
        {
            // Setup the SerializedProperties.
            enableAds = serializedObject.FindProperty("enableAds");
            adType = serializedObject.FindProperty("adType");
            adsLibrary = serializedObject.FindProperty("adsLibrary");

            isTestMode = serializedObject.FindProperty("isTestMode");
            maxAppId = serializedObject.FindProperty("maxAppId");
            openAdsId_Tier1 = serializedObject.FindProperty("openAdsId_Tier1");
            openAdsId_Tier2 = serializedObject.FindProperty("openAdsId_Tier2");
            openAdsId_Tier3 = serializedObject.FindProperty("openAdsId_Tier3");
            bannerId = serializedObject.FindProperty("bannerId");
            bannerPosition = serializedObject.FindProperty("bannerPosition");
            interstitialId = serializedObject.FindProperty("interstitialId");
            rewardedVideoId = serializedObject.FindProperty("rewardedVideoId");


            enableAnalyze = serializedObject.FindProperty("enableAnalyze");
            analyzeLibrary = serializedObject.FindProperty("analyzeLibrary");
            AF_Dev_Key = serializedObject.FindProperty("AF_Dev_Key");
            AF_App_Id = serializedObject.FindProperty("AF_App_Id");

            enableDebugLog = serializedObject.FindProperty("enableDebugLog");
            debugLogLevel = serializedObject.FindProperty("debugLogLevel");

            isUsingDoTween = serializedObject.FindProperty("isUsingDoTween");
            isUsingIAP = serializedObject.FindProperty("isUsingIAP");
            isUsingLocalNotification = serializedObject.FindProperty("isUsingLocalNotification");
            isUsingAddressable = serializedObject.FindProperty("isUsingAddressable");

        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(enableAds);
            if (enableAds.boolValue)
            {
                EditorGUILayout.PropertyField(adType);
                EditorGUILayout.PropertyField(adsLibrary);
                if ((AdsLibrary)adsLibrary.enumValueFlag != AdsLibrary.None)
                {
                    EditorGUILayout.PropertyField(isTestMode);
                    if (((AdsLibrary)adsLibrary.enumValueFlag).HasFlag(AdsLibrary.MaxMediation))
                    {
                        EditorGUILayout.PropertyField(maxAppId);
                    }

                    if (((AdsType)adType.enumValueFlag).HasFlag(AdsType.OpenAds))
                    {
                        EditorGUILayout.PropertyField(openAdsId_Tier1);
                        EditorGUILayout.PropertyField(openAdsId_Tier2);
                        EditorGUILayout.PropertyField(openAdsId_Tier3);
                    }
                    if (((AdsType)adType.enumValueFlag).HasFlag(Ads.AdsType.Banner))
                    {
                        EditorGUILayout.PropertyField(bannerId);
                        EditorGUILayout.PropertyField(bannerPosition);
                    }
                    if (((AdsType)adType.enumValueFlag).HasFlag(Ads.AdsType.Interstitial))
                    {
                        EditorGUILayout.PropertyField(interstitialId);
                    }
                    if (((AdsType)adType.enumValueFlag).HasFlag(Ads.AdsType.RewardedVideo))
                    {
                        EditorGUILayout.PropertyField(rewardedVideoId);
                    }
                }
            }
            GUILayout.Space(20);

            EditorGUILayout.PropertyField(enableAnalyze);
            if (enableAnalyze.boolValue)
            {
                EditorGUILayout.PropertyField(analyzeLibrary);
                if (((AnalyzeLibrary)analyzeLibrary.enumValueFlag).HasFlag(AnalyzeLibrary.AppsFlyer))
                {
                    EditorGUILayout.PropertyField(AF_Dev_Key);
#if UNITY_ANDROID
                    GUILayout.Label("AF_App_Id: Unnecessary for Android Build Platform!");
#else
                    EditorGUILayout.PropertyField(AF_App_Id);
#endif
                }
            }
            GUILayout.Space(20);

            EditorGUILayout.PropertyField(enableDebugLog);
            if (enableDebugLog.boolValue)
            {
                EditorGUILayout.PropertyField(debugLogLevel);
            }
            GUILayout.Space(20);
            EditorGUILayout.PropertyField(isUsingDoTween);
            EditorGUILayout.PropertyField(isUsingIAP);
            EditorGUILayout.PropertyField(isUsingLocalNotification);
            EditorGUILayout.PropertyField(isUsingAddressable);
            GUILayout.Space(20);


            if (serializedObject.ApplyModifiedProperties())
            {
                SaveChanges();
            }
        }
        private void SaveChanges()
        {
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
            Changes(ref defines);
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

        public void Changes(ref HashSet<string> defines)
        {
            #region Ads check
            if (enableAds.boolValue && isTestMode.boolValue)
            {
                defines.Add(Define.VMC_ADS_TESTMODE.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ADS_TESTMODE.ToString());
            }

            if (enableAds.boolValue && ((AdsLibrary)adsLibrary.enumValueFlag).HasFlag(AdsLibrary.Admob))
            {
                defines.Add(Define.VMC_ADS_ADMOB.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ADS_ADMOB.ToString());
            }
            if (enableAds.boolValue && ((AdsLibrary)adsLibrary.enumValueFlag).HasFlag(AdsLibrary.MaxMediation))
            {
                defines.Add(Define.VMC_ADS_MAX.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ADS_MAX.ToString());
            }
            #endregion
            #region Analytics check
            if (enableAnalyze.boolValue && ((AnalyzeLibrary)analyzeLibrary.enumValueFlag).HasFlag(AnalyzeLibrary.Firebase))
            {
                defines.Add(Define.VMC_ANALYZE_FIREBASE.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ANALYZE_FIREBASE.ToString());
            }
            if (enableAnalyze.boolValue && ((AnalyzeLibrary)analyzeLibrary.enumValueFlag).HasFlag(AnalyzeLibrary.AppsFlyer))
            {
                defines.Add(Define.VMC_ANALYZE_APPFLYER.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ANALYZE_APPFLYER.ToString());
            }
            #endregion
            #region Debug log check
            if (enableDebugLog.boolValue && ((DebugLogLevel)debugLogLevel.enumValueFlag).HasFlag(DebugLogLevel.Debug))
            {
                defines.Add(Define.VMC_DEBUG_NORMAL.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_DEBUG_NORMAL.ToString());
            }
            if (enableDebugLog.boolValue && ((DebugLogLevel)debugLogLevel.enumValueFlag).HasFlag(DebugLogLevel.Warning))
            {
                defines.Add(Define.VMC_DEBUG_WARNING.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_DEBUG_WARNING.ToString());
            }
            if (enableDebugLog.boolValue && ((DebugLogLevel)debugLogLevel.enumValueFlag).HasFlag(DebugLogLevel.Error))
            {
                defines.Add(Define.VMC_DEBUG_ERROR.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_DEBUG_ERROR.ToString());
            }
            if (enableDebugLog.boolValue && ((DebugLogLevel)debugLogLevel.enumValueFlag).HasFlag(DebugLogLevel.Assert))
            {
                defines.Add(Define.VMC_DEBUG_ASSERT.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_DEBUG_ASSERT.ToString());
            }

            #endregion

            if (isUsingDoTween.boolValue)
            {
                defines.Add(Define.VMC_DOTWEEN.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_DOTWEEN.ToString());
            }
            if (isUsingIAP.boolValue)
            {
                defines.Add(Define.VMC_IAP.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_IAP.ToString());
            }
            if (isUsingLocalNotification.boolValue)
            {
                defines.Add(Define.VMC_NOTIFICATION.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_NOTIFICATION.ToString());
            }
            if (isUsingAddressable.boolValue)
            {
                defines.Add(Define.VMC_ADDRESSABLE.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ADDRESSABLE.ToString());
            }
        }


    }

}