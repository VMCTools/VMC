using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class UnityATTPlugin : MonoBehaviour {
    private static UnityATTPlugin instance;
    public static UnityATTPlugin Instance { get {
            if (!instance)
            {
                GameObject ga = new GameObject();
                ga.name = "UnityATTPlugin";
                instance = ga.AddComponent<UnityATTPlugin>();
                DontDestroyOnLoad(ga);
            }
            return instance;
        } }

    #region Declare external C interface
    #if UNITY_IOS && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern int getATTStatus();

    [DllImport("__Internal")]
    private static extern void showATTRequest();

    #endif
    #endregion

    public bool IsIOS14AndAbove()
    {
#if UNITY_IOS
        Version currentVersion = new Version(UnityEngine.iOS.Device.systemVersion);
        Version iOS145 = new Version("14.5");

        if(currentVersion >= iOS145)
        {
            return true;
        }
#endif
        return false;
    }

    public ATTStatus GetATTStatus()
    {
#if UNITY_IOS && !UNITY_EDITOR
        return (ATTStatus)getATTStatus();
#endif
        return ATTStatus.Unknow;
    }

    private Action<ATTStatus> requestATTAction;

    public void ShowATTRequest(Action<ATTStatus> action)
    {
#if UNITY_IOS && !UNITY_EDITOR
        requestATTAction = action;
        showATTRequest();
#else
        action(ATTStatus.Unknow);
#endif
    }

    public void OnRequestATTCallBack(string stt)
    {
        ATTStatus status = (ATTStatus)int.Parse(stt);
        Debug.Log("OnRequestATTCallBack " + status.ToString());
        if(requestATTAction != null)
        {
            requestATTAction(status);
        }
    }
}

public enum ATTStatus
{
    NotDetermined = 0,
    Restricted = 1,
    Denied = 2,
    Authorized = 3,
    Unknow = 4
}