using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace VMC.Ingame.DailyReward
{
    public class VMCDailyReward :VMC.Ultilities.Singleton<VMCDailyReward>
    {
        [SerializeField] private bool isContinue = true;
        [SerializeField] private int MaxDay = 7;
        [SerializeField, ReadOnly] private bool hasGift;
        [SerializeField, ReadOnly] private int dayGift;

        private DateTime lastTimeGift;
        private DateTime nextTimeGift;
        private const string KEY_LASTTIME_DAILYREWARD = "vmc_dailyreward_lasttime";
        private const string KEY_CURRENTDAY_DAILYREWARD = "vmc_dailyreward_currentDay";

        public bool HasGift => hasGift;
        public int DayGift => dayGift;
        public TimeSpan RemainTime => nextTimeGift - DateTime.Now;

        protected override void Awake()
        {
            base.Awake();

            CheckGift();
        }
        private void CheckGift()
        {
            if (!PlayerPrefs.HasKey(KEY_LASTTIME_DAILYREWARD))
            {
                lastTimeGift = new DateTime();

                hasGift = true;
                dayGift = 1;
            }
            else
            {
                lastTimeGift = DateTime.Parse(PlayerPrefs.GetString(KEY_LASTTIME_DAILYREWARD));

                DateTime now = DateTime.Today;

                if (now > lastTimeGift)
                {
                    if (isContinue)
                    {
                        // check sự liên tục
                        double hours = (now - lastTimeGift).TotalHours;
                        if (hours > 24)
                        {
                            // quá ngày liên tiếp => reset về ngày 1
                            hasGift = true;
                            dayGift = 1;
                        }
                        else
                        {
                            hasGift = true;
                            dayGift = PlayerPrefs.GetInt(KEY_CURRENTDAY_DAILYREWARD) + 1;
                            if (dayGift > MaxDay) dayGift = 1;
                        }

                    }
                    else
                    {
                        hasGift = true;
                        dayGift = PlayerPrefs.GetInt(KEY_CURRENTDAY_DAILYREWARD) + 1;
                        if (dayGift > MaxDay) dayGift = 1;
                    }
                }
                else
                {
                    //same day
                    hasGift = false;
                    dayGift = PlayerPrefs.GetInt(KEY_CURRENTDAY_DAILYREWARD);
                    nextTimeGift = DateTime.Today.AddDays(1);
                }
            }
        }
        public void TakeGift()
        {
            if (!hasGift) return;
            hasGift = false;
            nextTimeGift = DateTime.Today.AddDays(1);
            PlayerPrefs.SetInt(KEY_CURRENTDAY_DAILYREWARD, dayGift);
            PlayerPrefs.SetString(KEY_LASTTIME_DAILYREWARD, DateTime.Now.ToString());
        }
    }
}