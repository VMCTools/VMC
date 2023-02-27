using UnityEngine;
using UnityEngine.Events;

namespace VMC.Ultilities
{
    public static class UIExtensions
    {
        /// <summary>
        /// Remove all previous listener then add new listener.
        /// </summary>
        /// <param name="onClick"></param>
        /// <param name="call"></param>
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