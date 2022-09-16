using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VMC.Settings
{
    [CustomEditor(typeof(VMCSettingConfig))]
    public class VMCSettingConfigEditor : Editor
    {
        bool enableAds;
        AdsLibrary adsLibrary;
        bool enableAnalyze;
        AnalyzeLibrary analyzeLibrary;
        //SerializedProperty enableAds;
        //SerializedProperty enableAds;
        private void OnEnable()
        {
            enableAds = serializedObject.FindProperty("enableAds").boolValue;
            adsLibrary = (AdsLibrary)serializedObject.FindProperty("adsLibrary").enumValueFlag;
            enableAnalyze = serializedObject.FindProperty("enableAnalyze").boolValue;
            analyzeLibrary = (AnalyzeLibrary)serializedObject.FindProperty("analyzeLibrary").enumValueFlag;
            //enableAds = serializedObject.FindProperty("enableAds");
            //enableAds = serializedObject.FindProperty("enableAds");
            //enableAds = serializedObject.FindProperty("enableAds");
            //enableAds = serializedObject.FindProperty("enableAds");
        }
        public override void OnInspectorGUI()
        {
            GUILayout.Space(20);
            GUILayout.Label("Ads Config", EditorStyles.boldLabel);
            GUILayout.Space(5);
            enableAds = EditorGUILayout.BeginToggleGroup("Enable Ads", enableAds);
            adsLibrary = (AdsLibrary)EditorGUILayout.EnumFlagsField("Ads Library", adsLibrary);
            EditorGUILayout.EndToggleGroup();
            GUILayout.Space(20);

            GUILayout.Space(20);
            GUILayout.Label("Analystic Config", EditorStyles.boldLabel);
            GUILayout.Space(5);
            enableAnalyze = EditorGUILayout.BeginToggleGroup("Enable Analytics", enableAnalyze);
            analyzeLibrary = (AnalyzeLibrary)EditorGUILayout.EnumFlagsField("Analytics Library", analyzeLibrary);
            EditorGUILayout.EndToggleGroup();
            GUILayout.Space(20);
        }
    }
}