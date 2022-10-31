using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VMC.Ultilities
{
    public class SingletonAdvance<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("");
                    var component = obj.AddComponent<T>();
                    obj.name = $"[Singleton] {typeof(T).ToString()}";

                    _instance = component;
                }
                return _instance;
            }
        }
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (_instance == this)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this) _instance = null;
        }
    }
}