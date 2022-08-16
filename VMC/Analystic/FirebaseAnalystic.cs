using Debug = VMC.Debugger.Debug;
namespace VMC.Analystic
{
    public class FirebaseAnalystic : AnalysticManager
    {
        private bool isReady = false;
        protected override void I_Initialize()
        {
#if VMC_FIREBASE
            Debug.Log("Firebase", "Init!");
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    //app = Firebase.FirebaseApp.DefaultInstance;
                    isReady = true;
                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                }
                else
                {
                    Debug.LogError(System.String.Format(
                        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
#endif
        }
        protected override void I_LogEvent(string nameEvent)
        {
            if (!isReady)
            {
                Debug.Log("Firebase-Analystic", "Not ready to use!");
                return;
            }
            // Log an event with no parameters.
            Debug.Log("Firebase", "Log message: " + nameEvent);
#if VMC_FIREBASE
            Firebase.Analytics.FirebaseAnalytics.LogEvent(nameEvent);
#endif
        }
    }

}