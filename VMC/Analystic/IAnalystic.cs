using System.Collections.Generic;

namespace VMC.Analystic
{
    public interface IAnalystic
    {
        public void Initialize();
        public void LogEvent(string nameEvent);
        public void LogEvent(string nameEvent, Dictionary<string, string> param);
    }
}