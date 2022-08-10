using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VMC.Ultilities;

namespace VMC.Analystic
{
    public class AnalysticManager :  Singleton<AnalysticManager>, IAnalystic
    {
        private void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            I_Initialize();
        }
        protected virtual void I_Initialize() { }

        public void LogEvent(string nameEvent)
        {
            I_LogEvent(nameEvent);
        }

        public void LogEvent(AnalysticType type, string param)
        {
            I_LogEvent($"[{type}] {param}");
        }
        protected virtual void I_LogEvent(string param) { }
    }
}