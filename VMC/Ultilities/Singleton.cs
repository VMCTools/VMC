using UnityEngine;

namespace VMC.Ultilities
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private bool dontDestroyOnLoad = true;
        public static T Instance;
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(this.gameObject);
                }
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
