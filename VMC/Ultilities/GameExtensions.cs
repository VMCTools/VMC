using System;
using UnityEngine;

namespace VMC.Ultilities
{
    public class GameExtensions
    {

#if UNITY_ANDROID
        public static void RateUs()
        {
            Application.OpenURL("market://details?id=" + Application.identifier);
        }
#elif UNITY_IOS
        public static void RateUs(string appleId)
        {
            Application.OpenURL("itms-apps://itunes.apple.com/app/id" + appleId);
        }
#endif

        public static void TakeScreenShot()
        {
#if UNITY_EDITOR
            DateTime now = DateTime.Now;
            string fileName = string.Format("Screenshot-{0}-{1}-{2}-{3}-{4}.png", now.Month, now.Day, now.Hour, now.Minute, now.Second);
            ScreenCapture.CaptureScreenshot(fileName);
            Debugger.Debug.Log("[Screenshot]", $"Save {fileName} to {Application.dataPath}");
#else
            Debugger.Debug.Log("[Screenshot]", $"Only work in the Editor!");
#endif
        }
        public static void TakeScreenShot(string fileName)
        {
#if UNITY_EDITOR
            if (fileName.Length < 5) // not required length
            {
                fileName = string.Format("Screenshot-{0}.png", fileName);
            }
            else if (!fileName.EndsWith(".png"))
            {
                fileName += ".png";
            }
            ScreenCapture.CaptureScreenshot(fileName);
            Debugger.Debug.Log("[Screenshot]", $"Save {fileName} to {Application.dataPath}");
#else
            Debugger.Debug.Log("[Screenshot]", $"Only work in the Editor!");
#endif
        }
    }
}