# VMC
- Tool phục vụ
090993
030397


----------------------------------------------------------------------------------------------------------------
Admob Unity SDK: GoogleMobileAds-v7.2.0.unitypackage
ExternalDependencyManager: 1.2.172
Firebase Analystic: 9.5.0 - {FirebaseAnalytics}
AppsFlyer: appsflyer-unity-plugin-6.8.3.unitypackage
Applovin: AppLovin-MAX-Unity-Plugin-5.4.7-Android-11.4.6-iOS-11.4.4.unitypackage

----------------------------------------------------------------------------------------------------------------
1. Import package vào projects
2. Điền thông tin Admob AppID: Assets -> Google Mobile Ads -> Settings...
3. Điền thông tin Applovin SDK: Applovin -> Integration Manager -> Applovin SDK Key
4. Kiểm tra đã bật Jetifier: Assets -> Exteneral Dependency Manager -> Android Resolver -> Settings -> Use Jetifier
5. Kéo prefabs VMC(Assets/VMC/Prefabs/VMC)  vào scene.
6. Điền tất cả các thông số trong VMC -> Setting trên menu bar.
7. Nhớ import google-services.json và kiểm tra trùng package giữa file json và project
8. Sử dụng


	- Gọi quảng cáo: AdsManager

		AdsManager.Instance.ShowBanner();
 		AdsManager.Instance.HideBanner();
		AdsManager.Instance.ShowInterstitial("Test", () =>
                    {
                        Debug.Log("Close inters");
                    });

		AdsManager.Instance.ShowRewardedVideo("Test", (result) =>
                    {
                        Debug.Log("Close rewarded and got reward? " + result);
                    });


	- Log event: Gọi thông qua class AnalysticManager

		AnalysticManager.Instance.Logevent();