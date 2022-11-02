# VMC
- Tool Ads-Analytics-Ultilities
----------------------------------------------------------------------------------------------------------------
						CHANGE LOGS
[2022 / 11 / 01] -------1.0.2.5---------
Upgrade Firebase Analytics 9.5.0 -> 10.0.1
Add Firebase Remote Config 10.0.1
Upgrade MaxSDK lastest
Upgrade ExternalDependencyManager 1.2.173 -> 1.2.174


[2022 / 10 / 18] -------1.0.2.4--------- 
- Thêm Fake Loading. Để thực hiện init các thư viện độc lập =>> Từ bây giờ muốn sử dụng thì phải init các thư viện lên trước
- Tham khảo VMCFirstLoading.cs
- Các hàm Initialize() sẽ phải init bằng code:
	+ VMC.Ads.AdsManager.Instance.Initialize(); // init ads
        + VMC.Ads.AdsManager.Instance.InitializeAOA();// init admob-open-ads

	+ VMC.Analystic.AnalysticManager.Instance.InitializeAppflyer();// init appsflyer
	+ VMC.Analystic.AnalysticManager.Instance.InitializeFirebase();// init firebase
- Đưa các callback của admob vào trong UnityMainThreadDispatcher.cs	



----------------------------------------------------------------------------------------------------------------
Admob Unity SDK: GoogleMobileAds-v7.2.0.unitypackage
ExternalDependencyManager: 1.2.174    -> https://developers.google.com/unity/packages
Firebase Analystic: 10.0.1
Firebase Remote Config: 10.0.1
AppsFlyer: 6.8.3  -> https://github.com/AppsFlyerSDK/appsflyer-unity-plugin/releases/tag/6.8.4
Applovin: AppLovin-MAX-Unity-Plugin-5.4.7-Android-11.4.6-iOS-11.4.4.unitypackage
AppReview: 1.8.1  -> https://developers.google.com/unity/packages#play_in-app_review

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

		VMC.Ads.AdsManager.Instance.Initialize(); // init ads

            	VMC.Ads.AdsManager.Instance.InitializeAOA();// init admob-open-ads

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

            	VMC.Analystic.AnalysticManager.Instance.InitializeAppflyer();// init appsflyer

            	VMC.Analystic.AnalysticManager.Instance.InitializeFirebase();// init firebase

		AnalysticManager.Instance.Logevent();