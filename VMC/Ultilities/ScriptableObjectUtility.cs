#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace VMC.Ultilities
{
    public static class ScriptableObjectUtility
    {
        /// <summary>
        //  This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static void CreateAsset<T>(string path) where T : ScriptableObject
        {
#if UNITY_EDITOR
            T asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
#endif
        }

        public static void OnDestroy()
        {
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
        }

        public static void OnApplicationQuit()
        {
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
        }
    }
}