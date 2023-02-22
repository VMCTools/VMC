using System.Collections;
using UnityEngine;
using VMC.Ultilities;
namespace VMC
{
    public class Share : SingletonAdvance<Share>
    {
        string subject = $"Hey I am playing this awesome new game called {Application.productName},do give it try and enjoy \n";
        string body = "https://play.google.com/store/apps/details?id=" + Application.identifier;

        public void OnAndroidTextSharingClick()
        {
#if UNITY_ANDROID
            StartCoroutine(ShareAndroidText());
#else
            Debug.LogError("Not supported!");
#endif
        }

        IEnumerator ShareAndroidText()
        {
            yield return new WaitForEndOfFrame();
            //execute the below lines if being run on a Android device
            //Reference of AndroidJavaClass class for intent
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            //Reference of AndroidJavaObject class for intent
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            //call setAction method of the Intent object created
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            //set the type of sharing that is happening
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            //add data to be passed to the other activity i.e., the data to be sent
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "TITLE");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), subject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
            //get the current activity
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            //start the activity by sending the intent data
            AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
            currentActivity.Call("startActivity", jChooser);
        }
    }
}