using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Assets
{
    public class ResourceManager : VMC.Ultilities.SingletonAdvance<ResourceManager>
    {
        public Dictionary<string, UnityEngine.Object> dictAssets = new Dictionary<string, UnityEngine.Object>();
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            if (dictAssets.ContainsKey(path))
            {
                return dictAssets[path] as T;
            }
            else
            {
                UnityEngine.Object obj = Resources.Load<T>(path);
                if (obj != null)
                {
                    dictAssets.Add(path, obj);
                    return obj as T;
                }
                else
                {
                    Debug.LogError($"Can't load resouces at path: {path}");
                    return null;
                }
            }
        }
    }
}