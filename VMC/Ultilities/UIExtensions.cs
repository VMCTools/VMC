using UnityEngine;
using UnityEngine.Events;

namespace VMC.Ultilities
{
    public static class UIExtensions
    {
        public static void SetListener(this UnityEvent onClick, UnityAction call)
        {
            onClick.RemoveAllListeners();
            onClick.AddListener(call);
        }
        public static bool EventBack(this MonoBehaviour mono)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) return true;
            return false;
        }
    }
}