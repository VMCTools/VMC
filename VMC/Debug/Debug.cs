using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace VMC.Debugger
{
    public class Debug
    {
        private const string KEY_LOG = "VMC_ENABLE_LOG";
        private const string KEY_ASSERTTIONS = "UNITY_ASSERTIONS";

        [Conditional(KEY_LOG)]
        public static void Log(object message, UnityEngine.Object context)
        {
            UnityEngine.Debug.Log(message, context);
        }


        [Conditional(KEY_LOG)]
        public static void Log(string key, string mess)
        {
            UnityEngine.Debug.Log($"<color=green>{key}</color> {mess}");
        }

        [Conditional(KEY_LOG)]
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }
        [Conditional(KEY_ASSERTTIONS), Conditional(KEY_LOG)]
        public static void LogAssertion(object message, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogAssertion(message, context);
        }
        [Conditional(KEY_ASSERTTIONS), Conditional(KEY_LOG)]
        public static void LogAssertion(object message)
        {
            UnityEngine.Debug.LogAssertion(message);
        }
        [Conditional(KEY_ASSERTTIONS), Conditional(KEY_LOG)]
        public static void LogAssertionFormat(UnityEngine.Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogAssertionFormat(context, format, args);
        }
        [Conditional(KEY_ASSERTTIONS), Conditional(KEY_LOG)]
        public static void LogAssertionFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogAssertionFormat(format, args);
        }
        [Conditional(KEY_LOG)]
        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }
        [Conditional(KEY_LOG)]
        public static void LogError(object message, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError(message, context);
        }
        [Conditional(KEY_LOG)]
        public static void LogErrorFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(format, args);
        }
        [Conditional(KEY_LOG)]
        public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(context, format, args);
        }
        [Conditional(KEY_LOG)]
        public static void LogException(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }
        [Conditional(KEY_LOG)]
        public static void LogException(Exception exception, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogException(exception, context);
        }
        [Conditional(KEY_LOG)]
        public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(context, format, args);
        }
        [Conditional(KEY_LOG)]
        public static void LogFormat(LogType logType, LogOption logOptions, UnityEngine.Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args);
        }
        [Conditional(KEY_LOG)]
        public static void LogFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(format, args);
        }
        [Conditional(KEY_LOG)]
        public static void LogWarning(object message, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogWarning(message, context);
        }
        [Conditional(KEY_LOG)]
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        [Conditional(KEY_LOG)]
        public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(context, format, args);
        }
        [Conditional(KEY_LOG)]
        public static void LogWarningFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(format, args);
        }
    }
}