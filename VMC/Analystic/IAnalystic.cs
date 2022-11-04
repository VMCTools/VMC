namespace VMC.Analystic
{
    public interface IAnalystic
    {
        public void Initialize();


        public void ATTShow();
        public void ATTSuccess();
        public void LogEvent(string nameEvent);
#if VMC_GROUP_1
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
#elif VMC_GROUP_2
        public void Log_CheckPoint(int id);
        public void Log_LevelStart(int level, int current_gold);
        public void Log_LevelComplete(int level, float timeplayed);
        public void Log_LevelFail(int level, int failcount);
        public void Log_EarnVirtualCurrency(string virtual_currency_name, long value, string source);
        public void Log_SpendVirtualCurrency(string virtual_currency_name, long value, string item_name);

        public void Log_AdsRewardOffer(string placment);
        public void Log_AdsRewardClick(string placment);
        public void Log_AdsRewardShow(string placment);
        public void Log_AdsRewardFail(string placment, string errormsg);
        public void Log_AdsRewardComplete(string placment);

        public void Log_AdsInterOffer();
        public void Log_AdsInterFail(string errormsg);
        public void Log_AdsInterLoad();
        public void Log_AdsInterShow();
        public void Log_AdsInterClick();


        public void UserProperty_RetentType(int retent_type);
        public void UserProperty_DayPlayed(int days_played);
        public void UserProperty_PayingType(int paying_type);
        public void UserProperty_Level(int level);


        public void Log_TutorialCompletion(bool isSuccess, string tutorialId);
        public void Log_LevelAchieved(int level, int score);
        public void Log_AchievementUnlocked(int contentId, int level);
        public void Log_Purchase(float revenue, string currency, int quantity, int contentId);
#endif
    }
}