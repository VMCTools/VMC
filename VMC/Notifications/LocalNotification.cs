
using System;
#if VMC_NOTIFICATION
#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif
#endif
using UnityEngine;
using VMC.Ultilities;

namespace VMC.Notifications
{
    public class LocalNotification : VMC.Ultilities.Singleton<LocalNotification>
    {
        public void RegisterNotificationChannel()
        {
#if VMC_NOTIFICATION
#if UNITY_ANDROID
            var c = new AndroidNotificationChannel()
            {
                Id = "channel_id",
                Name = "Default Channel",
                Importance = Importance.High,
                Description = "Generic notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(c);
            Logger.Logger.Log("[LocalNotification] RegisterNotificationChannel");
#elif UNITY_IOS
            StartCoroutine(RequestAuthorization());
#endif
#endif
        }

#if VMC_NOTIFICATION
#if UNITY_IOS
        IEnumerator RequestAuthorization()
        {
            var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
            using (var req = new AuthorizationRequest(authorizationOption, true))
            {
                while (!req.IsFinished)
                {
                    yield return null;
                };

                string res = "\n RequestAuthorization:";
                res += "\n finished: " + req.IsFinished;
                res += "\n granted :  " + req.Granted;
                res += "\n error:  " + req.Error;
                res += "\n deviceToken:  " + req.DeviceToken;
                Debug.Log(res);
            }
        }
#endif
#endif
        public int SendNotification(int id, double delayMs, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "")
        {

#if VMC_NOTIFICATION
#if UNITY_ANDROID
            var notification = new AndroidNotification();
            notification.Title = title;
            notification.Text = message;
            notification.Color = bgColor;
            notification.SmallIcon = "app_icon";
            notification.FireTime = System.DateTime.Now.AddMilliseconds(delayMs);
            Logger.Logger.Log("[LocalNotification] Set notify: " + message + " !!!delay: " + delayMs);
            AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "channel_id", id);
#elif UNITY_IOS
            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(0, 0, (int)(delayMs / 1000)),
                Repeats = false
            };

            var notification = new iOSNotification()
            {
                // You can specify a custom identifier which can be used to manage the notification later.
                // If you don't provide one, a unique string will be generated automatically.
                Identifier = id.ToString(),
                Title = title,
                Body = message,
                Subtitle = "",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);
#endif
#endif
            return id;
        }
        public int SendNotification(int id, double delayMs, TimeSpan repeatTime, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "")
        {

#if VMC_NOTIFICATION
#if UNITY_ANDROID
            var notification = new AndroidNotification();
            notification.Title = title;
            notification.Text = message;
            notification.Color = bgColor;
            notification.SmallIcon = "app_icon";
            notification.FireTime = System.DateTime.Now.AddMilliseconds(delayMs);
            notification.RepeatInterval = repeatTime;
            Logger.Logger.Log("[LocalNotification] Set notify: " + message + " !!!delay: " + delayMs);
            AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "channel_id", id);
#elif UNITY_IOS
            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = repeatTime,
                Repeats = true
            };

            var notification = new iOSNotification()
            {
                // You can specify a custom identifier which can be used to manage the notification later.
                // If you don't provide one, a unique string will be generated automatically.
                Identifier = id.ToString(),
                Title = title,
                Body = message,
                Subtitle = "",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,

            };

            iOSNotificationCenter.ScheduleNotification(notification);
#endif
#endif
            return id;
        }
        public void CancelNotification(int id)
        {

#if VMC_NOTIFICATION
#if UNITY_ANDROID
            AndroidNotificationCenter.CancelNotification(id);
            Logger.Logger.Log("[LocalNotification] Cancel notification id: " + id);
#elif UNITY_IOS
            iOSNotificationCenter.RemoveScheduledNotification(id.ToString());
#endif
#endif
        }

        public void ClearNotifications()
        {

#if VMC_NOTIFICATION
#if UNITY_ANDROID
            AndroidNotificationCenter.CancelAllNotifications();
            Logger.Logger.Log("[LocalNotification] Cancel all notification!");
#elif UNITY_IOS
            iOSNotificationCenter.RemoveAllScheduledNotifications();
#endif
#endif
        }

#if VMC_NOTIFICATION
#if UNITY_IOS
        public void ClearDeliveredNotifications()
        {
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
        }
#endif
#endif
    }
}
