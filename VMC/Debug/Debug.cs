using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace VMC.Debugger
{
    public class Debug
    {
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_NORMAL))]
        public static void Log(object message, UnityEngine.Object context)
        {
            UnityEngine.Debug.Log(message, context);
        }

        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_NORMAL))]
        public static void Log(string key, string mess)
        {
            UnityEngine.Debug.Log($"<color=green>{key}</color> {mess}");
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_NORMAL))]
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_NORMAL))]
        public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(context, format, args);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_NORMAL))]
        public static void LogFormat(LogType logType, LogOption logOptions, UnityEngine.Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_NORMAL))]
        public static void LogFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(format, args);
        }


        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_ASSERT)), Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_NORMAL))]
        public static void LogAssertion(object message, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogAssertion(message, context);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_ASSERT)), Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_NORMAL))]
        public static void LogAssertion(object message)
        {
            UnityEngine.Debug.LogAssertion(message);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_ASSERT)), Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_NORMAL))]
        public static void LogAssertionFormat(UnityEngine.Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogAssertionFormat(context, format, args);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_ASSERT)), Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_NORMAL))]
        public static void LogAssertionFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogAssertionFormat(format, args);
        }



        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_ERROR))]
        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }

        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_ERROR))]
        public static void LogError(string key, string mess)
        {
            UnityEngine.Debug.LogError($"<color=green>{key}</color> {mess}");
        }

        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_ERROR))]
        public static void LogError(object message, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError(message, context);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_ERROR))]
        public static void LogErrorFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(format, args);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_ERROR))]
        public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(context, format, args);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_ERROR))]
        public static void LogException(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_ERROR))]
        public static void LogException(Exception exception, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogException(exception, context);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_WARNING))]
        public static void LogWarning(object message, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogWarning(message, context);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_WARNING))]
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_WARNING))]
        public static void LogWarning(string key, string mess)
        {
            UnityEngine.Debug.LogError($"<color=red>{key}</color> {mess}");
        }


        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_WARNING))]
        public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(context, format, args);
        }
        [Conditional(nameof(VMC.Settings.Define.VMC_DEBUG_WARNING))]
        public static void LogWarningFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(format, args);
        }
    }
}