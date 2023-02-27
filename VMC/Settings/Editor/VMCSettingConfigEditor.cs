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
        SerializedProperty intervalTimeAOA;
        SerializedProperty bannerId;
        SerializedProperty bannerPosition;
        SerializedProperty interstitialId;
        SerializedProperty intervalTimeInterstitial;
        SerializedProperty rewardedVideoId;



        [Header("Analytics Config")]
        SerializedProperty enableAnalyze;
        SerializedProperty analyzeLibrary;
        SerializedProperty AF_Dev_Key;
        SerializedProperty AF_App_Id;

        [Header("Debug Config")]
        SerializedProperty enableDebugLog;
        SerializedProperty debugLogLevel;

        [Header("Remote config")]
        SerializedProperty enableRemoteConfig;

        [Header("Facebook")]
        SerializedProperty isUsingFacebook;

        [Header("DoTween")]
        SerializedProperty isUsingDoTween;
        [Header("IAP")]
        SerializedProperty isUsingIAP;

        [Header("Notification")]
        SerializedProperty notificationType;
        [Header("Addressable")]
        SerializedProperty isUsingAddressable;
        [Header("InappReview")]
        SerializedProperty isUsingAppReview;

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
            intervalTimeAOA = serializedObject.FindProperty("intervalTimeAOA");


            bannerId = serializedObject.FindProperty("bannerId");
            bannerPosition = serializedObject.FindProperty("bannerPosition");
            interstitialId = serializedObject.FindProperty("interstitialId");
            intervalTimeInterstitial = serializedObject.FindProperty("intervalTimeInterstitial");
            rewardedVideoId = serializedObject.FindProperty("rewardedVideoId");


            enableAnalyze = serializedObject.FindProperty("enableAnalyze");
            analyzeLibrary = serializedObject.FindProperty("analyzeLibrary");
            AF_Dev_Key = serializedObject.FindProperty("AF_Dev_Key");
            AF_App_Id = serializedObject.FindProperty("AF_App_Id");

            enableDebugLog = serializedObject.FindProperty("enableDebugLog");
            debugLogLevel = serializedObject.FindProperty("debugLogLevel");


            enableRemoteConfig = serializedObject.FindProperty("enableRemoteConfig");

            isUsingFacebook = serializedObject.FindProperty("isUsingFacebook");
            isUsingDoTween = serializedObject.FindProperty("isUsingDoTween");
            isUsingIAP = serializedObject.FindProperty("isUsingIAP");
            notificationType = serializedObject.FindProperty("notificationType");
            isUsingAddressable = serializedObject.FindProperty("isUsingAddressable");
            isUsingAppReview = serializedObject.FindProperty("isUsingAppReview");
        }
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);

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
                        EditorGUILayout.PropertyField(intervalTimeAOA);
                        GUILayout.Space(10);
                    }
                    if (((AdsType)adType.enumValueFlag).HasFlag(Ads.AdsType.Banner))
                    {
                        EditorGUILayout.PropertyField(bannerId);
                        EditorGUILayout.PropertyField(bannerPosition);
                        GUILayout.Space(10);
                    }
                    if (((AdsType)adType.enumValueFlag).HasFlag(Ads.AdsType.Interstitial))
                    {
                        EditorGUILayout.PropertyField(interstitialId);
                        EditorGUILayout.PropertyField(intervalTimeInterstitial);
                        GUILayout.Space(10);
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
            EditorGUILayout.PropertyField(enableRemoteConfig);

            GUILayout.Space(20);
            EditorGUILayout.PropertyField(isUsingFacebook);

            GUILayout.Space(20);
            EditorGUILayout.PropertyField(isUsingDoTween);
            EditorGUILayout.PropertyField(isUsingIAP);
            EditorGUILayout.PropertyField(notificationType);
            EditorGUILayout.PropertyField(isUsingAddressable);

            GUILayout.Space(20);
            EditorGUILayout.PropertyField(isUsingAppReview);


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
            #region DOTween
            if (isUsingDoTween.boolValue)
            {
                defines.Add(Define.VMC_DOTWEEN.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_DOTWEEN.ToString());
            }
            #endregion
            #region IAP
            if (isUsingIAP.boolValue)
            {
                defines.Add(Define.VMC_IAP.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_IAP.ToString());
            }
            #endregion

            #region Remote Config
            if (enableRemoteConfig.boolValue)
            {
                defines.Add(Define.VMC_REMOTE_FIREBASE.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_REMOTE_FIREBASE.ToString());
            }
            #endregion
            #region Facebook
            if (isUsingFacebook.boolValue)
            {
                defines.Add(Define.VMC_FACEBOOK.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_FACEBOOK.ToString());
            }
            #endregion

            #region notification
            if (((NotificationType)notificationType.enumValueFlag).HasFlag(NotificationType.LocalNotification))
            {
                defines.Add(Define.VMC_NOTIFICATION.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_NOTIFICATION.ToString());
            }
            if (((NotificationType)notificationType.enumValueFlag).HasFlag(NotificationType.FirebaseMessaging))
            {
                defines.Add(Define.VMC_FIREBASE_MESSAGING.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_FIREBASE_MESSAGING.ToString());
            }
            #endregion
            #region Addressable
            if (isUsingAddressable.boolValue)
            {
                defines.Add(Define.VMC_ADDRESSABLE.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_ADDRESSABLE.ToString());
            }
            #endregion
            #region Inapp Review
            if (isUsingAppReview.boolValue)
            {
                defines.Add(Define.VMC_APP_REVIEW.ToString());
            }
            else
            {
                defines.Remove(Define.VMC_APP_REVIEW.ToString());
            }
            #endregion
        }


    }

}