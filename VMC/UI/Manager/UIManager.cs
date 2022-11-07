using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VMC.Ultilities;

namespace VMC.UI
{
    public class UIManager : SingletonAdvance<UIManager>
    {
        private Dictionary<string, MonoBehaviour> dictPopup = new Dictionary<string, MonoBehaviour>();
        public T Show<T>(string path) where T : MonoBehaviour
        {
            if (dictPopup.ContainsKey(path))
            {
                if (dictPopup[path].gameObject != null)
                {
                    dictPopup[path].gameObject.SetActive(true);
                    return (T)dictPopup[path];
                }
                else
                {
                    dictPopup.Remove(path);
                }
            }

            T prefab = Resources.Load<T>($"UI/{path}");
            T obj = Instantiate(prefab);
            dictPopup.Add(path, obj);
            return obj;
        }
    }
}