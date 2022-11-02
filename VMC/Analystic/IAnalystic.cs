namespace VMC.Analystic
{
    public interface IAnalystic
    {
        public void Initialize();


        public void ATTShow();
        public void ATTSuccess();
        public void LogEvent(string nameEvent);



        public void Log_LevelStart(int level);
        public void Log_LevelWin(int level, float time);
        public void Log_LevelLose(int level, float time);
        public void Log_CoinEarn(int value, string position);
        public void Log_CoinSpent(int value, string position, string item);
        public void Log_InappPurchase(string package, float amount);
        public void Log_UserInteract(string screen);
        public void Log_RewardedAdsShow(int level, string placement);
        public void Log_RewardedAdsSuccessed(int level, string placement);
        public void Log_IntersitialAdsShow(int level, string placement);
        public void Log_IntersitialAdsSuccessed(int level, string placement);
        public void SetUserProperties_TotalSpend(int total);
        public void SetUserProperties_TotalEarn(int total);
        public void SetUserProperties_LevelReach(int level);
        public void SetUserProperties_DayDisplaying(int days);
    }
}