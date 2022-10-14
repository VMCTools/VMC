using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if VMC_ADDRESSABLE
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif
using VMC.Ultilities;

namespace VMC.Addressable
{
    public class AddressableManager : VMC.Ultilities.Singleton<AddressableManager>
    {
        public Dictionary<string, GameObject> dictAssets;
        private string curPath;
        private event Action<GameObject> curCallback;
        protected override void Awake()
        {
            base.Awake();
            dictAssets = new Dictionary<string, GameObject>();
        }


        public void LoadAsset(string path, Action<GameObject> callback)
        {
            if (dictAssets.ContainsKey(path))
            {
                callback?.Invoke(dictAssets[path]);
                return;
            }
            this.curPath = path;
            this.curCallback = callback;

#if VMC_ADDRESSABLE
            Addressables.LoadAssetAsync<GameObject>(path).Completed += OnLoadDone;
#else
            Debugger.Debug.LogError("[Addressable]", "Not active yet!!!!");
            callback?.Invoke(null);
#endif
        }

#if VMC_ADDRESSABLE
        private void OnLoadDone(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.IsDone)
            {
                if (!dictAssets.ContainsKey(this.curPath))
                {
                    dictAssets.Add(this.curPath, obj.Result);
                    this.curCallback?.Invoke(obj.Result);
                }
            }
            else
            {
                this.curCallback?.Invoke(null);
            }
        }
#endif
    }
}