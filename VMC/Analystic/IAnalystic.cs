namespace VMC.Analystic
{
    public interface IAnalystic
    {
        public void Initialize();
        public void LogEvent(string nameEvent);
        public void LogEvent(AnalysticType type, string param); 
    }
}