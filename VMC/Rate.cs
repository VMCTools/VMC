using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VMC.Ultilities;

namespace VMC
{
    public class Rate : SingletonAdvance<Rate>
    { 
        public void OpenStorePage()
        {
#if UNITY_ANDROID
            Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
#elif UNITY_IPHONE || UNITY_IOS
            Application.OpenURL("https://apps.apple.com/app/id" + STORE_APP_ID);
#endif
        }
    }
}
